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
            this.labelGroup.Location = new System.Drawing.Point(9, 9);
            this.labelGroup.Margin = new System.Windows.Forms.Padding(4);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(60, 20);
            this.labelGroup.TabIndex = 0;
            this.labelGroup.Text = "Group";
            // 
            // panelGroup
            // 
            this.panelGroup.AutoSize = true;
            this.panelGroup.BackColor = System.Drawing.Color.White;
            this.panelGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGroup.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelGroup.Location = new System.Drawing.Point(8, 40);
            this.panelGroup.Name = "panelGroup";
            this.panelGroup.Size = new System.Drawing.Size(540, 40);
            this.panelGroup.TabIndex = 1;
            // 
            // ControlGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Lavender;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panelGroup);
            this.Controls.Add(this.labelGroup);
            this.ForeColor = System.Drawing.Color.Black;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ControlGroup";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(555, 87);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.FlowLayoutPanel panelGroup;
    }
}
