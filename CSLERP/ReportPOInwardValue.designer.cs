namespace CSLERP
{
    partial class ReportPOInwardValue
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlUI = new System.Windows.Forms.Panel();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlList = new System.Windows.Forms.Panel();
            this.lblInvoiceTotal = new System.Windows.Forms.Label();
            this.txtTotalInvoiceValue = new System.Windows.Forms.TextBox();
            this.lblPOTotal = new System.Windows.Forms.Label();
            this.rdbLakhs = new System.Windows.Forms.RadioButton();
            this.rdbThousands = new System.Windows.Forms.RadioButton();
            this.rdbNormal = new System.Windows.Forms.RadioButton();
            this.txtTotalPOValue = new System.Windows.Forms.TextBox();
            this.btnView = new System.Windows.Forms.Button();
            this.btnShowChart = new System.Windows.Forms.Button();
            this.chkRegionWise = new System.Windows.Forms.CheckBox();
            this.chkPartWise = new System.Windows.Forms.CheckBox();
            this.chkservicePO = new System.Windows.Forms.CheckBox();
            this.chkProductPO = new System.Windows.Forms.CheckBox();
            this.dtToDate = new System.Windows.Forms.DateTimePicker();
            this.dtFromDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.grdDetailList = new System.Windows.Forms.DataGridView();
            this.LineNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gParty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gRegion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.POType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.POValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InvoiceValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlShowChart = new System.Windows.Forms.Panel();
            this.chrtServicePO = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chrtProductPO = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chkGraphGrid = new System.Windows.Forms.CheckBox();
            this.rdbArea = new System.Windows.Forms.RadioButton();
            this.rdbColumn = new System.Windows.Forms.RadioButton();
            this.rdbBar = new System.Windows.Forms.RadioButton();
            this.rbdList = new System.Windows.Forms.RadioButton();
            this.rbdPie = new System.Windows.Forms.RadioButton();
            this.btndefault = new System.Windows.Forms.Button();
            this.btncls = new System.Windows.Forms.Button();
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
            this.pnlShowChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chrtServicePO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrtProductPO)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.pnlBottomButtons);
            this.pnlUI.Controls.Add(this.pnlList);
            this.pnlUI.Controls.Add(this.pnlShowChart);
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
            this.pnlList.Controls.Add(this.lblInvoiceTotal);
            this.pnlList.Controls.Add(this.txtTotalInvoiceValue);
            this.pnlList.Controls.Add(this.lblPOTotal);
            this.pnlList.Controls.Add(this.rdbLakhs);
            this.pnlList.Controls.Add(this.rdbThousands);
            this.pnlList.Controls.Add(this.rdbNormal);
            this.pnlList.Controls.Add(this.txtTotalPOValue);
            this.pnlList.Controls.Add(this.btnView);
            this.pnlList.Controls.Add(this.btnShowChart);
            this.pnlList.Controls.Add(this.chkRegionWise);
            this.pnlList.Controls.Add(this.chkPartWise);
            this.pnlList.Controls.Add(this.chkservicePO);
            this.pnlList.Controls.Add(this.chkProductPO);
            this.pnlList.Controls.Add(this.dtToDate);
            this.pnlList.Controls.Add(this.dtFromDate);
            this.pnlList.Controls.Add(this.label2);
            this.pnlList.Controls.Add(this.label1);
            this.pnlList.Controls.Add(this.btnClose);
            this.pnlList.Controls.Add(this.grdDetailList);
            this.pnlList.Location = new System.Drawing.Point(11, 28);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(1079, 454);
            this.pnlList.TabIndex = 6;
            // 
            // lblInvoiceTotal
            // 
            this.lblInvoiceTotal.AutoSize = true;
            this.lblInvoiceTotal.Location = new System.Drawing.Point(819, 134);
            this.lblInvoiceTotal.Name = "lblInvoiceTotal";
            this.lblInvoiceTotal.Size = new System.Drawing.Size(99, 13);
            this.lblInvoiceTotal.TabIndex = 69;
            this.lblInvoiceTotal.Text = "Total Invoice Value";
            this.lblInvoiceTotal.Visible = false;
            // 
            // txtTotalInvoiceValue
            // 
            this.txtTotalInvoiceValue.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.txtTotalInvoiceValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalInvoiceValue.Enabled = false;
            this.txtTotalInvoiceValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalInvoiceValue.Location = new System.Drawing.Point(927, 130);
            this.txtTotalInvoiceValue.Name = "txtTotalInvoiceValue";
            this.txtTotalInvoiceValue.Size = new System.Drawing.Size(136, 22);
            this.txtTotalInvoiceValue.TabIndex = 68;
            this.txtTotalInvoiceValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTotalInvoiceValue.Visible = false;
            // 
            // lblPOTotal
            // 
            this.lblPOTotal.AutoSize = true;
            this.lblPOTotal.Location = new System.Drawing.Point(839, 94);
            this.lblPOTotal.Name = "lblPOTotal";
            this.lblPOTotal.Size = new System.Drawing.Size(79, 13);
            this.lblPOTotal.TabIndex = 67;
            this.lblPOTotal.Text = "Total PO Value";
            this.lblPOTotal.Visible = false;
            // 
            // rdbLakhs
            // 
            this.rdbLakhs.AutoSize = true;
            this.rdbLakhs.Location = new System.Drawing.Point(586, 63);
            this.rdbLakhs.Name = "rdbLakhs";
            this.rdbLakhs.Size = new System.Drawing.Size(54, 17);
            this.rdbLakhs.TabIndex = 66;
            this.rdbLakhs.TabStop = true;
            this.rdbLakhs.Text = "Lakhs";
            this.rdbLakhs.UseVisualStyleBackColor = true;
            this.rdbLakhs.Click += new System.EventHandler(this.rdbLakhs_Click);
            // 
            // rdbThousands
            // 
            this.rdbThousands.AutoSize = true;
            this.rdbThousands.Location = new System.Drawing.Point(475, 63);
            this.rdbThousands.Name = "rdbThousands";
            this.rdbThousands.Size = new System.Drawing.Size(78, 17);
            this.rdbThousands.TabIndex = 65;
            this.rdbThousands.TabStop = true;
            this.rdbThousands.Text = "Thousands";
            this.rdbThousands.UseVisualStyleBackColor = true;
            this.rdbThousands.Click += new System.EventHandler(this.rdbThousands_Click);
            // 
            // rdbNormal
            // 
            this.rdbNormal.AutoSize = true;
            this.rdbNormal.Location = new System.Drawing.Point(378, 63);
            this.rdbNormal.Name = "rdbNormal";
            this.rdbNormal.Size = new System.Drawing.Size(58, 17);
            this.rdbNormal.TabIndex = 64;
            this.rdbNormal.TabStop = true;
            this.rdbNormal.Text = "Normal";
            this.rdbNormal.UseVisualStyleBackColor = true;
            this.rdbNormal.Click += new System.EventHandler(this.rdbNormal_Click);
            // 
            // txtTotalPOValue
            // 
            this.txtTotalPOValue.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.txtTotalPOValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalPOValue.Enabled = false;
            this.txtTotalPOValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalPOValue.Location = new System.Drawing.Point(927, 90);
            this.txtTotalPOValue.Name = "txtTotalPOValue";
            this.txtTotalPOValue.Size = new System.Drawing.Size(136, 22);
            this.txtTotalPOValue.TabIndex = 63;
            this.txtTotalPOValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTotalPOValue.Visible = false;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(691, 16);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(77, 23);
            this.btnView.TabIndex = 62;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnShowChart
            // 
            this.btnShowChart.Location = new System.Drawing.Point(962, 222);
            this.btnShowChart.Name = "btnShowChart";
            this.btnShowChart.Size = new System.Drawing.Size(101, 25);
            this.btnShowChart.TabIndex = 61;
            this.btnShowChart.Text = "Show Chart";
            this.btnShowChart.UseVisualStyleBackColor = true;
            this.btnShowChart.Visible = false;
            this.btnShowChart.Click += new System.EventHandler(this.btnShowChart_Click);
            // 
            // chkRegionWise
            // 
            this.chkRegionWise.AutoSize = true;
            this.chkRegionWise.Location = new System.Drawing.Point(586, 39);
            this.chkRegionWise.Name = "chkRegionWise";
            this.chkRegionWise.Size = new System.Drawing.Size(87, 17);
            this.chkRegionWise.TabIndex = 60;
            this.chkRegionWise.Text = "Region Wise";
            this.chkRegionWise.UseVisualStyleBackColor = true;
            this.chkRegionWise.Click += new System.EventHandler(this.chkRegionWise_Click);
            // 
            // chkPartWise
            // 
            this.chkPartWise.AutoSize = true;
            this.chkPartWise.Location = new System.Drawing.Point(586, 16);
            this.chkPartWise.Name = "chkPartWise";
            this.chkPartWise.Size = new System.Drawing.Size(77, 17);
            this.chkPartWise.TabIndex = 59;
            this.chkPartWise.Text = "Party Wise";
            this.chkPartWise.UseVisualStyleBackColor = true;
            this.chkPartWise.Click += new System.EventHandler(this.chkPartWise_Click);
            // 
            // chkservicePO
            // 
            this.chkservicePO.AutoSize = true;
            this.chkservicePO.Location = new System.Drawing.Point(456, 38);
            this.chkservicePO.Name = "chkservicePO";
            this.chkservicePO.Size = new System.Drawing.Size(80, 17);
            this.chkservicePO.TabIndex = 58;
            this.chkservicePO.Text = "Service PO";
            this.chkservicePO.UseVisualStyleBackColor = true;
            this.chkservicePO.Click += new System.EventHandler(this.chkservicePO_Click);
            // 
            // chkProductPO
            // 
            this.chkProductPO.AutoSize = true;
            this.chkProductPO.Location = new System.Drawing.Point(456, 15);
            this.chkProductPO.Name = "chkProductPO";
            this.chkProductPO.Size = new System.Drawing.Size(81, 17);
            this.chkProductPO.TabIndex = 57;
            this.chkProductPO.Text = "Product PO";
            this.chkProductPO.UseVisualStyleBackColor = true;
            this.chkProductPO.Click += new System.EventHandler(this.chkProductPO_Click);
            // 
            // dtToDate
            // 
            this.dtToDate.Location = new System.Drawing.Point(304, 36);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(128, 20);
            this.dtToDate.TabIndex = 56;
            // 
            // dtFromDate
            // 
            this.dtFromDate.Location = new System.Drawing.Point(304, 10);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(128, 20);
            this.dtFromDate.TabIndex = 55;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(252, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 54;
            this.label2.Text = "To Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(242, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 53;
            this.label1.Text = "From Date";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(962, 181);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(101, 25);
            this.btnClose.TabIndex = 52;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grdDetailList
            // 
            this.grdDetailList.AllowUserToAddRows = false;
            this.grdDetailList.AllowUserToDeleteRows = false;
            this.grdDetailList.AllowUserToOrderColumns = true;
            this.grdDetailList.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grdDetailList.BorderStyle = System.Windows.Forms.BorderStyle.None;
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
            this.gParty,
            this.gRegion,
            this.POType,
            this.POValue,
            this.InvoiceValue});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdDetailList.DefaultCellStyle = dataGridViewCellStyle3;
            this.grdDetailList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdDetailList.Location = new System.Drawing.Point(102, 86);
            this.grdDetailList.Name = "grdDetailList";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdDetailList.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.grdDetailList.RowHeadersVisible = false;
            this.grdDetailList.Size = new System.Drawing.Size(710, 309);
            this.grdDetailList.TabIndex = 50;
            this.grdDetailList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdDetailList_CellContentClick);
            this.grdDetailList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdDetailList_ColumnHeaderMouseClick);
            // 
            // LineNo
            // 
            this.LineNo.Frozen = true;
            this.LineNo.HeaderText = "Si No";
            this.LineNo.Name = "LineNo";
            this.LineNo.ReadOnly = true;
            this.LineNo.Width = 50;
            // 
            // gParty
            // 
            this.gParty.Frozen = true;
            this.gParty.HeaderText = "Party";
            this.gParty.Name = "gParty";
            this.gParty.Width = 300;
            // 
            // gRegion
            // 
            this.gRegion.Frozen = true;
            this.gRegion.HeaderText = "Region";
            this.gRegion.Name = "gRegion";
            this.gRegion.ReadOnly = true;
            this.gRegion.Width = 300;
            // 
            // POType
            // 
            this.POType.HeaderText = "POType";
            this.POType.Name = "POType";
            this.POType.ReadOnly = true;
            // 
            // POValue
            // 
            dataGridViewCellStyle2.NullValue = "0";
            this.POValue.DefaultCellStyle = dataGridViewCellStyle2;
            this.POValue.HeaderText = "PO Value";
            this.POValue.Name = "POValue";
            this.POValue.ReadOnly = true;
            // 
            // InvoiceValue
            // 
            this.InvoiceValue.HeaderText = "Invoice Value";
            this.InvoiceValue.Name = "InvoiceValue";
            // 
            // pnlShowChart
            // 
            this.pnlShowChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlShowChart.Controls.Add(this.chrtServicePO);
            this.pnlShowChart.Controls.Add(this.chrtProductPO);
            this.pnlShowChart.Controls.Add(this.chkGraphGrid);
            this.pnlShowChart.Controls.Add(this.rdbArea);
            this.pnlShowChart.Controls.Add(this.rdbColumn);
            this.pnlShowChart.Controls.Add(this.rdbBar);
            this.pnlShowChart.Controls.Add(this.rbdList);
            this.pnlShowChart.Controls.Add(this.rbdPie);
            this.pnlShowChart.Controls.Add(this.btndefault);
            this.pnlShowChart.Controls.Add(this.btncls);
            this.pnlShowChart.Location = new System.Drawing.Point(18, 39);
            this.pnlShowChart.Name = "pnlShowChart";
            this.pnlShowChart.Size = new System.Drawing.Size(1069, 439);
            this.pnlShowChart.TabIndex = 116;
            // 
            // chrtServicePO
            // 
            this.chrtServicePO.BackColor = System.Drawing.Color.CadetBlue;
            chartArea1.AxisX.ScaleBreakStyle.Enabled = true;
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.LeftRight;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 95F;
            chartArea1.Position.Width = 83F;
            chartArea1.Position.X = 2F;
            chartArea1.Position.Y = 2F;
            this.chrtServicePO.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chrtServicePO.Legends.Add(legend1);
            this.chrtServicePO.Location = new System.Drawing.Point(497, 27);
            this.chrtServicePO.Name = "chrtServicePO";
            series1.BackImageTransparentColor = System.Drawing.Color.DarkGoldenrod;
            series1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            series1.ChartArea = "ChartArea1";
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            series1.Font = new System.Drawing.Font("Copperplate Gothic Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.IsValueShownAsLabel = true;
            series1.Label = "#VAL";
            series1.LabelAngle = 90;
            series1.Legend = "Legend1";
            series1.Name = "Service PO";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chrtServicePO.Series.Add(series1);
            this.chrtServicePO.Size = new System.Drawing.Size(480, 348);
            this.chrtServicePO.TabIndex = 129;
            this.chrtServicePO.Text = "chart1";
            // 
            // chrtProductPO
            // 
            this.chrtProductPO.BackColor = System.Drawing.Color.CadetBlue;
            chartArea2.AxisX.ScaleBreakStyle.Enabled = true;
            chartArea2.AxisX.TitleFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.AxisY.TitleFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.LeftRight;
            chartArea2.Name = "ChartArea1";
            chartArea2.Position.Auto = false;
            chartArea2.Position.Height = 95F;
            chartArea2.Position.Width = 83F;
            chartArea2.Position.X = 2F;
            chartArea2.Position.Y = 2F;
            this.chrtProductPO.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chrtProductPO.Legends.Add(legend2);
            this.chrtProductPO.Location = new System.Drawing.Point(11, 27);
            this.chrtProductPO.Name = "chrtProductPO";
            series2.BackImageTransparentColor = System.Drawing.Color.DarkGoldenrod;
            series2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            series2.ChartArea = "ChartArea1";
            series2.Color = System.Drawing.Color.Chocolate;
            series2.Font = new System.Drawing.Font("Copperplate Gothic Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series2.IsValueShownAsLabel = true;
            series2.Label = "#VAL";
            series2.LabelAngle = 90;
            series2.Legend = "Legend1";
            series2.Name = "Product PO";
            this.chrtProductPO.Series.Add(series2);
            this.chrtProductPO.Size = new System.Drawing.Size(480, 348);
            this.chrtProductPO.TabIndex = 128;
            this.chrtProductPO.Text = "chart1";
            // 
            // chkGraphGrid
            // 
            this.chkGraphGrid.AutoSize = true;
            this.chkGraphGrid.Location = new System.Drawing.Point(990, 227);
            this.chkGraphGrid.Name = "chkGraphGrid";
            this.chkGraphGrid.Size = new System.Drawing.Size(75, 17);
            this.chkGraphGrid.TabIndex = 126;
            this.chkGraphGrid.Text = "Show Grid";
            this.chkGraphGrid.UseVisualStyleBackColor = true;
            this.chkGraphGrid.CheckedChanged += new System.EventHandler(this.chkGraphGrid_CheckedChanged);
            // 
            // rdbArea
            // 
            this.rdbArea.AutoSize = true;
            this.rdbArea.Location = new System.Drawing.Point(990, 146);
            this.rdbArea.Name = "rdbArea";
            this.rdbArea.Size = new System.Drawing.Size(47, 17);
            this.rdbArea.TabIndex = 123;
            this.rdbArea.TabStop = true;
            this.rdbArea.Text = "Area";
            this.rdbArea.UseVisualStyleBackColor = true;
            this.rdbArea.Click += new System.EventHandler(this.rdbArea_Click);
            // 
            // rdbColumn
            // 
            this.rdbColumn.AutoSize = true;
            this.rdbColumn.Location = new System.Drawing.Point(990, 122);
            this.rdbColumn.Name = "rdbColumn";
            this.rdbColumn.Size = new System.Drawing.Size(60, 17);
            this.rdbColumn.TabIndex = 122;
            this.rdbColumn.TabStop = true;
            this.rdbColumn.Text = "Column";
            this.rdbColumn.UseVisualStyleBackColor = true;
            this.rdbColumn.Click += new System.EventHandler(this.rdbColumn_Click);
            // 
            // rdbBar
            // 
            this.rdbBar.AutoSize = true;
            this.rdbBar.Location = new System.Drawing.Point(990, 98);
            this.rdbBar.Name = "rdbBar";
            this.rdbBar.Size = new System.Drawing.Size(41, 17);
            this.rdbBar.TabIndex = 121;
            this.rdbBar.TabStop = true;
            this.rdbBar.Text = "Bar";
            this.rdbBar.UseVisualStyleBackColor = true;
            this.rdbBar.Click += new System.EventHandler(this.rdbBar_Click);
            // 
            // rbdList
            // 
            this.rbdList.AutoSize = true;
            this.rbdList.Location = new System.Drawing.Point(990, 78);
            this.rbdList.Name = "rbdList";
            this.rbdList.Size = new System.Drawing.Size(45, 17);
            this.rbdList.TabIndex = 119;
            this.rbdList.TabStop = true;
            this.rbdList.Text = "Line";
            this.rbdList.UseVisualStyleBackColor = true;
            this.rbdList.Click += new System.EventHandler(this.rbdList_Click);
            // 
            // rbdPie
            // 
            this.rbdPie.AutoSize = true;
            this.rbdPie.Location = new System.Drawing.Point(990, 52);
            this.rbdPie.Name = "rbdPie";
            this.rbdPie.Size = new System.Drawing.Size(40, 17);
            this.rbdPie.TabIndex = 118;
            this.rbdPie.TabStop = true;
            this.rbdPie.Text = "Pie";
            this.rbdPie.UseVisualStyleBackColor = true;
            this.rbdPie.Click += new System.EventHandler(this.rbdPie_Click);
            // 
            // btndefault
            // 
            this.btndefault.Location = new System.Drawing.Point(990, 169);
            this.btndefault.Name = "btndefault";
            this.btndefault.Size = new System.Drawing.Size(75, 23);
            this.btndefault.TabIndex = 117;
            this.btndefault.Text = "Default";
            this.btndefault.UseVisualStyleBackColor = true;
            this.btndefault.Click += new System.EventHandler(this.btndefault_Click);
            // 
            // btncls
            // 
            this.btncls.Location = new System.Drawing.Point(990, 198);
            this.btncls.Name = "btncls";
            this.btncls.Size = new System.Drawing.Size(75, 23);
            this.btncls.TabIndex = 40;
            this.btncls.Text = "close";
            this.btncls.UseVisualStyleBackColor = true;
            this.btncls.Click += new System.EventHandler(this.btncls_Click);
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
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DocumentName.DefaultCellStyle = dataGridViewCellStyle5;
            this.DocumentName.HeaderText = "Document Name";
            this.DocumentName.Name = "DocumentName";
            // 
            // TemporaryNo
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TemporaryNo.DefaultCellStyle = dataGridViewCellStyle6;
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
            // ReportPOInwardValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "ReportPOInwardValue";
            this.Text = "Customer Group";
            this.Load += new System.EventHandler(this.ReportPOAnalysis_Load);
            this.Enter += new System.EventHandler(this.ReportPOInwardValue_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlList.ResumeLayout(false);
            this.pnlList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetailList)).EndInit();
            this.pnlShowChart.ResumeLayout(false);
            this.pnlShowChart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chrtServicePO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrtProductPO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Panel pnlList;
        private System.Windows.Forms.Button btnExit;
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
        private System.Windows.Forms.DataGridView grdDetailList;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnShowChart;
        private System.Windows.Forms.CheckBox chkRegionWise;
        private System.Windows.Forms.CheckBox chkPartWise;
        private System.Windows.Forms.CheckBox chkservicePO;
        private System.Windows.Forms.CheckBox chkProductPO;
        private System.Windows.Forms.DateTimePicker dtToDate;
        private System.Windows.Forms.DateTimePicker dtFromDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Panel pnlShowChart;
        private System.Windows.Forms.CheckBox chkGraphGrid;
        private System.Windows.Forms.RadioButton rdbArea;
        private System.Windows.Forms.RadioButton rdbColumn;
        private System.Windows.Forms.RadioButton rdbBar;
        private System.Windows.Forms.RadioButton rbdList;
        private System.Windows.Forms.RadioButton rbdPie;
        private System.Windows.Forms.Button btndefault;
        private System.Windows.Forms.Button btncls;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrtProductPO;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrtServicePO;
        private System.Windows.Forms.TextBox txtTotalPOValue;
        private System.Windows.Forms.RadioButton rdbLakhs;
        private System.Windows.Forms.RadioButton rdbThousands;
        private System.Windows.Forms.RadioButton rdbNormal;
        private System.Windows.Forms.Label lblPOTotal;
        private System.Windows.Forms.Label lblInvoiceTotal;
        private System.Windows.Forms.TextBox txtTotalInvoiceValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn gParty;
        private System.Windows.Forms.DataGridViewTextBoxColumn gRegion;
        private System.Windows.Forms.DataGridViewTextBoxColumn POType;
        private System.Windows.Forms.DataGridViewTextBoxColumn POValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceValue;
    }
}