namespace bark_GUI
{
    partial class PreferencesForm
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
            System.Windows.Forms.Panel panelButtons;
            System.Windows.Forms.Panel panelPaths;
            System.Windows.Forms.Panel panel3;
            System.Windows.Forms.Panel panel4;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferencesForm));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonConfirm = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            panelButtons = new System.Windows.Forms.Panel();
            panelPaths = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            panel4 = new System.Windows.Forms.Panel();
            panelButtons.SuspendLayout();
            panelPaths.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCancel.Location = new System.Drawing.Point(381, 8);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 32);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonConfirm.Location = new System.Drawing.Point(306, 8);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(75, 32);
            this.buttonConfirm.TabIndex = 1;
            this.buttonConfirm.Text = "OK";
            this.buttonConfirm.UseVisualStyleBackColor = true;
            this.buttonConfirm.Click += new System.EventHandler(this.buttonConfirm_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Paths";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(444, 239);
            this.flowLayoutPanel1.TabIndex = 35;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(this.buttonConfirm);
            panelButtons.Controls.Add(this.buttonCancel);
            panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelButtons.Location = new System.Drawing.Point(5, 284);
            panelButtons.Name = "panelButtons";
            panelButtons.Padding = new System.Windows.Forms.Padding(8);
            panelButtons.Size = new System.Drawing.Size(464, 48);
            panelButtons.TabIndex = 36;
            // 
            // panelPaths
            // 
            panelPaths.Controls.Add(panel4);
            panelPaths.Controls.Add(panel3);
            panelPaths.Dock = System.Windows.Forms.DockStyle.Fill;
            panelPaths.Location = new System.Drawing.Point(5, 5);
            panelPaths.Name = "panelPaths";
            panelPaths.Padding = new System.Windows.Forms.Padding(10);
            panelPaths.Size = new System.Drawing.Size(464, 279);
            panelPaths.TabIndex = 37;
            // 
            // panel3
            // 
            panel3.Controls.Add(this.label3);
            panel3.Dock = System.Windows.Forms.DockStyle.Top;
            panel3.Location = new System.Drawing.Point(10, 10);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(444, 20);
            panel3.TabIndex = 36;
            // 
            // panel4
            // 
            panel4.Controls.Add(this.flowLayoutPanel1);
            panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            panel4.Location = new System.Drawing.Point(10, 30);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(444, 239);
            panel4.TabIndex = 37;
            // 
            // preferencesForm
            // 
            this.AcceptButton = this.buttonConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(474, 337);
            this.Controls.Add(panelPaths);
            this.Controls.Add(panelButtons);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(475, 375);
            this.Name = "PreferencesForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferences";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.preferencesForm_FormClosed);
            panelButtons.ResumeLayout(false);
            panelPaths.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonConfirm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}