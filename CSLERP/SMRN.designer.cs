namespace CSLERP
{
    partial class SMRN
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
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblActionHeader = new System.Windows.Forms.Label();
            this.pnlTopButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnActionPending = new System.Windows.Forms.Button();
            this.btnApprovalPending = new System.Windows.Forms.Button();
            this.btnApproved = new System.Windows.Forms.Button();
            this.pnlOuter = new System.Windows.Forms.Panel();
            this.pnlEditButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnForward = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnReverse = new System.Windows.Forms.Button();
            this.pnlInner = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNoOfPacket = new System.Windows.Forms.TextBox();
            this.cmbCourierID = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.txtRemarks = new System.Windows.Forms.RichTextBox();
            this.dtCustomerDocDate = new System.Windows.Forms.DateTimePicker();
            this.dtSMRNDate = new System.Windows.Forms.DateTimePicker();
            this.txtCuxtomerDocNo = new System.Windows.Forms.TextBox();
            this.txtSMRNNo = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlList = new System.Windows.Forms.Panel();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.DocumentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SMRNNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SMRNDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerDocumentNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerDocumentDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CourierID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CourierName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoOfPacket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ForwardUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ApproveUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Creator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Forwarder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Approver = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Forwarders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Approve = new System.Windows.Forms.DataGridViewButtonColumn();
            this.View = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlUI.SuspendLayout();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlTopButtons.SuspendLayout();
            this.pnlOuter.SuspendLayout();
            this.pnlEditButtons.SuspendLayout();
            this.pnlInner.SuspendLayout();
            this.pnlList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.pnlBottomButtons);
            this.pnlUI.Controls.Add(this.lblActionHeader);
            this.pnlUI.Controls.Add(this.pnlTopButtons);
            this.pnlUI.Controls.Add(this.pnlOuter);
            this.pnlUI.Controls.Add(this.pnlList);
            this.pnlUI.Location = new System.Drawing.Point(15, 15);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(1100, 540);
            this.pnlUI.TabIndex = 8;
            // 
            // pnlBottomButtons
            // 
            this.pnlBottomButtons.Controls.Add(this.btnNew);
            this.pnlBottomButtons.Controls.Add(this.btnExit);
            this.pnlBottomButtons.Location = new System.Drawing.Point(15, 510);
            this.pnlBottomButtons.Name = "pnlBottomButtons";
            this.pnlBottomButtons.Size = new System.Drawing.Size(510, 28);
            this.pnlBottomButtons.TabIndex = 13;
            // 
            // btnNew
            // 
            this.btnNew.Image = global::CSLERP.Properties.Resources._new;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(3, 3);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 5;
            this.btnNew.Text = "New ";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnExit
            // 
            this.btnExit.Image = global::CSLERP.Properties.Resources.cancel;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(84, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblActionHeader
            // 
            this.lblActionHeader.AutoSize = true;
            this.lblActionHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActionHeader.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblActionHeader.Location = new System.Drawing.Point(705, 9);
            this.lblActionHeader.Name = "lblActionHeader";
            this.lblActionHeader.Size = new System.Drawing.Size(0, 17);
            this.lblActionHeader.TabIndex = 12;
            // 
            // pnlTopButtons
            // 
            this.pnlTopButtons.Controls.Add(this.btnActionPending);
            this.pnlTopButtons.Controls.Add(this.btnApprovalPending);
            this.pnlTopButtons.Controls.Add(this.btnApproved);
            this.pnlTopButtons.Location = new System.Drawing.Point(12, 3);
            this.pnlTopButtons.Name = "pnlTopButtons";
            this.pnlTopButtons.Size = new System.Drawing.Size(463, 28);
            this.pnlTopButtons.TabIndex = 11;
            // 
            // btnActionPending
            // 
            this.btnActionPending.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnActionPending.Location = new System.Drawing.Point(3, 3);
            this.btnActionPending.Name = "btnActionPending";
            this.btnActionPending.Size = new System.Drawing.Size(103, 23);
            this.btnActionPending.TabIndex = 5;
            this.btnActionPending.Text = "Action Pending";
            this.btnActionPending.UseVisualStyleBackColor = true;
            this.btnActionPending.Click += new System.EventHandler(this.btnActionPending_Click);
            // 
            // btnApprovalPending
            // 
            this.btnApprovalPending.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApprovalPending.Location = new System.Drawing.Point(112, 3);
            this.btnApprovalPending.Name = "btnApprovalPending";
            this.btnApprovalPending.Size = new System.Drawing.Size(103, 23);
            this.btnApprovalPending.TabIndex = 11;
            this.btnApprovalPending.Text = "Approval Pending";
            this.btnApprovalPending.UseVisualStyleBackColor = true;
            this.btnApprovalPending.Click += new System.EventHandler(this.btnApprovalPending_Click_1);
            // 
            // btnApproved
            // 
            this.btnApproved.Location = new System.Drawing.Point(221, 3);
            this.btnApproved.Name = "btnApproved";
            this.btnApproved.Size = new System.Drawing.Size(75, 23);
            this.btnApproved.TabIndex = 10;
            this.btnApproved.Text = "Approved";
            this.btnApproved.UseVisualStyleBackColor = true;
            this.btnApproved.Click += new System.EventHandler(this.btnApproved_Click_1);
            // 
            // pnlOuter
            // 
            this.pnlOuter.BackColor = System.Drawing.Color.DarkGray;
            this.pnlOuter.Controls.Add(this.pnlEditButtons);
            this.pnlOuter.Controls.Add(this.pnlInner);
            this.pnlOuter.Location = new System.Drawing.Point(115, 32);
            this.pnlOuter.Name = "pnlOuter";
            this.pnlOuter.Size = new System.Drawing.Size(856, 415);
            this.pnlOuter.TabIndex = 8;
            this.pnlOuter.Visible = false;
            // 
            // pnlEditButtons
            // 
            this.pnlEditButtons.Controls.Add(this.btnForward);
            this.pnlEditButtons.Controls.Add(this.btnCancel);
            this.pnlEditButtons.Controls.Add(this.btnSave);
            this.pnlEditButtons.Controls.Add(this.btnApprove);
            this.pnlEditButtons.Controls.Add(this.btnReverse);
            this.pnlEditButtons.Location = new System.Drawing.Point(10, 378);
            this.pnlEditButtons.Name = "pnlEditButtons";
            this.pnlEditButtons.Size = new System.Drawing.Size(753, 32);
            this.pnlEditButtons.TabIndex = 71;
            // 
            // btnForward
            // 
            this.btnForward.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnForward.Location = new System.Drawing.Point(3, 3);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(75, 23);
            this.btnForward.TabIndex = 37;
            this.btnForward.Text = "Forward";
            this.btnForward.UseVisualStyleBackColor = true;
            this.btnForward.Visible = false;
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::CSLERP.Properties.Resources.cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(84, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::CSLERP.Properties.Resources.accept;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(165, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(246, 3);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(75, 23);
            this.btnApprove.TabIndex = 38;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // btnReverse
            // 
            this.btnReverse.Location = new System.Drawing.Point(327, 3);
            this.btnReverse.Name = "btnReverse";
            this.btnReverse.Size = new System.Drawing.Size(75, 23);
            this.btnReverse.TabIndex = 39;
            this.btnReverse.Text = "Reverse";
            this.btnReverse.UseVisualStyleBackColor = true;
            this.btnReverse.Click += new System.EventHandler(this.btnReverse_Click);
            // 
            // pnlInner
            // 
            this.pnlInner.BackColor = System.Drawing.Color.White;
            this.pnlInner.Controls.Add(this.label5);
            this.pnlInner.Controls.Add(this.txtNoOfPacket);
            this.pnlInner.Controls.Add(this.cmbCourierID);
            this.pnlInner.Controls.Add(this.label7);
            this.pnlInner.Controls.Add(this.cmbCustomer);
            this.pnlInner.Controls.Add(this.txtRemarks);
            this.pnlInner.Controls.Add(this.dtCustomerDocDate);
            this.pnlInner.Controls.Add(this.dtSMRNDate);
            this.pnlInner.Controls.Add(this.txtCuxtomerDocNo);
            this.pnlInner.Controls.Add(this.txtSMRNNo);
            this.pnlInner.Controls.Add(this.label13);
            this.pnlInner.Controls.Add(this.label9);
            this.pnlInner.Controls.Add(this.label8);
            this.pnlInner.Controls.Add(this.label4);
            this.pnlInner.Controls.Add(this.label3);
            this.pnlInner.Controls.Add(this.label2);
            this.pnlInner.Controls.Add(this.label1);
            this.pnlInner.Location = new System.Drawing.Point(12, 15);
            this.pnlInner.Name = "pnlInner";
            this.pnlInner.Size = new System.Drawing.Size(828, 362);
            this.pnlInner.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(367, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 73;
            this.label5.Text = "No Of packets";
            // 
            // txtNoOfPacket
            // 
            this.txtNoOfPacket.Location = new System.Drawing.Point(449, 141);
            this.txtNoOfPacket.Name = "txtNoOfPacket";
            this.txtNoOfPacket.Size = new System.Drawing.Size(100, 20);
            this.txtNoOfPacket.TabIndex = 72;
            // 
            // cmbCourierID
            // 
            this.cmbCourierID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCourierID.FormattingEnabled = true;
            this.cmbCourierID.Location = new System.Drawing.Point(449, 100);
            this.cmbCourierID.Name = "cmbCourierID";
            this.cmbCourierID.Size = new System.Drawing.Size(172, 21);
            this.cmbCourierID.TabIndex = 71;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(389, 104);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 70;
            this.label7.Text = "Courier ID";
            // 
            // cmbCustomer
            // 
            this.cmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomer.FormattingEnabled = true;
            this.cmbCustomer.Location = new System.Drawing.Point(449, 64);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(172, 21);
            this.cmbCustomer.TabIndex = 67;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(156, 229);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(542, 73);
            this.txtRemarks.TabIndex = 63;
            this.txtRemarks.Text = "";
            // 
            // dtCustomerDocDate
            // 
            this.dtCustomerDocDate.CustomFormat = "";
            this.dtCustomerDocDate.Location = new System.Drawing.Point(197, 162);
            this.dtCustomerDocDate.Name = "dtCustomerDocDate";
            this.dtCustomerDocDate.Size = new System.Drawing.Size(100, 20);
            this.dtCustomerDocDate.TabIndex = 54;
            // 
            // dtSMRNDate
            // 
            this.dtSMRNDate.CustomFormat = "";
            this.dtSMRNDate.Location = new System.Drawing.Point(197, 91);
            this.dtSMRNDate.Name = "dtSMRNDate";
            this.dtSMRNDate.Size = new System.Drawing.Size(100, 20);
            this.dtSMRNDate.TabIndex = 53;
            // 
            // txtCuxtomerDocNo
            // 
            this.txtCuxtomerDocNo.Location = new System.Drawing.Point(197, 126);
            this.txtCuxtomerDocNo.Name = "txtCuxtomerDocNo";
            this.txtCuxtomerDocNo.Size = new System.Drawing.Size(100, 20);
            this.txtCuxtomerDocNo.TabIndex = 52;
            // 
            // txtSMRNNo
            // 
            this.txtSMRNNo.Location = new System.Drawing.Point(197, 59);
            this.txtSMRNNo.Name = "txtSMRNNo";
            this.txtSMRNNo.Size = new System.Drawing.Size(100, 20);
            this.txtSMRNNo.TabIndex = 51;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(465, 229);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(0, 13);
            this.label13.TabIndex = 50;
            this.label13.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(392, 68);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 46;
            this.label9.Text = "Customer";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(79, 229);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 45;
            this.label8.Text = "Remarks";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "CustomerDocument Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(70, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "CustomerDocument No";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(122, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 39;
            this.label2.Text = "SMRN Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(131, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "SMRN No";
            // 
            // pnlList
            // 
            this.pnlList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlList.Controls.Add(this.grdList);
            this.pnlList.Location = new System.Drawing.Point(9, 32);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(1043, 462);
            this.pnlList.TabIndex = 6;
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
            this.grdList.ColumnHeadersHeight = 35;
            this.grdList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DocumentID,
            this.DocumentName,
            this.SMRNNo,
            this.SMRNDate,
            this.CustomerID,
            this.CustomerName,
            this.CustomerDocumentNo,
            this.CustomerDocumentDate,
            this.CourierID,
            this.CourierName,
            this.NoOfPacket,
            this.Remarks,
            this.Status,
            this.DocumentStatus,
            this.CreateTime,
            this.CreateUser,
            this.ForwardUser,
            this.ApproveUser,
            this.Creator,
            this.Forwarder,
            this.Approver,
            this.Forwarders,
            this.Edit,
            this.Approve,
            this.View});
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
            this.grdList.Location = new System.Drawing.Point(8, 23);
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
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.AliceBlue;
            this.grdList.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.grdList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            this.grdList.Size = new System.Drawing.Size(1027, 297);
            this.grdList.TabIndex = 4;
            this.grdList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdList_CellContentClick);
            // 
            // DocumentID
            // 
            this.DocumentID.Frozen = true;
            this.DocumentID.HeaderText = "Document ID";
            this.DocumentID.Name = "DocumentID";
            // 
            // DocumentName
            // 
            this.DocumentName.Frozen = true;
            this.DocumentName.HeaderText = "Document Name";
            this.DocumentName.Name = "DocumentName";
            // 
            // SMRNNo
            // 
            this.SMRNNo.Frozen = true;
            this.SMRNNo.HeaderText = "SMRN No";
            this.SMRNNo.Name = "SMRNNo";
            // 
            // SMRNDate
            // 
            dataGridViewCellStyle3.Format = "dd-MM-yyyy";
            dataGridViewCellStyle3.NullValue = null;
            this.SMRNDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.SMRNDate.Frozen = true;
            this.SMRNDate.HeaderText = "SMRN Date";
            this.SMRNDate.Name = "SMRNDate";
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
            // CustomerDocumentNo
            // 
            this.CustomerDocumentNo.HeaderText = "Customer Document No";
            this.CustomerDocumentNo.Name = "CustomerDocumentNo";
            // 
            // CustomerDocumentDate
            // 
            dataGridViewCellStyle4.Format = "dd-MM-yyyy";
            dataGridViewCellStyle4.NullValue = null;
            this.CustomerDocumentDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.CustomerDocumentDate.HeaderText = "Customer Document Date";
            this.CustomerDocumentDate.Name = "CustomerDocumentDate";
            // 
            // CourierID
            // 
            this.CourierID.HeaderText = "Courier ID";
            this.CourierID.Name = "CourierID";
            // 
            // CourierName
            // 
            this.CourierName.HeaderText = "Courier Name";
            this.CourierName.Name = "CourierName";
            // 
            // NoOfPacket
            // 
            this.NoOfPacket.HeaderText = "No Of Packet";
            this.NoOfPacket.MaxInputLength = 3276760;
            this.NoOfPacket.Name = "NoOfPacket";
            // 
            // Remarks
            // 
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            // 
            // Status
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Status.DefaultCellStyle = dataGridViewCellStyle5;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            // 
            // DocumentStatus
            // 
            this.DocumentStatus.HeaderText = "Document Status";
            this.DocumentStatus.Name = "DocumentStatus";
            // 
            // CreateTime
            // 
            dataGridViewCellStyle6.Format = "dd-MM-yyyy HH:mm:ss";
            dataGridViewCellStyle6.NullValue = null;
            this.CreateTime.DefaultCellStyle = dataGridViewCellStyle6;
            this.CreateTime.HeaderText = "Create Time";
            this.CreateTime.Name = "CreateTime";
            // 
            // CreateUser
            // 
            this.CreateUser.HeaderText = "Create User";
            this.CreateUser.Name = "CreateUser";
            // 
            // ForwardUser
            // 
            this.ForwardUser.HeaderText = "Forward User";
            this.ForwardUser.Name = "ForwardUser";
            // 
            // ApproveUser
            // 
            this.ApproveUser.HeaderText = "Approve User";
            this.ApproveUser.Name = "ApproveUser";
            // 
            // Creator
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Creator.DefaultCellStyle = dataGridViewCellStyle7;
            this.Creator.HeaderText = "Creator";
            this.Creator.Name = "Creator";
            // 
            // Forwarder
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Forwarder.DefaultCellStyle = dataGridViewCellStyle8;
            this.Forwarder.HeaderText = "Forwarder";
            this.Forwarder.Name = "Forwarder";
            // 
            // Approver
            // 
            this.Approver.HeaderText = "Approver";
            this.Approver.Name = "Approver";
            // 
            // Forwarders
            // 
            this.Forwarders.HeaderText = "Forwarder List";
            this.Forwarders.Name = "Forwarders";
            // 
            // Edit
            // 
            this.Edit.HeaderText = "Edit";
            this.Edit.Name = "Edit";
            this.Edit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Edit.Text = "Edit";
            this.Edit.ToolTipText = "Edit Employee";
            this.Edit.UseColumnTextForButtonValue = true;
            // 
            // Approve
            // 
            this.Approve.HeaderText = "Approve";
            this.Approve.Name = "Approve";
            this.Approve.Text = "Forward/Approve";
            this.Approve.ToolTipText = "Forward/Approve";
            this.Approve.UseColumnTextForButtonValue = true;
            // 
            // View
            // 
            this.View.HeaderText = "View";
            this.View.Name = "View";
            this.View.Text = "View";
            this.View.UseColumnTextForButtonValue = true;
            // 
            // SMRN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1096, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "SMRN";
            this.Text = "Currency Vs INR";
            this.Load += new System.EventHandler(this.SMRN_Load);
            this.Enter += new System.EventHandler(this.SMRN_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlUI.PerformLayout();
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlTopButtons.ResumeLayout(false);
            this.pnlOuter.ResumeLayout(false);
            this.pnlEditButtons.ResumeLayout(false);
            this.pnlInner.ResumeLayout(false);
            this.pnlInner.PerformLayout();
            this.pnlList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Panel pnlOuter;
        private System.Windows.Forms.Panel pnlInner;
        private System.Windows.Forms.Panel pnlList;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.Button btnActionPending;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtCustomerDocDate;
        private System.Windows.Forms.DateTimePicker dtSMRNDate;
        private System.Windows.Forms.TextBox txtCuxtomerDocNo;
        private System.Windows.Forms.TextBox txtSMRNNo;
        private System.Windows.Forms.RichTextBox txtRemarks;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.Button btnApproved;
        private System.Windows.Forms.Label lblActionHeader;
        private System.Windows.Forms.FlowLayoutPanel pnlTopButtons;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.FlowLayoutPanel pnlEditButtons;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtNoOfPacket;
        private System.Windows.Forms.ComboBox cmbCourierID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnApprovalPending;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Button btnReverse;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SMRNNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn SMRNDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerDocumentNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerDocumentDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn CourierID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CourierName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoOfPacket;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn ForwardUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn ApproveUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn Creator;
        private System.Windows.Forms.DataGridViewTextBoxColumn Forwarder;
        private System.Windows.Forms.DataGridViewTextBoxColumn Approver;
        private System.Windows.Forms.DataGridViewTextBoxColumn Forwarders;
        private System.Windows.Forms.DataGridViewButtonColumn Edit;
        private System.Windows.Forms.DataGridViewButtonColumn Approve;
        private System.Windows.Forms.DataGridViewButtonColumn View;
    }
}