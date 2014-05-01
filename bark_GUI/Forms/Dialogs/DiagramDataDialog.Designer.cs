namespace bark_GUI.Forms.Dialogs
{
    partial class DiagramDataDialog
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Panel panelTop;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Panel panelBot;
            System.Windows.Forms.Panel btnSeparator;
            System.Windows.Forms.Panel panelMid;
            this.xAxisDropdownList = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.yAxisListView = new System.Windows.Forms.ListView();
            this.panelTopMid = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            panelTop = new System.Windows.Forms.Panel();
            label2 = new System.Windows.Forms.Label();
            panelBot = new System.Windows.Forms.Panel();
            btnSeparator = new System.Windows.Forms.Panel();
            panelMid = new System.Windows.Forms.Panel();
            panelTop.SuspendLayout();
            panelBot.SuspendLayout();
            panelMid.SuspendLayout();
            this.panelTopMid.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(7, 7);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(87, 13);
            label1.TabIndex = 2;
            label1.Text = "Select the X Axis";
            // 
            // panelTop
            // 
            panelTop.Controls.Add(label1);
            panelTop.Controls.Add(this.xAxisDropdownList);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(0, 0);
            panelTop.Margin = new System.Windows.Forms.Padding(15);
            panelTop.Name = "panelTop";
            panelTop.Padding = new System.Windows.Forms.Padding(7);
            panelTop.Size = new System.Drawing.Size(334, 37);
            panelTop.TabIndex = 6;
            // 
            // xAxisDropdownList
            // 
            this.xAxisDropdownList.Dock = System.Windows.Forms.DockStyle.Right;
            this.xAxisDropdownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.xAxisDropdownList.FormattingEnabled = true;
            this.xAxisDropdownList.Location = new System.Drawing.Point(127, 7);
            this.xAxisDropdownList.Name = "xAxisDropdownList";
            this.xAxisDropdownList.Size = new System.Drawing.Size(200, 21);
            this.xAxisDropdownList.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = System.Windows.Forms.DockStyle.Fill;
            label2.Location = new System.Drawing.Point(7, 7);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(154, 13);
            label2.TabIndex = 5;
            label2.Text = "Select the Y Axis (One or more)";
            // 
            // panelBot
            // 
            panelBot.Controls.Add(this.btnOk);
            panelBot.Controls.Add(btnSeparator);
            panelBot.Controls.Add(this.btnCancel);
            panelBot.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelBot.Location = new System.Drawing.Point(0, 180);
            panelBot.Name = "panelBot";
            panelBot.Padding = new System.Windows.Forms.Padding(5);
            panelBot.Size = new System.Drawing.Size(334, 31);
            panelBot.TabIndex = 10;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(168, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 21);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnSeparator
            // 
            btnSeparator.Dock = System.Windows.Forms.DockStyle.Right;
            btnSeparator.Location = new System.Drawing.Point(243, 5);
            btnSeparator.Name = "btnSeparator";
            btnSeparator.Size = new System.Drawing.Size(11, 21);
            btnSeparator.TabIndex = 3;
            btnSeparator.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(254, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 21);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panelMid
            // 
            panelMid.Controls.Add(this.yAxisListView);
            panelMid.Dock = System.Windows.Forms.DockStyle.Fill;
            panelMid.Location = new System.Drawing.Point(0, 63);
            panelMid.Name = "panelMid";
            panelMid.Size = new System.Drawing.Size(334, 117);
            panelMid.TabIndex = 11;
            // 
            // yAxisListView
            // 
            this.yAxisListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.yAxisListView.GridLines = true;
            this.yAxisListView.HideSelection = false;
            this.yAxisListView.Location = new System.Drawing.Point(0, 0);
            this.yAxisListView.Name = "yAxisListView";
            this.yAxisListView.ShowGroups = false;
            this.yAxisListView.Size = new System.Drawing.Size(334, 117);
            this.yAxisListView.TabIndex = 3;
            this.yAxisListView.TileSize = new System.Drawing.Size(30, 10);
            this.yAxisListView.UseCompatibleStateImageBehavior = false;
            this.yAxisListView.View = System.Windows.Forms.View.List;
            // 
            // panelTopMid
            // 
            this.panelTopMid.Controls.Add(label2);
            this.panelTopMid.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTopMid.Location = new System.Drawing.Point(0, 37);
            this.panelTopMid.Name = "panelTopMid";
            this.panelTopMid.Padding = new System.Windows.Forms.Padding(7);
            this.panelTopMid.Size = new System.Drawing.Size(334, 26);
            this.panelTopMid.TabIndex = 9;
            // 
            // DiagramDataDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 211);
            this.Controls.Add(panelMid);
            this.Controls.Add(panelBot);
            this.Controls.Add(this.panelTopMid);
            this.Controls.Add(panelTop);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 250);
            this.Name = "DiagramDataDialog";
            this.ShowIcon = false;
            this.Text = "Data for Diagram";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DiagramDataDialog_FormClosing);
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelBot.ResumeLayout(false);
            panelMid.ResumeLayout(false);
            this.panelTopMid.ResumeLayout(false);
            this.panelTopMid.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox xAxisDropdownList;
        private System.Windows.Forms.Panel panelTopMid;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView yAxisListView;
    }
}