using Funcular.DomainTools.Applications.Properties;
using Funcular.DomainTools.ClassBuilders;
using Settings = Funcular.DomainTools.Applications.Properties.Settings;

namespace Funcular.DomainTools.Applications
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Settings settings1 = new Settings();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.loadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abbreviationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectionStringTextBox = new System.Windows.Forms.TextBox();
            this.generatorOptionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.InspectDatabaseButton = new System.Windows.Forms.Button();
            this.OutputDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.OutputDirectoryLabel = new System.Windows.Forms.Label();
            this.ConnectionDialogButton = new System.Windows.Forms.Button();
            this.ChooseOutputDirectoryButton = new System.Windows.Forms.Button();
            this.ConnectionStringsDropDown = new System.Windows.Forms.ComboBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.clearTablesFilterButton = new System.Windows.Forms.Button();
            this.TableNameTextBox = new System.Windows.Forms.TextBox();
            this.FullAutoTablesButton = new System.Windows.Forms.Button();
            this.GenerateFromViewButton = new System.Windows.Forms.Button();
            this.InspectViewsButton = new System.Windows.Forms.Button();
            this.GenerateFromTableButton = new System.Windows.Forms.Button();
            this.InspectTablesButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.GenerateFromStoredProcedureButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ViewsDropdown = new System.Windows.Forms.ComboBox();
            this.InspectProcedureButton = new System.Windows.Forms.Button();
            this.TablesDropdown = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.StoredProceduresDropdown = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.SqlCommandTabPage = new System.Windows.Forms.TabPage();
            this.SqlCommandTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            this.SchemaTableTabPage = new System.Windows.Forms.TabPage();
            this.InspectSqlCommandButton = new System.Windows.Forms.Button();
            this.GenerateFromSqlCommandButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.HomeToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProgressStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.generatorOptionsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SqlCommandTabPage.SuspendLayout();
            this.SchemaTableTabPage.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.splitContainer1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.ConnectionStringTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.InspectDatabaseButton);
            this.splitContainer1.Panel1.Controls.Add(this.OutputDirectoryTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.OutputDirectoryLabel);
            this.splitContainer1.Panel1.Controls.Add(this.ConnectionDialogButton);
            this.splitContainer1.Panel1.Controls.Add(this.ChooseOutputDirectoryButton);
            this.splitContainer1.Panel1.Controls.Add(this.ConnectionStringsDropDown);
            this.splitContainer1.Panel1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1390, 705);
            this.splitContainer1.SplitterDistance = 145;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.DimGray;
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(1390, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadMenuItem,
            this.editMenuItem,
            this.saveMenuItem,
            this.saveAsMenuItem,
            this.abbreviationsToolStripMenuItem});
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(67, 22);
            this.toolStripDropDownButton1.Text = "&Settings";
            // 
            // loadMenuItem
            // 
            this.loadMenuItem.Name = "loadMenuItem";
            this.loadMenuItem.Size = new System.Drawing.Size(156, 22);
            this.loadMenuItem.Text = "&Load...";
            this.loadMenuItem.Click += new System.EventHandler(this.loadMenuItem_Click);
            // 
            // editMenuItem
            // 
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(156, 22);
            this.editMenuItem.Text = "&Edit";
            this.editMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.Size = new System.Drawing.Size(156, 22);
            this.saveMenuItem.Text = "&Save";
            this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.Name = "saveAsMenuItem";
            this.saveAsMenuItem.Size = new System.Drawing.Size(156, 22);
            this.saveAsMenuItem.Text = "Save &As...";
            this.saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
            // 
            // abbreviationsToolStripMenuItem
            // 
            this.abbreviationsToolStripMenuItem.Name = "abbreviationsToolStripMenuItem";
            this.abbreviationsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.abbreviationsToolStripMenuItem.Text = "&Abbreviations";
            this.abbreviationsToolStripMenuItem.ToolTipText = "Provide translations for known abbreviations and acronyms used in column and tabl" +
    "e names";
            this.abbreviationsToolStripMenuItem.Click += new System.EventHandler(this.abbreviationsToolStripMenuItem_Click);
            // 
            // ConnectionStringTextBox
            // 
            this.ConnectionStringTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.generatorOptionsBindingSource, "SqlConnectionString", true));
            this.ConnectionStringTextBox.Location = new System.Drawing.Point(160, 96);
            this.ConnectionStringTextBox.Name = "ConnectionStringTextBox";
            this.ConnectionStringTextBox.Size = new System.Drawing.Size(738, 27);
            this.ConnectionStringTextBox.TabIndex = 5;
            settings1.AdditionalCollapseTokens = "new;_";
            settings1.AdditionalTokenFind = "Base";
            settings1.AdditionalTokenReplace = "ExtensionBase";
            settings1.AppendDbSchemaToNamepaces = true;
            settings1.BaseNamespace = "Company.Solution";
            settings1.DataProviderNamespace = "Providers";
            settings1.EntitiesImplementInterfaces = "";
            settings1.EntitiesInherit = "";
            settings1.EntityAttributes = "";
            settings1.EntityNamespace = "Entities";
            settings1.GenerateCrmSpecificProperties = false;
            settings1.GeneratedIdDataType = "\"\"";
            settings1.GenerateFluentEFMappings = true;
            settings1.IdGenerationFunc = "\"\"";
            settings1.ImplementIdGenerator = false;
            settings1.MappingAttributes = "";
            settings1.OutputDirectory = "c:\\temp";
            settings1.PrimaryKeyGetsNamedId = false;
            settings1.ProjectFile = "";
            settings1.SavedConnectionStrings = null;
            settings1.SettingsKey = "";
            settings1.SqlConnectionString = "";
            settings1.SqlTextCommand = "";
            settings1.UseAutomaticProperties = true;
            settings1.Usings = "";
            this.ConnectionStringTextBox.Text = settings1.SqlConnectionString;
            // 
            // generatorOptionsBindingSource
            // 
            this.generatorOptionsBindingSource.DataSource = typeof(GeneratorOptions);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Connection String";
            // 
            // InspectDatabaseButton
            // 
            this.InspectDatabaseButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.InspectDatabaseButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.InspectDatabaseButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InspectDatabaseButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.InspectDatabaseButton.Image = global::Funcular.DomainTools.Applications.Properties.Resources.search;
            this.InspectDatabaseButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.InspectDatabaseButton.Location = new System.Drawing.Point(965, 99);
            this.InspectDatabaseButton.Name = "InspectDatabaseButton";
            this.InspectDatabaseButton.Size = new System.Drawing.Size(131, 33);
            this.InspectDatabaseButton.TabIndex = 7;
            this.InspectDatabaseButton.Text = "Inspect DB";
            this.InspectDatabaseButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.InspectDatabaseButton.UseVisualStyleBackColor = true;
            this.InspectDatabaseButton.Click += new System.EventHandler(this.InspectDatabaseButton_Click);
            // 
            // OutputDirectoryTextBox
            // 
            this.OutputDirectoryTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", settings1, "OutputDirectory", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.OutputDirectoryTextBox.Location = new System.Drawing.Point(160, 61);
            this.OutputDirectoryTextBox.Name = "OutputDirectoryTextBox";
            this.OutputDirectoryTextBox.Size = new System.Drawing.Size(763, 27);
            this.OutputDirectoryTextBox.TabIndex = 2;
            this.OutputDirectoryTextBox.Text = settings1.OutputDirectory;
            // 
            // OutputDirectoryLabel
            // 
            this.OutputDirectoryLabel.AutoSize = true;
            this.OutputDirectoryLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OutputDirectoryLabel.Location = new System.Drawing.Point(13, 61);
            this.OutputDirectoryLabel.Name = "OutputDirectoryLabel";
            this.OutputDirectoryLabel.Size = new System.Drawing.Size(116, 20);
            this.OutputDirectoryLabel.TabIndex = 1;
            this.OutputDirectoryLabel.Text = "OutputDirectory";
            // 
            // ConnectionDialogButton
            // 
            this.ConnectionDialogButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ConnectionDialogButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ConnectionDialogButton.Location = new System.Drawing.Point(930, 98);
            this.ConnectionDialogButton.Name = "ConnectionDialogButton";
            this.ConnectionDialogButton.Size = new System.Drawing.Size(29, 31);
            this.ConnectionDialogButton.TabIndex = 6;
            this.ConnectionDialogButton.Text = "...";
            this.ConnectionDialogButton.UseVisualStyleBackColor = true;
            // 
            // ChooseOutputDirectoryButton
            // 
            this.ChooseOutputDirectoryButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ChooseOutputDirectoryButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ChooseOutputDirectoryButton.Location = new System.Drawing.Point(930, 47);
            this.ChooseOutputDirectoryButton.Name = "ChooseOutputDirectoryButton";
            this.ChooseOutputDirectoryButton.Size = new System.Drawing.Size(29, 31);
            this.ChooseOutputDirectoryButton.TabIndex = 3;
            this.ChooseOutputDirectoryButton.Text = "...";
            this.ChooseOutputDirectoryButton.UseVisualStyleBackColor = true;
            this.ChooseOutputDirectoryButton.Click += new System.EventHandler(this.ChooseOutputDirectoryButton_Click);
            // 
            // ConnectionStringsDropDown
            // 
            this.ConnectionStringsDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ConnectionStringsDropDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConnectionStringsDropDown.FormattingEnabled = true;
            this.ConnectionStringsDropDown.Location = new System.Drawing.Point(160, 95);
            this.ConnectionStringsDropDown.Name = "ConnectionStringsDropDown";
            this.ConnectionStringsDropDown.Size = new System.Drawing.Size(763, 28);
            this.ConnectionStringsDropDown.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.splitContainer2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.clearTablesFilterButton);
            this.splitContainer2.Panel1.Controls.Add(this.TableNameTextBox);
            this.splitContainer2.Panel1.Controls.Add(this.FullAutoTablesButton);
            this.splitContainer2.Panel1.Controls.Add(this.GenerateFromViewButton);
            this.splitContainer2.Panel1.Controls.Add(this.InspectViewsButton);
            this.splitContainer2.Panel1.Controls.Add(this.GenerateFromTableButton);
            this.splitContainer2.Panel1.Controls.Add(this.InspectTablesButton);
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            this.splitContainer2.Panel1.Controls.Add(this.GenerateFromStoredProcedureButton);
            this.splitContainer2.Panel1.Controls.Add(this.label5);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.ViewsDropdown);
            this.splitContainer2.Panel1.Controls.Add(this.InspectProcedureButton);
            this.splitContainer2.Panel1.Controls.Add(this.TablesDropdown);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.StoredProceduresDropdown);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer2.Panel2MinSize = 100;
            this.splitContainer2.Size = new System.Drawing.Size(1390, 555);
            this.splitContainer2.SplitterDistance = 385;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.TabStop = false;
            // 
            // clearTablesFilterButton
            // 
            this.clearTablesFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.clearTablesFilterButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.clearTablesFilterButton.FlatAppearance.BorderSize = 0;
            this.clearTablesFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearTablesFilterButton.Font = new System.Drawing.Font("Tahoma", 8F);
            this.clearTablesFilterButton.Location = new System.Drawing.Point(300, 232);
            this.clearTablesFilterButton.Name = "clearTablesFilterButton";
            this.clearTablesFilterButton.Size = new System.Drawing.Size(27, 19);
            this.clearTablesFilterButton.TabIndex = 7;
            this.clearTablesFilterButton.Text = "X";
            this.clearTablesFilterButton.UseVisualStyleBackColor = false;
            this.clearTablesFilterButton.Click += new System.EventHandler(this.clearTablesFilterButton_Click);
            // 
            // TableNameTextBox
            // 
            this.TableNameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.TableNameTextBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.TableNameTextBox.Location = new System.Drawing.Point(17, 230);
            this.TableNameTextBox.Name = "TableNameTextBox";
            this.TableNameTextBox.Size = new System.Drawing.Size(316, 23);
            this.TableNameTextBox.TabIndex = 11;
            // 
            // FullAutoTablesButton
            // 
            this.FullAutoTablesButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.FullAutoTablesButton.FlatAppearance.BorderSize = 2;
            this.FullAutoTablesButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.FullAutoTablesButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.FullAutoTablesButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullAutoTablesButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FullAutoTablesButton.Location = new System.Drawing.Point(150, 291);
            this.FullAutoTablesButton.Name = "FullAutoTablesButton";
            this.FullAutoTablesButton.Size = new System.Drawing.Size(107, 41);
            this.FullAutoTablesButton.TabIndex = 5;
            this.FullAutoTablesButton.TabStop = false;
            this.FullAutoTablesButton.Text = "Full Auto";
            this.FullAutoTablesButton.UseVisualStyleBackColor = true;
            // 
            // GenerateFromViewButton
            // 
            this.GenerateFromViewButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.GenerateFromViewButton.FlatAppearance.BorderSize = 2;
            this.GenerateFromViewButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.GenerateFromViewButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.GenerateFromViewButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateFromViewButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.GenerateFromViewButton.Location = new System.Drawing.Point(281, 438);
            this.GenerateFromViewButton.Name = "GenerateFromViewButton";
            this.GenerateFromViewButton.Size = new System.Drawing.Size(101, 41);
            this.GenerateFromViewButton.TabIndex = 17;
            this.GenerateFromViewButton.Text = "Generate...";
            this.GenerateFromViewButton.UseVisualStyleBackColor = true;
            // 
            // InspectViewsButton
            // 
            this.InspectViewsButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.InspectViewsButton.FlatAppearance.BorderSize = 2;
            this.InspectViewsButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.InspectViewsButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.InspectViewsButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InspectViewsButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.InspectViewsButton.Location = new System.Drawing.Point(20, 438);
            this.InspectViewsButton.Name = "InspectViewsButton";
            this.InspectViewsButton.Size = new System.Drawing.Size(107, 41);
            this.InspectViewsButton.TabIndex = 16;
            this.InspectViewsButton.Text = "Inspect";
            this.InspectViewsButton.UseVisualStyleBackColor = true;
            // 
            // GenerateFromTableButton
            // 
            this.GenerateFromTableButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.GenerateFromTableButton.FlatAppearance.BorderSize = 2;
            this.GenerateFromTableButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.GenerateFromTableButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.GenerateFromTableButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateFromTableButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.GenerateFromTableButton.Location = new System.Drawing.Point(281, 291);
            this.GenerateFromTableButton.Name = "GenerateFromTableButton";
            this.GenerateFromTableButton.Size = new System.Drawing.Size(101, 41);
            this.GenerateFromTableButton.TabIndex = 14;
            this.GenerateFromTableButton.Text = "Generate...";
            this.GenerateFromTableButton.UseVisualStyleBackColor = true;
            // 
            // InspectTablesButton
            // 
            this.InspectTablesButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.InspectTablesButton.FlatAppearance.BorderSize = 2;
            this.InspectTablesButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.InspectTablesButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.InspectTablesButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InspectTablesButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.InspectTablesButton.Location = new System.Drawing.Point(20, 291);
            this.InspectTablesButton.Name = "InspectTablesButton";
            this.InspectTablesButton.Size = new System.Drawing.Size(107, 41);
            this.InspectTablesButton.TabIndex = 13;
            this.InspectTablesButton.Text = "Inspect";
            this.InspectTablesButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label4.Location = new System.Drawing.Point(17, 377);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Views:";
            // 
            // GenerateFromStoredProcedureButton
            // 
            this.GenerateFromStoredProcedureButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.GenerateFromStoredProcedureButton.FlatAppearance.BorderSize = 2;
            this.GenerateFromStoredProcedureButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.GenerateFromStoredProcedureButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.GenerateFromStoredProcedureButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateFromStoredProcedureButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.GenerateFromStoredProcedureButton.Location = new System.Drawing.Point(281, 85);
            this.GenerateFromStoredProcedureButton.Name = "GenerateFromStoredProcedureButton";
            this.GenerateFromStoredProcedureButton.Size = new System.Drawing.Size(101, 41);
            this.GenerateFromStoredProcedureButton.TabIndex = 10;
            this.GenerateFromStoredProcedureButton.Text = "Generate...";
            this.GenerateFromStoredProcedureButton.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label5.Location = new System.Drawing.Point(339, 230);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "Filter";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Location = new System.Drawing.Point(17, 198);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Tables:";
            // 
            // ViewsDropdown
            // 
            this.ViewsDropdown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.ViewsDropdown.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ViewsDropdown.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ViewsDropdown.Location = new System.Drawing.Point(17, 403);
            this.ViewsDropdown.Name = "ViewsDropdown";
            this.ViewsDropdown.Size = new System.Drawing.Size(364, 28);
            this.ViewsDropdown.TabIndex = 15;
            // 
            // InspectProcedureButton
            // 
            this.InspectProcedureButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.InspectProcedureButton.FlatAppearance.BorderSize = 2;
            this.InspectProcedureButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.InspectProcedureButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.InspectProcedureButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InspectProcedureButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.InspectProcedureButton.Location = new System.Drawing.Point(20, 85);
            this.InspectProcedureButton.Name = "InspectProcedureButton";
            this.InspectProcedureButton.Size = new System.Drawing.Size(107, 41);
            this.InspectProcedureButton.TabIndex = 9;
            this.InspectProcedureButton.Text = "Inspect";
            this.InspectProcedureButton.UseVisualStyleBackColor = true;
            // 
            // TablesDropdown
            // 
            this.TablesDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.TablesDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.TablesDropdown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.TablesDropdown.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TablesDropdown.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.TablesDropdown.Location = new System.Drawing.Point(17, 256);
            this.TablesDropdown.Name = "TablesDropdown";
            this.TablesDropdown.Size = new System.Drawing.Size(364, 28);
            this.TablesDropdown.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(17, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Stored Procedures:";
            // 
            // StoredProceduresDropdown
            // 
            this.StoredProceduresDropdown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.StoredProceduresDropdown.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StoredProceduresDropdown.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.StoredProceduresDropdown.Location = new System.Drawing.Point(17, 51);
            this.StoredProceduresDropdown.Name = "StoredProceduresDropdown";
            this.StoredProceduresDropdown.Size = new System.Drawing.Size(364, 28);
            this.StoredProceduresDropdown.TabIndex = 8;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 845F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 845F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 555);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Controls.Add(this.InspectSqlCommandButton);
            this.groupBox1.Controls.Add(this.GenerateFromSqlCommandButton);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(994, 839);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.SqlCommandTabPage);
            this.tabControl1.Controls.Add(this.SchemaTableTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl1.Location = new System.Drawing.Point(3, 19);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 27, 3, 20);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(6, 6);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(944, 817);
            this.tabControl1.TabIndex = 19;
            // 
            // SqlCommandTabPage
            // 
            this.SqlCommandTabPage.Controls.Add(this.SqlCommandTextBox);
            this.SqlCommandTabPage.Location = new System.Drawing.Point(4, 31);
            this.SqlCommandTabPage.Name = "SqlCommandTabPage";
            this.SqlCommandTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.SqlCommandTabPage.Size = new System.Drawing.Size(936, 782);
            this.SqlCommandTabPage.TabIndex = 0;
            this.SqlCommandTabPage.Text = "SQL Command";
            this.SqlCommandTabPage.UseVisualStyleBackColor = true;
            // 
            // SqlCommandTextBox
            // 
            this.SqlCommandTextBox.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this.SqlCommandTextBox.BackBrush = null;
            this.SqlCommandTextBox.CommentPrefix = "--";
            this.SqlCommandTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.SqlCommandTextBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.SqlCommandTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.SqlCommandTextBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SqlCommandTextBox.Language = FastColoredTextBoxNS.Language.SQL;
            this.SqlCommandTextBox.LeftBracket = '(';
            this.SqlCommandTextBox.Location = new System.Drawing.Point(3, 3);
            this.SqlCommandTextBox.Name = "SqlCommandTextBox";
            this.SqlCommandTextBox.Paddings = new System.Windows.Forms.Padding(0);
            this.SqlCommandTextBox.RightBracket = ')';
            this.SqlCommandTextBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.SqlCommandTextBox.Size = new System.Drawing.Size(930, 423);
            this.SqlCommandTextBox.TabIndex = 0;
            // 
            // SchemaTableTabPage
            // 
            this.SchemaTableTabPage.Controls.Add(this.dataGridView1);
            this.SchemaTableTabPage.Location = new System.Drawing.Point(4, 31);
            this.SchemaTableTabPage.Name = "SchemaTableTabPage";
            this.SchemaTableTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.SchemaTableTabPage.Size = new System.Drawing.Size(936, 782);
            this.SchemaTableTabPage.TabIndex = 1;
            this.SchemaTableTabPage.Text = "Schema Table";
            this.SchemaTableTabPage.UseVisualStyleBackColor = true;
            // 
            // InspectSqlCommandButton
            // 
            this.InspectSqlCommandButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.InspectSqlCommandButton.FlatAppearance.BorderSize = 2;
            this.InspectSqlCommandButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.InspectSqlCommandButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.InspectSqlCommandButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InspectSqlCommandButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.InspectSqlCommandButton.Location = new System.Drawing.Point(5, 19);
            this.InspectSqlCommandButton.Name = "InspectSqlCommandButton";
            this.InspectSqlCommandButton.Size = new System.Drawing.Size(107, 41);
            this.InspectSqlCommandButton.TabIndex = 20;
            this.InspectSqlCommandButton.Text = "Inspect";
            this.InspectSqlCommandButton.UseVisualStyleBackColor = true;
            // 
            // GenerateFromSqlCommandButton
            // 
            this.GenerateFromSqlCommandButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.GenerateFromSqlCommandButton.FlatAppearance.BorderSize = 2;
            this.GenerateFromSqlCommandButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.GenerateFromSqlCommandButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.GenerateFromSqlCommandButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateFromSqlCommandButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.GenerateFromSqlCommandButton.Location = new System.Drawing.Point(119, 19);
            this.GenerateFromSqlCommandButton.Name = "GenerateFromSqlCommandButton";
            this.GenerateFromSqlCommandButton.Size = new System.Drawing.Size(101, 41);
            this.GenerateFromSqlCommandButton.TabIndex = 21;
            this.GenerateFromSqlCommandButton.Text = "Generate...";
            this.GenerateFromSqlCommandButton.UseVisualStyleBackColor = true;
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // HomeToolStripDropDownButton
            // 
            this.HomeToolStripDropDownButton.Name = "HomeToolStripDropDownButton";
            this.HomeToolStripDropDownButton.Size = new System.Drawing.Size(23, 23);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgressStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 683);
            this.statusStrip1.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 13, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1390, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ProgressStatusLabel
            // 
            this.ProgressStatusLabel.Name = "ProgressStatusLabel";
            this.ProgressStatusLabel.Size = new System.Drawing.Size(1376, 17);
            this.ProgressStatusLabel.Spring = true;
            this.ProgressStatusLabel.Text = "Ready";
            this.ProgressStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Candara", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Size = new System.Drawing.Size(930, 776);
            this.dataGridView1.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(1390, 705);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "SQL POCO Generator";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.generatorOptionsBindingSource)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.SqlCommandTabPage.ResumeLayout(false);
            this.SchemaTableTabPage.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TextBox ConnectionStringTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button InspectDatabaseButton;
		private System.Windows.Forms.TextBox OutputDirectoryTextBox;
		private System.Windows.Forms.Label OutputDirectoryLabel;
		private System.Windows.Forms.Button ChooseOutputDirectoryButton;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button ConnectionDialogButton;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.ToolStripDropDownButton HomeToolStripDropDownButton;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
		private System.Windows.Forms.ToolStripMenuItem editMenuItem;
		private System.Windows.Forms.ComboBox StoredProceduresDropdown;
		private System.Windows.Forms.Button GenerateFromStoredProcedureButton;
		private System.Windows.Forms.Button InspectProcedureButton;
		private System.Windows.Forms.Button GenerateFromTableButton;
		private System.Windows.Forms.Button InspectTablesButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox TablesDropdown;
		private System.Windows.Forms.Button GenerateFromViewButton;
		private System.Windows.Forms.Button InspectViewsButton;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox ViewsDropdown;
		private System.Windows.Forms.Button GenerateFromSqlCommandButton;
		private System.Windows.Forms.Button InspectSqlCommandButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ToolStripStatusLabel ProgressStatusLabel;
		private System.Windows.Forms.ComboBox ConnectionStringsDropDown;
        private System.Windows.Forms.ToolStripMenuItem abbreviationsToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage SqlCommandTabPage;
        private FastColoredTextBoxNS.FastColoredTextBox SqlCommandTextBox;
        private System.Windows.Forms.TabPage SchemaTableTabPage;
        private System.Windows.Forms.Button FullAutoTablesButton;
        private System.Windows.Forms.ToolStripMenuItem loadMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TableNameTextBox;
        private System.Windows.Forms.Button clearTablesFilterButton;
        private System.Windows.Forms.BindingSource generatorOptionsBindingSource;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

