#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using DomainTools.Applications.Annotations;
using DomainTools.Applications.Properties;
using DomainTools.Applications.Utilities;
using DomainTools.ClassBuilders;
using DomainTools.Utilities.Utilities;
using FastColoredTextBoxNS;
using Microsoft.Data.ConnectionUI;
using NServiceKit.Text;

#endregion

namespace DomainTools.Applications
{
    internal partial class MainForm : Form, INotifyPropertyChanged
    {
        private readonly Timer _tmr = new Timer();
        private Action _action;

        //private System.Xml.Serialization.XmlSerializer _serializer =
        //    new System.Xml.Serialization.XmlSerializer(typeof(Settings));// TypeSerializer<Settings>();
        DataSource _ods = new DataSource("SqlStatementIsPopulated", "SQL Statement Valid?");

        public bool SqlCommandHasText
        {
            get
            {
                var sqlCommandHasText = this.SqlCommandTextBox.Text?.Length > 7;
                this.GenerateFromSqlCommandButton.Enabled = sqlCommandHasText;
                this.InspectSqlCommandButton.Enabled = sqlCommandHasText;
                return sqlCommandHasText;
            }
            set { Console.WriteLine(value); }
        }

        public MainForm()
        {
            InitializeComponent();

            this.GenerateFromSqlCommandButton.DataBindings.Add(
                nameof(GenerateFromSqlCommandButton.Enabled), 
                this,
                nameof(SqlCommandHasText));

            this.InspectSqlCommandButton.DataBindings.Add(
                nameof(GenerateFromSqlCommandButton.Enabled),
                this,
                nameof(SqlCommandHasText));

            this.Load += MainForm_Load;
            
            this.ConnectionDialogButton.Click += ConnectionDialogButton_Click;

            _config = _config = Settings.Default;
            _config.PropertyChanged += config_PropertyChanged;

            this.InspectProcedureButton.Click += InspectProcedureButton_Click;
            this.InspectSqlCommandButton.Click += InspectSqlCommandButton_Click;
            this.InspectTablesButton.Click += InspectTablesButton_Click;
            this.InspectViewsButton.Click += InspectViewsButton_Click;

            this.GenerateFromSqlCommandButton.Click += GenerateFromSqlCommandButton_Click;
            this.GenerateFromStoredProcedureButton.Click += GenerateFromStoredProcedureButton_Click;
            this.GenerateFromTableButton.Click += GenerateFromTableButton_Click;
            this.GenerateFromViewButton.Click += GenerateFromViewButton_Click;

            this.SqlCommandTextBox.Text = _config.SqlTextCommand;
            this.SqlCommandTextBox.TextChangedDelayed += SqlCommandTextBox_TextChangedDelayed;
            _config.OutputDirectory = _config.OutputDirectory;
            this.OutputDirectoryTextBox.Text = _config.OutputDirectory;
            this.OutputDirectoryTextBox.Leave += OutputDirectoryTextBox_TextChanged;
            _config.SqlConnectionString = _config.SqlConnectionString;
            this.ConnectionStringTextBox.Text = _config.SqlConnectionString;
            //this.ConnectionStringsDropDown.SelectionChangeCommitted += ConnectionStringsDropDown_SelectionChange;
            foreach (var item in _config.SavedConnectionStrings ?? new StringCollection())
            {
                this.ConnectionStringsDropDown.Items.Add(item);
            }
            if (this.ConnectionStringsDropDown.Items.Contains(_config.SqlConnectionString))
                ConnectionStringsDropDown.SelectedItem = _config.SqlConnectionString;

            this.ConnectionStringsDropDown.SelectionChangeCommitted += ConnectionStringsDropDown_SelectionChange;
            this.ConnectionStringsDropDown.PreviewKeyDown += ConnectionStringsDropDown_PreviewKeyDown;
            this.ConnectionStringTextBox.Leave += ConnectionStringTextBox_Leave;

            // todo: Column select mode? // new version required?
            this.SqlCommandTextBox.MouseDown += SqlCommandTextBox_MouseDown;
            this.SqlCommandTextBox.WordWrap = false;

            this.dataGridView1.AllowUserToDeleteRows = true;
            this.dataGridView1.KeyUp += dataGridView1_KeyUp;

            this.FormClosing += MainForm_FormClosing;

            //MessageBox.Show(_config.SqlTextCommand);
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            //this.MainMenuStrip.BackColor = Color.FromArgb(64, 64, 64);
            this.toolStrip1.BackColor = Color.FromArgb(64, 64, 64);
            Settings.Default.Upgrade();
            flushConnectionString();
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            updateConfigValues();
        }

        void OutputDirectoryTextBox_TextChanged(object sender, EventArgs e)
        {
            _config.OutputDirectory = OutputDirectoryTextBox.Text;
            _config.OutputDirectory = OutputDirectoryTextBox.Text;
            updateConfigValues();
        }

        void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            var selectedRows = dataGridView1.SelectedRows.Cast<DataGridViewRow>().ToArray();
            if (e.KeyCode != Keys.Delete || !selectedRows.Any())
                return;
            foreach (var row in selectedRows)
            {
                this.dataGridView1.Rows.Remove(row);
            }
        }



        void SqlCommandTextBox_MouseDown(object sender, MouseEventArgs e)
        {

        }

        void ConnectionStringTextBox_Leave(object sender, EventArgs e)
        {
            string connectionString = this.ConnectionStringTextBox.Text;
            addToConnectionStrings(connectionString);
            _config.SqlConnectionString = connectionString;
            SaveConnectionStrings();
            updateConfigValues();
            //_config.Save();

        }

        void ConnectionStringsDropDown_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (this.ConnectionStringsDropDown.SelectedItem != null && ConnectionStringsDropDown.Items.Count > 0)
            {

                if (e.KeyCode == Keys.Delete && (e.Modifiers.HasFlag(Keys.Control) || e.Modifiers.HasFlag(Keys.Shift)))
                {
                    ConnectionStringsDropDown.Items.Remove(ConnectionStringsDropDown.SelectedItem);
                    _config.SavedConnectionStrings.Clear();
                    _config.SavedConnectionStrings.AddRange(ConnectionStringsDropDown.Items.Cast<string>().ToArray());
                    _config.Save();
                }
            }

        }

        void ConnectionStringsDropDown_SelectionChange(object sender, EventArgs e)
        {
            if (this.ConnectionStringsDropDown.SelectedItem == null || ConnectionStringsDropDown.Items.Count == 0)
                return;
            var text = this.ConnectionStringsDropDown.SelectedItem.ToString();
            if (!text.HasValue(nonWhitespaceOnly: true))
                return;
            this.ConnectionStringTextBox.Text = text;
            _config.SqlConnectionString = text;
            updateConfigValues();
            _config.SqlConnectionString = text;
        }

        private GeneratorOptions getGeneratorOptions()
        {
            updateConfigValues();
            var generatorOptions = _config.AsGeneratorOptions();
            var dataSource = this.dataGridView1.DataSource;
            var bindingList = dataSource as BindingList<SchemaColumnInfo>;
            if (bindingList != null)
                generatorOptions.ColumnInfos = bindingList.ToList();
            return generatorOptions;
        }

        private ClassGenerator getClassGenerator()
        {
            return new ClassGenerator(new EntityBuilder(_config.AsGeneratorOptions()), getGeneratorOptions());
        }

        private void SqlCommandTextBox_TextChangedDelayed(object sender, TextChangedEventArgs e)
        {
            _tmr.Stop();
            _tmr.Tick -= _tmr_Tick;
            _action = () =>
                {
                    // suppress property change notifications or we'll get a loop here
                    // if the control and property are bound:
                    _config.PropertyChanged -= config_PropertyChanged;
                    // update the setting:
                    _config.SqlTextCommand = SqlCommandTextBox.Text;
                    // resume property change notifications:
                    _config.PropertyChanged += config_PropertyChanged;

                    Console.WriteLine(SqlCommandHasText);
                };
            _tmr.Tick += _tmr_Tick;
            _tmr.Interval = 100;
            _tmr.Start();
        }

        private void _tmr_Tick(object sender, EventArgs e)
        {
            // stop the event pump
            _tmr.Stop();
            _action?.Invoke();
        }

        private void config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _config.Save();
        }

        private void showSchemaTab()
        {
            tabControl1.SelectTab(SchemaTableTabPage);
        }

        #region Designer Wired-Up Events

        private readonly DataConnectionDialog _dcd = new DataConnectionDialog();
        private DataProvider _dataProvider;
        private DataSource _dataSource;

        private void ConnectionDialogButton_Click(object sender, EventArgs e)
        {
            _dcd.SaveSelection = true;
            DataSource dataSource = this._dataSource;
            if (dataSource != null)
            {
                if (this._dataProvider != null)
                    this._dcd.SetSelectedDataProvider(dataSource: _dataSource, dataProvider: this._dataProvider);
            }
            _dcd.DataSources.Add(DataSource.SqlDataSource);
            _dcd.UnspecifiedDataSource.Providers.Add(DataProvider.SqlDataProvider);
            _dcd.ConnectionString = _config.SqlConnectionString;

            DialogResult dialogResult = DialogResult.None;

            DisableForm();
            dialogResult = DataConnectionDialog.Show(this._dcd);
            EnableForm();
            
            if (dialogResult != DialogResult.OK)
                return;

            this._dataSource = this._dcd.SelectedDataSource;
            this._dataProvider = this._dcd.SelectedDataProvider;
            this.ConnectionStringTextBox.Text = this._dcd.ConnectionString;
            addToConnectionStrings(this.ConnectionStringTextBox.Text);
            SaveConnectionStrings();
        }

        void addToConnectionStrings(string value)
        {
            try
            {
                int idx = this.ConnectionStringsDropDown.FindString(value, 0);
                if (idx < 0)
                {
                    idx = this.ConnectionStringsDropDown.Items.Add(value);
                }
                this.ConnectionStringsDropDown.SelectedIndex = idx;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        void SaveConnectionStrings()
        {

            var array = new string[this.ConnectionStringsDropDown.Items.Count];
            if (this.ConnectionStringsDropDown.Items.Count > 0)
                this.ConnectionStringsDropDown.Items.CopyTo(array, 0);
            if (_config.SavedConnectionStrings != null)
                _config.SavedConnectionStrings.Clear();
            else
                _config.SavedConnectionStrings = new StringCollection();
            _config.SavedConnectionStrings.AddRange(array);
            _config.SavedConnectionStrings = _config.SavedConnectionStrings;
            Settings.Default.Save();
            _config.Save();
        }

        private void ChooseOutputDirectoryButton_Click(object sender, EventArgs e)
        {
            string selectedPath;
            if (OutputDirectoryTextBox.Text.HasValue() && Directory.Exists(OutputDirectoryTextBox.Text))
            {
                folderBrowserDialog1.SelectedPath = OutputDirectoryTextBox.Text;
            }
            DialogResult rslt = this.folderBrowserDialog1.ShowDialog(this);
            if (rslt == DialogResult.OK && Directory.Exists(selectedPath = this.folderBrowserDialog1.SelectedPath))
                this.OutputDirectoryTextBox.Text = selectedPath;
        }

        private void InspectDatabaseButton_Click(object sender, EventArgs e)
        {
            // _config.SqlConnectionString is bound to ConnectionStringTextBox.Text,
            // so either will do: // no, failing to update, so do it manually:
            _config.SqlConnectionString = this.ConnectionStringTextBox.Text;
            _config.Save();
            _interrogator.ConnectionString = _config.SqlConnectionString;

            Task.Run(() => 
                 DisableForm())
                .ContinueWith(task => InspectDatabase())
                .ContinueWith(task => EnableForm());
            }

        private void InspectDatabase()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                this.StoredProceduresDropdown.DataSource = null;
                this.StoredProceduresDropdown.Items.Clear();
                this.StoredProceduresDropdown.DataSource = _interrogator.GetStoredProcedureNames(SqlServerInterrogator.GetProcedureNamesCmdText)
                    .ToList();
                this.TablesDropdown.DataSource = null;
                this.TablesDropdown.Items.Clear();
                this.TablesDropdown.DataSource = _interrogator.GetTableNames()
                    .ToList();
                this.ViewsDropdown.DataSource = null;
                this.ViewsDropdown.Items.Clear();
                this.ViewsDropdown.DataSource = _interrogator.GetViewNames()
                    .ToList();
            }));
        }


        private void DisableForm()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                this.splitContainer1.Enabled = false;
                this.splitContainer2.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
            }));
        }

        private void EnableForm()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                this.splitContainer1.Enabled = true;
                this.splitContainer2.Enabled = true;
                this.Cursor = Cursors.Default;
            }));
        }


        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settingsForm = new SimpleReflectionForm<Settings>(_config, false, "SqlTextCommand");
            settingsForm.ShowDialog(this);
            _config.Save();
            flushConnectionString();
        }

        private void flushConnectionString()
        {
            _config.Upgrade();
            this.ConnectionStringTextBox.Text = _config.SqlConnectionString;
        }

        private void abbreviationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dict = new Dictionary<string, string>();
            var ser = new JsonSerializer<Dictionary<string, string>>();
            if (File.Exists("abbreviations.dict"))
            {
                try
                {
                    using (var streamReader = File.OpenText("abbreviations.dict"))
                    {
                        dict = ser.DeserializeFromReader(streamReader);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }

            var abbreviationsDictForm
                = new SimpleReflectionForm<Dictionary<string, string>>(dict ?? new Dictionary<string, string>(), true, "Abbreviations");
            var dialogResult = abbreviationsDictForm.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            dict = abbreviationsDictForm.Obj;
            using (var streamWriter = new StreamWriter("abbreviations.dict"))
            {
                ser.SerializeToWriter(dict, streamWriter);
            }
        }

        #endregion  Designer Wired-Up Events

        #region Imperatively (constructor) wired events

        private readonly SqlServerInterrogator _interrogator = new SqlServerInterrogator("");
        private static Settings _config;

        private void GenerateFromViewButton_Click(object sender, EventArgs e)
        {
            if (!(dataGridView1.DataSource is BindingList<SchemaColumnInfo>)
                || ((BindingList<SchemaColumnInfo>) dataGridView1.DataSource).Count == 0)
            {
                MessageBox.Show(@"You must click Inspect View before generating a class");
                return;
            }
            generateClass(CommandType.TableDirect, ViewsDropdown.SelectedValue.ToString());
        }

        private void GenerateFromTableButton_Click(object sender, EventArgs e)
        {
            if (!(dataGridView1.DataSource is BindingList<SchemaColumnInfo>) 
                || ((BindingList<SchemaColumnInfo>) dataGridView1.DataSource).Count == 0)
            {
                MessageBox.Show(@"You must click Inspect Table before generating a class");
                return;
            }
            updateConfigValues();
            generateClass(CommandType.TableDirect, TablesDropdown.SelectedValue.ToString());
        }

        private void updateConfigValues()
        {
            if (this.OutputDirectoryTextBox.Text.HasValue() && Directory.Exists(this.OutputDirectoryTextBox.Text))
                _config.OutputDirectory = this.OutputDirectoryTextBox.Text;
            _config.SqlConnectionString = this.ConnectionStringTextBox.Text;

            /*            var sourcetype = _config.GetType();
                        var typeAccessor = TypeAccessor.Create(sourcetype);
            

                        var properties = _config.GetType().GetPublicProperties();
                        foreach (var propertyInfo in properties)
                        {
                            try
                            {
                                _config[propertyInfo.Name] = typeAccessor[_config, propertyInfo.Name];
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(e);
                            }
                        }*/
            _config.Save();
        }

        private void GenerateFromStoredProcedureButton_Click(object sender, EventArgs e)
        {
            if (!(dataGridView1.DataSource is BindingList<SchemaColumnInfo>)
                || ((BindingList<SchemaColumnInfo>)dataGridView1.DataSource).Count == 0)
            {
                MessageBox.Show(@"You must click Inspect Procedure before generating a class");
                return;
            }
            generateClass(CommandType.StoredProcedure, StoredProceduresDropdown.SelectedValue.ToString());
        }

        private void GenerateFromSqlCommandButton_Click(object sender, EventArgs e)
        {
            generateClass(CommandType.Text, SqlCommandTextBox.Text);
            MessageBox.Show(
                @"Class names and namespaces must be manually specified for custom SQL commands; please edit your file accordingly.",
                @"Class name and namespace");
        }

        private void generateClass(CommandType commandType, string commandText)
        {
            ProgressStatusLabel.Text = @"Working...";
            try
            {
                DisableForm();
                switch (commandType)
                {
                    case CommandType.Text:
                        getClassGenerator()
                            .CreateTextCommandClass(commandText);
                        break;
                    case CommandType.StoredProcedure:
                        getClassGenerator()
                            .CreateProcedureClass(commandText);
                        break;
                    case CommandType.TableDirect:
                        getClassGenerator()
                            .CreateTableClass(commandText);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(commandType));
                }
                ProgressStatusLabel.Text = @"Done.";
                MessageBox.Show($"Class created for {commandText}");
            }
            catch (Exception e)
            {
                ProgressStatusLabel.Text = @"Error";
                MessageBox.Show(e.ToString(), @"Error generating class");
            }
            finally
            {
                EnableForm();
            }
        }

        private void InspectViewsButton_Click(object sender, EventArgs e)
        {
            string viewName = ViewsDropdown.SelectedValue?.ToString();
            if (!viewName.HasValue())
                return;
            // this shows the table:
            //DataTable dt = _interrogator.GetTextCommandSchema(textCommand:"SELECT top 100 * FROM " + viewName, traceUnderlyingSchemaColumns: false);
            // this shows the columns:
            try
            {
                DisableForm();
                var columnInfos = _interrogator.GetTableColumnInfoList(viewName);//.GetTableSchema(viewName);
                dataGridView1.DataSource = new BindingList<SchemaColumnInfo>(columnInfos);
                showSchemaTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                EnableForm();
            }

        }

        private void InspectTablesButton_Click(object sender, EventArgs e)
        {
            string tableName = TablesDropdown.SelectedValue?.ToString();
            if (!tableName.HasValue())
                return;
            try
            {
                DisableForm();
                var columnInfos = _interrogator.GetTableColumnInfoList(tableName);//.GetTableSchema(tableName);
                dataGridView1.DataSource = new BindingList<SchemaColumnInfo>(columnInfos);
                showSchemaTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                EnableForm();
            }
        }

        private void InspectSqlCommandButton_Click(object sender, EventArgs e)
        {
            string textCommand = this.SqlCommandTextBox.Text;
            if (!textCommand.HasValue())
                return;
            try
            {
                DisableForm();
                var dt = _interrogator.GetTextCommandColumnInfoList(textCommand);//.GetTextCommandSchema(textCommand);
                //this.dataGridView1.DataSource = dt;
                dataGridView1.DataSource = new BindingList<SchemaColumnInfo>(dt);
                showSchemaTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                EnableForm();
            }
        }

        private void InspectProcedureButton_Click(object sender, EventArgs e)
        {
            DisableForm();
            string procedureName = StoredProceduresDropdown?.SelectedValue?.ToString();
            if (!procedureName.HasValue())
                return;
            try
            {
                IList<SchemaColumnInfo> columnInfos = _interrogator.GetProcedureColumnInfoList(procedureName);
                this.dataGridView1.DataSource = columnInfos;
                showSchemaTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                EnableForm();
            }
        }

        #endregion Imperatively (constructor) wired events

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}