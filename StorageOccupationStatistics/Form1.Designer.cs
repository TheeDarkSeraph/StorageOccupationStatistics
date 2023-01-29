namespace StorageOccupationStatistics {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.filePanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // filePanel
            // 
            this.filePanel.Location = new System.Drawing.Point(5, 5);
            this.filePanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.filePanel.Name = "filePanel";
            this.filePanel.Size = new System.Drawing.Size(500, 576);
            this.filePanel.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 587);
            this.Controls.Add(this.filePanel);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private Panel filePanel;
    }
}