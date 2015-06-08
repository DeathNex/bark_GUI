namespace bark_GUI.CustomControls
{
    partial class ControlReference
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
            this.components = new System.ComponentModel.Container();
            this.comboBoxValue = new System.Windows.Forms.ComboBox();
            this.labelName = new System.Windows.Forms.Label();
            this.toolTipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // comboBoxValue
            // 
            this.comboBoxValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxValue.FormattingEnabled = true;
            this.comboBoxValue.Location = new System.Drawing.Point(327, 6);
            this.comboBoxValue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxValue.Name = "comboBoxValue";
            this.comboBoxValue.Size = new System.Drawing.Size(199, 24);
            this.comboBoxValue.TabIndex = 10;
            this.comboBoxValue.SelectedIndexChanged += new System.EventHandler(this.comboBoxValue_SelectedIndexChanged);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(4, 6);
            this.labelName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(43, 17);
            this.labelName.TabIndex = 8;
            this.labelName.Text = "name";
            // 
            // ControlReference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxValue);
            this.Controls.Add(this.labelName);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ControlReference";
            this.Size = new System.Drawing.Size(530, 37);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxValue;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ToolTip toolTipHelp;
    }
}
