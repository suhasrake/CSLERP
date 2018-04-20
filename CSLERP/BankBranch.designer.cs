namespace CSLERP
{
    partial class BankBranch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlUI = new System.Windows.Forms.Panel();
            this.pnlOuter = new System.Windows.Forms.Panel();
            this.pnlInner = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSWIFTCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBSRCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMICRCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIFSCCode = new System.Windows.Forms.TextBox();
            this.txtaddress3 = new System.Windows.Forms.TextBox();
            this.txtaddress2 = new System.Windows.Forms.TextBox();
            this.txtaddress1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbBank = new System.Windows.Forms.ComboBox();
            this.txtBranchName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlList = new System.Windows.Forms.Panel();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.BankID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BranchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IFSCCOde = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SWIFTCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MICRCOde = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BSRCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rowID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlUI.SuspendLayout();
            this.pnlOuter.SuspendLayout();
            this.pnlInner.SuspendLayout();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.pnlOuter);
            this.pnlUI.Controls.Add(this.pnlBottomButtons);
            this.pnlUI.Controls.Add(this.pnlList);
            this.pnlUI.Location = new System.Drawing.Point(15, 15);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(1100, 540);
            this.pnlUI.TabIndex = 8;
            // 
            // pnlOuter
            // 
            this.pnlOuter.BackColor = System.Drawing.Color.Black;
            this.pnlOuter.Controls.Add(this.pnlInner);
            this.pnlOuter.Location = new System.Drawing.Point(249, 29);
            this.pnlOuter.Name = "pnlOuter";
            this.pnlOuter.Size = new System.Drawing.Size(544, 428);
            this.pnlOuter.TabIndex = 8;
            this.pnlOuter.Visible = false;
            // 
            // pnlInner
            // 
            this.pnlInner.BackColor = System.Drawing.Color.White;
            this.pnlInner.Controls.Add(this.label6);
            this.pnlInner.Controls.Add(this.txtSWIFTCode);
            this.pnlInner.Controls.Add(this.label5);
            this.pnlInner.Controls.Add(this.txtBSRCode);
            this.pnlInner.Controls.Add(this.label4);
            this.pnlInner.Controls.Add(this.txtMICRCode);
            this.pnlInner.Controls.Add(this.label2);
            this.pnlInner.Controls.Add(this.label3);
            this.pnlInner.Controls.Add(this.txtIFSCCode);
            this.pnlInner.Controls.Add(this.txtaddress3);
            this.pnlInner.Controls.Add(this.txtaddress2);
            this.pnlInner.Controls.Add(this.txtaddress1);
            this.pnlInner.Controls.Add(this.label1);
            this.pnlInner.Controls.Add(this.cmbBank);
            this.pnlInner.Controls.Add(this.txtBranchName);
            this.pnlInner.Controls.Add(this.btnSave);
            this.pnlInner.Controls.Add(this.btnCancel);
            this.pnlInner.Controls.Add(this.cmbStatus);
            this.pnlInner.Controls.Add(this.label10);
            this.pnlInner.Controls.Add(this.label7);
            this.pnlInner.Location = new System.Drawing.Point(15, 16);
            this.pnlInner.Name = "pnlInner";
            this.pnlInner.Size = new System.Drawing.Size(515, 393);
            this.pnlInner.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(72, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "SWIFT Code";
            // 
            // txtSWIFTCode
            // 
            this.txtSWIFTCode.Location = new System.Drawing.Point(179, 174);
            this.txtSWIFTCode.Name = "txtSWIFTCode";
            this.txtSWIFTCode.Size = new System.Drawing.Size(285, 20);
            this.txtSWIFTCode.TabIndex = 38;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(75, 278);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "BSR Code";
            // 
            // txtBSRCode
            // 
            this.txtBSRCode.Location = new System.Drawing.Point(179, 271);
            this.txtBSRCode.Name = "txtBSRCode";
            this.txtBSRCode.Size = new System.Drawing.Size(285, 20);
            this.txtBSRCode.TabIndex = 36;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(70, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "MICR Code";
            // 
            // txtMICRCode
            // 
            this.txtMICRCode.Location = new System.Drawing.Point(179, 236);
            this.txtMICRCode.Name = "txtMICRCode";
            this.txtMICRCode.Size = new System.Drawing.Size(285, 20);
            this.txtMICRCode.TabIndex = 34;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(74, 207);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "IFSC Code";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(87, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Address";
            // 
            // txtIFSCCode
            // 
            this.txtIFSCCode.Location = new System.Drawing.Point(179, 200);
            this.txtIFSCCode.Name = "txtIFSCCode";
            this.txtIFSCCode.Size = new System.Drawing.Size(285, 20);
            this.txtIFSCCode.TabIndex = 31;
            // 
            // txtaddress3
            // 
            this.txtaddress3.Location = new System.Drawing.Point(179, 145);
            this.txtaddress3.Name = "txtaddress3";
            this.txtaddress3.Size = new System.Drawing.Size(285, 20);
            this.txtaddress3.TabIndex = 30;
            // 
            // txtaddress2
            // 
            this.txtaddress2.Location = new System.Drawing.Point(179, 119);
            this.txtaddress2.Name = "txtaddress2";
            this.txtaddress2.Size = new System.Drawing.Size(285, 20);
            this.txtaddress2.TabIndex = 29;
            // 
            // txtaddress1
            // 
            this.txtaddress1.Location = new System.Drawing.Point(179, 93);
            this.txtaddress1.Name = "txtaddress1";
            this.txtaddress1.Size = new System.Drawing.Size(285, 20);
            this.txtaddress1.TabIndex = 28;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Bank";
            // 
            // cmbBank
            // 
            this.cmbBank.FormattingEnabled = true;
            this.cmbBank.Location = new System.Drawing.Point(179, 30);
            this.cmbBank.Name = "cmbBank";
            this.cmbBank.Size = new System.Drawing.Size(250, 21);
            this.cmbBank.TabIndex = 26;
            // 
            // txtBranchName
            // 
            this.txtBranchName.Location = new System.Drawing.Point(179, 57);
            this.txtBranchName.Name = "txtBranchName";
            this.txtBranchName.Size = new System.Drawing.Size(250, 20);
            this.txtBranchName.TabIndex = 25;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::CSLERP.Properties.Resources.accept;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(179, 358);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnRegionSave_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::CSLERP.Properties.Resources.cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(75, 358);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(179, 306);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(100, 21);
            this.cmbStatus.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(60, 64);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Branch Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(95, 310);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Status";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pnlBottomButtons
            // 
            this.pnlBottomButtons.Controls.Add(this.btnNew);
            this.pnlBottomButtons.Controls.Add(this.btnExit);
            this.pnlBottomButtons.Location = new System.Drawing.Point(15, 510);
            this.pnlBottomButtons.Name = "pnlBottomButtons";
            this.pnlBottomButtons.Size = new System.Drawing.Size(510, 28);
            this.pnlBottomButtons.TabIndex = 10;
            this.pnlBottomButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBottomButtons_Paint);
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
            this.btnNew.Click += new System.EventHandler(this.btnNewUser_Click_1);
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
            this.btnExit.Click += new System.EventHandler(this.btnUserListExit_Click);
            // 
            // pnlList
            // 
            this.pnlList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlList.Controls.Add(this.grdList);
            this.pnlList.Location = new System.Drawing.Point(29, 57);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(1033, 357);
            this.pnlList.TabIndex = 6;
            this.pnlList.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlList_Paint);
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
            this.BankID,
            this.BranchName,
            this.Address1,
            this.Address2,
            this.Address3,
            this.IFSCCOde,
            this.SWIFTCode,
            this.MICRCOde,
            this.BSRCode,
            this.Status,
            this.rowID,
            this.Edit});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdList.DefaultCellStyle = dataGridViewCellStyle11;
            this.grdList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdList.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdList.Location = new System.Drawing.Point(3, 13);
            this.grdList.Name = "grdList";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdList.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.grdList.RowHeadersVisible = false;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.AliceBlue;
            this.grdList.RowsDefaultCellStyle = dataGridViewCellStyle13;
            this.grdList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            this.grdList.Size = new System.Drawing.Size(1009, 297);
            this.grdList.TabIndex = 4;
            this.grdList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdRegionList_CellContentClick);
            // 
            // BankID
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.BankID.DefaultCellStyle = dataGridViewCellStyle3;
            this.BankID.Frozen = true;
            this.BankID.HeaderText = "Bank";
            this.BankID.Name = "BankID";
            // 
            // BranchName
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.BranchName.DefaultCellStyle = dataGridViewCellStyle4;
            this.BranchName.Frozen = true;
            this.BranchName.HeaderText = "BranchName";
            this.BranchName.Name = "BranchName";
            this.BranchName.Width = 200;
            // 
            // Address1
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Address1.DefaultCellStyle = dataGridViewCellStyle5;
            this.Address1.Frozen = true;
            this.Address1.HeaderText = "Address 1";
            this.Address1.Name = "Address1";
            // 
            // Address2
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Address2.DefaultCellStyle = dataGridViewCellStyle6;
            this.Address2.Frozen = true;
            this.Address2.HeaderText = "Address 2";
            this.Address2.Name = "Address2";
            // 
            // Address3
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Address3.DefaultCellStyle = dataGridViewCellStyle7;
            this.Address3.Frozen = true;
            this.Address3.HeaderText = "Address 3";
            this.Address3.Name = "Address3";
            // 
            // IFSCCOde
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.IFSCCOde.DefaultCellStyle = dataGridViewCellStyle8;
            this.IFSCCOde.Frozen = true;
            this.IFSCCOde.HeaderText = "IFSC Code";
            this.IFSCCOde.Name = "IFSCCOde";
            // 
            // SWIFTCode
            // 
            this.SWIFTCode.Frozen = true;
            this.SWIFTCode.HeaderText = "SWIFTCode";
            this.SWIFTCode.Name = "SWIFTCode";
            // 
            // MICRCOde
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.MICRCOde.DefaultCellStyle = dataGridViewCellStyle9;
            this.MICRCOde.HeaderText = "NICR Code";
            this.MICRCOde.Name = "MICRCOde";
            // 
            // BSRCode
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.BSRCode.DefaultCellStyle = dataGridViewCellStyle10;
            this.BSRCode.HeaderText = "BSRCode";
            this.BSRCode.Name = "BSRCode";
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            // 
            // rowID
            // 
            this.rowID.HeaderText = "ROw ID";
            this.rowID.Name = "rowID";
            this.rowID.Visible = false;
            // 
            // Edit
            // 
            this.Edit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Edit.HeaderText = "Edit";
            this.Edit.Name = "Edit";
            this.Edit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Edit.Text = "Edit";
            this.Edit.ToolTipText = "Edit Employee";
            this.Edit.UseColumnTextForButtonValue = true;
            // 
            // BankBranch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "BankBranch";
            this.Text = "ERP User";
            this.Load += new System.EventHandler(this.BankBranch_Load);
            this.Enter += new System.EventHandler(this.BankBranch_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlOuter.ResumeLayout(false);
            this.pnlInner.ResumeLayout(false);
            this.pnlInner.PerformLayout();
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Panel pnlOuter;
        private System.Windows.Forms.Panel pnlInner;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pnlList;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.TextBox txtBranchName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIFSCCode;
        private System.Windows.Forms.TextBox txtaddress3;
        private System.Windows.Forms.TextBox txtaddress2;
        private System.Windows.Forms.TextBox txtaddress1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBank;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBSRCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMICRCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSWIFTCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn BankID;
        private System.Windows.Forms.DataGridViewTextBoxColumn BranchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address3;
        private System.Windows.Forms.DataGridViewTextBoxColumn IFSCCOde;
        private System.Windows.Forms.DataGridViewTextBoxColumn SWIFTCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn MICRCOde;
        private System.Windows.Forms.DataGridViewTextBoxColumn BSRCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowID;
        private System.Windows.Forms.DataGridViewButtonColumn Edit;
    }
}