using DomainTools.Applications.Properties;

namespace DomainTools.Applications
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
            DomainTools.Applications.Properties.Settings settings1 = new DomainTools.Applications.Properties.Settings();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.optionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.abbreviationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectionStringTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.InspectDatabaseButton = new System.Windows.Forms.Button();
            this.OutputDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.OutputDirectoryLabel = new System.Windows.Forms.Label();
            this.ConnectionDialogButton = new System.Windows.Forms.Button();
            this.ChooseOutputDirectoryButton = new System.Windows.Forms.Button();
            this.ConnectionStringsDropDown = new System.Windows.Forms.ComboBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.GenerateFromViewButton = new System.Windows.Forms.Button();
            this.InspectViewsButton = new System.Windows.Forms.Button();
            this.GenerateFromTableButton = new System.Windows.Forms.Button();
            this.InspectTablesButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.GenerateFromStoredProcedureButton = new System.Windows.Forms.Button();
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.InspectSqlCommandButton = new System.Windows.Forms.Button();
            this.GenerateFromSqlCommandButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.HomeToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProgressStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SqlCommandTabPage.SuspendLayout();
            this.SchemaTableTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1236, 780);
            this.splitContainer1.SplitterDistance = 143;
            this.splitContainer1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(1236, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem1,
            this.abbreviationsToolStripMenuItem});
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(62, 22);
            this.toolStripDropDownButton1.Text = "&Settings";
            // 
            // optionsToolStripMenuItem1
            // 
            this.optionsToolStripMenuItem1.Name = "optionsToolStripMenuItem1";
            this.optionsToolStripMenuItem1.Size = new System.Drawing.Size(147, 22);
            this.optionsToolStripMenuItem1.Text = "&Options";
            this.optionsToolStripMenuItem1.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // abbreviationsToolStripMenuItem
            // 
            this.abbreviationsToolStripMenuItem.Name = "abbreviationsToolStripMenuItem";
            this.abbreviationsToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.abbreviationsToolStripMenuItem.Text = "&Abbreviations";
            this.abbreviationsToolStripMenuItem.ToolTipText = "Provide translations for known abbreviations and acronyms used in column and tabl" +
    "e names";
            this.abbreviationsToolStripMenuItem.Click += new System.EventHandler(this.abbreviationsToolStripMenuItem_Click);
            // 
            // ConnectionStringTextBox
            // 
            settings1.AdditionalCollapseTokens = "new;_";
            settings1.AdditionalTokenFind = "Base";
            settings1.AdditionalTokenReplace = "ExtensionBase";
            settings1.AppendDbSchemaToNamepaces = true;
            settings1.BaseNamespace = "HollisCobb.Payments.Cim";
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
            settings1.SavedConnectionStrings = null;
            settings1.SettingsKey = "";
            settings1.SqlConnectionString = "Data Source=.;Initial Catalog=HollisCobbPayments;Integrated Security=True";
            settings1.SqlTextCommand = "";
            settings1.UseAutomaticProperties = true;
            settings1.Usings = "";
            this.ConnectionStringTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", settings1, "SqlConnectionString", true));
            this.ConnectionStringTextBox.Location = new System.Drawing.Point(137, 85);
            this.ConnectionStringTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ConnectionStringTextBox.Name = "ConnectionStringTextBox";
            this.ConnectionStringTextBox.Size = new System.Drawing.Size(633, 20);
            this.ConnectionStringTextBox.TabIndex = 5;
            this.ConnectionStringTextBox.Text = settings1.SqlConnectionString;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Connection String";
            // 
            // InspectDatabaseButton
            // 
            this.InspectDatabaseButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.InspectDatabaseButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.InspectDatabaseButton.Image = global::DomainTools.Applications.Properties.Resources.search;
            this.InspectDatabaseButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.InspectDatabaseButton.Location = new System.Drawing.Point(827, 85);
            this.InspectDatabaseButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.InspectDatabaseButton.Name = "InspectDatabaseButton";
            this.InspectDatabaseButton.Size = new System.Drawing.Size(112, 25);
            this.InspectDatabaseButton.TabIndex = 3;
            this.InspectDatabaseButton.Text = "Inspect DB";
            this.InspectDatabaseButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.InspectDatabaseButton.UseVisualStyleBackColor = true;
            this.InspectDatabaseButton.Click += new System.EventHandler(this.InspectDatabaseButton_Click);
            // 
            // OutputDirectoryTextBox
            // 
            this.OutputDirectoryTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", settings1, "OutputDirectory", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.OutputDirectoryTextBox.Location = new System.Drawing.Point(137, 46);
            this.OutputDirectoryTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.OutputDirectoryTextBox.Name = "OutputDirectoryTextBox";
            this.OutputDirectoryTextBox.Size = new System.Drawing.Size(655, 20);
            this.OutputDirectoryTextBox.TabIndex = 2;
            this.OutputDirectoryTextBox.Text = settings1.OutputDirectory;
            // 
            // OutputDirectoryLabel
            // 
            this.OutputDirectoryLabel.AutoSize = true;
            this.OutputDirectoryLabel.Location = new System.Drawing.Point(11, 46);
            this.OutputDirectoryLabel.Name = "OutputDirectoryLabel";
            this.OutputDirectoryLabel.Size = new System.Drawing.Size(85, 13);
            this.OutputDirectoryLabel.TabIndex = 1;
            this.OutputDirectoryLabel.Text = "OutputDirectory";
            // 
            // ConnectionDialogButton
            // 
            this.ConnectionDialogButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ConnectionDialogButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ConnectionDialogButton.Location = new System.Drawing.Point(797, 84);
            this.ConnectionDialogButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ConnectionDialogButton.Name = "ConnectionDialogButton";
            this.ConnectionDialogButton.Size = new System.Drawing.Size(25, 23);
            this.ConnectionDialogButton.TabIndex = 0;
            this.ConnectionDialogButton.Text = "...";
            this.ConnectionDialogButton.UseVisualStyleBackColor = true;
            // 
            // ChooseOutputDirectoryButton
            // 
            this.ChooseOutputDirectoryButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ChooseOutputDirectoryButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ChooseOutputDirectoryButton.Location = new System.Drawing.Point(797, 46);
            this.ChooseOutputDirectoryButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ChooseOutputDirectoryButton.Name = "ChooseOutputDirectoryButton";
            this.ChooseOutputDirectoryButton.Size = new System.Drawing.Size(25, 23);
            this.ChooseOutputDirectoryButton.TabIndex = 0;
            this.ChooseOutputDirectoryButton.Text = "...";
            this.ChooseOutputDirectoryButton.UseVisualStyleBackColor = true;
            this.ChooseOutputDirectoryButton.Click += new System.EventHandler(this.ChooseOutputDirectoryButton_Click);
            // 
            // ConnectionStringsDropDown
            // 
            this.ConnectionStringsDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ConnectionStringsDropDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConnectionStringsDropDown.FormattingEnabled = true;
            this.ConnectionStringsDropDown.Location = new System.Drawing.Point(137, 85);
            this.ConnectionStringsDropDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ConnectionStringsDropDown.Name = "ConnectionStringsDropDown";
            this.ConnectionStringsDropDown.Size = new System.Drawing.Size(655, 20);
            this.ConnectionStringsDropDown.TabIndex = 7;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.GenerateFromViewButton);
            this.splitContainer2.Panel1.Controls.Add(this.InspectViewsButton);
            this.splitContainer2.Panel1.Controls.Add(this.GenerateFromTableButton);
            this.splitContainer2.Panel1.Controls.Add(this.InspectTablesButton);
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            this.splitContainer2.Panel1.Controls.Add(this.GenerateFromStoredProcedureButton);
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
            this.splitContainer2.Size = new System.Drawing.Size(1236, 633);
            this.splitContainer2.SplitterDistance = 385;
            this.splitContainer2.TabIndex = 0;
            // 
            // GenerateFromViewButton
            // 
            this.GenerateFromViewButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.GenerateFromViewButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.GenerateFromViewButton.Location = new System.Drawing.Point(241, 261);
            this.GenerateFromViewButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GenerateFromViewButton.Name = "GenerateFromViewButton";
            this.GenerateFromViewButton.Size = new System.Drawing.Size(87, 31);
            this.GenerateFromViewButton.TabIndex = 4;
            this.GenerateFromViewButton.Text = "Generate...";
            this.GenerateFromViewButton.UseVisualStyleBackColor = true;
            // 
            // InspectViewsButton
            // 
            this.InspectViewsButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.InspectViewsButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.InspectViewsButton.Location = new System.Drawing.Point(17, 261);
            this.InspectViewsButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.InspectViewsButton.Name = "InspectViewsButton";
            this.InspectViewsButton.Size = new System.Drawing.Size(92, 31);
            this.InspectViewsButton.TabIndex = 3;
            this.InspectViewsButton.Text = "Inspect";
            this.InspectViewsButton.UseVisualStyleBackColor = true;
            // 
            // GenerateFromTableButton
            // 
            this.GenerateFromTableButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.GenerateFromTableButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.GenerateFromTableButton.Location = new System.Drawing.Point(241, 165);
            this.GenerateFromTableButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GenerateFromTableButton.Name = "GenerateFromTableButton";
            this.GenerateFromTableButton.Size = new System.Drawing.Size(87, 31);
            this.GenerateFromTableButton.TabIndex = 4;
            this.GenerateFromTableButton.Text = "Generate...";
            this.GenerateFromTableButton.UseVisualStyleBackColor = true;
            // 
            // InspectTablesButton
            // 
            this.InspectTablesButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.InspectTablesButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.InspectTablesButton.Location = new System.Drawing.Point(17, 165);
            this.InspectTablesButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.InspectTablesButton.Name = "InspectTablesButton";
            this.InspectTablesButton.Size = new System.Drawing.Size(92, 31);
            this.InspectTablesButton.TabIndex = 3;
            this.InspectTablesButton.Text = "Inspect";
            this.InspectTablesButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 215);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Views:";
            // 
            // GenerateFromStoredProcedureButton
            // 
            this.GenerateFromStoredProcedureButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.GenerateFromStoredProcedureButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.GenerateFromStoredProcedureButton.Location = new System.Drawing.Point(241, 64);
            this.GenerateFromStoredProcedureButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GenerateFromStoredProcedureButton.Name = "GenerateFromStoredProcedureButton";
            this.GenerateFromStoredProcedureButton.Size = new System.Drawing.Size(87, 31);
            this.GenerateFromStoredProcedureButton.TabIndex = 4;
            this.GenerateFromStoredProcedureButton.Text = "Generate...";
            this.GenerateFromStoredProcedureButton.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Tables:";
            // 
            // ViewsDropdown
            // 
            this.ViewsDropdown.FormattingEnabled = true;
            this.ViewsDropdown.Location = new System.Drawing.Point(15, 235);
            this.ViewsDropdown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ViewsDropdown.Name = "ViewsDropdown";
            this.ViewsDropdown.Size = new System.Drawing.Size(313, 20);
            this.ViewsDropdown.TabIndex = 0;
            // 
            // InspectProcedureButton
            // 
            this.InspectProcedureButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.InspectProcedureButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.InspectProcedureButton.Location = new System.Drawing.Point(17, 64);
            this.InspectProcedureButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.InspectProcedureButton.Name = "InspectProcedureButton";
            this.InspectProcedureButton.Size = new System.Drawing.Size(92, 31);
            this.InspectProcedureButton.TabIndex = 3;
            this.InspectProcedureButton.Text = "Inspect";
            this.InspectProcedureButton.UseVisualStyleBackColor = true;
            // 
            // TablesDropdown
            // 
            this.TablesDropdown.FormattingEnabled = true;
            this.TablesDropdown.Location = new System.Drawing.Point(15, 139);
            this.TablesDropdown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TablesDropdown.Name = "TablesDropdown";
            this.TablesDropdown.Size = new System.Drawing.Size(313, 20);
            this.TablesDropdown.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Stored Procedures:";
            // 
            // StoredProceduresDropdown
            // 
            this.StoredProceduresDropdown.FormattingEnabled = true;
            this.StoredProceduresDropdown.Location = new System.Drawing.Point(15, 38);
            this.StoredProceduresDropdown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.StoredProceduresDropdown.Name = "StoredProceduresDropdown";
            this.StoredProceduresDropdown.Size = new System.Drawing.Size(313, 20);
            this.StoredProceduresDropdown.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 633F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 633F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(847, 633);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Controls.Add(this.InspectSqlCommandButton);
            this.groupBox1.Controls.Add(this.GenerateFromSqlCommandButton);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(841, 629);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.SqlCommandTabPage);
            this.tabControl1.Controls.Add(this.SchemaTableTabPage);
            this.tabControl1.Location = new System.Drawing.Point(3, 53);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 20, 3, 15);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(841, 563);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 21;
            // 
            // SqlCommandTabPage
            // 
            this.SqlCommandTabPage.Controls.Add(this.SqlCommandTextBox);
            this.SqlCommandTabPage.Location = new System.Drawing.Point(4, 21);
            this.SqlCommandTabPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SqlCommandTabPage.Name = "SqlCommandTabPage";
            this.SqlCommandTabPage.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SqlCommandTabPage.Size = new System.Drawing.Size(833, 538);
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
            this.SqlCommandTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SqlCommandTextBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SqlCommandTextBox.Language = FastColoredTextBoxNS.Language.SQL;
            this.SqlCommandTextBox.LeftBracket = '(';
            this.SqlCommandTextBox.Location = new System.Drawing.Point(3, 2);
            this.SqlCommandTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SqlCommandTextBox.Name = "SqlCommandTextBox";
            this.SqlCommandTextBox.Paddings = new System.Windows.Forms.Padding(0);
            this.SqlCommandTextBox.RightBracket = ')';
            this.SqlCommandTextBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.SqlCommandTextBox.Size = new System.Drawing.Size(827, 534);
            this.SqlCommandTextBox.TabIndex = 0;
            // 
            // SchemaTableTabPage
            // 
            this.SchemaTableTabPage.Controls.Add(this.dataGridView1);
            this.SchemaTableTabPage.Location = new System.Drawing.Point(4, 22);
            this.SchemaTableTabPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SchemaTableTabPage.Name = "SchemaTableTabPage";
            this.SchemaTableTabPage.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SchemaTableTabPage.Size = new System.Drawing.Size(833, 537);
            this.SchemaTableTabPage.TabIndex = 1;
            this.SchemaTableTabPage.Text = "Schema Table";
            this.SchemaTableTabPage.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 2);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Candara", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Size = new System.Drawing.Size(827, 533);
            this.dataGridView1.TabIndex = 0;
            // 
            // InspectSqlCommandButton
            // 
            this.InspectSqlCommandButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.InspectSqlCommandButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.InspectSqlCommandButton.Location = new System.Drawing.Point(4, 14);
            this.InspectSqlCommandButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.InspectSqlCommandButton.Name = "InspectSqlCommandButton";
            this.InspectSqlCommandButton.Size = new System.Drawing.Size(92, 31);
            this.InspectSqlCommandButton.TabIndex = 3;
            this.InspectSqlCommandButton.Text = "Inspect";
            this.InspectSqlCommandButton.UseVisualStyleBackColor = true;
            // 
            // GenerateFromSqlCommandButton
            // 
            this.GenerateFromSqlCommandButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.GenerateFromSqlCommandButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.GenerateFromSqlCommandButton.Location = new System.Drawing.Point(102, 14);
            this.GenerateFromSqlCommandButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GenerateFromSqlCommandButton.Name = "GenerateFromSqlCommandButton";
            this.GenerateFromSqlCommandButton.Size = new System.Drawing.Size(87, 31);
            this.GenerateFromSqlCommandButton.TabIndex = 4;
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 758);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 11, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1236, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ProgressStatusLabel
            // 
            this.ProgressStatusLabel.Name = "ProgressStatusLabel";
            this.ProgressStatusLabel.Size = new System.Drawing.Size(1224, 17);
            this.ProgressStatusLabel.Spring = true;
            this.ProgressStatusLabel.Text = "Ready";
            this.ProgressStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1236, 780);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "SQL POCO Generator";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem1;
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
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

