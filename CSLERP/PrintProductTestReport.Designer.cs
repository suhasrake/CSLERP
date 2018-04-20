namespace CSLERP
{
    partial class PrintProductTestReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlUI = new System.Windows.Forms.Panel();
            this.pnlProductionList = new System.Windows.Forms.Panel();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.ProductionPlanNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductionPlanDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockItemID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModelNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Print = new System.Windows.Forms.DataGridViewButtonColumn();
            this.View = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlPDFViewer = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCloseDocument = new System.Windows.Forms.Button();
            this.btnListDocument = new System.Windows.Forms.Button();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlUI.SuspendLayout();
            this.pnlProductionList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.pnlPDFViewer.SuspendLayout();
            this.pnlBottomButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.pnlProductionList);
            this.pnlUI.Controls.Add(this.pnlPDFViewer);
            this.pnlUI.Controls.Add(this.pnlBottomButtons);
            this.pnlUI.Location = new System.Drawing.Point(15, 15);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(1100, 540);
            this.pnlUI.TabIndex = 8;
            // 
            // pnlProductionList
            // 
            this.pnlProductionList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlProductionList.Controls.Add(this.grdList);
            this.pnlProductionList.Location = new System.Drawing.Point(129, 58);
            this.pnlProductionList.Name = "pnlProductionList";
            this.pnlProductionList.Size = new System.Drawing.Size(773, 357);
            this.pnlProductionList.TabIndex = 6;
            // 
            // grdList
            // 
            this.grdList.AllowUserToAddRows = false;
            this.grdList.AllowUserToDeleteRows = false;
            this.grdList.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grdList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdList.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grdList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grdList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductionPlanNo,
            this.ProductionPlanDate,
            this.StockItemID,
            this.StockItemName,
            this.ModelNo,
            this.ModelName,
            this.Print,
            this.View});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdList.DefaultCellStyle = dataGridViewCellStyle6;
            this.grdList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdList.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdList.Location = new System.Drawing.Point(37, 15);
            this.grdList.Name = "grdList";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdList.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.grdList.RowHeadersVisible = false;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.AliceBlue;
            this.grdList.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.grdList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            this.grdList.Size = new System.Drawing.Size(707, 297);
            this.grdList.TabIndex = 4;
            this.grdList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdList_CellContentClick);
            // 
            // ProductionPlanNo
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ProductionPlanNo.DefaultCellStyle = dataGridViewCellStyle3;
            this.ProductionPlanNo.HeaderText = "ProdPlanNo";
            this.ProductionPlanNo.Name = "ProductionPlanNo";
            // 
            // ProductionPlanDate
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Format = "dd-MM-yyyy";
            this.ProductionPlanDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.ProductionPlanDate.HeaderText = "ProdPlanDate";
            this.ProductionPlanDate.Name = "ProductionPlanDate";
            this.ProductionPlanDate.Width = 200;
            // 
            // StockItemID
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.StockItemID.DefaultCellStyle = dataGridViewCellStyle5;
            this.StockItemID.HeaderText = "StockItemID";
            this.StockItemID.Name = "StockItemID";
            // 
            // StockItemName
            // 
            this.StockItemName.HeaderText = "StockItemName";
            this.StockItemName.Name = "StockItemName";
            // 
            // ModelNo
            // 
            this.ModelNo.HeaderText = "ModelNo";
            this.ModelNo.Name = "ModelNo";
            // 
            // ModelName
            // 
            this.ModelName.HeaderText = "ModelName";
            this.ModelName.Name = "ModelName";
            // 
            // Print
            // 
            this.Print.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Print.HeaderText = "Print";
            this.Print.Name = "Print";
            this.Print.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Print.Text = "Print";
            this.Print.UseColumnTextForButtonValue = true;
            // 
            // View
            // 
            this.View.HeaderText = "View";
            this.View.Name = "View";
            this.View.Text = "View";
            this.View.UseColumnTextForButtonValue = true;
            // 
            // pnlPDFViewer
            // 
            this.pnlPDFViewer.Controls.Add(this.btnClose);
            this.pnlPDFViewer.Controls.Add(this.btnCloseDocument);
            this.pnlPDFViewer.Controls.Add(this.btnListDocument);
            this.pnlPDFViewer.Location = new System.Drawing.Point(101, 73);
            this.pnlPDFViewer.Name = "pnlPDFViewer";
            this.pnlPDFViewer.Size = new System.Drawing.Size(885, 393);
            this.pnlPDFViewer.TabIndex = 11;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(253, 357);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCloseDocument
            // 
            this.btnCloseDocument.Location = new System.Drawing.Point(135, 358);
            this.btnCloseDocument.Name = "btnCloseDocument";
            this.btnCloseDocument.Size = new System.Drawing.Size(97, 23);
            this.btnCloseDocument.TabIndex = 3;
            this.btnCloseDocument.Text = "Close Document";
            this.btnCloseDocument.UseVisualStyleBackColor = true;
            this.btnCloseDocument.Click += new System.EventHandler(this.btnCloseDocument_Click);
            // 
            // btnListDocument
            // 
            this.btnListDocument.Location = new System.Drawing.Point(13, 358);
            this.btnListDocument.Name = "btnListDocument";
            this.btnListDocument.Size = new System.Drawing.Size(97, 23);
            this.btnListDocument.TabIndex = 2;
            this.btnListDocument.Text = "List Document";
            this.btnListDocument.UseVisualStyleBackColor = true;
            this.btnListDocument.Click += new System.EventHandler(this.btnListDocument_Click);
            // 
            // pnlBottomButtons
            // 
            this.pnlBottomButtons.Controls.Add(this.btnExit);
            this.pnlBottomButtons.Location = new System.Drawing.Point(15, 510);
            this.pnlBottomButtons.Name = "pnlBottomButtons";
            this.pnlBottomButtons.Size = new System.Drawing.Size(510, 28);
            this.pnlBottomButtons.TabIndex = 10;
            // 
            // btnExit
            // 
            this.btnExit.Image = global::CSLERP.Properties.Resources.cancel;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(3, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // PrintProductTestReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "PrintProductTestReport";
            this.Text = "ERP User";
            this.Load += new System.EventHandler(this.PrintProductTestReport_Load);
            this.Enter += new System.EventHandler(this.PrintProductTestReport_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlProductionList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.pnlPDFViewer.ResumeLayout(false);
            this.pnlBottomButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Panel pnlProductionList;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.Panel pnlPDFViewer;
        private System.Windows.Forms.Button btnCloseDocument;
        private System.Windows.Forms.Button btnListDocument;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductionPlanNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductionPlanDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockItemID;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModelNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModelName;
        private System.Windows.Forms.DataGridViewButtonColumn Print;
        private System.Windows.Forms.DataGridViewButtonColumn View;
    }
}