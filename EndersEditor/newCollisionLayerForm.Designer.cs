namespace EndersEditor
{
    partial class newCollisionLayerForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.collName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.collWidth = new System.Windows.Forms.TextBox();
            this.collHeight = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.OKCollpressed = new System.Windows.Forms.Button();
            this.cancelPressed = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // collName
            // 
            this.collName.Location = new System.Drawing.Point(44, 23);
            this.collName.Name = "collName";
            this.collName.Size = new System.Drawing.Size(268, 20);
            this.collName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Width";
            // 
            // collWidth
            // 
            this.collWidth.Location = new System.Drawing.Point(44, 54);
            this.collWidth.Name = "collWidth";
            this.collWidth.Size = new System.Drawing.Size(55, 20);
            this.collWidth.TabIndex = 3;
            // 
            // collHeight
            // 
            this.collHeight.Location = new System.Drawing.Point(44, 80);
            this.collHeight.Name = "collHeight";
            this.collHeight.Size = new System.Drawing.Size(55, 20);
            this.collHeight.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Height";
            // 
            // OKCollpressed
            // 
            this.OKCollpressed.Location = new System.Drawing.Point(182, 122);
            this.OKCollpressed.Name = "OKCollpressed";
            this.OKCollpressed.Size = new System.Drawing.Size(75, 23);
            this.OKCollpressed.TabIndex = 6;
            this.OKCollpressed.Text = "OK";
            this.OKCollpressed.UseVisualStyleBackColor = true;
            this.OKCollpressed.Click += new System.EventHandler(this.OKpressed_Click);
            // 
            // cancelPressed
            // 
            this.cancelPressed.Location = new System.Drawing.Point(263, 122);
            this.cancelPressed.Name = "cancelPressed";
            this.cancelPressed.Size = new System.Drawing.Size(75, 23);
            this.cancelPressed.TabIndex = 7;
            this.cancelPressed.Text = "Cancel";
            this.cancelPressed.UseVisualStyleBackColor = true;
            this.cancelPressed.Click += new System.EventHandler(this.cancelPressed_Click);
            // 
            // newCollisionLayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 157);
            this.Controls.Add(this.cancelPressed);
            this.Controls.Add(this.OKCollpressed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.collHeight);
            this.Controls.Add(this.collWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.collName);
            this.Controls.Add(this.label1);
            this.Name = "newCollisionLayerForm";
            this.Text = "newCollisionLayerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cancelPressed;
        public System.Windows.Forms.Button OKCollpressed;
        public System.Windows.Forms.TextBox collName;
        public System.Windows.Forms.TextBox collWidth;
        public System.Windows.Forms.TextBox collHeight;
    }
}