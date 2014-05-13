namespace bark_GUI
{
    partial class FormDiagram
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDiagram));
            this.diagram = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // diagram
            // 
            this.diagram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagram.Location = new System.Drawing.Point(0, 0);
            this.diagram.Name = "diagram";
            this.diagram.ScrollGrace = 0D;
            this.diagram.ScrollMaxX = 0D;
            this.diagram.ScrollMaxY = 0D;
            this.diagram.ScrollMaxY2 = 0D;
            this.diagram.ScrollMinX = 0D;
            this.diagram.ScrollMinY = 0D;
            this.diagram.ScrollMinY2 = 0D;
            this.diagram.Size = new System.Drawing.Size(964, 641);
            this.diagram.TabIndex = 0;
            // 
            // FormDiagram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(964, 641);
            this.Controls.Add(this.diagram);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDiagram";
            this.Text = "Diagram";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormDiagram_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl diagram;
    }
}

