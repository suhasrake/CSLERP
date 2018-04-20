namespace CSLERP
{
    partial class MaterialDeliveryStatus
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
            this.pnlUI = new System.Windows.Forms.Panel();
            this.pnlDocumentOuter = new System.Windows.Forms.Panel();
            this.pnlDocumentInner = new System.Windows.Forms.Panel();
            this.btnAddRemarks = new System.Windows.Forms.Button();
            this.dtDeliveryDate = new System.Windows.Forms.DateTimePicker();
            this.cmbDeliveryStatus = new System.Windows.Forms.ComboBox();
            this.lblDeliveryDate = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.txtLRno = new System.Windows.Forms.TextBox();
            this.dtLRdate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtDocumentDate = new System.Windows.Forms.DateTimePicker();
            this.cmbTransportationmode = new System.Windows.Forms.ComboBox();
            this.cmbConsignee = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDocno = new System.Windows.Forms.TextBox();
            this.pnlSelection = new System.Windows.Forms.Panel();
            this.lblDeliveryStatus = new System.Windows.Forms.Label();
            this.rdbDelivered = new System.Windows.Forms.RadioButton();
            this.rdbInTransit = new System.Windows.Forms.RadioButton();
            this.pnlDocumentList = new System.Windows.Forms.Panel();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.gDeliveryDocumentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gDocumentNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gDocumentDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gConsignee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gCourierID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gTransportationMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gLRNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gLRDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gCreateUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gCreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gDeliveryStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gDeliveryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.gView = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlUI.SuspendLayout();
            this.pnlDocumentOuter.SuspendLayout();
            this.pnlDocumentInner.SuspendLayout();
            this.pnlSelection.SuspendLayout();
            this.pnlDocumentList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.pnlBottomButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.pnlDocumentOuter);
            this.pnlUI.Controls.Add(this.pnlSelection);
            this.pnlUI.Controls.Add(this.pnlDocumentList);
            this.pnlUI.Controls.Add(this.pnlBottomButtons);
            this.pnlUI.Location = new System.Drawing.Point(15, 15);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(1100, 540);
            this.pnlUI.TabIndex = 8;
            // 
            // pnlDocumentOuter
            // 
            this.pnlDocumentOuter.BackColor = System.Drawing.Color.Black;
            this.pnlDocumentOuter.Controls.Add(this.pnlDocumentInner);
            this.pnlDocumentOuter.Location = new System.Drawing.Point(154, 82);
            this.pnlDocumentOuter.Name = "pnlDocumentOuter";
            this.pnlDocumentOuter.Size = new System.Drawing.Size(790, 403);
            this.pnlDocumentOuter.TabIndex = 8;
            this.pnlDocumentOuter.Visible = false;
            this.pnlDocumentOuter.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDocumentOuter_Paint);
            // 
            // pnlDocumentInner
            // 
            this.pnlDocumentInner.BackColor = System.Drawing.Color.White;
            this.pnlDocumentInner.Controls.Add(this.btnAddRemarks);
            this.pnlDocumentInner.Controls.Add(this.dtDeliveryDate);
            this.pnlDocumentInner.Controls.Add(this.cmbDeliveryStatus);
            this.pnlDocumentInner.Controls.Add(this.lblDeliveryDate);
            this.pnlDocumentInner.Controls.Add(this.txtRemarks);
            this.pnlDocumentInner.Controls.Add(this.txtLRno);
            this.pnlDocumentInner.Controls.Add(this.dtLRdate);
            this.pnlDocumentInner.Controls.Add(this.label8);
            this.pnlDocumentInner.Controls.Add(this.label6);
            this.pnlDocumentInner.Controls.Add(this.label5);
            this.pnlDocumentInner.Controls.Add(this.label4);
            this.pnlDocumentInner.Controls.Add(this.label3);
            this.pnlDocumentInner.Controls.Add(this.label1);
            this.pnlDocumentInner.Controls.Add(this.dtDocumentDate);
            this.pnlDocumentInner.Controls.Add(this.cmbTransportationmode);
            this.pnlDocumentInner.Controls.Add(this.cmbConsignee);
            this.pnlDocumentInner.Controls.Add(this.btnSave);
            this.pnlDocumentInner.Controls.Add(this.btnCancel);
            this.pnlDocumentInner.Controls.Add(this.label7);
            this.pnlDocumentInner.Controls.Add(this.label2);
            this.pnlDocumentInner.Controls.Add(this.txtDocno);
            this.pnlDocumentInner.Location = new System.Drawing.Point(18, 15);
            this.pnlDocumentInner.Name = "pnlDocumentInner";
            this.pnlDocumentInner.Size = new System.Drawing.Size(754, 372);
            this.pnlDocumentInner.TabIndex = 0;
            // 
            // btnAddRemarks
            // 
            this.btnAddRemarks.Location = new System.Drawing.Point(560, 319);
            this.btnAddRemarks.Name = "btnAddRemarks";
            this.btnAddRemarks.Size = new System.Drawing.Size(127, 23);
            this.btnAddRemarks.TabIndex = 40;
            this.btnAddRemarks.Text = "Add Remarks";
            this.btnAddRemarks.UseVisualStyleBackColor = true;
            this.btnAddRemarks.Click += new System.EventHandler(this.btnAddRemarks_Click);
            // 
            // dtDeliveryDate
            // 
            this.dtDeliveryDate.Location = new System.Drawing.Point(535, 146);
            this.dtDeliveryDate.Name = "dtDeliveryDate";
            this.dtDeliveryDate.Size = new System.Drawing.Size(144, 20);
            this.dtDeliveryDate.TabIndex = 39;
            // 
            // cmbDeliveryStatus
            // 
            this.cmbDeliveryStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeliveryStatus.FormattingEnabled = true;
            this.cmbDeliveryStatus.Items.AddRange(new object[] {
            "In Transit",
            "Delivered"});
            this.cmbDeliveryStatus.Location = new System.Drawing.Point(143, 146);
            this.cmbDeliveryStatus.Name = "cmbDeliveryStatus";
            this.cmbDeliveryStatus.Size = new System.Drawing.Size(152, 21);
            this.cmbDeliveryStatus.TabIndex = 38;
            this.cmbDeliveryStatus.SelectedIndexChanged += new System.EventHandler(this.cmbDeliveryStatus_SelectedIndexChanged);
            // 
            // lblDeliveryDate
            // 
            this.lblDeliveryDate.AutoSize = true;
            this.lblDeliveryDate.Location = new System.Drawing.Point(406, 149);
            this.lblDeliveryDate.Name = "lblDeliveryDate";
            this.lblDeliveryDate.Size = new System.Drawing.Size(71, 13);
            this.lblDeliveryDate.TabIndex = 36;
            this.lblDeliveryDate.Text = "Delivery Date";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(143, 183);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemarks.Size = new System.Drawing.Size(544, 133);
            this.txtRemarks.TabIndex = 34;
            // 
            // txtLRno
            // 
            this.txtLRno.Location = new System.Drawing.Point(539, 24);
            this.txtLRno.Name = "txtLRno";
            this.txtLRno.Size = new System.Drawing.Size(140, 20);
            this.txtLRno.TabIndex = 33;
            // 
            // dtLRdate
            // 
            this.dtLRdate.Location = new System.Drawing.Point(539, 61);
            this.dtLRdate.Name = "dtLRdate";
            this.dtLRdate.Size = new System.Drawing.Size(140, 20);
            this.dtLRdate.TabIndex = 32;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(56, 202);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Remarks";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "Delivery Status";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(448, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "LR Date";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(457, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "LR No";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(390, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Transportation Mode";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Document Date";
            // 
            // dtDocumentDate
            // 
            this.dtDocumentDate.Location = new System.Drawing.Point(143, 67);
            this.dtDocumentDate.Name = "dtDocumentDate";
            this.dtDocumentDate.Size = new System.Drawing.Size(140, 20);
            this.dtDocumentDate.TabIndex = 25;
            // 
            // cmbTransportationmode
            // 
            this.cmbTransportationmode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTransportationmode.FormattingEnabled = true;
            this.cmbTransportationmode.Location = new System.Drawing.Point(539, 107);
            this.cmbTransportationmode.Name = "cmbTransportationmode";
            this.cmbTransportationmode.Size = new System.Drawing.Size(152, 21);
            this.cmbTransportationmode.TabIndex = 24;
            // 
            // cmbConsignee
            // 
            this.cmbConsignee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConsignee.FormattingEnabled = true;
            this.cmbConsignee.Location = new System.Drawing.Point(143, 112);
            this.cmbConsignee.Name = "cmbConsignee";
            this.cmbConsignee.Size = new System.Drawing.Size(152, 21);
            this.cmbConsignee.TabIndex = 23;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::CSLERP.Properties.Resources.accept;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(158, 333);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::CSLERP.Properties.Resources.cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(53, 333);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(48, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Consignee";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Document No";
            // 
            // txtDocno
            // 
            this.txtDocno.Location = new System.Drawing.Point(143, 28);
            this.txtDocno.Name = "txtDocno";
            this.txtDocno.Size = new System.Drawing.Size(140, 20);
            this.txtDocno.TabIndex = 0;
            // 
            // pnlSelection
            // 
            this.pnlSelection.Controls.Add(this.lblDeliveryStatus);
            this.pnlSelection.Controls.Add(this.rdbDelivered);
            this.pnlSelection.Controls.Add(this.rdbInTransit);
            this.pnlSelection.Location = new System.Drawing.Point(3, 3);
            this.pnlSelection.Name = "pnlSelection";
            this.pnlSelection.Size = new System.Drawing.Size(198, 73);
            this.pnlSelection.TabIndex = 14;
            // 
            // lblDeliveryStatus
            // 
            this.lblDeliveryStatus.AutoSize = true;
            this.lblDeliveryStatus.Location = new System.Drawing.Point(9, 16);
            this.lblDeliveryStatus.Name = "lblDeliveryStatus";
            this.lblDeliveryStatus.Size = new System.Drawing.Size(111, 13);
            this.lblDeliveryStatus.TabIndex = 16;
            this.lblDeliveryStatus.Text = "Select Delivery Status";
            // 
            // rdbDelivered
            // 
            this.rdbDelivered.AutoSize = true;
            this.rdbDelivered.Location = new System.Drawing.Point(123, 45);
            this.rdbDelivered.Name = "rdbDelivered";
            this.rdbDelivered.Size = new System.Drawing.Size(70, 17);
            this.rdbDelivered.TabIndex = 15;
            this.rdbDelivered.TabStop = true;
            this.rdbDelivered.Text = "Delivered";
            this.rdbDelivered.UseVisualStyleBackColor = true;
            this.rdbDelivered.CheckedChanged += new System.EventHandler(this.rdbDelivered_CheckedChanged);
            // 
            // rdbInTransit
            // 
            this.rdbInTransit.AutoSize = true;
            this.rdbInTransit.Location = new System.Drawing.Point(12, 45);
            this.rdbInTransit.Name = "rdbInTransit";
            this.rdbInTransit.Size = new System.Drawing.Size(69, 17);
            this.rdbInTransit.TabIndex = 14;
            this.rdbInTransit.TabStop = true;
            this.rdbInTransit.Text = "In Transit";
            this.rdbInTransit.UseVisualStyleBackColor = true;
            this.rdbInTransit.CheckedChanged += new System.EventHandler(this.rdbInTransit_CheckedChanged);
            // 
            // pnlDocumentList
            // 
            this.pnlDocumentList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlDocumentList.Controls.Add(this.grdList);
            this.pnlDocumentList.Location = new System.Drawing.Point(67, 137);
            this.pnlDocumentList.Name = "pnlDocumentList";
            this.pnlDocumentList.Size = new System.Drawing.Size(1010, 307);
            this.pnlDocumentList.TabIndex = 6;
            this.pnlDocumentList.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDocumentList_Paint);
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
            this.gDeliveryDocumentType,
            this.gDocumentNo,
            this.gDocumentDate,
            this.gConsignee,
            this.gCourierID,
            this.gTransportationMode,
            this.gLRNo,
            this.gLRDate,
            this.gCreateUser,
            this.gCreateTime,
            this.gStatus,
            this.gRemarks,
            this.gDeliveryStatus,
            this.gDeliveryDate,
            this.gEdit,
            this.gView});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdList.DefaultCellStyle = dataGridViewCellStyle9;
            this.grdList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdList.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdList.Location = new System.Drawing.Point(3, 3);
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
            this.grdList.Size = new System.Drawing.Size(1004, 299);
            this.grdList.TabIndex = 5;
            this.grdList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdList_CellContentClick_1);
            // 
            // gDeliveryDocumentType
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gDeliveryDocumentType.DefaultCellStyle = dataGridViewCellStyle3;
            this.gDeliveryDocumentType.HeaderText = "Delivery Document Type";
            this.gDeliveryDocumentType.Name = "gDeliveryDocumentType";
            // 
            // gDocumentNo
            // 
            this.gDocumentNo.HeaderText = "Document No";
            this.gDocumentNo.Name = "gDocumentNo";
            this.gDocumentNo.Visible = false;
            // 
            // gDocumentDate
            // 
            dataGridViewCellStyle4.Format = "dd-MM-yyyy";
            this.gDocumentDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.gDocumentDate.HeaderText = "Document Date";
            this.gDocumentDate.Name = "gDocumentDate";
            // 
            // gConsignee
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gConsignee.DefaultCellStyle = dataGridViewCellStyle5;
            this.gConsignee.HeaderText = "Consignee";
            this.gConsignee.Name = "gConsignee";
            // 
            // gCourierID
            // 
            this.gCourierID.HeaderText = "CourierID";
            this.gCourierID.Name = "gCourierID";
            // 
            // gTransportationMode
            // 
            this.gTransportationMode.HeaderText = "Transportation Mode";
            this.gTransportationMode.Name = "gTransportationMode";
            // 
            // gLRNo
            // 
            this.gLRNo.HeaderText = "LRNo";
            this.gLRNo.Name = "gLRNo";
            // 
            // gLRDate
            // 
            dataGridViewCellStyle6.Format = "dd-MM-yyyy";
            this.gLRDate.DefaultCellStyle = dataGridViewCellStyle6;
            this.gLRDate.HeaderText = "LRDate";
            this.gLRDate.Name = "gLRDate";
            // 
            // gCreateUser
            // 
            this.gCreateUser.HeaderText = "CreateUser";
            this.gCreateUser.Name = "gCreateUser";
            // 
            // gCreateTime
            // 
            dataGridViewCellStyle7.Format = "dd-MM-yyyy HH:mm:ss";
            this.gCreateTime.DefaultCellStyle = dataGridViewCellStyle7;
            this.gCreateTime.HeaderText = "CreateTime";
            this.gCreateTime.Name = "gCreateTime";
            // 
            // gStatus
            // 
            this.gStatus.HeaderText = "Status";
            this.gStatus.Name = "gStatus";
            this.gStatus.Visible = false;
            // 
            // gRemarks
            // 
            this.gRemarks.HeaderText = "Remarks";
            this.gRemarks.Name = "gRemarks";
            this.gRemarks.Visible = false;
            // 
            // gDeliveryStatus
            // 
            this.gDeliveryStatus.HeaderText = "Delivery Status";
            this.gDeliveryStatus.Name = "gDeliveryStatus";
            // 
            // gDeliveryDate
            // 
            dataGridViewCellStyle8.Format = "dd-MM-yyyy";
            this.gDeliveryDate.DefaultCellStyle = dataGridViewCellStyle8;
            this.gDeliveryDate.HeaderText = "Delivery Date";
            this.gDeliveryDate.Name = "gDeliveryDate";
            // 
            // gEdit
            // 
            this.gEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.gEdit.HeaderText = "Edit";
            this.gEdit.Name = "gEdit";
            this.gEdit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gEdit.Text = "Edit";
            this.gEdit.ToolTipText = "Edit Employee";
            this.gEdit.UseColumnTextForButtonValue = true;
            // 
            // gView
            // 
            this.gView.HeaderText = "View";
            this.gView.Name = "gView";
            this.gView.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gView.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.gView.Text = "View";
            this.gView.UseColumnTextForButtonValue = true;
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
            // MaterialDeliveryStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "MaterialDeliveryStatus";
            this.Text = "ERP User";
            this.Load += new System.EventHandler(this.MaterialDeliveryStatus_Load);
            this.Enter += new System.EventHandler(this.MaterialDeliveryStatus_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlDocumentOuter.ResumeLayout(false);
            this.pnlDocumentInner.ResumeLayout(false);
            this.pnlDocumentInner.PerformLayout();
            this.pnlSelection.ResumeLayout(false);
            this.pnlSelection.PerformLayout();
            this.pnlDocumentList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.pnlBottomButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Panel pnlDocumentOuter;
        private System.Windows.Forms.Panel pnlDocumentInner;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDocno;
        private System.Windows.Forms.Panel pnlDocumentList;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.TextBox txtLRno;
        private System.Windows.Forms.DateTimePicker dtLRdate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtDocumentDate;
        private System.Windows.Forms.ComboBox cmbTransportationmode;
        private System.Windows.Forms.ComboBox cmbConsignee;
        private System.Windows.Forms.Panel pnlSelection;
        private System.Windows.Forms.Label lblDeliveryStatus;
        private System.Windows.Forms.RadioButton rdbDelivered;
        private System.Windows.Forms.RadioButton rdbInTransit;
        private System.Windows.Forms.Label lblDeliveryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn gDeliveryDocumentType;
        private System.Windows.Forms.DataGridViewTextBoxColumn gDocumentNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn gDocumentDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn gConsignee;
        private System.Windows.Forms.DataGridViewTextBoxColumn gCourierID;
        private System.Windows.Forms.DataGridViewTextBoxColumn gTransportationMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn gLRNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn gLRDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn gCreateUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn gCreateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn gStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn gRemarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn gDeliveryStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn gDeliveryDate;
        private System.Windows.Forms.DataGridViewButtonColumn gEdit;
        private System.Windows.Forms.DataGridViewButtonColumn gView;
        private System.Windows.Forms.ComboBox cmbDeliveryStatus;
        private System.Windows.Forms.DateTimePicker dtDeliveryDate;
        private System.Windows.Forms.Button btnAddRemarks;
    }
}