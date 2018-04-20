namespace CSLERP.FileManager
{
    partial class PDFViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PDFViewer));
            this.axPDF = new AxAcroPDFLib.AxAcroPDF();
            ((System.ComponentModel.ISupportInitialize)(this.axPDF)).BeginInit();
            this.SuspendLayout();
            // 
            // axPDF
            // 
            this.axPDF.Enabled = true;
            this.axPDF.Location = new System.Drawing.Point(68, 12);
            this.axPDF.Name = "axPDF";
            this.axPDF.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPDF.OcxState")));
            this.axPDF.Size = new System.Drawing.Size(747, 439);
            this.axPDF.TabIndex = 0;
            // 
            // PDFViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 494);
            this.Controls.Add(this.axPDF);
            this.Name = "PDFViewer";
            this.Text = "PDFViewer";
            ((System.ComponentModel.ISupportInitialize)(this.axPDF)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxAcroPDFLib.AxAcroPDF axPDF;
    }
}