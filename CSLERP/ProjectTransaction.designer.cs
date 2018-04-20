namespace CSLERP
{
    partial class ProjectTransaction
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlUI = new System.Windows.Forms.Panel();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlList = new System.Windows.Forms.Panel();
            this.btnPrjctSel = new System.Windows.Forms.Button();
            this.txtProjectManager = new System.Windows.Forms.TextBox();
            this.lblPrjctMngr = new System.Windows.Forms.Label();
            this.dtTargetDate = new System.Windows.Forms.DateTimePicker();
            this.dtStartDate = new System.Windows.Forms.DateTimePicker();
            this.grdDetailList = new System.Windows.Forms.DataGridView();
            this.LineNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustPONo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustPODate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtClient = new System.Windows.Forms.TextBox();
            this.txtProjectID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grdMainList = new System.Windows.Forms.DataGridView();
            this.Received = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Detail = new System.Windows.Forms.DataGridViewButtonColumn();
            this.lblActionHeader = new System.Windows.Forms.Label();
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
            this.pnlBottomButtons.SuspendLayout();
            this.pnlList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetailList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMainList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.pnlBottomButtons);
            this.pnlUI.Controls.Add(this.pnlList);
            this.pnlUI.Location = new System.Drawing.Point(15, 15);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(1100, 540);
            this.pnlUI.TabIndex = 8;
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
            // pnlList
            // 
            this.pnlList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlList.Controls.Add(this.btnPrjctSel);
            this.pnlList.Controls.Add(this.txtProjectManager);
            this.pnlList.Controls.Add(this.lblPrjctMngr);
            this.pnlList.Controls.Add(this.dtTargetDate);
            this.pnlList.Controls.Add(this.dtStartDate);
            this.pnlList.Controls.Add(this.grdDetailList);
            this.pnlList.Controls.Add(this.txtClient);
            this.pnlList.Controls.Add(this.txtProjectID);
            this.pnlList.Controls.Add(this.label4);
            this.pnlList.Controls.Add(this.label3);
            this.pnlList.Controls.Add(this.label2);
            this.pnlList.Controls.Add(this.label1);
            this.pnlList.Controls.Add(this.btnCancel);
            this.pnlList.Controls.Add(this.grdMainList);
            this.pnlList.Controls.Add(this.lblActionHeader);
            this.pnlList.Location = new System.Drawing.Point(11, 7);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(1079, 501);
            this.pnlList.TabIndex = 6;
            this.pnlList.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlList_Paint);
            // 
            // btnPrjctSel
            // 
            this.btnPrjctSel.Location = new System.Drawing.Point(263, 19);
            this.btnPrjctSel.Name = "btnPrjctSel";
            this.btnPrjctSel.Size = new System.Drawing.Size(89, 21);
            this.btnPrjctSel.TabIndex = 70;
            this.btnPrjctSel.Text = "Select Project";
            this.btnPrjctSel.UseVisualStyleBackColor = true;
            this.btnPrjctSel.Click += new System.EventHandler(this.btnPrjctSel_Click);
            // 
            // txtProjectManager
            // 
            this.txtProjectManager.Enabled = false;
            this.txtProjectManager.Location = new System.Drawing.Point(95, 47);
            this.txtProjectManager.Name = "txtProjectManager";
            this.txtProjectManager.Size = new System.Drawing.Size(329, 20);
            this.txtProjectManager.TabIndex = 69;
            // 
            // lblPrjctMngr
            // 
            this.lblPrjctMngr.AutoSize = true;
            this.lblPrjctMngr.Location = new System.Drawing.Point(8, 50);
            this.lblPrjctMngr.Name = "lblPrjctMngr";
            this.lblPrjctMngr.Size = new System.Drawing.Size(85, 13);
            this.lblPrjctMngr.TabIndex = 68;
            this.lblPrjctMngr.Text = "Project Manager";
            // 
            // dtTargetDate
            // 
            this.dtTargetDate.Enabled = false;
            this.dtTargetDate.Location = new System.Drawing.Point(95, 130);
            this.dtTargetDate.Name = "dtTargetDate";
            this.dtTargetDate.Size = new System.Drawing.Size(165, 20);
            this.dtTargetDate.TabIndex = 65;
            // 
            // dtStartDate
            // 
            this.dtStartDate.Enabled = false;
            this.dtStartDate.Location = new System.Drawing.Point(95, 101);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Size = new System.Drawing.Size(165, 20);
            this.dtStartDate.TabIndex = 64;
            // 
            // grdDetailList
            // 
            this.grdDetailList.AllowUserToAddRows = false;
            this.grdDetailList.AllowUserToDeleteRows = false;
            this.grdDetailList.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdDetailList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdDetailList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDetailList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LineNo,
            this.DocumentNo,
            this.DocumentDate,
            this.CustPONo,
            this.CustPODate,
            this.Customer,
            this.gValue,
            this.TaxAmount,
            this.TotalAmount});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdDetailList.DefaultCellStyle = dataGridViewCellStyle5;
            this.grdDetailList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grdDetailList.Location = new System.Drawing.Point(95, 204);
            this.grdDetailList.Name = "grdDetailList";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdDetailList.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.grdDetailList.RowHeadersVisible = false;
            this.grdDetailList.Size = new System.Drawing.Size(904, 239);
            this.grdDetailList.TabIndex = 61;
            // 
            // LineNo
            // 
            this.LineNo.Frozen = true;
            this.LineNo.HeaderText = "Si No";
            this.LineNo.Name = "LineNo";
            this.LineNo.ReadOnly = true;
            this.LineNo.Width = 50;
            // 
            // DocumentNo
            // 
            this.DocumentNo.HeaderText = "DocumentNo";
            this.DocumentNo.Name = "DocumentNo";
            this.DocumentNo.ReadOnly = true;
            // 
            // DocumentDate
            // 
            dataGridViewCellStyle2.Format = "dd-MM-yyyy";
            this.DocumentDate.DefaultCellStyle = dataGridViewCellStyle2;
            this.DocumentDate.HeaderText = "DocumentDate";
            this.DocumentDate.Name = "DocumentDate";
            this.DocumentDate.ReadOnly = true;
            // 
            // CustPONo
            // 
            this.CustPONo.HeaderText = "CustPONo";
            this.CustPONo.Name = "CustPONo";
            this.CustPONo.ReadOnly = true;
            // 
            // CustPODate
            // 
            dataGridViewCellStyle3.Format = "dd-MM-yyyy";
            this.CustPODate.DefaultCellStyle = dataGridViewCellStyle3;
            this.CustPODate.HeaderText = "CustPODate";
            this.CustPODate.Name = "CustPODate";
            this.CustPODate.ReadOnly = true;
            // 
            // Customer
            // 
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Customer.DefaultCellStyle = dataGridViewCellStyle4;
            this.Customer.HeaderText = "Customer";
            this.Customer.Name = "Customer";
            this.Customer.ReadOnly = true;
            this.Customer.Width = 150;
            // 
            // gValue
            // 
            this.gValue.HeaderText = "Value";
            this.gValue.Name = "gValue";
            this.gValue.ReadOnly = true;
            // 
            // TaxAmount
            // 
            this.TaxAmount.HeaderText = "TaxAmount";
            this.TaxAmount.Name = "TaxAmount";
            this.TaxAmount.ReadOnly = true;
            // 
            // TotalAmount
            // 
            this.TotalAmount.HeaderText = "TotalAmount";
            this.TotalAmount.Name = "TotalAmount";
            this.TotalAmount.ReadOnly = true;
            // 
            // txtClient
            // 
            this.txtClient.Enabled = false;
            this.txtClient.Location = new System.Drawing.Point(95, 73);
            this.txtClient.Name = "txtClient";
            this.txtClient.Size = new System.Drawing.Size(329, 20);
            this.txtClient.TabIndex = 58;
            // 
            // txtProjectID
            // 
            this.txtProjectID.Enabled = false;
            this.txtProjectID.Location = new System.Drawing.Point(95, 20);
            this.txtProjectID.Name = "txtProjectID";
            this.txtProjectID.Size = new System.Drawing.Size(165, 20);
            this.txtProjectID.TabIndex = 57;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 56;
            this.label4.Text = "TargetDate";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 55;
            this.label3.Text = "StartDate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 54;
            this.label2.Text = "Client";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 53;
            this.label1.Text = "Project Name";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(936, 443);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 23);
            this.btnCancel.TabIndex = 52;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grdMainList
            // 
            this.grdMainList.AllowUserToAddRows = false;
            this.grdMainList.AllowUserToDeleteRows = false;
            this.grdMainList.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grdMainList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdMainList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.grdMainList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMainList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Received,
            this.gNo,
            this.Value,
            this.Detail});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdMainList.DefaultCellStyle = dataGridViewCellStyle8;
            this.grdMainList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grdMainList.Location = new System.Drawing.Point(430, 15);
            this.grdMainList.Name = "grdMainList";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdMainList.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.grdMainList.RowHeadersVisible = false;
            this.grdMainList.Size = new System.Drawing.Size(521, 183);
            this.grdMainList.TabIndex = 51;
            this.grdMainList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdMainList_CellContentClick);
            // 
            // Received
            // 
            this.Received.HeaderText = "Received";
            this.Received.Name = "Received";
            this.Received.ReadOnly = true;
            this.Received.Width = 150;
            // 
            // gNo
            // 
            this.gNo.HeaderText = "No";
            this.gNo.Name = "gNo";
            this.gNo.ReadOnly = true;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            this.Value.Width = 150;
            // 
            // Detail
            // 
            this.Detail.HeaderText = "Detail";
            this.Detail.Name = "Detail";
            this.Detail.Text = "Detail";
            this.Detail.UseColumnTextForButtonValue = true;
            // 
            // lblActionHeader
            // 
            this.lblActionHeader.AutoSize = true;
            this.lblActionHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActionHeader.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblActionHeader.Location = new System.Drawing.Point(746, 4);
            this.lblActionHeader.Name = "lblActionHeader";
            this.lblActionHeader.Size = new System.Drawing.Size(0, 17);
            this.lblActionHeader.TabIndex = 30;
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
            // ProjectTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "ProjectTransaction";
            this.Text = "Customer Group";
            this.Load += new System.EventHandler(this.ProjectTransaction_Load);
            this.Enter += new System.EventHandler(this.ProjectTransaction_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlList.ResumeLayout(false);
            this.pnlList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetailList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMainList)).EndInit();
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
        private System.Windows.Forms.DataGridView grdMainList;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView grdDetailList;
        private System.Windows.Forms.TextBox txtClient;
        private System.Windows.Forms.TextBox txtProjectID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtTargetDate;
        private System.Windows.Forms.DateTimePicker dtStartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Received;
        private System.Windows.Forms.DataGridViewTextBoxColumn gNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewButtonColumn Detail;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustPONo;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustPODate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn gValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalAmount;
        private System.Windows.Forms.TextBox txtProjectManager;
        private System.Windows.Forms.Label lblPrjctMngr;
        private System.Windows.Forms.Button btnPrjctSel;
    }
}