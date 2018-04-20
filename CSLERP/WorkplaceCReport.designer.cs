namespace CSLERP
{
    partial class WorkplaceCReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlUI = new System.Windows.Forms.Panel();
            this.pnlList = new System.Windows.Forms.Panel();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.dtFromDate = new System.Windows.Forms.DateTimePicker();
            this.btnCheck = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbComplaintType = new System.Windows.Forms.ComboBox();
            this.dtToDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.lblActionHeader = new System.Windows.Forms.Label();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.cDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Complainer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComplaintType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComplaintDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AcceptTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClosedTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExit = new System.Windows.Forms.Button();
            this.DocumentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemporaryNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemporaryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TrackingNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TrackingDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerPONO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomrPODate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeliveryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentTerms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreditPeriods = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrencyID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BillingAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeliveryAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlUI.SuspendLayout();
            this.pnlList.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.pnlBottomButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.pnlList);
            this.pnlUI.Controls.Add(this.pnlBottomButtons);
            this.pnlUI.Location = new System.Drawing.Point(15, 15);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(1100, 540);
            this.pnlUI.TabIndex = 8;
            // 
            // pnlList
            // 
            this.pnlList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlList.Controls.Add(this.btnExportExcel);
            this.pnlList.Controls.Add(this.pnlHeader);
            this.pnlList.Controls.Add(this.lblActionHeader);
            this.pnlList.Controls.Add(this.grdList);
            this.pnlList.Location = new System.Drawing.Point(9, 3);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(1081, 477);
            this.pnlList.TabIndex = 6;
            this.pnlList.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlList_Paint);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(925, 445);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(114, 23);
            this.btnExportExcel.TabIndex = 48;
            this.btnExportExcel.Text = "Export To Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnClear);
            this.pnlHeader.Controls.Add(this.dtFromDate);
            this.pnlHeader.Controls.Add(this.btnCheck);
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Controls.Add(this.label7);
            this.pnlHeader.Controls.Add(this.cmbComplaintType);
            this.pnlHeader.Controls.Add(this.dtToDate);
            this.pnlHeader.Controls.Add(this.label6);
            this.pnlHeader.Location = new System.Drawing.Point(160, 24);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(781, 37);
            this.pnlHeader.TabIndex = 47;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(703, 7);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(56, 23);
            this.btnClear.TabIndex = 47;
            this.btnClear.Text = "CLEAR";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // dtFromDate
            // 
            this.dtFromDate.Enabled = false;
            this.dtFromDate.Location = new System.Drawing.Point(66, 7);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(128, 20);
            this.dtFromDate.TabIndex = 36;
            this.dtFromDate.ValueChanged += new System.EventHandler(this.dtFromDate_ValueChanged);
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(620, 7);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(75, 23);
            this.btnCheck.TabIndex = 43;
            this.btnCheck.Text = "VIEW";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(367, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 46;
            this.label1.Text = "Complaint Type";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 40;
            this.label7.Text = "FROM";
            // 
            // cmbComplaintType
            // 
            this.cmbComplaintType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbComplaintType.FormattingEnabled = true;
            this.cmbComplaintType.Location = new System.Drawing.Point(453, 7);
            this.cmbComplaintType.Name = "cmbComplaintType";
            this.cmbComplaintType.Size = new System.Drawing.Size(153, 21);
            this.cmbComplaintType.TabIndex = 45;
            this.cmbComplaintType.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // dtToDate
            // 
            this.dtToDate.Enabled = false;
            this.dtToDate.Location = new System.Drawing.Point(237, 7);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(124, 20);
            this.dtToDate.TabIndex = 39;
            this.dtToDate.ValueChanged += new System.EventHandler(this.dtToDate_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(207, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 13);
            this.label6.TabIndex = 41;
            this.label6.Text = "TO";
            // 
            // lblActionHeader
            // 
            this.lblActionHeader.AutoSize = true;
            this.lblActionHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActionHeader.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblActionHeader.Location = new System.Drawing.Point(460, -1);
            this.lblActionHeader.Name = "lblActionHeader";
            this.lblActionHeader.Size = new System.Drawing.Size(0, 17);
            this.lblActionHeader.TabIndex = 30;
            // 
            // grdList
            // 
            this.grdList.AllowUserToAddRows = false;
            this.grdList.AllowUserToDeleteRows = false;
            this.grdList.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grdList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdList.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
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
            this.cDate,
            this.Complainer,
            this.ComplaintType,
            this.ComplaintDescription,
            this.Remark,
            this.AcceptTime,
            this.Status,
            this.ClosedTime});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdList.DefaultCellStyle = dataGridViewCellStyle7;
            this.grdList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdList.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdList.Location = new System.Drawing.Point(42, 67);
            this.grdList.Name = "grdList";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdList.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.grdList.RowHeadersVisible = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.AliceBlue;
            this.grdList.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.grdList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            this.grdList.Size = new System.Drawing.Size(1011, 378);
            this.grdList.TabIndex = 32;
            // 
            // cDate
            // 
            dataGridViewCellStyle3.Format = "dd-MM-yyyy";
            this.cDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.cDate.HeaderText = "Date";
            this.cDate.Name = "cDate";
            // 
            // Complainer
            // 
            this.Complainer.HeaderText = "Complainee";
            this.Complainer.Name = "Complainer";
            this.Complainer.Width = 150;
            // 
            // ComplaintType
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ComplaintType.DefaultCellStyle = dataGridViewCellStyle4;
            this.ComplaintType.HeaderText = "ComplaintType";
            this.ComplaintType.Name = "ComplaintType";
            // 
            // ComplaintDescription
            // 
            this.ComplaintDescription.HeaderText = "Complaint Description";
            this.ComplaintDescription.Name = "ComplaintDescription";
            this.ComplaintDescription.Width = 200;
            // 
            // Remark
            // 
            this.Remark.HeaderText = "Remark";
            this.Remark.Name = "Remark";
            this.Remark.Width = 200;
            // 
            // AcceptTime
            // 
            dataGridViewCellStyle5.Format = "dd-MM-yyyy HH:mm:ss";
            dataGridViewCellStyle5.NullValue = null;
            this.AcceptTime.DefaultCellStyle = dataGridViewCellStyle5;
            this.AcceptTime.HeaderText = "Accept Time";
            this.AcceptTime.Name = "AcceptTime";
            this.AcceptTime.Width = 150;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            // 
            // ClosedTime
            // 
            dataGridViewCellStyle6.Format = "dd-MM-yyyy HH:mm:ss";
            this.ClosedTime.DefaultCellStyle = dataGridViewCellStyle6;
            this.ClosedTime.HeaderText = "Closed Time";
            this.ClosedTime.Name = "ClosedTime";
            this.ClosedTime.Width = 150;
            // 
            // pnlBottomButtons
            // 
            this.pnlBottomButtons.Controls.Add(this.btnExit);
            this.pnlBottomButtons.Location = new System.Drawing.Point(9, 509);
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
            // DocumentID
            // 
            this.DocumentID.HeaderText = "Document ID";
            this.DocumentID.Name = "DocumentID";
            this.DocumentID.ReadOnly = true;
            this.DocumentID.Visible = false;
            // 
            // DocumentName
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DocumentName.DefaultCellStyle = dataGridViewCellStyle10;
            this.DocumentName.HeaderText = "Document Name";
            this.DocumentName.Name = "DocumentName";
            // 
            // TemporaryNo
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TemporaryNo.DefaultCellStyle = dataGridViewCellStyle11;
            this.TemporaryNo.HeaderText = "Temporary No";
            this.TemporaryNo.Name = "TemporaryNo";
            // 
            // TemporaryDate
            // 
            this.TemporaryDate.HeaderText = "Temporary Date";
            this.TemporaryDate.Name = "TemporaryDate";
            // 
            // TrackingNo
            // 
            this.TrackingNo.HeaderText = "Tracking No";
            this.TrackingNo.Name = "TrackingNo";
            // 
            // TrackingDate
            // 
            this.TrackingDate.HeaderText = "Tracking Date";
            this.TrackingDate.Name = "TrackingDate";
            // 
            // CustomerID
            // 
            this.CustomerID.HeaderText = "Customer ID";
            this.CustomerID.Name = "CustomerID";
            // 
            // CustomerName
            // 
            this.CustomerName.HeaderText = "Customer Name";
            this.CustomerName.Name = "CustomerName";
            // 
            // CustomerPONO
            // 
            this.CustomerPONO.HeaderText = "Customer PO NO";
            this.CustomerPONO.Name = "CustomerPONO";
            // 
            // CustomrPODate
            // 
            this.CustomrPODate.HeaderText = "Customer PO Date";
            this.CustomrPODate.Name = "CustomrPODate";
            // 
            // DeliveryDate
            // 
            this.DeliveryDate.HeaderText = "Delivery Date";
            this.DeliveryDate.Name = "DeliveryDate";
            // 
            // PaymentTerms
            // 
            this.PaymentTerms.HeaderText = "Payment Terms";
            this.PaymentTerms.Name = "PaymentTerms";
            // 
            // CreditPeriods
            // 
            this.CreditPeriods.HeaderText = "Credit Periods";
            this.CreditPeriods.Name = "CreditPeriods";
            this.CreditPeriods.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CreditPeriods.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CreditPeriods.ToolTipText = "Edit Employee";
            // 
            // CurrencyID
            // 
            this.CurrencyID.HeaderText = "Currency ID";
            this.CurrencyID.Name = "CurrencyID";
            this.CurrencyID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CurrencyID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CurrencyID.ToolTipText = "Forward/Approve";
            // 
            // TaxCode
            // 
            this.TaxCode.HeaderText = "Tax Code";
            this.TaxCode.Name = "TaxCode";
            this.TaxCode.ReadOnly = true;
            // 
            // BillingAddress
            // 
            this.BillingAddress.HeaderText = "Billing Address";
            this.BillingAddress.Name = "BillingAddress";
            this.BillingAddress.ReadOnly = true;
            this.BillingAddress.Visible = false;
            // 
            // DeliveryAddress
            // 
            this.DeliveryAddress.HeaderText = "Delivey Address";
            this.DeliveryAddress.Name = "DeliveryAddress";
            this.DeliveryAddress.ReadOnly = true;
            this.DeliveryAddress.Visible = false;
            // 
            // WorkplaceCReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "WorkplaceCReport";
            this.Text = "Stock OB";
            this.Load += new System.EventHandler(this.BREntry_Load);
            this.Enter += new System.EventHandler(this.WorkplaceCReport_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlList.ResumeLayout(false);
            this.pnlList.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.pnlBottomButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Panel pnlList;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblActionHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemporaryNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemporaryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn TrackingNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TrackingDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerPONO;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomrPODate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeliveryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentTerms;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreditPeriods;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrencyID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn BillingAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeliveryAddress;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtToDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtFromDate;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.ComboBox cmbComplaintType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Complainer;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComplaintType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComplaintDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn AcceptTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClosedTime;
    }
}