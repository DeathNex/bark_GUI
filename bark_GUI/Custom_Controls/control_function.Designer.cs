namespace bark_GUI.Custom_Controls
{
    partial class control_function
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
            this.comboBoxFunc = new System.Windows.Forms.ComboBox();
            this.labelName = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.toolTipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // comboBoxFunc
            // 
            this.comboBoxFunc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFunc.FormattingEnabled = true;
            this.comboBoxFunc.Location = new System.Drawing.Point(245, 6);
            this.comboBoxFunc.Name = "comboBoxFunc";
            this.comboBoxFunc.Size = new System.Drawing.Size(150, 21);
            this.comboBoxFunc.TabIndex = 12;
            this.comboBoxFunc.SelectedIndexChanged += new System.EventHandler(this.comboBoxFunc_SelectedIndexChanged);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(3, 9);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(33, 13);
            this.labelName.TabIndex = 11;
            this.labelName.Text = "name";
            // 
            // comboBoxType
            // 
            this.comboBoxType.DisplayMember = "0";
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(175, 5);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(64, 21);
            this.comboBoxType.TabIndex = 13;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
            // 
            // control_function
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.comboBoxFunc);
            this.Controls.Add(this.labelName);
            this.Name = "control_function";
            this.Size = new System.Drawing.Size(397, 37);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxFunc;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.ToolTip toolTipHelp;
    }
}
