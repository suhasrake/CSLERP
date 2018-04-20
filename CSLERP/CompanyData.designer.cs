namespace CSLERP
{
    partial class CompanyData
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlUI = new System.Windows.Forms.Panel();
            this.pnlRegionList = new System.Windows.Forms.Panel();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.DataID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CompanyStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbcmpnysrch = new System.Windows.Forms.ComboBox();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlRegionOuter = new System.Windows.Forms.Panel();
            this.pnlRegionInner = new System.Windows.Forms.Panel();
            this.cmbDataID = new System.Windows.Forms.ComboBox();
            this.cmbCmpnyID = new System.Windows.Forms.ComboBox();
            this.lbldataValue = new System.Windows.Forms.Label();
            this.txtDataValue = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblDataID = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblcmpnyID = new System.Windows.Forms.Label();
            this.pnlUI.SuspendLayout();
            this.pnlRegionList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlRegionOuter.SuspendLayout();
            this.pnlRegionInner.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.pnlRegionList);
            this.pnlUI.Controls.Add(this.btnSearch);
            this.pnlUI.Controls.Add(this.cmbcmpnysrch);
            this.pnlUI.Controls.Add(this.pnlBottomButtons);
            this.pnlUI.Controls.Add(this.pnlRegionOuter);
            this.pnlUI.Location = new System.Drawing.Point(15, 15);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(1100, 540);
            this.pnlUI.TabIndex = 8;
            // 
            // pnlRegionList
            // 
            this.pnlRegionList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlRegionList.Controls.Add(this.grdList);
            this.pnlRegionList.Location = new System.Drawing.Point(121, 94);
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
            this.DataID,
            this.DataValue,
            this.CompanyStatus,
            this.Edit});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdList.DefaultCellStyle = dataGridViewCellStyle5;
            this.grdList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdList.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdList.Location = new System.Drawing.Point(207, 3);
            this.grdList.Name = "grdList";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdList.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.grdList.RowHeadersVisible = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.AliceBlue;
            this.grdList.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.grdList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            this.grdList.Size = new System.Drawing.Size(501, 351);
            this.grdList.TabIndex = 4;
            this.grdList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdList_CellContentClick);
            // 
            // DataID
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DataID.DefaultCellStyle = dataGridViewCellStyle3;
            this.DataID.HeaderText = "DataID";
            this.DataID.Name = "DataID";
            this.DataID.ReadOnly = true;
            this.DataID.Width = 200;
            // 
            // DataValue
            // 
            this.DataValue.HeaderText = "Data Value";
            this.DataValue.Name = "DataValue";
            this.DataValue.ReadOnly = true;
            // 
            // CompanyStatus
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CompanyStatus.DefaultCellStyle = dataGridViewCellStyle4;
            this.CompanyStatus.HeaderText = "Status";
            this.CompanyStatus.Name = "CompanyStatus";
            this.CompanyStatus.ReadOnly = true;
            // 
            // Edit
            // 
            this.Edit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Edit.HeaderText = "Edit";
            this.Edit.Name = "Edit";
            this.Edit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Edit.Text = "Edit";
            this.Edit.ToolTipText = "Edit CompanyData";
            this.Edit.UseColumnTextForButtonValue = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(597, 32);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 23);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbcmpnysrch
            // 
            this.cmbcmpnysrch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbcmpnysrch.FormattingEnabled = true;
            this.cmbcmpnysrch.Location = new System.Drawing.Point(410, 33);
            this.cmbcmpnysrch.Name = "cmbcmpnysrch";
            this.cmbcmpnysrch.Size = new System.Drawing.Size(178, 21);
            this.cmbcmpnysrch.TabIndex = 11;
            this.cmbcmpnysrch.SelectedIndexChanged += new System.EventHandler(this.cmbcmpnysrch_SelectedIndexChanged);
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
            // pnlRegionOuter
            // 
            this.pnlRegionOuter.BackColor = System.Drawing.Color.Black;
            this.pnlRegionOuter.Controls.Add(this.pnlRegionInner);
            this.pnlRegionOuter.Location = new System.Drawing.Point(346, 73);
            this.pnlRegionOuter.Name = "pnlRegionOuter";
            this.pnlRegionOuter.Size = new System.Drawing.Size(456, 361);
            this.pnlRegionOuter.TabIndex = 8;
            this.pnlRegionOuter.Visible = false;
            // 
            // pnlRegionInner
            // 
            this.pnlRegionInner.BackColor = System.Drawing.Color.White;
            this.pnlRegionInner.Controls.Add(this.cmbDataID);
            this.pnlRegionInner.Controls.Add(this.cmbCmpnyID);
            this.pnlRegionInner.Controls.Add(this.lbldataValue);
            this.pnlRegionInner.Controls.Add(this.txtDataValue);
            this.pnlRegionInner.Controls.Add(this.btnSave);
            this.pnlRegionInner.Controls.Add(this.btnCancel);
            this.pnlRegionInner.Controls.Add(this.cmbStatus);
            this.pnlRegionInner.Controls.Add(this.lblDataID);
            this.pnlRegionInner.Controls.Add(this.label7);
            this.pnlRegionInner.Controls.Add(this.lblcmpnyID);
            this.pnlRegionInner.Location = new System.Drawing.Point(16, 20);
            this.pnlRegionInner.Name = "pnlRegionInner";
            this.pnlRegionInner.Size = new System.Drawing.Size(428, 320);
            this.pnlRegionInner.TabIndex = 0;
            // 
            // cmbDataID
            // 
            this.cmbDataID.FormattingEnabled = true;
            this.cmbDataID.Location = new System.Drawing.Point(193, 93);
            this.cmbDataID.Name = "cmbDataID";
            this.cmbDataID.Size = new System.Drawing.Size(175, 21);
            this.cmbDataID.TabIndex = 29;
            // 
            // cmbCmpnyID
            // 
            this.cmbCmpnyID.FormattingEnabled = true;
            this.cmbCmpnyID.Location = new System.Drawing.Point(193, 52);
            this.cmbCmpnyID.Name = "cmbCmpnyID";
            this.cmbCmpnyID.Size = new System.Drawing.Size(175, 21);
            this.cmbCmpnyID.TabIndex = 28;
            // 
            // lbldataValue
            // 
            this.lbldataValue.AutoSize = true;
            this.lbldataValue.Location = new System.Drawing.Point(83, 136);
            this.lbldataValue.Name = "lbldataValue";
            this.lbldataValue.Size = new System.Drawing.Size(60, 13);
            this.lbldataValue.TabIndex = 27;
            this.lbldataValue.Text = "Data Value";
            // 
            // txtDataValue
            // 
            this.txtDataValue.Location = new System.Drawing.Point(193, 132);
            this.txtDataValue.Name = "txtDataValue";
            this.txtDataValue.Size = new System.Drawing.Size(175, 20);
            this.txtDataValue.TabIndex = 26;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::CSLERP.Properties.Resources.accept;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(193, 232);
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
            this.btnCancel.Location = new System.Drawing.Point(68, 232);
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
            this.cmbStatus.Location = new System.Drawing.Point(193, 171);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(100, 21);
            this.cmbStatus.TabIndex = 17;
            // 
            // lblDataID
            // 
            this.lblDataID.AutoSize = true;
            this.lblDataID.Location = new System.Drawing.Point(102, 98);
            this.lblDataID.Name = "lblDataID";
            this.lblDataID.Size = new System.Drawing.Size(41, 13);
            this.lblDataID.TabIndex = 16;
            this.lblDataID.Text = "DataID";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(106, 179);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Status";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblcmpnyID
            // 
            this.lblcmpnyID.AutoSize = true;
            this.lblcmpnyID.Location = new System.Drawing.Point(81, 60);
            this.lblcmpnyID.Name = "lblcmpnyID";
            this.lblcmpnyID.Size = new System.Drawing.Size(62, 13);
            this.lblcmpnyID.TabIndex = 8;
            this.lblcmpnyID.Text = "CompanyID";
            // 
            // CompanyData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "CompanyData";
            this.Text = "ERP User";
            this.Load += new System.EventHandler(this.CompanyData_Load);
            this.Enter += new System.EventHandler(this.CompanyData_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlRegionList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlRegionOuter.ResumeLayout(false);
            this.pnlRegionInner.ResumeLayout(false);
            this.pnlRegionInner.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Panel pnlRegionOuter;
        private System.Windows.Forms.Panel pnlRegionInner;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblDataID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblcmpnyID;
        private System.Windows.Forms.Panel pnlRegionList;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.ComboBox cmbDataID;
        private System.Windows.Forms.ComboBox cmbCmpnyID;
        private System.Windows.Forms.Label lbldataValue;
        private System.Windows.Forms.TextBox txtDataValue;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cmbcmpnysrch;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn CompanyStatus;
        private System.Windows.Forms.DataGridViewButtonColumn Edit;
    }
}