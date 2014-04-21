namespace bark_GUI.CustomControls
{
    partial class ControlVariable
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
            this.comboBoxUnit2 = new System.Windows.Forms.ComboBox();
            this.comboBoxUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.labelName = new System.Windows.Forms.Label();
            this.control_variable_table = new bark_GUI.CustomControls.ControlVariableTable();
            this.toolTipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // comboBoxUnit2
            // 
            this.comboBoxUnit2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUnit2.FormattingEnabled = true;
            this.comboBoxUnit2.Location = new System.Drawing.Point(428, 4);
            this.comboBoxUnit2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxUnit2.Name = "comboBoxUnit2";
            this.comboBoxUnit2.Size = new System.Drawing.Size(84, 24);
            this.comboBoxUnit2.TabIndex = 13;
            this.comboBoxUnit2.SelectedIndexChanged += new System.EventHandler(this.comboBoxUnit2_SelectedIndexChanged);
            // 
            // comboBoxUnit
            // 
            this.comboBoxUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUnit.FormattingEnabled = true;
            this.comboBoxUnit.Location = new System.Drawing.Point(334, 4);
            this.comboBoxUnit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxUnit.Name = "comboBoxUnit";
            this.comboBoxUnit.Size = new System.Drawing.Size(84, 24);
            this.comboBoxUnit.TabIndex = 12;
            this.comboBoxUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxUnit_SelectedIndexChanged);
            // 
            // comboBoxType
            // 
            this.comboBoxType.DisplayMember = "0";
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(234, 4);
            this.comboBoxType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(84, 24);
            this.comboBoxType.TabIndex = 11;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(4, 4);
            this.labelName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(43, 17);
            this.labelName.TabIndex = 9;
            this.labelName.Text = "name";
            // 
            // control_variable_table
            // 
            this.control_variable_table.Location = new System.Drawing.Point(324, 37);
            this.control_variable_table.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.control_variable_table.Name = "control_variable_table";
            this.control_variable_table.Size = new System.Drawing.Size(200, 234);
            this.control_variable_table.TabIndex = 8;
            // 
            // ControlVariable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxUnit2);
            this.Controls.Add(this.comboBoxUnit);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.control_variable_table);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ControlVariable";
            this.Size = new System.Drawing.Size(530, 277);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ControlVariableTable control_variable_table;
        private System.Windows.Forms.ComboBox comboBoxUnit;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ComboBox comboBoxUnit2;
        private System.Windows.Forms.ToolTip toolTipHelp;
    }
}
