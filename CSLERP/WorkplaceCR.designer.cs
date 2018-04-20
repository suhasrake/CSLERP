namespace CSLERP
{
    partial class WorkplaceCR
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
            this.pnlUI = new System.Windows.Forms.Panel();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblActionHeader = new System.Windows.Forms.Label();
            this.pnlTopButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnComplaintReg = new System.Windows.Forms.Button();
            this.btnCmplntAccepted = new System.Windows.Forms.Button();
            this.btnCmplntStatus = new System.Windows.Forms.Button();
            this.pnlOuter = new System.Windows.Forms.Panel();
            this.pnlEditButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancelComplaint = new System.Windows.Forms.Button();
            this.btnReject = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRejectCLosing = new System.Windows.Forms.Button();
            this.pnlInner = new System.Windows.Forms.Panel();
            this.txtComplDesc = new System.Windows.Forms.TextBox();
            this.btnAddComment = new System.Windows.Forms.Button();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dtAcceptTime = new System.Windows.Forms.DateTimePicker();
            this.lblAcceptTime = new System.Windows.Forms.Label();
            this.lblCloseTime = new System.Windows.Forms.Label();
            this.dtCloseTime = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbComplaintType = new System.Windows.Forms.ComboBox();
            this.txtEmpId = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlList = new System.Windows.Forms.Panel();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.RowID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmpId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmployeeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComplaintType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComplaintDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AcceptTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CloseTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Creator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewButtonColumn();
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
            this.btnNew.TabIndex = 4;
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
            this.btnExit.TabIndex = 5;
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
            this.pnlTopButtons.Controls.Add(this.btnComplaintReg);
            this.pnlTopButtons.Controls.Add(this.btnCmplntAccepted);
            this.pnlTopButtons.Controls.Add(this.btnCmplntStatus);
            this.pnlTopButtons.Location = new System.Drawing.Point(12, 3);
            this.pnlTopButtons.Name = "pnlTopButtons";
            this.pnlTopButtons.Size = new System.Drawing.Size(463, 28);
            this.pnlTopButtons.TabIndex = 11;
            // 
            // btnComplaintReg
            // 
            this.btnComplaintReg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnComplaintReg.Location = new System.Drawing.Point(3, 3);
            this.btnComplaintReg.Name = "btnComplaintReg";
            this.btnComplaintReg.Size = new System.Drawing.Size(87, 23);
            this.btnComplaintReg.TabIndex = 0;
            this.btnComplaintReg.Text = "Registered";
            this.btnComplaintReg.UseVisualStyleBackColor = true;
            this.btnComplaintReg.Click += new System.EventHandler(this.btnRegisteredCompl_Click_1);
            // 
            // btnCmplntAccepted
            // 
            this.btnCmplntAccepted.Location = new System.Drawing.Point(96, 3);
            this.btnCmplntAccepted.Name = "btnCmplntAccepted";
            this.btnCmplntAccepted.Size = new System.Drawing.Size(75, 23);
            this.btnCmplntAccepted.TabIndex = 1;
            this.btnCmplntAccepted.Text = "Accepted";
            this.btnCmplntAccepted.UseVisualStyleBackColor = true;
            this.btnCmplntAccepted.Click += new System.EventHandler(this.btnAcceptedComp_Click);
            // 
            // btnCmplntStatus
            // 
            this.btnCmplntStatus.Location = new System.Drawing.Point(177, 3);
            this.btnCmplntStatus.Name = "btnCmplntStatus";
            this.btnCmplntStatus.Size = new System.Drawing.Size(111, 23);
            this.btnCmplntStatus.TabIndex = 2;
            this.btnCmplntStatus.Text = "Complaint Status";
            this.btnCmplntStatus.UseVisualStyleBackColor = true;
            this.btnCmplntStatus.Click += new System.EventHandler(this.btnComplaintStatus_Click);
            // 
            // pnlOuter
            // 
            this.pnlOuter.BackColor = System.Drawing.Color.CornflowerBlue;
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
            this.pnlEditButtons.Controls.Add(this.btnCancel);
            this.pnlEditButtons.Controls.Add(this.btnSave);
            this.pnlEditButtons.Controls.Add(this.btnAccept);
            this.pnlEditButtons.Controls.Add(this.btnCancelComplaint);
            this.pnlEditButtons.Controls.Add(this.btnReject);
            this.pnlEditButtons.Controls.Add(this.btnClose);
            this.pnlEditButtons.Controls.Add(this.btnRejectCLosing);
            this.pnlEditButtons.Location = new System.Drawing.Point(10, 378);
            this.pnlEditButtons.Name = "pnlEditButtons";
            this.pnlEditButtons.Size = new System.Drawing.Size(753, 32);
            this.pnlEditButtons.TabIndex = 71;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::CSLERP.Properties.Resources.cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Back";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::CSLERP.Properties.Resources.accept;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(84, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(165, 3);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 6;
            this.btnAccept.Text = "Accept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // btnCancelComplaint
            // 
            this.btnCancelComplaint.Location = new System.Drawing.Point(246, 3);
            this.btnCancelComplaint.Name = "btnCancelComplaint";
            this.btnCancelComplaint.Size = new System.Drawing.Size(109, 23);
            this.btnCancelComplaint.TabIndex = 7;
            this.btnCancelComplaint.Text = "Cancel Complaint";
            this.btnCancelComplaint.UseVisualStyleBackColor = true;
            this.btnCancelComplaint.Click += new System.EventHandler(this.btnCancelDocument_Click);
            // 
            // btnReject
            // 
            this.btnReject.Location = new System.Drawing.Point(361, 3);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(75, 23);
            this.btnReject.TabIndex = 8;
            this.btnReject.Text = "Reject";
            this.btnReject.UseVisualStyleBackColor = true;
            this.btnReject.Visible = false;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(442, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 23);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close Complaint";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRejectCLosing
            // 
            this.btnRejectCLosing.Location = new System.Drawing.Point(550, 3);
            this.btnRejectCLosing.Name = "btnRejectCLosing";
            this.btnRejectCLosing.Size = new System.Drawing.Size(102, 23);
            this.btnRejectCLosing.TabIndex = 10;
            this.btnRejectCLosing.Text = "Reject Closing";
            this.btnRejectCLosing.UseVisualStyleBackColor = true;
            this.btnRejectCLosing.Click += new System.EventHandler(this.btnRejectCLosing_Click);
            // 
            // pnlInner
            // 
            this.pnlInner.BackColor = System.Drawing.Color.White;
            this.pnlInner.Controls.Add(this.txtComplDesc);
            this.pnlInner.Controls.Add(this.btnAddComment);
            this.pnlInner.Controls.Add(this.txtRemarks);
            this.pnlInner.Controls.Add(this.label8);
            this.pnlInner.Controls.Add(this.dtAcceptTime);
            this.pnlInner.Controls.Add(this.lblAcceptTime);
            this.pnlInner.Controls.Add(this.lblCloseTime);
            this.pnlInner.Controls.Add(this.dtCloseTime);
            this.pnlInner.Controls.Add(this.label3);
            this.pnlInner.Controls.Add(this.cmbComplaintType);
            this.pnlInner.Controls.Add(this.txtEmpId);
            this.pnlInner.Controls.Add(this.label13);
            this.pnlInner.Controls.Add(this.label9);
            this.pnlInner.Controls.Add(this.label1);
            this.pnlInner.Location = new System.Drawing.Point(12, 15);
            this.pnlInner.Name = "pnlInner";
            this.pnlInner.Size = new System.Drawing.Size(828, 362);
            this.pnlInner.TabIndex = 0;
            // 
            // txtComplDesc
            // 
            this.txtComplDesc.BackColor = System.Drawing.Color.White;
            this.txtComplDesc.Location = new System.Drawing.Point(165, 108);
            this.txtComplDesc.Multiline = true;
            this.txtComplDesc.Name = "txtComplDesc";
            this.txtComplDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtComplDesc.Size = new System.Drawing.Size(544, 56);
            this.txtComplDesc.TabIndex = 3;
            // 
            // btnAddComment
            // 
            this.btnAddComment.Location = new System.Drawing.Point(582, 283);
            this.btnAddComment.Name = "btnAddComment";
            this.btnAddComment.Size = new System.Drawing.Size(127, 23);
            this.btnAddComment.TabIndex = 4;
            this.btnAddComment.Text = "Add Remark";
            this.btnAddComment.UseVisualStyleBackColor = true;
            this.btnAddComment.Click += new System.EventHandler(this.btnAddRemarks_Click);
            // 
            // txtRemarks
            // 
            this.txtRemarks.BackColor = System.Drawing.Color.White;
            this.txtRemarks.Location = new System.Drawing.Point(165, 170);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ReadOnly = true;
            this.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemarks.Size = new System.Drawing.Size(544, 107);
            this.txtRemarks.TabIndex = 84;
            this.txtRemarks.TextChanged += new System.EventHandler(this.txtComment_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(110, 173);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 83;
            this.label8.Text = "Remarks";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // dtAcceptTime
            // 
            this.dtAcceptTime.CustomFormat = "";
            this.dtAcceptTime.Enabled = false;
            this.dtAcceptTime.Location = new System.Drawing.Point(598, 52);
            this.dtAcceptTime.Name = "dtAcceptTime";
            this.dtAcceptTime.Size = new System.Drawing.Size(180, 20);
            this.dtAcceptTime.TabIndex = 78;
            this.dtAcceptTime.Visible = false;
            // 
            // lblAcceptTime
            // 
            this.lblAcceptTime.AutoSize = true;
            this.lblAcceptTime.Enabled = false;
            this.lblAcceptTime.Location = new System.Drawing.Point(525, 56);
            this.lblAcceptTime.Name = "lblAcceptTime";
            this.lblAcceptTime.Size = new System.Drawing.Size(67, 13);
            this.lblAcceptTime.TabIndex = 76;
            this.lblAcceptTime.Text = "Accept Time";
            this.lblAcceptTime.Visible = false;
            // 
            // lblCloseTime
            // 
            this.lblCloseTime.AutoSize = true;
            this.lblCloseTime.Enabled = false;
            this.lblCloseTime.Location = new System.Drawing.Point(533, 82);
            this.lblCloseTime.Name = "lblCloseTime";
            this.lblCloseTime.Size = new System.Drawing.Size(59, 13);
            this.lblCloseTime.TabIndex = 77;
            this.lblCloseTime.Text = "Close Time";
            this.lblCloseTime.Visible = false;
            // 
            // dtCloseTime
            // 
            this.dtCloseTime.CustomFormat = "";
            this.dtCloseTime.Enabled = false;
            this.dtCloseTime.Location = new System.Drawing.Point(598, 78);
            this.dtCloseTime.Name = "dtCloseTime";
            this.dtCloseTime.Size = new System.Drawing.Size(180, 20);
            this.dtCloseTime.TabIndex = 79;
            this.dtCloseTime.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 74;
            this.label3.Text = "Complaint Description";
            // 
            // cmbComplaintType
            // 
            this.cmbComplaintType.BackColor = System.Drawing.Color.White;
            this.cmbComplaintType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbComplaintType.FormattingEnabled = true;
            this.cmbComplaintType.Location = new System.Drawing.Point(165, 78);
            this.cmbComplaintType.Name = "cmbComplaintType";
            this.cmbComplaintType.Size = new System.Drawing.Size(172, 21);
            this.cmbComplaintType.TabIndex = 2;
            // 
            // txtEmpId
            // 
            this.txtEmpId.BackColor = System.Drawing.Color.White;
            this.txtEmpId.Location = new System.Drawing.Point(165, 52);
            this.txtEmpId.Name = "txtEmpId";
            this.txtEmpId.Size = new System.Drawing.Size(78, 20);
            this.txtEmpId.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(515, 212);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(0, 13);
            this.label13.TabIndex = 50;
            this.label13.Visible = false;
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(79, 81);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 13);
            this.label9.TabIndex = 46;
            this.label9.Text = "Complaint Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(92, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "Employee ID";
            // 
            // pnlList
            // 
            this.pnlList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlList.Controls.Add(this.grdList);
            this.pnlList.Location = new System.Drawing.Point(9, 32);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(1053, 429);
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
            this.RowID,
            this.EmpId,
            this.EmployeeName,
            this.ComplaintType,
            this.ComplaintDesc,
            this.CreateTime,
            this.AcceptTime,
            this.CloseTime,
            this.Remarks,
            this.Status,
            this.DocumentStatus,
            this.CreateUser,
            this.Creator,
            this.Edit,
            this.View});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdList.DefaultCellStyle = dataGridViewCellStyle7;
            this.grdList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdList.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdList.Location = new System.Drawing.Point(9, 3);
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
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.AliceBlue;
            this.grdList.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.grdList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            this.grdList.Size = new System.Drawing.Size(1028, 421);
            this.grdList.TabIndex = 3;
            this.grdList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdList_CellContentClick);
            // 
            // RowID
            // 
            this.RowID.HeaderText = "RowID";
            this.RowID.Name = "RowID";
            this.RowID.Visible = false;
            // 
            // EmpId
            // 
            this.EmpId.HeaderText = "Employee ID";
            this.EmpId.Name = "EmpId";
            // 
            // EmployeeName
            // 
            this.EmployeeName.HeaderText = "Employee Name";
            this.EmployeeName.Name = "EmployeeName";
            // 
            // ComplaintType
            // 
            this.ComplaintType.HeaderText = "Complaint Type";
            this.ComplaintType.Name = "ComplaintType";
            // 
            // ComplaintDesc
            // 
            this.ComplaintDesc.HeaderText = "Complaint Description";
            this.ComplaintDesc.Name = "ComplaintDesc";
            this.ComplaintDesc.Width = 200;
            // 
            // CreateTime
            // 
            dataGridViewCellStyle3.Format = "dd-MM-yyyy HH:mm:ss";
            dataGridViewCellStyle3.NullValue = null;
            this.CreateTime.DefaultCellStyle = dataGridViewCellStyle3;
            this.CreateTime.HeaderText = "Complaint Time";
            this.CreateTime.Name = "CreateTime";
            this.CreateTime.Width = 150;
            // 
            // AcceptTime
            // 
            dataGridViewCellStyle4.Format = "dd-MM-yyyy HH:mm:ss";
            this.AcceptTime.DefaultCellStyle = dataGridViewCellStyle4;
            this.AcceptTime.HeaderText = "Accept Time";
            this.AcceptTime.Name = "AcceptTime";
            this.AcceptTime.Visible = false;
            // 
            // CloseTime
            // 
            dataGridViewCellStyle5.Format = "dd-MM-yyyy HH:mm:ss";
            this.CloseTime.DefaultCellStyle = dataGridViewCellStyle5;
            this.CloseTime.HeaderText = "Close Time";
            this.CloseTime.Name = "CloseTime";
            this.CloseTime.Visible = false;
            // 
            // Remarks
            // 
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            // 
            // Status
            // 
            this.Status.HeaderText = "Complaint Status";
            this.Status.Name = "Status";
            // 
            // DocumentStatus
            // 
            this.DocumentStatus.HeaderText = "Document Status";
            this.DocumentStatus.Name = "DocumentStatus";
            this.DocumentStatus.Visible = false;
            // 
            // CreateUser
            // 
            this.CreateUser.HeaderText = "Create User";
            this.CreateUser.Name = "CreateUser";
            this.CreateUser.Visible = false;
            // 
            // Creator
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Creator.DefaultCellStyle = dataGridViewCellStyle6;
            this.Creator.HeaderText = "Complainee";
            this.Creator.Name = "Creator";
            this.Creator.Visible = false;
            // 
            // Edit
            // 
            this.Edit.HeaderText = "Edit";
            this.Edit.Name = "Edit";
            this.Edit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Edit.Text = "Edit";
            this.Edit.UseColumnTextForButtonValue = true;
            // 
            // View
            // 
            this.View.HeaderText = "View";
            this.View.Name = "View";
            this.View.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.View.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.View.Text = "View";
            this.View.UseColumnTextForButtonValue = true;
            // 
            // WorkplaceCR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1096, 631);
            this.Controls.Add(this.pnlUI);
            this.Name = "WorkplaceCR";
            this.Text = "Currency Vs INR";
            this.Load += new System.EventHandler(this.SMRN_Load);
            this.Enter += new System.EventHandler(this.WorkplaceCR_Enter);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtEmpId;
        private System.Windows.Forms.ComboBox cmbComplaintType;
        private System.Windows.Forms.Button btnCmplntAccepted;
        private System.Windows.Forms.Label lblActionHeader;
        private System.Windows.Forms.FlowLayoutPanel pnlTopButtons;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.FlowLayoutPanel pnlEditButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnComplaintReg;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancelComplaint;
        private System.Windows.Forms.DateTimePicker dtCloseTime;
        private System.Windows.Forms.DateTimePicker dtAcceptTime;
        private System.Windows.Forms.Label lblCloseTime;
        private System.Windows.Forms.Label lblAcceptTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCmplntStatus;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Button btnAddComment;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRejectCLosing;
        private System.Windows.Forms.TextBox txtComplDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn RowID;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmpId;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmployeeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComplaintType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComplaintDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn AcceptTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn CloseTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn Creator;
        private System.Windows.Forms.DataGridViewButtonColumn Edit;
        private System.Windows.Forms.DataGridViewButtonColumn View;
    }
}