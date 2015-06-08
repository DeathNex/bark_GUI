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
            this.toolTipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridViewValue = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewValue)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxUnit2
            // 
            this.comboBoxUnit2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUnit2.FormattingEnabled = true;
            this.comboBoxUnit2.Location = new System.Drawing.Point(321, 3);
            this.comboBoxUnit2.Name = "comboBoxUnit2";
            this.comboBoxUnit2.Size = new System.Drawing.Size(64, 21);
            this.comboBoxUnit2.TabIndex = 13;
            this.comboBoxUnit2.SelectedIndexChanged += new System.EventHandler(this.comboBoxUnit2_SelectedIndexChanged);
            // 
            // comboBoxUnit
            // 
            this.comboBoxUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUnit.FormattingEnabled = true;
            this.comboBoxUnit.Location = new System.Drawing.Point(250, 3);
            this.comboBoxUnit.Name = "comboBoxUnit";
            this.comboBoxUnit.Size = new System.Drawing.Size(64, 21);
            this.comboBoxUnit.TabIndex = 12;
            this.comboBoxUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxUnit_SelectedIndexChanged);
            // 
            // comboBoxType
            // 
            this.comboBoxType.DisplayMember = "0";
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(176, 3);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(64, 21);
            this.comboBoxType.TabIndex = 11;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(3, 3);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(33, 13);
            this.labelName.TabIndex = 9;
            this.labelName.Text = "name";
            // 
            // dataGridViewValue
            // 
            this.dataGridViewValue.AllowUserToResizeColumns = false;
            this.dataGridViewValue.AllowUserToResizeRows = false;
            this.dataGridViewValue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewValue.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewValue.ColumnHeadersVisible = false;
            this.dataGridViewValue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridViewValue.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataGridViewValue.Location = new System.Drawing.Point(250, 31);
            this.dataGridViewValue.Name = "dataGridViewValue";
            this.dataGridViewValue.RowHeadersVisible = false;
            this.dataGridViewValue.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewValue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewValue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewValue.Size = new System.Drawing.Size(145, 190);
            this.dataGridViewValue.TabIndex = 14;
            this.dataGridViewValue.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewValue_CellValueChanged);
            this.dataGridViewValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewValue_KeyDown);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ControlVariable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridViewValue);
            this.Controls.Add(this.comboBoxUnit2);
            this.Controls.Add(this.comboBoxUnit);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.labelName);
            this.Name = "ControlVariable";
            this.Size = new System.Drawing.Size(398, 225);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxUnit;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ComboBox comboBoxUnit2;
        private System.Windows.Forms.ToolTip toolTipHelp;
        private System.Windows.Forms.DataGridView dataGridViewValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}
