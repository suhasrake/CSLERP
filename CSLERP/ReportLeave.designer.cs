﻿namespace CSLERP
{
    partial class ReportLeave
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlUI = new System.Windows.Forms.Panel();
            this.lblRegion = new System.Windows.Forms.Label();
            this.lblOffice = new System.Windows.Forms.Label();
            this.cmbfilterOffice = new System.Windows.Forms.ComboBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnView = new System.Windows.Forms.Button();
            this.cmbFilterRegion = new System.Windows.Forms.ComboBox();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlLeaveDetailOuter = new System.Windows.Forms.Panel();
            this.pnlEditButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnBack = new System.Windows.Forms.Button();
            this.pnlLeaveDetailInner = new System.Windows.Forms.Panel();
            this.txtEmpName = new System.Windows.Forms.TextBox();
            this.txtEmpid = new System.Windows.Forms.TextBox();
            this.lblEmployeeID = new System.Windows.Forms.Label();
            this.lblEmpname = new System.Windows.Forms.Label();
            this.lblDetails = new System.Windows.Forms.Label();
            this.pnlLeaveDetails = new System.Windows.Forms.Panel();
            this.dgvLeaveDetails = new System.Windows.Forms.DataGridView();
            this.pnlgrdList = new System.Windows.Forms.Panel();
            this.btnExportToExcel = new System.Windows.Forms.Button();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.pnlUI.SuspendLayout();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlLeaveDetailOuter.SuspendLayout();
            this.pnlEditButtons.SuspendLayout();
            this.pnlLeaveDetailInner.SuspendLayout();
            this.pnlLeaveDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLeaveDetails)).BeginInit();
            this.pnlgrdList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.lblRegion);
            this.pnlUI.Controls.Add(this.lblOffice);
            this.pnlUI.Controls.Add(this.cmbfilterOffice);
            this.pnlUI.Controls.Add(this.lblSearch);
            this.pnlUI.Controls.Add(this.txtSearch);
            this.pnlUI.Controls.Add(this.btnView);
            this.pnlUI.Controls.Add(this.cmbFilterRegion);
            this.pnlUI.Controls.Add(this.pnlBottomButtons);
            this.pnlUI.Controls.Add(this.pnlLeaveDetailOuter);
            this.pnlUI.Controls.Add(this.pnlgrdList);
            this.pnlUI.Location = new System.Drawing.Point(15, 15);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(1100, 540);
            this.pnlUI.TabIndex = 8;
            // 
            // lblRegion
            // 
            this.lblRegion.AutoSize = true;
            this.lblRegion.Location = new System.Drawing.Point(31, 21);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(41, 13);
            this.lblRegion.TabIndex = 41;
            this.lblRegion.Text = "Region";
            // 
            // lblOffice
            // 
            this.lblOffice.AutoSize = true;
            this.lblOffice.Location = new System.Drawing.Point(288, 21);
            this.lblOffice.Name = "lblOffice";
            this.lblOffice.Size = new System.Drawing.Size(35, 13);
            this.lblOffice.TabIndex = 40;
            this.lblOffice.Text = "Office";
            // 
            // cmbfilterOffice
            // 
            this.cmbfilterOffice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbfilterOffice.FormattingEnabled = true;
            this.cmbfilterOffice.Location = new System.Drawing.Point(325, 17);
            this.cmbfilterOffice.Name = "cmbfilterOffice";
            this.cmbfilterOffice.Size = new System.Drawing.Size(170, 21);
            this.cmbfilterOffice.TabIndex = 39;
            this.cmbfilterOffice.SelectedIndexChanged += new System.EventHandler(this.cmbStoreLocation_SelectedIndexChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblSearch.Location = new System.Drawing.Point(879, 36);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(46, 15);
            this.lblSearch.TabIndex = 38;
            this.lblSearch.Text = "Search";
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(931, 34);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(137, 21);
            this.txtSearch.TabIndex = 37;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnView
            // 
            this.btnView.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnView.Location = new System.Drawing.Point(525, 15);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(64, 23);
            this.btnView.TabIndex = 12;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // cmbFilterRegion
            // 
            this.cmbFilterRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterRegion.FormattingEnabled = true;
            this.cmbFilterRegion.Location = new System.Drawing.Point(73, 17);
            this.cmbFilterRegion.Name = "cmbFilterRegion";
            this.cmbFilterRegion.Size = new System.Drawing.Size(198, 21);
            this.cmbFilterRegion.TabIndex = 11;
            this.cmbFilterRegion.SelectedIndexChanged += new System.EventHandler(this.cmbcmpnysrch_SelectedIndexChanged);
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
            // pnlLeaveDetailOuter
            // 
            this.pnlLeaveDetailOuter.BackColor = System.Drawing.Color.DarkGray;
            this.pnlLeaveDetailOuter.Controls.Add(this.pnlEditButtons);
            this.pnlLeaveDetailOuter.Controls.Add(this.pnlLeaveDetailInner);
            this.pnlLeaveDetailOuter.Location = new System.Drawing.Point(20, 100);
            this.pnlLeaveDetailOuter.Name = "pnlLeaveDetailOuter";
            this.pnlLeaveDetailOuter.Size = new System.Drawing.Size(1053, 322);
            this.pnlLeaveDetailOuter.TabIndex = 42;
            this.pnlLeaveDetailOuter.Visible = false;
            // 
            // pnlEditButtons
            // 
            this.pnlEditButtons.Controls.Add(this.btnBack);
            this.pnlEditButtons.Location = new System.Drawing.Point(14, 290);
            this.pnlEditButtons.Name = "pnlEditButtons";
            this.pnlEditButtons.Size = new System.Drawing.Size(580, 30);
            this.pnlEditButtons.TabIndex = 47;
            // 
            // btnBack
            // 
            this.btnBack.Image = global::CSLERP.Properties.Resources.cancel;
            this.btnBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBack.Location = new System.Drawing.Point(3, 3);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 21;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // pnlLeaveDetailInner
            // 
            this.pnlLeaveDetailInner.BackColor = System.Drawing.Color.White;
            this.pnlLeaveDetailInner.Controls.Add(this.txtEmpName);
            this.pnlLeaveDetailInner.Controls.Add(this.txtEmpid);
            this.pnlLeaveDetailInner.Controls.Add(this.lblEmployeeID);
            this.pnlLeaveDetailInner.Controls.Add(this.lblEmpname);
            this.pnlLeaveDetailInner.Controls.Add(this.lblDetails);
            this.pnlLeaveDetailInner.Controls.Add(this.pnlLeaveDetails);
            this.pnlLeaveDetailInner.Location = new System.Drawing.Point(14, 14);
            this.pnlLeaveDetailInner.Name = "pnlLeaveDetailInner";
            this.pnlLeaveDetailInner.Size = new System.Drawing.Size(1023, 277);
            this.pnlLeaveDetailInner.TabIndex = 0;
            // 
            // txtEmpName
            // 
            this.txtEmpName.Enabled = false;
            this.txtEmpName.Location = new System.Drawing.Point(99, 63);
            this.txtEmpName.Name = "txtEmpName";
            this.txtEmpName.Size = new System.Drawing.Size(157, 20);
            this.txtEmpName.TabIndex = 45;
            // 
            // txtEmpid
            // 
            this.txtEmpid.Enabled = false;
            this.txtEmpid.Location = new System.Drawing.Point(99, 37);
            this.txtEmpid.Name = "txtEmpid";
            this.txtEmpid.Size = new System.Drawing.Size(157, 20);
            this.txtEmpid.TabIndex = 44;
            // 
            // lblEmployeeID
            // 
            this.lblEmployeeID.AutoSize = true;
            this.lblEmployeeID.Location = new System.Drawing.Point(30, 40);
            this.lblEmployeeID.Name = "lblEmployeeID";
            this.lblEmployeeID.Size = new System.Drawing.Size(67, 13);
            this.lblEmployeeID.TabIndex = 43;
            this.lblEmployeeID.Text = "Employee ID";
            // 
            // lblEmpname
            // 
            this.lblEmpname.AutoSize = true;
            this.lblEmpname.Location = new System.Drawing.Point(13, 66);
            this.lblEmpname.Name = "lblEmpname";
            this.lblEmpname.Size = new System.Drawing.Size(84, 13);
            this.lblEmpname.TabIndex = 42;
            this.lblEmpname.Text = "Employee Name";
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetails.Location = new System.Drawing.Point(3, 5);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(57, 16);
            this.lblDetails.TabIndex = 0;
            this.lblDetails.Text = "Details";
            // 
            // pnlLeaveDetails
            // 
            this.pnlLeaveDetails.Controls.Add(this.dgvLeaveDetails);
            this.pnlLeaveDetails.Location = new System.Drawing.Point(6, 89);
            this.pnlLeaveDetails.Name = "pnlLeaveDetails";
            this.pnlLeaveDetails.Size = new System.Drawing.Size(1014, 181);
            this.pnlLeaveDetails.TabIndex = 6;
            // 
            // dgvLeaveDetails
            // 
            this.dgvLeaveDetails.AllowUserToAddRows = false;
            this.dgvLeaveDetails.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvLeaveDetails.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLeaveDetails.BackgroundColor = System.Drawing.Color.White;
            this.dgvLeaveDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLeaveDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLeaveDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLeaveDetails.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvLeaveDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvLeaveDetails.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvLeaveDetails.Location = new System.Drawing.Point(109, 51);
            this.dgvLeaveDetails.Name = "dgvLeaveDetails";
            this.dgvLeaveDetails.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLeaveDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvLeaveDetails.RowHeadersVisible = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.AliceBlue;
            this.dgvLeaveDetails.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvLeaveDetails.Size = new System.Drawing.Size(823, 122);
            this.dgvLeaveDetails.TabIndex = 5;
            // 
            // pnlgrdList
            // 
            this.pnlgrdList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlgrdList.Controls.Add(this.btnExportToExcel);
            this.pnlgrdList.Controls.Add(this.grdList);
            this.pnlgrdList.Location = new System.Drawing.Point(18, 61);
            this.pnlgrdList.Name = "pnlgrdList";
            this.pnlgrdList.Size = new System.Drawing.Size(1060, 433);
            this.pnlgrdList.TabIndex = 6;
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.Location = new System.Drawing.Point(947, 397);
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new System.Drawing.Size(104, 23);
            this.btnExportToExcel.TabIndex = 14;
            this.btnExportToExcel.Text = "Export To Excel";
            this.btnExportToExcel.UseVisualStyleBackColor = true;
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // grdList
            // 
            this.grdList.AllowUserToAddRows = false;
            this.grdList.AllowUserToDeleteRows = false;
            this.grdList.AllowUserToOrderColumns = true;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grdList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.grdList.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grdList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.grdList.ColumnHeadersHeight = 25;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.NullValue = "0";
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdList.DefaultCellStyle = dataGridViewCellStyle8;
            this.grdList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdList.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdList.Location = new System.Drawing.Point(8, 16);
            this.grdList.Name = "grdList";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdList.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.grdList.RowHeadersVisible = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.AliceBlue;
            this.grdList.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.grdList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            this.grdList.Size = new System.Drawing.Size(1042, 375);
            this.grdList.TabIndex = 4;
            this.grdList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdList_CellContentClick);
            // 
            // ReportLeave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "ReportLeave";
            this.Text = "ERP User";
            this.Load += new System.EventHandler(this.CompanyData_Load);
            this.Enter += new System.EventHandler(this.ReportLeave_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlUI.PerformLayout();
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlLeaveDetailOuter.ResumeLayout(false);
            this.pnlEditButtons.ResumeLayout(false);
            this.pnlLeaveDetailInner.ResumeLayout(false);
            this.pnlLeaveDetailInner.PerformLayout();
            this.pnlLeaveDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLeaveDetails)).EndInit();
            this.pnlgrdList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Panel pnlgrdList;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.ComboBox cmbFilterRegion;
        private System.Windows.Forms.Button btnExportToExcel;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbfilterOffice;
        private System.Windows.Forms.Label lblOffice;
        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.Panel pnlLeaveDetailOuter;
        private System.Windows.Forms.FlowLayoutPanel pnlEditButtons;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Panel pnlLeaveDetailInner;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Panel pnlLeaveDetails;
        private System.Windows.Forms.DataGridView dgvLeaveDetails;
        private System.Windows.Forms.TextBox txtEmpName;
        private System.Windows.Forms.TextBox txtEmpid;
        private System.Windows.Forms.Label lblEmployeeID;
        private System.Windows.Forms.Label lblEmpname;
    }
}