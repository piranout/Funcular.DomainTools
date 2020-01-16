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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using Funcular.DomainTools.Applications.Properties;
using Funcular.DomainTools.Applications.Utilities;
using Funcular.DomainTools.ClassBuilders;
using Funcular.DomainTools.ClassBuilders.SqlMetaData;
using Funcular.DomainTools.Utilities;
using Microsoft.Data.ConnectionUI;
using NServiceKit.Text;
using Settings = Funcular.DomainTools.Applications.Properties.Settings;
using Timer = System.Windows.Forms.Timer;

#endregion

namespace Funcular.DomainTools.Applications
{
    internal partial class MainForm : Form, INotifyPropertyChanged
    {
        #region Fields

        private readonly Timer _tmr = new Timer();
        private Action _action;

        private readonly DataConnectionDialog _dcd = new DataConnectionDialog();
        private DataProvider _dataProvider;
        private DataSource _dataSource;
        private SqlServerInterrogator _interrogator = new SqlServerInterrogator("");
        
        private static GeneratorOptions _generatorOptions;
        private DomainManager _domainManager;
        private string _projectFilename;
        private IList<string> _tableNames;
        private IList<string> _storedProcedures;
        private IList<string> _viewNames;

        private static bool _isFiltering;
        private static string _lastTableFilterText = "";
        private static readonly Keys[] _stopKeys = new[] {Keys.Tab, Keys.Space, Keys.Enter, Keys.OemPeriod, Keys.OemOpenBrackets, Keys.OemCloseBrackets, Keys.Oemcomma, };

        #endregion

        #region Properties

        public string ProjectFilename
        {
            get { return _projectFilename; }
            private set
            {
                _projectFilename = value;
                saveMenuItem.Enabled = !string.IsNullOrWhiteSpace(value) && File.Exists(value);
            }
        }

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

        #endregion

        #region Constructor

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

            WireUpControlEvents();

        }

        void MainForm_Load(object sender, EventArgs e)
        {
            this.toolStrip1.BackColor = Color.FromArgb(64, 64, 64);
            // Settings.Default.Upgrade();
            if (StringHelpers.HasValue(Settings.Default.ProjectFile) && File.Exists(Settings.Default.ProjectFile))
            {
                _generatorOptions = LoadProject(Settings.Default.ProjectFile);
            }
            else if (File.Exists("PocoSettings.json"))
            {
                _generatorOptions = LoadProject("PocoSettings.json");
            }
            RefreshControlValues(_generatorOptions);
            // refreshConnectionStringFromConfigFile();
            this.Text += $" [{Environment.UserDomainName}\\{Environment.UserName}]";
            this.TablesDropdown.AutoCompleteMode = AutoCompleteMode.Suggest;
        }

        private void WireUpControlEvents()
        {
            this.InspectProcedureButton.Click += InspectProcedureButton_Click;
            this.InspectSqlCommandButton.Click += InspectSqlCommandButton_Click;
            this.InspectTablesButton.Click += InspectTablesButton_Click;
            this.InspectViewsButton.Click += InspectViewButton_Click;

            this.GenerateFromSqlCommandButton.Click += GenerateFromSqlCommandButton_Click;
            this.GenerateFromStoredProcedureButton.Click += GenerateFromStoredProcedureButton_Click;
            this.GenerateFromTableButton.Click += GenerateFromTableButton_Click;
            this.GenerateFromViewButton.Click += GenerateFromViewButton_Click;
            this.FullAutoTablesButton.Click += FullAutoTablesButton_Click;
            this.OutputDirectoryTextBox.Leave += OutputDirectoryTextBox_TextChanged;
            // this.SqlCommandTextBox.TextChangedDelayed += SqlCommandTextBox_TextChangedDelayed;
            this.ConnectionStringsDropDown.SelectionChangeCommitted += ConnectionStringsDropDown_SelectionChange;
            // this.ConnectionStringsDropDown.PreviewKeyDown += ConnectionStringsDropDown_PreviewKeyDown;
            // this.ConnectionStringTextBox.Leave += ConnectionStringTextBox_Leave;
            this.ConnectionDialogButton.Click += ConnectionDialogButton_Click;


            // this.TablesDropdown.TextChanged += TableFilterChanged;
            // this.TableNameTextBox.TextChanged += TableFilterChanged;
            // this.TableNameTextBox.KeyPress += TableNameTextBox_KeyPress;
            TableNameTextBox.KeyUp += TableNameTextBox_KeyUp;

            // todo: Column select mode? // new version required?
            this.SqlCommandTextBox.MouseDown += SqlCommandTextBox_MouseDown;
            this.dataGridView1.KeyUp += dataGridView1_KeyUp;
            this.FormClosing += MainForm_FormClosing;

            this.SqlCommandTextBox.WordWrap = false;
            this.dataGridView1.AllowUserToDeleteRows = true;

            foreach (Control c in this.Controls.OfType<TableLayoutPanel>())
            {
                c.TabStop = false;
            }
            foreach (Control c in this.Controls.OfType<GroupBox>())
            {
                c.TabStop = false;
            }

            RefreshControlValues();
        }

        #endregion

        #region Event handlers

        private void TableNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (_stopKeys.Contains(e.KeyCode))
            {
                e.SuppressKeyPress = true;
                InvokeDisableForm();
                TableFilterChanged(null,null);
                InvokeEnableForm();
            }
        }

        private void TableFilterChanged(object sender, EventArgs e)
        {
            // TableNameTextBox.TextChanged -= TableFilterChanged;
            if (_isFiltering || TableNameTextBox.Text.Equals(_lastTableFilterText))
                return;
            _isFiltering = true;
            _lastTableFilterText = TableNameTextBox.Text;
            Task.Factory.StartNew(() =>
            {
                Invoke((MethodInvoker) (() =>
                {
                    FilterComboBox(TableNameTextBox, _tableNames.ToArray(), TablesDropdown);
                }));
            });
            // TableNameTextBox.TextChanged += TableFilterChanged;
            _isFiltering = false;
        }

        private void FilterComboBox(TextBox textBoxFilter, string[] stringCollection, ComboBox comboBox)
        {
            comboBox.BeginUpdate();
            // Thread.Sleep(250);
            var filterString = textBoxFilter.Text.RemoveAll(_stopKeys.Select(s => s.ToString()).ToArray());
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                var filteredCollection = stringCollection
                        .Where(s => s.IndexOf(filterString, StringComparison.OrdinalIgnoreCase) > -1)
                        .ToArray();
                if (filteredCollection.HasContents())
                {
                    SetListControlDataSource(comboBox, filteredCollection);
                    comboBox.SelectedItem = filteredCollection.FirstOrDefault();
                }
            }
            else
            {
                SetListControlDataSource(comboBox, stringCollection);
            }
            // textBoxFilter.TextChanged += TableFilterChanged;
            // Thread.Sleep(10);
            comboBox.EndUpdate();
        }

        private void timerTick(object sender, EventArgs e)
        {
            // stop the event pump
            _tmr.Stop();
            _action?.Invoke();
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        #endregion

        #region Helper methods

        private void RefreshControlValues(GeneratorOptions generatorOptions = null)
        {
            generatorOptions = generatorOptions ?? getGeneratorOptions();

            this.SqlCommandTextBox.Text = generatorOptions.SqlTextCommand;
            this.OutputDirectoryTextBox.Text = generatorOptions.OutputDirectory;
            this.ConnectionStringTextBox.Text = generatorOptions.SqlConnectionString;
            this.ConnectionStringsDropDown.Items.Clear();
            foreach (var item in generatorOptions.SavedConnectionStrings ?? new StringCollection())
            {
                // avoid dupes:
                if (!this.ConnectionStringsDropDown.Items.Contains(item))
                    this.ConnectionStringsDropDown.Items.Add(item);
            }
            if (generatorOptions?.SqlConnectionString != null && this.ConnectionStringsDropDown.Items.Contains(generatorOptions.SqlConnectionString))
            {
                ConnectionStringsDropDown.SelectedItem = generatorOptions.SqlConnectionString;
            }
            this.ConnectionStringTextBox.Focus();
        }

        private GeneratorOptions getGeneratorOptions()
        {
            //saveOutputDirectoryAndConnectionString();
            var generatorOptions = _generatorOptions ??/* _config?.AsGeneratorOptions() ??*/(_generatorOptions = new GeneratorOptions());
            
            generatorOptionsBindingSource.DataSource = generatorOptions;
            // generatorOptions.OutputDirectory = this.OutputDirectoryTextBox.Text;
            // var dataSource = this.dataGridView1.DataSource;
            // var bindingList = dataSource as BindingList<SchemaColumnInfo>;
            /*if (bindingList != null)
                generatorOptions.ColumnInfos = bindingList.ToList();*/
            return generatorOptions;
        }

        private void setGeneratorOptions(GeneratorOptions options)
        {
            _generatorOptions = options;
            Settings.Default.OutputDirectory = options.OutputDirectory;
            Settings.Default.ProjectFile = options.ProjectFile;
            Settings.Default.Save();
            RefreshControlValues(options);
        }

        private ClassGenerator getClassGenerator()
        {
            var generatorOptions = getGeneratorOptions();
            var entityBuilder = new EntityBuilder(generatorOptions);
            var classGenerator = new ClassGenerator(entityBuilder, generatorOptions);
            return classGenerator;
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
            var generatorOptions = getGeneratorOptions();
            if (generatorOptions.SavedConnectionStrings != null)
                generatorOptions.SavedConnectionStrings.Clear();
            else
                generatorOptions.SavedConnectionStrings = new StringCollection();
            generatorOptions.SavedConnectionStrings.AddRange(array);
            generatorOptions.SavedConnectionStrings = generatorOptions.SavedConnectionStrings;
            Settings.FromGeneratorOptions(generatorOptions).Save();
        }

        private void InvokeInspectDatabase()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                _storedProcedures = _domainManager.GetStoredProcedureNames();
                _tableNames = _domainManager.GetTableNames();
                _viewNames = _domainManager.GetViewNames();

                SetListControlDataSource(this.StoredProceduresDropdown, _storedProcedures);
                SetListControlDataSource(this.TablesDropdown, _tableNames);
                SetListControlDataSource(this.ViewsDropdown, _viewNames);
            }));
        }

        private static void SetListControlDataSource(ListControl control, ICollection<string> dataSource)
        {
            control.DataSource = new []{""};
            control.DataSource = dataSource;
            var box = control as ComboBox;
            if (box != null)
            {
                var autoCompleteStringCollection = new AutoCompleteStringCollection();
                autoCompleteStringCollection.AddRange(dataSource.ToArray());
                box.AutoCompleteCustomSource = autoCompleteStringCollection;
            }
            control.Refresh();
        }


        private void InvokeDisableForm()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                this.splitContainer1.Enabled = false;
                this.splitContainer2.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
            }));
        }

        private void InvokeEnableForm()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                this.splitContainer1.Enabled = true;
                this.splitContainer2.Enabled = true;
                this.Cursor = Cursors.Default;
            }));
        }

        private void refreshConnectionStringFromConfigFile()
        {
            // _config.Upgrade();
            // this.ConnectionStringTextBox.Text = _config.SqlConnectionString;
        }

        private void showSchemaTab()
        {
            tabControl1.SelectTab(SchemaTableTabPage);
        }

        #endregion

        #region Designer Wired-Up Events

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
            _dcd.ConnectionString = getGeneratorOptions().SqlConnectionString;

            DialogResult dialogResult = DialogResult.None;

            InvokeDisableForm();
            dialogResult = DataConnectionDialog.Show(this._dcd);
            InvokeEnableForm();

            if (dialogResult != DialogResult.OK)
                return;

            this._dataSource = this._dcd.SelectedDataSource;
            this._dataProvider = this._dcd.SelectedDataProvider;
            this.ConnectionStringTextBox.Text = this._dcd.ConnectionString;
            addToConnectionStrings(this.ConnectionStringTextBox.Text);
            SaveConnectionStrings();
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
            // so either will do: // no, failing to update, doing it manually is 
            // quicker than figuring out why:
            //_config.SqlConnectionString = this.ConnectionStringTextBox.Text;
            //_config.Save();
            var interrogatorConnectionString = _generatorOptions.SqlConnectionString;
            if(_interrogator.ConnectionString != interrogatorConnectionString)
                _interrogator = new SqlServerInterrogator(interrogatorConnectionString);

            _domainManager = new DomainManager(
                generatorOptions: _generatorOptions,
                classGenerator: getClassGenerator(), 
                interrogator: _interrogator);

            Task.Run(() =>
                 InvokeDisableForm())
                .ContinueWith(task => InvokeInspectDatabase())
                .ContinueWith(task => InvokeEnableForm());
        }


        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settingsForm = new SimpleReflectionForm<GeneratorOptions>(getGeneratorOptions(), false, "SqlTextCommand");
            settingsForm.ShowDialog(this);
            if (settingsForm.DialogResult == DialogResult.OK)
                _generatorOptions = settingsForm.Obj;
            SaveProject(_generatorOptions.ProjectFile, _generatorOptions);
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

        private void GenerateFromViewButton_Click(object sender, EventArgs e)
        {
            if (!(dataGridView1.DataSource is BindingList<SchemaColumnInfo>)
                || ((BindingList<SchemaColumnInfo>)dataGridView1.DataSource).Count == 0)
            {
                MessageBox.Show(@"You must click Inspect View before generating a class");
                return;
            }
            generateClass(CommandType.TableDirect, ViewsDropdown.SelectedValue.ToString());
        }

        private void GenerateFromTableButton_Click(object sender, EventArgs e)
        {
            if (!(dataGridView1.DataSource is BindingList<SchemaColumnInfo>)
                || ((BindingList<SchemaColumnInfo>)dataGridView1.DataSource).Count == 0)
            {
                MessageBox.Show(@"You must click Inspect Table before generating a class");
                return;
            }
            generateClass(CommandType.TableDirect, TablesDropdown.SelectedValue.ToString());
        }

        private void FullAutoTablesButton_Click(object sender, EventArgs e)
        {
            foreach (var item in TablesDropdown.Items)
            {
                    
            }
            
        }

        void OutputDirectoryTextBox_TextChanged(object sender, EventArgs e)
        {
         /*   _config.OutputDirectory = OutputDirectoryTextBox.Text;
            _config.OutputDirectory = OutputDirectoryTextBox.Text;*/
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
            _generatorOptions.SqlConnectionString = connectionString;
            SaveConnectionStrings();
            //_config.Save();

        }

        void ConnectionStringsDropDown_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (this.ConnectionStringsDropDown.SelectedItem != null && ConnectionStringsDropDown.Items.Count > 0)
            {

                if (e.KeyCode == Keys.Delete && (e.Modifiers.HasFlag(Keys.Control) || e.Modifiers.HasFlag(Keys.Shift)))
                {
                    ConnectionStringsDropDown.Items.Remove(ConnectionStringsDropDown.SelectedItem);
                    _generatorOptions.SavedConnectionStrings.Clear();
                    _generatorOptions.SavedConnectionStrings.AddRange(ConnectionStringsDropDown.Items.Cast<string>().ToArray());
                    // _generatorOptions.Save();
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
            _generatorOptions.SqlConnectionString = text;
            // _config.SqlConnectionString = text;
        }

        private void SqlCommandTextBox_TextChangedDelayed(object sender, TextChangedEventArgs e)
        {
            _tmr.Stop();
            _tmr.Tick -= timerTick;
/*            _action = () =>
            {
                // suppress property change notifications or we'll get a loop here
                // if the control and property are bound:
                _config.PropertyChanged -= config_PropertyChanged;
                // update the setting:
                _config.SqlTextCommand = SqlCommandTextBox.Text;
                // resume property change notifications:
                _config.PropertyChanged += config_PropertyChanged;

                Console.WriteLine(SqlCommandHasText);
            };*/
            _tmr.Tick += timerTick;
            _tmr.Interval = 100;
            _tmr.Start();
        }

        private void config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           // _config.Save();
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
                InvokeDisableForm();
                _domainManager.GeneratorOptions = getGeneratorOptions();
                _domainManager.CreateClass(commandType, commandText);
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
                InvokeEnableForm();
            }
        }

        private void InspectViewButton_Click(object sender, EventArgs e)
        {
            string viewName = ViewsDropdown.SelectedValue?.ToString();
            if (!viewName.HasValue())
                return;
            // this shows the table:
            //DataTable dt = _interrogator.GetTextCommandSchema(textCommand:"SELECT top 100 * FROM " + viewName, traceUnderlyingSchemaColumns: false);
            // this shows the columns:
            try
            {
                InvokeDisableForm();
                var columnInfos = _domainManager.GetTableColumnInfoList(viewName);
                dataGridView1.DataSource = new BindingList<SchemaColumnInfo>(columnInfos);
                showSchemaTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                InvokeEnableForm();
            }

        }

        private void InspectTablesButton_Click(object sender, EventArgs e)
        {
            string tableName = TablesDropdown.SelectedValue?.ToString();
            if (!tableName.HasValue())
                return;
            try
            {
                InvokeDisableForm();
                var columnInfos = _domainManager.GetTableColumnInfoList(tableName);
                dataGridView1.DataSource = new BindingList<SchemaColumnInfo>(columnInfos);
                showSchemaTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                InvokeEnableForm();
            }
        }

        private void InspectSqlCommandButton_Click(object sender, EventArgs e)
        {

            try
            {
                string textCommand = this.SqlCommandTextBox.Text;
                if (!textCommand.HasValue())
                    return;
                InvokeDisableForm();
                var columnInfos = _domainManager.GetTextCommandColumnInfoList(textCommand);
                dataGridView1.DataSource = new BindingList<SchemaColumnInfo>(columnInfos);
                showSchemaTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                InvokeEnableForm();
            }
        }

        private void InspectProcedureButton_Click(object sender, EventArgs e)
        {
            try
            {
                InvokeDisableForm();
                string procedureName = StoredProceduresDropdown?.SelectedValue?.ToString();
                if (!procedureName.HasValue())
                    return;
                this.dataGridView1.DataSource = new BindingList<SchemaColumnInfo>(_domainManager.GetProcedureColumnInfoList(procedureName));
                showSchemaTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                InvokeEnableForm();
            }
        }

        #endregion Imperatively (constructor) wired events

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void loadMenuItem_Click(object sender, EventArgs e)
        {
            var generatorOptions = getGeneratorOptions();
            var initialDirectory = generatorOptions.OutputDirectory;
            OpenFileDialog open = new OpenFileDialog
            {
                InitialDirectory =
                    Directory.Exists(initialDirectory) ? initialDirectory : @"C:\\temp",
                Title = @"Select project file to load…",
            };
            if (open.ShowDialog(this) == DialogResult.OK)
            {
                var fileName = open.FileName;
                var project = LoadProject(fileName);
                generatorOptions.OutputDirectory = Path.GetDirectoryName(fileName);
                generatorOptions.ProjectFile = fileName;
                setGeneratorOptions(project);

            }
        }

        private static GeneratorOptions LoadProject(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (var fileStream = File.OpenRead(fileName))
                {
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        var ser = new JsonSerializer<GeneratorOptions>();
                        var options = ser.DeserializeFromReader(streamReader);
                        Settings.Default.OutputDirectory = options.OutputDirectory;
                        Settings.Default.ProjectFile = options.ProjectFile;
                        return options;
                    }
                }
            }
            return null;
        }

        private void saveAsMenuItem_Click(object sender, EventArgs e)
        {
            var generatorOptions = getGeneratorOptions();
            var initialDirectory = generatorOptions.OutputDirectory;
            SaveFileDialog saveDialog = new SaveFileDialog()
            {
                InitialDirectory =
                    Directory.Exists(initialDirectory) ? initialDirectory : @"C:\\temp",
                Title = @"Save current settings as…",
            };
            if (saveDialog.ShowDialog(this) == DialogResult.OK)
            {
                var projectFilename = saveDialog.FileName;
                if (SaveProject(projectFilename, generatorOptions)) 
                    this.ProjectFilename = projectFilename;
            }
        }

        private static bool SaveProject(string projectFilename, GeneratorOptions generatorOptions)
        {
            try
            {
                var fileInfo = new FileInfo(projectFilename);
                JsonSerializer<GeneratorOptions> ser = new JsonSerializer<GeneratorOptions>();
                if (!(Directory.Exists(fileInfo.DirectoryName)))
                {
                    MessageBox.Show($@"Directory does not exist: {fileInfo.DirectoryName}", @"Oopsie!", MessageBoxButtons.OK,
                        MessageBoxIcon.Hand);
                    return true;
                }
                using (var fileStream = File.OpenWrite(projectFilename))
                {
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        ser.SerializeToWriter(generatorOptions, streamWriter);
                        Settings.Default.OutputDirectory = new FileInfo(projectFilename).DirectoryName;
                        Settings.Default.ProjectFile = projectFilename;
                        Settings.Default.Save();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            var project = getGeneratorOptions();
            SaveProject(this.ProjectFilename, project);
            setGeneratorOptions(project);
        }

        private void clearTablesFilterButton_Click(object sender, EventArgs e)
        {
            TableNameTextBox.Text = null;
            TableFilterChanged(sender,e);
        }

        private void menuItemAssemblies_Click(object sender, EventArgs e)
        {
            new AssembliesForm().Show();
        }
    }
}