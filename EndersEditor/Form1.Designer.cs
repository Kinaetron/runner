namespace EndersEditor
{
    partial class Form1
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllLayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.entitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.associateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.withNumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contentPathTextBox = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.drawRadioButton = new System.Windows.Forms.RadioButton();
            this.eraseRadioButton = new System.Windows.Forms.RadioButton();
            this.TilelayerListBox = new System.Windows.Forms.ListBox();
            this.AddTileLayerButton = new System.Windows.Forms.Button();
            this.removeTileLayerButton = new System.Windows.Forms.Button();
            this.textureListBox = new System.Windows.Forms.ListBox();
            this.addTextureButton = new System.Windows.Forms.Button();
            this.removeTextureButton = new System.Windows.Forms.Button();
            this.texturePreviewBox = new System.Windows.Forms.PictureBox();
            this.fillCheckBox = new System.Windows.Forms.CheckBox();
            this.collisionTiles = new System.Windows.Forms.ComboBox();
            this.AddCollLayerButton = new System.Windows.Forms.Button();
            this.removeCollLayerButton = new System.Windows.Forms.Button();
            this.CollLayerListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AssociateBox = new System.Windows.Forms.ComboBox();
            this.LevelName = new System.Windows.Forms.TextBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.tileDisplay1 = new EndersEditor.TileDisplay();
            this.NameLevel = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreviewBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.LargeChange = 2;
            this.hScrollBar1.Location = new System.Drawing.Point(12, 750);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(1280, 27);
            this.hScrollBar1.TabIndex = 1;
            this.hScrollBar1.Visible = false;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.LargeChange = 2;
            this.vScrollBar1.Location = new System.Drawing.Point(1295, 27);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(23, 720);
            this.vScrollBar1.TabIndex = 2;
            this.vScrollBar1.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.displayToolStripMenuItem,
            this.entitiesToolStripMenuItem,
            this.associateToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1596, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save ";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAllLayersToolStripMenuItem,
            this.showGridToolStripMenuItem,
            this.showNodesToolStripMenuItem});
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.displayToolStripMenuItem.Text = "Display";
            // 
            // showAllLayersToolStripMenuItem
            // 
            this.showAllLayersToolStripMenuItem.CheckOnClick = true;
            this.showAllLayersToolStripMenuItem.Name = "showAllLayersToolStripMenuItem";
            this.showAllLayersToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.showAllLayersToolStripMenuItem.Text = "Show All Layers";
            // 
            // showGridToolStripMenuItem
            // 
            this.showGridToolStripMenuItem.Checked = true;
            this.showGridToolStripMenuItem.CheckOnClick = true;
            this.showGridToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showGridToolStripMenuItem.Name = "showGridToolStripMenuItem";
            this.showGridToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.showGridToolStripMenuItem.Text = "Show Grid";
            // 
            // showNodesToolStripMenuItem
            // 
            this.showNodesToolStripMenuItem.CheckOnClick = true;
            this.showNodesToolStripMenuItem.Name = "showNodesToolStripMenuItem";
            this.showNodesToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.showNodesToolStripMenuItem.Text = "Show Nodes";
            // 
            // entitiesToolStripMenuItem
            // 
            this.entitiesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageToolStripMenuItem});
            this.entitiesToolStripMenuItem.Name = "entitiesToolStripMenuItem";
            this.entitiesToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.entitiesToolStripMenuItem.Text = "Entities";
            // 
            // manageToolStripMenuItem
            // 
            this.manageToolStripMenuItem.Name = "manageToolStripMenuItem";
            this.manageToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.manageToolStripMenuItem.Text = "Manage";
            this.manageToolStripMenuItem.Click += new System.EventHandler(this.manageToolStripMenuItem_Click);
            // 
            // associateToolStripMenuItem
            // 
            this.associateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.withNumberToolStripMenuItem});
            this.associateToolStripMenuItem.Name = "associateToolStripMenuItem";
            this.associateToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.associateToolStripMenuItem.Text = "Associate";
            // 
            // withNumberToolStripMenuItem
            // 
            this.withNumberToolStripMenuItem.Name = "withNumberToolStripMenuItem";
            this.withNumberToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.withNumberToolStripMenuItem.Text = "With Number";
            this.withNumberToolStripMenuItem.Click += new System.EventHandler(this.withNumberToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // contentPathTextBox
            // 
            this.contentPathTextBox.Location = new System.Drawing.Point(1324, 78);
            this.contentPathTextBox.Name = "contentPathTextBox";
            this.contentPathTextBox.ReadOnly = true;
            this.contentPathTextBox.Size = new System.Drawing.Size(263, 20);
            this.contentPathTextBox.TabIndex = 4;
            // 
            // drawRadioButton
            // 
            this.drawRadioButton.AutoSize = true;
            this.drawRadioButton.Checked = true;
            this.drawRadioButton.Location = new System.Drawing.Point(1329, 104);
            this.drawRadioButton.Name = "drawRadioButton";
            this.drawRadioButton.Size = new System.Drawing.Size(50, 17);
            this.drawRadioButton.TabIndex = 6;
            this.drawRadioButton.TabStop = true;
            this.drawRadioButton.Text = "Draw";
            this.drawRadioButton.UseVisualStyleBackColor = true;
            // 
            // eraseRadioButton
            // 
            this.eraseRadioButton.AutoSize = true;
            this.eraseRadioButton.Location = new System.Drawing.Point(1329, 127);
            this.eraseRadioButton.Name = "eraseRadioButton";
            this.eraseRadioButton.Size = new System.Drawing.Size(52, 17);
            this.eraseRadioButton.TabIndex = 6;
            this.eraseRadioButton.Text = "Erase";
            this.eraseRadioButton.UseVisualStyleBackColor = true;
            // 
            // TilelayerListBox
            // 
            this.TilelayerListBox.FormattingEnabled = true;
            this.TilelayerListBox.Location = new System.Drawing.Point(1328, 171);
            this.TilelayerListBox.Name = "TilelayerListBox";
            this.TilelayerListBox.Size = new System.Drawing.Size(262, 82);
            this.TilelayerListBox.TabIndex = 7;
            this.TilelayerListBox.SelectedIndexChanged += new System.EventHandler(this.TilelayerListBox_SelectedIndexChanged);
            // 
            // AddTileLayerButton
            // 
            this.AddTileLayerButton.Location = new System.Drawing.Point(1376, 259);
            this.AddTileLayerButton.Name = "AddTileLayerButton";
            this.AddTileLayerButton.Size = new System.Drawing.Size(75, 23);
            this.AddTileLayerButton.TabIndex = 8;
            this.AddTileLayerButton.Text = "Add";
            this.AddTileLayerButton.UseVisualStyleBackColor = true;
            this.AddTileLayerButton.Click += new System.EventHandler(this.AddLayerButton_Click);
            // 
            // removeTileLayerButton
            // 
            this.removeTileLayerButton.Location = new System.Drawing.Point(1464, 259);
            this.removeTileLayerButton.Name = "removeTileLayerButton";
            this.removeTileLayerButton.Size = new System.Drawing.Size(75, 23);
            this.removeTileLayerButton.TabIndex = 8;
            this.removeTileLayerButton.Text = "Remove";
            this.removeTileLayerButton.UseVisualStyleBackColor = true;
            this.removeTileLayerButton.Click += new System.EventHandler(this.removeLayerButton_Click);
            // 
            // textureListBox
            // 
            this.textureListBox.FormattingEnabled = true;
            this.textureListBox.Location = new System.Drawing.Point(1329, 418);
            this.textureListBox.Name = "textureListBox";
            this.textureListBox.Size = new System.Drawing.Size(258, 82);
            this.textureListBox.TabIndex = 7;
            this.textureListBox.SelectedIndexChanged += new System.EventHandler(this.textureListBox_SelectedIndexChanged);
            // 
            // addTextureButton
            // 
            this.addTextureButton.Location = new System.Drawing.Point(1376, 506);
            this.addTextureButton.Name = "addTextureButton";
            this.addTextureButton.Size = new System.Drawing.Size(75, 23);
            this.addTextureButton.TabIndex = 8;
            this.addTextureButton.Text = "Add";
            this.addTextureButton.UseVisualStyleBackColor = true;
            this.addTextureButton.Click += new System.EventHandler(this.addTextureButton_Click);
            // 
            // removeTextureButton
            // 
            this.removeTextureButton.Location = new System.Drawing.Point(1464, 506);
            this.removeTextureButton.Name = "removeTextureButton";
            this.removeTextureButton.Size = new System.Drawing.Size(75, 23);
            this.removeTextureButton.TabIndex = 8;
            this.removeTextureButton.Text = "Remove";
            this.removeTextureButton.UseVisualStyleBackColor = true;
            this.removeTextureButton.Click += new System.EventHandler(this.removeTextureButton_Click);
            // 
            // texturePreviewBox
            // 
            this.texturePreviewBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.texturePreviewBox.Location = new System.Drawing.Point(1373, 533);
            this.texturePreviewBox.Name = "texturePreviewBox";
            this.texturePreviewBox.Size = new System.Drawing.Size(166, 145);
            this.texturePreviewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.texturePreviewBox.TabIndex = 9;
            this.texturePreviewBox.TabStop = false;
            // 
            // fillCheckBox
            // 
            this.fillCheckBox.AutoSize = true;
            this.fillCheckBox.Location = new System.Drawing.Point(1457, 105);
            this.fillCheckBox.Name = "fillCheckBox";
            this.fillCheckBox.Size = new System.Drawing.Size(38, 17);
            this.fillCheckBox.TabIndex = 10;
            this.fillCheckBox.Text = "Fill";
            this.fillCheckBox.UseVisualStyleBackColor = true;
            // 
            // collisionTiles
            // 
            this.collisionTiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.collisionTiles.FormattingEnabled = true;
            this.collisionTiles.Location = new System.Drawing.Point(384, 3);
            this.collisionTiles.Name = "collisionTiles";
            this.collisionTiles.Size = new System.Drawing.Size(121, 21);
            this.collisionTiles.TabIndex = 12;
            // 
            // AddCollLayerButton
            // 
            this.AddCollLayerButton.Location = new System.Drawing.Point(1376, 389);
            this.AddCollLayerButton.Name = "AddCollLayerButton";
            this.AddCollLayerButton.Size = new System.Drawing.Size(75, 23);
            this.AddCollLayerButton.TabIndex = 13;
            this.AddCollLayerButton.Text = "Add";
            this.AddCollLayerButton.UseVisualStyleBackColor = true;
            this.AddCollLayerButton.Click += new System.EventHandler(this.AddCollLayerButton_Click);
            // 
            // removeCollLayerButton
            // 
            this.removeCollLayerButton.Location = new System.Drawing.Point(1464, 389);
            this.removeCollLayerButton.Name = "removeCollLayerButton";
            this.removeCollLayerButton.Size = new System.Drawing.Size(75, 23);
            this.removeCollLayerButton.TabIndex = 14;
            this.removeCollLayerButton.Text = "Remove";
            this.removeCollLayerButton.UseVisualStyleBackColor = true;
            this.removeCollLayerButton.Click += new System.EventHandler(this.removeCollLayerButton_Click);
            // 
            // CollLayerListBox
            // 
            this.CollLayerListBox.FormattingEnabled = true;
            this.CollLayerListBox.Location = new System.Drawing.Point(1327, 301);
            this.CollLayerListBox.Name = "CollLayerListBox";
            this.CollLayerListBox.Size = new System.Drawing.Size(260, 82);
            this.CollLayerListBox.TabIndex = 15;
            this.CollLayerListBox.SelectedIndexChanged += new System.EventHandler(this.CollLayerListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1324, 285);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "CollisonLayers";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1328, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "TileLayers";
            // 
            // AssociateBox
            // 
            this.AssociateBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AssociateBox.FormattingEnabled = true;
            this.AssociateBox.Location = new System.Drawing.Point(236, 3);
            this.AssociateBox.Name = "AssociateBox";
            this.AssociateBox.Size = new System.Drawing.Size(121, 21);
            this.AssociateBox.TabIndex = 12;
            // 
            // LevelName
            // 
            this.LevelName.Location = new System.Drawing.Point(533, 4);
            this.LevelName.Name = "LevelName";
            this.LevelName.ReadOnly = true;
            this.LevelName.Size = new System.Drawing.Size(117, 20);
            this.LevelName.TabIndex = 4;
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(1324, 27);
            this.trackBar1.Maximum = 20;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(266, 45);
            this.trackBar1.TabIndex = 18;
            this.trackBar1.Value = 10;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // tileDisplay1
            // 
            this.tileDisplay1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tileDisplay1.Location = new System.Drawing.Point(12, 27);
            this.tileDisplay1.Name = "tileDisplay1";
            this.tileDisplay1.Size = new System.Drawing.Size(1280, 720);
            this.tileDisplay1.TabIndex = 0;
            this.tileDisplay1.Text = "tileDisplay1";
            this.tileDisplay1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tileDisplay1_MouseDown);
            this.tileDisplay1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tileDisplay1_MouseUp);
            // 
            // NameLevel
            // 
            this.NameLevel.Location = new System.Drawing.Point(921, 3);
            this.NameLevel.Name = "NameLevel";
            this.NameLevel.Size = new System.Drawing.Size(192, 20);
            this.NameLevel.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(851, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Level Name";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1596, 844);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.NameLevel);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CollLayerListBox);
            this.Controls.Add(this.removeCollLayerButton);
            this.Controls.Add(this.AddCollLayerButton);
            this.Controls.Add(this.AssociateBox);
            this.Controls.Add(this.collisionTiles);
            this.Controls.Add(this.fillCheckBox);
            this.Controls.Add(this.texturePreviewBox);
            this.Controls.Add(this.removeTextureButton);
            this.Controls.Add(this.removeTileLayerButton);
            this.Controls.Add(this.addTextureButton);
            this.Controls.Add(this.AddTileLayerButton);
            this.Controls.Add(this.textureListBox);
            this.Controls.Add(this.TilelayerListBox);
            this.Controls.Add(this.eraseRadioButton);
            this.Controls.Add(this.drawRadioButton);
            this.Controls.Add(this.LevelName);
            this.Controls.Add(this.contentPathTextBox);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.tileDisplay1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Enders Tile Editor ";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreviewBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TileDisplay tileDisplay1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox contentPathTextBox;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.RadioButton drawRadioButton;
        private System.Windows.Forms.RadioButton eraseRadioButton;
        private System.Windows.Forms.ListBox TilelayerListBox;
        private System.Windows.Forms.Button AddTileLayerButton;
        private System.Windows.Forms.Button removeTileLayerButton;
        private System.Windows.Forms.ListBox textureListBox;
        private System.Windows.Forms.Button addTextureButton;
        private System.Windows.Forms.Button removeTextureButton;
        private System.Windows.Forms.PictureBox texturePreviewBox;
        private System.Windows.Forms.CheckBox fillCheckBox;
        private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllLayersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showGridToolStripMenuItem;
        private System.Windows.Forms.ComboBox collisionTiles;
        private System.Windows.Forms.ToolStripMenuItem entitiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageToolStripMenuItem;
        private System.Windows.Forms.Button AddCollLayerButton;
        private System.Windows.Forms.Button removeCollLayerButton;
        private System.Windows.Forms.ListBox CollLayerListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem associateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem withNumberToolStripMenuItem;
        private System.Windows.Forms.ComboBox AssociateBox;
        private System.Windows.Forms.ToolStripMenuItem showNodesToolStripMenuItem;
        private System.Windows.Forms.TextBox LevelName;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox NameLevel;
    }
}

