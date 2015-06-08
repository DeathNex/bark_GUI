namespace bark_GUI.CustomControls
{
    partial class ControlGroup
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelGroup = new System.Windows.Forms.Label();
            this.panelGroup = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // labelGroup
            // 
            this.labelGroup.AutoSize = true;
            this.labelGroup.BackColor = System.Drawing.Color.Lavender;
            this.labelGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.labelGroup.ForeColor = System.Drawing.Color.Black;
            this.labelGroup.Location = new System.Drawing.Point(7, 7);
            this.labelGroup.Margin = new System.Windows.Forms.Padding(3);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(53, 17);
            this.labelGroup.TabIndex = 0;
            this.labelGroup.Text = "Group";
            // 
            // panelGroup
            // 
            this.panelGroup.AutoSize = true;
            this.panelGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelGroup.BackColor = System.Drawing.Color.White;
            this.panelGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGroup.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelGroup.Location = new System.Drawing.Point(6, 32);
            this.panelGroup.Margin = new System.Windows.Forms.Padding(2);
            this.panelGroup.MinimumSize = new System.Drawing.Size(400, 20);
            this.panelGroup.Name = "panelGroup";
            this.panelGroup.Size = new System.Drawing.Size(400, 20);
            this.panelGroup.TabIndex = 1;
            // 
            // ControlGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Lavender;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panelGroup);
            this.Controls.Add(this.labelGroup);
            this.ForeColor = System.Drawing.Color.Black;
            this.MinimumSize = new System.Drawing.Size(400, 40);
            this.Name = "ControlGroup";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(411, 57);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.FlowLayoutPanel panelGroup;
    }
}
