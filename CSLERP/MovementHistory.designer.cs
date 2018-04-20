namespace CSLERP
{
    partial class MovementHistory
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlUI = new System.Windows.Forms.Panel();
            this.pnlList = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.cmbPurpose = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDocumentStatus = new System.Windows.Forms.Label();
            this.cmbDocStatus = new System.Windows.Forms.ComboBox();
            this.btnViewHistory = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtEmpID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtToDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.dtFromDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.lblActionHeader = new System.Windows.Forms.Label();
            this.btnExportToExcell = new System.Windows.Forms.Button();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.mDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mEmployee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mPurpose = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MovementStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mExitTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mReturnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aExitTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aReturnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.pnlMenu.SuspendLayout();
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
            this.pnlList.Controls.Add(this.btnClear);
            this.pnlList.Controls.Add(this.pnlMenu);
            this.pnlList.Controls.Add(this.lblActionHeader);
            this.pnlList.Controls.Add(this.btnExportToExcell);
            this.pnlList.Controls.Add(this.grdList);
            this.pnlList.Location = new System.Drawing.Point(9, 15);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(1081, 472);
            this.pnlList.TabIndex = 6;
            this.pnlList.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlList_Paint);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(1030, 8);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(41, 23);
            this.btnClear.TabIndex = 60;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // pnlMenu
            // 
            this.pnlMenu.Controls.Add(this.cmbPurpose);
            this.pnlMenu.Controls.Add(this.label2);
            this.pnlMenu.Controls.Add(this.lblDocumentStatus);
            this.pnlMenu.Controls.Add(this.cmbDocStatus);
            this.pnlMenu.Controls.Add(this.btnViewHistory);
            this.pnlMenu.Controls.Add(this.btnSelect);
            this.pnlMenu.Controls.Add(this.txtEmpID);
            this.pnlMenu.Controls.Add(this.label6);
            this.pnlMenu.Controls.Add(this.dtToDate);
            this.pnlMenu.Controls.Add(this.label7);
            this.pnlMenu.Controls.Add(this.dtFromDate);
            this.pnlMenu.Controls.Add(this.label1);
            this.pnlMenu.Location = new System.Drawing.Point(13, 3);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(1059, 33);
            this.pnlMenu.TabIndex = 49;
            // 
            // cmbPurpose
            // 
            this.cmbPurpose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPurpose.FormattingEnabled = true;
            this.cmbPurpose.Location = new System.Drawing.Point(810, 6);
            this.cmbPurpose.Name = "cmbPurpose";
            this.cmbPurpose.Size = new System.Drawing.Size(148, 21);
            this.cmbPurpose.TabIndex = 61;
            this.cmbPurpose.SelectedIndexChanged += new System.EventHandler(this.cmbPurpose_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(760, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 60;
            this.label2.Text = "Purpose";
            // 
            // lblDocumentStatus
            // 
            this.lblDocumentStatus.AutoSize = true;
            this.lblDocumentStatus.Location = new System.Drawing.Point(585, 9);
            this.lblDocumentStatus.Name = "lblDocumentStatus";
            this.lblDocumentStatus.Size = new System.Drawing.Size(37, 13);
            this.lblDocumentStatus.TabIndex = 59;
            this.lblDocumentStatus.Text = "Status";
            // 
            // cmbDocStatus
            // 
            this.cmbDocStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDocStatus.FormattingEnabled = true;
            this.cmbDocStatus.Items.AddRange(new object[] {
            "All",
            "Auto-In",
            "ApprovalPending",
            "Approved",
            "Canceled",
            "Rejected",
            "Out",
            "In"});
            this.cmbDocStatus.Location = new System.Drawing.Point(624, 7);
            this.cmbDocStatus.Name = "cmbDocStatus";
            this.cmbDocStatus.Size = new System.Drawing.Size(117, 21);
            this.cmbDocStatus.TabIndex = 57;
            this.cmbDocStatus.SelectedIndexChanged += new System.EventHandler(this.cmbDocStatus_SelectedIndexChanged);
            // 
            // btnViewHistory
            // 
            this.btnViewHistory.Location = new System.Drawing.Point(964, 5);
            this.btnViewHistory.Name = "btnViewHistory";
            this.btnViewHistory.Size = new System.Drawing.Size(47, 23);
            this.btnViewHistory.TabIndex = 56;
            this.btnViewHistory.Text = "View";
            this.btnViewHistory.UseVisualStyleBackColor = true;
            this.btnViewHistory.Click += new System.EventHandler(this.btnViewHistory_Click_1);
            // 
            // btnSelect
            // 
            this.btnSelect.FlatAppearance.BorderSize = 0;
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelect.Image = global::CSLERP.Properties.Resources.select12;
            this.btnSelect.Location = new System.Drawing.Point(541, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(32, 22);
            this.btnSelect.TabIndex = 55;
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click_1);
            // 
            // txtEmpID
            // 
            this.txtEmpID.Enabled = false;
            this.txtEmpID.Location = new System.Drawing.Point(358, 3);
            this.txtEmpID.Multiline = true;
            this.txtEmpID.Name = "txtEmpID";
            this.txtEmpID.Size = new System.Drawing.Size(217, 25);
            this.txtEmpID.TabIndex = 49;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(163, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 54;
            this.label6.Text = "To";
            // 
            // dtToDate
            // 
            this.dtToDate.Location = new System.Drawing.Point(189, 4);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(98, 20);
            this.dtToDate.TabIndex = 52;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 53;
            this.label7.Text = "From";
            // 
            // dtFromDate
            // 
            this.dtFromDate.Location = new System.Drawing.Point(43, 4);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(107, 20);
            this.dtFromDate.TabIndex = 51;
            this.dtFromDate.Value = new System.DateTime(2017, 5, 15, 0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(304, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "Employee";
            // 
            // lblActionHeader
            // 
            this.lblActionHeader.AutoSize = true;
            this.lblActionHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActionHeader.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblActionHeader.Location = new System.Drawing.Point(371, 7);
            this.lblActionHeader.Name = "lblActionHeader";
            this.lblActionHeader.Size = new System.Drawing.Size(0, 17);
            this.lblActionHeader.TabIndex = 30;
            // 
            // btnExportToExcell
            // 
            this.btnExportToExcell.Location = new System.Drawing.Point(920, 446);
            this.btnExportToExcell.Name = "btnExportToExcell";
            this.btnExportToExcell.Size = new System.Drawing.Size(108, 23);
            this.btnExportToExcell.TabIndex = 58;
            this.btnExportToExcell.Text = "ExportToExcel";
            this.btnExportToExcell.UseVisualStyleBackColor = true;
            this.btnExportToExcell.Visible = false;
            this.btnExportToExcell.Click += new System.EventHandler(this.btnExportToExcell_Click);
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
            this.mDate,
            this.mEmployee,
            this.mPurpose,
            this.MovementStatus,
            this.mExitTime,
            this.mReturnTime,
            this.aExitTime,
            this.aReturnTime});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdList.DefaultCellStyle = dataGridViewCellStyle9;
            this.grdList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdList.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdList.Location = new System.Drawing.Point(9, 53);
            this.grdList.Name = "grdList";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdList.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.grdList.RowHeadersVisible = false;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.AliceBlue;
            this.grdList.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.grdList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            this.grdList.Size = new System.Drawing.Size(1064, 392);
            this.grdList.TabIndex = 32;
            this.grdList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdList_CellContentClick_1);
            // 
            // mDate
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "dd-MM-yyyy HH:mm:ss";
            this.mDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.mDate.HeaderText = "Date";
            this.mDate.Name = "mDate";
            this.mDate.Width = 150;
            // 
            // mEmployee
            // 
            this.mEmployee.HeaderText = "Employee";
            this.mEmployee.Name = "mEmployee";
            this.mEmployee.Width = 160;
            // 
            // mPurpose
            // 
            dataGridViewCellStyle4.Format = "dd-MM-yyyy";
            dataGridViewCellStyle4.NullValue = null;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mPurpose.DefaultCellStyle = dataGridViewCellStyle4;
            this.mPurpose.HeaderText = "Purpose";
            this.mPurpose.Name = "mPurpose";
            this.mPurpose.Width = 250;
            // 
            // MovementStatus
            // 
            this.MovementStatus.HeaderText = "Status";
            this.MovementStatus.Name = "MovementStatus";
            // 
            // mExitTime
            // 
            dataGridViewCellStyle5.Format = "dd-MM-yyyy HH:mm:ss";
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mExitTime.DefaultCellStyle = dataGridViewCellStyle5;
            this.mExitTime.HeaderText = "Planned Exit Time ";
            this.mExitTime.Name = "mExitTime";
            // 
            // mReturnTime
            // 
            dataGridViewCellStyle6.Format = "dd-MM-yyyy HH:mm:ss";
            dataGridViewCellStyle6.NullValue = null;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mReturnTime.DefaultCellStyle = dataGridViewCellStyle6;
            this.mReturnTime.HeaderText = "Planned Return Time";
            this.mReturnTime.Name = "mReturnTime";
            // 
            // aExitTime
            // 
            dataGridViewCellStyle7.Format = "dd-MM-yyyy HH:mm:ss";
            dataGridViewCellStyle7.NullValue = null;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.aExitTime.DefaultCellStyle = dataGridViewCellStyle7;
            this.aExitTime.HeaderText = "Actual Exit Time";
            this.aExitTime.Name = "aExitTime";
            // 
            // aReturnTime
            // 
            dataGridViewCellStyle8.Format = "dd-MM-yyyy HH:mm:ss";
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.aReturnTime.DefaultCellStyle = dataGridViewCellStyle8;
            this.aReturnTime.HeaderText = "Actual Return Time";
            this.aReturnTime.Name = "aReturnTime";
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
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DocumentName.DefaultCellStyle = dataGridViewCellStyle12;
            this.DocumentName.HeaderText = "Document Name";
            this.DocumentName.Name = "DocumentName";
            // 
            // TemporaryNo
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TemporaryNo.DefaultCellStyle = dataGridViewCellStyle13;
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
            // MovementHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "MovementHistory";
            this.Text = "Stock OB";
            this.Load += new System.EventHandler(this.BREntry_Load);
            this.Enter += new System.EventHandler(this.MovementHistory_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlList.ResumeLayout(false);
            this.pnlList.PerformLayout();
            this.pnlMenu.ResumeLayout(false);
            this.pnlMenu.PerformLayout();
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
        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Label lblDocumentStatus;
        private System.Windows.Forms.Button btnExportToExcell;
        private System.Windows.Forms.ComboBox cmbDocStatus;
        private System.Windows.Forms.Button btnViewHistory;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtEmpID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtToDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtFromDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridViewTextBoxColumn mDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn mEmployee;
        private System.Windows.Forms.DataGridViewTextBoxColumn mPurpose;
        private System.Windows.Forms.DataGridViewTextBoxColumn MovementStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn mExitTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn mReturnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn aExitTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn aReturnTime;
        private System.Windows.Forms.ComboBox cmbPurpose;
        private System.Windows.Forms.Label label2;
    }
}