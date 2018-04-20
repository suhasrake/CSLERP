namespace CSLERP
{
    partial class CompanyDetail
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
            this.pnlRegionOuter = new System.Windows.Forms.Panel();
            this.pnlRegionInner = new System.Windows.Forms.Panel();
            this.txtcmpnyAddrs = new System.Windows.Forms.TextBox();
            this.lblcmpnyAddrs = new System.Windows.Forms.Label();
            this.txtcmpnyName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblCmpnyName = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblcmpnyID = new System.Windows.Forms.Label();
            this.txtcmpnyID = new System.Windows.Forms.TextBox();
            this.pnlRegionList = new System.Windows.Forms.Panel();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.CompanyID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CompanyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CompanyAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CompanyStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlUI.SuspendLayout();
            this.pnlRegionOuter.SuspendLayout();
            this.pnlRegionInner.SuspendLayout();
            this.pnlRegionList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.pnlBottomButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.pnlRegionOuter);
            this.pnlUI.Controls.Add(this.pnlRegionList);
            this.pnlUI.Controls.Add(this.pnlBottomButtons);
            this.pnlUI.Location = new System.Drawing.Point(15, 15);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(1100, 540);
            this.pnlUI.TabIndex = 8;
            // 
            // pnlRegionOuter
            // 
            this.pnlRegionOuter.BackColor = System.Drawing.Color.Black;
            this.pnlRegionOuter.Controls.Add(this.pnlRegionInner);
            this.pnlRegionOuter.Location = new System.Drawing.Point(249, 29);
            this.pnlRegionOuter.Name = "pnlRegionOuter";
            this.pnlRegionOuter.Size = new System.Drawing.Size(544, 428);
            this.pnlRegionOuter.TabIndex = 8;
            this.pnlRegionOuter.Visible = false;
            // 
            // pnlRegionInner
            // 
            this.pnlRegionInner.BackColor = System.Drawing.Color.White;
            this.pnlRegionInner.Controls.Add(this.txtcmpnyAddrs);
            this.pnlRegionInner.Controls.Add(this.lblcmpnyAddrs);
            this.pnlRegionInner.Controls.Add(this.txtcmpnyName);
            this.pnlRegionInner.Controls.Add(this.btnSave);
            this.pnlRegionInner.Controls.Add(this.btnCancel);
            this.pnlRegionInner.Controls.Add(this.cmbStatus);
            this.pnlRegionInner.Controls.Add(this.lblCmpnyName);
            this.pnlRegionInner.Controls.Add(this.label7);
            this.pnlRegionInner.Controls.Add(this.lblcmpnyID);
            this.pnlRegionInner.Controls.Add(this.txtcmpnyID);
            this.pnlRegionInner.Location = new System.Drawing.Point(13, 16);
            this.pnlRegionInner.Name = "pnlRegionInner";
            this.pnlRegionInner.Size = new System.Drawing.Size(515, 393);
            this.pnlRegionInner.TabIndex = 0;
            // 
            // txtcmpnyAddrs
            // 
            this.txtcmpnyAddrs.Location = new System.Drawing.Point(188, 127);
            this.txtcmpnyAddrs.Multiline = true;
            this.txtcmpnyAddrs.Name = "txtcmpnyAddrs";
            this.txtcmpnyAddrs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtcmpnyAddrs.Size = new System.Drawing.Size(286, 109);
            this.txtcmpnyAddrs.TabIndex = 27;
            // 
            // lblcmpnyAddrs
            // 
            this.lblcmpnyAddrs.AutoSize = true;
            this.lblcmpnyAddrs.Location = new System.Drawing.Point(69, 174);
            this.lblcmpnyAddrs.Name = "lblcmpnyAddrs";
            this.lblcmpnyAddrs.Size = new System.Drawing.Size(92, 13);
            this.lblcmpnyAddrs.TabIndex = 26;
            this.lblcmpnyAddrs.Text = "Company Address";
            // 
            // txtcmpnyName
            // 
            this.txtcmpnyName.Location = new System.Drawing.Point(188, 84);
            this.txtcmpnyName.Name = "txtcmpnyName";
            this.txtcmpnyName.Size = new System.Drawing.Size(250, 20);
            this.txtcmpnyName.TabIndex = 25;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::CSLERP.Properties.Resources.accept;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(241, 319);
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
            this.btnCancel.Location = new System.Drawing.Point(104, 318);
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
            this.cmbStatus.Location = new System.Drawing.Point(188, 260);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(100, 21);
            this.cmbStatus.TabIndex = 17;
            // 
            // lblCmpnyName
            // 
            this.lblCmpnyName.AutoSize = true;
            this.lblCmpnyName.Location = new System.Drawing.Point(79, 91);
            this.lblCmpnyName.Name = "lblCmpnyName";
            this.lblCmpnyName.Size = new System.Drawing.Size(82, 13);
            this.lblCmpnyName.TabIndex = 16;
            this.lblCmpnyName.Text = "Company Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(124, 263);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Status";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblcmpnyID
            // 
            this.lblcmpnyID.AutoSize = true;
            this.lblcmpnyID.Location = new System.Drawing.Point(96, 48);
            this.lblcmpnyID.Name = "lblcmpnyID";
            this.lblcmpnyID.Size = new System.Drawing.Size(65, 13);
            this.lblcmpnyID.TabIndex = 8;
            this.lblcmpnyID.Text = "Company ID";
            // 
            // txtcmpnyID
            // 
            this.txtcmpnyID.Location = new System.Drawing.Point(188, 45);
            this.txtcmpnyID.Name = "txtcmpnyID";
            this.txtcmpnyID.Size = new System.Drawing.Size(202, 20);
            this.txtcmpnyID.TabIndex = 0;
            // 
            // pnlRegionList
            // 
            this.pnlRegionList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlRegionList.Controls.Add(this.grdList);
            this.pnlRegionList.Location = new System.Drawing.Point(90, 57);
            this.pnlRegionList.Name = "pnlRegionList";
            this.pnlRegionList.Size = new System.Drawing.Size(869, 357);
            this.pnlRegionList.TabIndex = 6;
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
            this.CompanyID,
            this.CompanyName,
            this.CompanyAddress,
            this.CompanyStatus,
            this.Edit});
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
            this.grdList.Location = new System.Drawing.Point(129, 27);
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
            this.grdList.Size = new System.Drawing.Size(604, 297);
            this.grdList.TabIndex = 4;
            this.grdList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdList_CellContentClick);
            // 
            // CompanyID
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CompanyID.DefaultCellStyle = dataGridViewCellStyle3;
            this.CompanyID.HeaderText = "Company ID";
            this.CompanyID.Name = "CompanyID";
            // 
            // CompanyName
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CompanyName.DefaultCellStyle = dataGridViewCellStyle4;
            this.CompanyName.HeaderText = "Company Name";
            this.CompanyName.Name = "CompanyName";
            this.CompanyName.Width = 200;
            // 
            // CompanyAddress
            // 
            this.CompanyAddress.HeaderText = "Company Address";
            this.CompanyAddress.Name = "CompanyAddress";
            // 
            // CompanyStatus
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CompanyStatus.DefaultCellStyle = dataGridViewCellStyle5;
            this.CompanyStatus.HeaderText = "Status";
            this.CompanyStatus.Name = "CompanyStatus";
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
            // pnlBottomButtons
            // 
            this.pnlBottomButtons.Controls.Add(this.btnNew);
            this.pnlBottomButtons.Controls.Add(this.btnExit);
            this.pnlBottomButtons.Location = new System.Drawing.Point(15, 510);
            this.pnlBottomButtons.Name = "pnlBottomButtons";
            this.pnlBottomButtons.Size = new System.Drawing.Size(510, 28);
            this.pnlBottomButtons.TabIndex = 10;
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
            // CompanyDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "CompanyDetail";
            this.Text = "ERP User";
            this.Load += new System.EventHandler(this.CompanyDetail_Load);
            this.Enter += new System.EventHandler(this.CompanyDetail_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlRegionOuter.ResumeLayout(false);
            this.pnlRegionInner.ResumeLayout(false);
            this.pnlRegionInner.PerformLayout();
            this.pnlRegionList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.pnlBottomButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Panel pnlRegionOuter;
        private System.Windows.Forms.Panel pnlRegionInner;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblCmpnyName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblcmpnyID;
        private System.Windows.Forms.TextBox txtcmpnyID;
        private System.Windows.Forms.Panel pnlRegionList;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.TextBox txtcmpnyName;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.TextBox txtcmpnyAddrs;
        private System.Windows.Forms.Label lblcmpnyAddrs;
        private System.Windows.Forms.DataGridViewTextBoxColumn CompanyID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CompanyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CompanyAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn CompanyStatus;
        private System.Windows.Forms.DataGridViewButtonColumn Edit;
    }
}