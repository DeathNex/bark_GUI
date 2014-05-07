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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Panel panelBot;
            System.Windows.Forms.Panel btnSeparator;
            System.Windows.Forms.Panel panelMid;
            System.Windows.Forms.Panel panelXAxis;
            System.Windows.Forms.Label label3;
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.yAxisListView = new System.Windows.Forms.ListView();
            this.xAxisDropdownList = new System.Windows.Forms.ComboBox();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.labelSimulationTime = new System.Windows.Forms.Label();
            this.panelYAxis = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            panelBot = new System.Windows.Forms.Panel();
            btnSeparator = new System.Windows.Forms.Panel();
            panelMid = new System.Windows.Forms.Panel();
            panelXAxis = new System.Windows.Forms.Panel();
            label3 = new System.Windows.Forms.Label();
            panelBot.SuspendLayout();
            panelMid.SuspendLayout();
            panelXAxis.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.panelYAxis.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(7, 7);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(80, 13);
            label1.TabIndex = 2;
            label1.Text = "Simulation time:";
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
            panelBot.Location = new System.Drawing.Point(2, 328);
            panelBot.Name = "panelBot";
            panelBot.Padding = new System.Windows.Forms.Padding(5);
            panelBot.Size = new System.Drawing.Size(430, 31);
            panelBot.TabIndex = 10;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(264, 5);
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
            btnSeparator.Location = new System.Drawing.Point(339, 5);
            btnSeparator.Name = "btnSeparator";
            btnSeparator.Size = new System.Drawing.Size(11, 21);
            btnSeparator.TabIndex = 3;
            btnSeparator.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(350, 5);
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
            panelMid.Location = new System.Drawing.Point(2, 94);
            panelMid.Name = "panelMid";
            panelMid.Size = new System.Drawing.Size(430, 234);
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
            this.yAxisListView.Size = new System.Drawing.Size(430, 234);
            this.yAxisListView.TabIndex = 3;
            this.yAxisListView.TileSize = new System.Drawing.Size(30, 10);
            this.yAxisListView.UseCompatibleStateImageBehavior = false;
            this.yAxisListView.View = System.Windows.Forms.View.List;
            // 
            // panelXAxis
            // 
            panelXAxis.Controls.Add(label3);
            panelXAxis.Controls.Add(this.xAxisDropdownList);
            panelXAxis.Dock = System.Windows.Forms.DockStyle.Top;
            panelXAxis.Location = new System.Drawing.Point(2, 31);
            panelXAxis.Margin = new System.Windows.Forms.Padding(15);
            panelXAxis.Name = "panelXAxis";
            panelXAxis.Padding = new System.Windows.Forms.Padding(7);
            panelXAxis.Size = new System.Drawing.Size(430, 37);
            panelXAxis.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(7, 7);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(87, 13);
            label3.TabIndex = 2;
            label3.Text = "Select the X Axis";
            // 
            // xAxisDropdownList
            // 
            this.xAxisDropdownList.Dock = System.Windows.Forms.DockStyle.Right;
            this.xAxisDropdownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.xAxisDropdownList.FormattingEnabled = true;
            this.xAxisDropdownList.Location = new System.Drawing.Point(223, 7);
            this.xAxisDropdownList.Name = "xAxisDropdownList";
            this.xAxisDropdownList.Size = new System.Drawing.Size(200, 21);
            this.xAxisDropdownList.TabIndex = 4;
            // 
            // panelInfo
            // 
            this.panelInfo.Controls.Add(this.labelSimulationTime);
            this.panelInfo.Controls.Add(label1);
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInfo.Location = new System.Drawing.Point(2, 2);
            this.panelInfo.Margin = new System.Windows.Forms.Padding(15);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Padding = new System.Windows.Forms.Padding(7);
            this.panelInfo.Size = new System.Drawing.Size(430, 29);
            this.panelInfo.TabIndex = 6;
            // 
            // labelSimulationTime
            // 
            this.labelSimulationTime.AutoSize = true;
            this.labelSimulationTime.Location = new System.Drawing.Point(124, 7);
            this.labelSimulationTime.Name = "labelSimulationTime";
            this.labelSimulationTime.Size = new System.Drawing.Size(0, 13);
            this.labelSimulationTime.TabIndex = 3;
            // 
            // panelYAxis
            // 
            this.panelYAxis.Controls.Add(label2);
            this.panelYAxis.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelYAxis.Location = new System.Drawing.Point(2, 68);
            this.panelYAxis.Name = "panelYAxis";
            this.panelYAxis.Padding = new System.Windows.Forms.Padding(7);
            this.panelYAxis.Size = new System.Drawing.Size(430, 26);
            this.panelYAxis.TabIndex = 9;
            // 
            // DiagramDataDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(434, 361);
            this.Controls.Add(panelMid);
            this.Controls.Add(panelBot);
            this.Controls.Add(this.panelYAxis);
            this.Controls.Add(panelXAxis);
            this.Controls.Add(this.panelInfo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 250);
            this.Name = "DiagramDataDialog";
            this.ShowIcon = false;
            this.Text = "Data for Diagram";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DiagramDataDialog_FormClosing);
            panelBot.ResumeLayout(false);
            panelMid.ResumeLayout(false);
            panelXAxis.ResumeLayout(false);
            panelXAxis.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.panelYAxis.ResumeLayout(false);
            this.panelYAxis.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelYAxis;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView yAxisListView;
        private System.Windows.Forms.ComboBox xAxisDropdownList;
        private System.Windows.Forms.Label labelSimulationTime;
        private System.Windows.Forms.Panel panelInfo;
    }
}