namespace CSLERP
{
    partial class LeaveOB
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlUI = new System.Windows.Forms.Panel();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlList = new System.Windows.Forms.Panel();
            this.btnShowOBList = new System.Windows.Forms.Button();
            this.pnlShowLeaveOB = new System.Windows.Forms.Panel();
            this.grdEmpWiseLeaveOB = new System.Windows.Forms.DataGridView();
            this.LeaveID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LeaveCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblEmployeeiD = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancelLeaveOB = new System.Windows.Forms.Button();
            this.btnSaveLeaveOB = new System.Windows.Forms.Button();
            this.cmbLeaveYear = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblEmployeename = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearchGrd = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.EmployeeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmployeeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OfficeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gLeaveOB = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlUI.SuspendLayout();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlList.SuspendLayout();
            this.pnlShowLeaveOB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEmpWiseLeaveOB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
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
            this.pnlList.Controls.Add(this.btnShowOBList);
            this.pnlList.Controls.Add(this.pnlShowLeaveOB);
            this.pnlList.Controls.Add(this.txtSearchGrd);
            this.pnlList.Controls.Add(this.label17);
            this.pnlList.Controls.Add(this.grdList);
            this.pnlList.Location = new System.Drawing.Point(10, 12);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(1082, 492);
            this.pnlList.TabIndex = 6;
            // 
            // btnShowOBList
            // 
            this.btnShowOBList.Location = new System.Drawing.Point(380, 6);
            this.btnShowOBList.Name = "btnShowOBList";
            this.btnShowOBList.Size = new System.Drawing.Size(115, 23);
            this.btnShowOBList.TabIndex = 42;
            this.btnShowOBList.Text = "Show OB List";
            this.btnShowOBList.UseVisualStyleBackColor = true;
            this.btnShowOBList.Click += new System.EventHandler(this.btnShowOBList_Click);
            // 
            // pnlShowLeaveOB
            // 
            this.pnlShowLeaveOB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlShowLeaveOB.Controls.Add(this.grdEmpWiseLeaveOB);
            this.pnlShowLeaveOB.Controls.Add(this.lblEmployeeiD);
            this.pnlShowLeaveOB.Controls.Add(this.label3);
            this.pnlShowLeaveOB.Controls.Add(this.btnCancelLeaveOB);
            this.pnlShowLeaveOB.Controls.Add(this.btnSaveLeaveOB);
            this.pnlShowLeaveOB.Controls.Add(this.cmbLeaveYear);
            this.pnlShowLeaveOB.Controls.Add(this.label2);
            this.pnlShowLeaveOB.Controls.Add(this.lblEmployeename);
            this.pnlShowLeaveOB.Controls.Add(this.label1);
            this.pnlShowLeaveOB.Location = new System.Drawing.Point(566, 69);
            this.pnlShowLeaveOB.Name = "pnlShowLeaveOB";
            this.pnlShowLeaveOB.Size = new System.Drawing.Size(499, 352);
            this.pnlShowLeaveOB.TabIndex = 41;
            this.pnlShowLeaveOB.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlShowLeaveOB_Paint);
            // 
            // grdEmpWiseLeaveOB
            // 
            this.grdEmpWiseLeaveOB.AllowUserToAddRows = false;
            this.grdEmpWiseLeaveOB.AllowUserToDeleteRows = false;
            this.grdEmpWiseLeaveOB.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grdEmpWiseLeaveOB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdEmpWiseLeaveOB.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdEmpWiseLeaveOB.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LeaveID,
            this.Description,
            this.LeaveCount});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdEmpWiseLeaveOB.DefaultCellStyle = dataGridViewCellStyle4;
            this.grdEmpWiseLeaveOB.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grdEmpWiseLeaveOB.Location = new System.Drawing.Point(38, 78);
            this.grdEmpWiseLeaveOB.Name = "grdEmpWiseLeaveOB";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdEmpWiseLeaveOB.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.grdEmpWiseLeaveOB.RowHeadersVisible = false;
            this.grdEmpWiseLeaveOB.Size = new System.Drawing.Size(419, 231);
            this.grdEmpWiseLeaveOB.TabIndex = 50;
            // 
            // LeaveID
            // 
            this.LeaveID.HeaderText = "Leave ID";
            this.LeaveID.Name = "LeaveID";
            this.LeaveID.ReadOnly = true;
            // 
            // Description
            // 
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Description.DefaultCellStyle = dataGridViewCellStyle2;
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 200;
            // 
            // LeaveCount
            // 
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.LeaveCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.LeaveCount.HeaderText = "Leave Count";
            this.LeaveCount.Name = "LeaveCount";
            // 
            // lblEmployeeiD
            // 
            this.lblEmployeeiD.AutoSize = true;
            this.lblEmployeeiD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeeiD.Location = new System.Drawing.Point(114, 13);
            this.lblEmployeeiD.Name = "lblEmployeeiD";
            this.lblEmployeeiD.Size = new System.Drawing.Size(0, 15);
            this.lblEmployeeiD.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(27, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 15);
            this.label3.TabIndex = 12;
            this.label3.Text = "Employee  Id :";
            // 
            // btnCancelLeaveOB
            // 
            this.btnCancelLeaveOB.Image = global::CSLERP.Properties.Resources.cancel;
            this.btnCancelLeaveOB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelLeaveOB.Location = new System.Drawing.Point(117, 315);
            this.btnCancelLeaveOB.Name = "btnCancelLeaveOB";
            this.btnCancelLeaveOB.Size = new System.Drawing.Size(75, 23);
            this.btnCancelLeaveOB.TabIndex = 11;
            this.btnCancelLeaveOB.Text = "Cancel";
            this.btnCancelLeaveOB.UseVisualStyleBackColor = true;
            this.btnCancelLeaveOB.Click += new System.EventHandler(this.btnCancelLeaveOB_Click);
            // 
            // btnSaveLeaveOB
            // 
            this.btnSaveLeaveOB.Image = global::CSLERP.Properties.Resources.accept;
            this.btnSaveLeaveOB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveLeaveOB.Location = new System.Drawing.Point(36, 315);
            this.btnSaveLeaveOB.Name = "btnSaveLeaveOB";
            this.btnSaveLeaveOB.Size = new System.Drawing.Size(75, 23);
            this.btnSaveLeaveOB.TabIndex = 10;
            this.btnSaveLeaveOB.Text = "Save";
            this.btnSaveLeaveOB.UseVisualStyleBackColor = true;
            this.btnSaveLeaveOB.Click += new System.EventHandler(this.btnSaveLeaveOB_Click);
            // 
            // cmbLeaveYear
            // 
            this.cmbLeaveYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLeaveYear.FormattingEnabled = true;
            this.cmbLeaveYear.Location = new System.Drawing.Point(69, 43);
            this.cmbLeaveYear.Name = "cmbLeaveYear";
            this.cmbLeaveYear.Size = new System.Drawing.Size(121, 21);
            this.cmbLeaveYear.TabIndex = 9;
            this.cmbLeaveYear.SelectedIndexChanged += new System.EventHandler(this.cmbLeaveYear_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(32, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Year :";
            // 
            // lblEmployeename
            // 
            this.lblEmployeename.AutoSize = true;
            this.lblEmployeename.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeename.Location = new System.Drawing.Point(244, 13);
            this.lblEmployeename.Name = "lblEmployeename";
            this.lblEmployeename.Size = new System.Drawing.Size(0, 15);
            this.lblEmployeename.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(172, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Employee :";
            // 
            // txtSearchGrd
            // 
            this.txtSearchGrd.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.txtSearchGrd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchGrd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSearchGrd.Location = new System.Drawing.Point(82, 7);
            this.txtSearchGrd.Name = "txtSearchGrd";
            this.txtSearchGrd.Size = new System.Drawing.Size(176, 21);
            this.txtSearchGrd.TabIndex = 40;
            this.txtSearchGrd.TextChanged += new System.EventHandler(this.txtSearchGrd_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label17.Location = new System.Drawing.Point(31, 8);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(46, 15);
            this.label17.TabIndex = 39;
            this.label17.Text = "Search";
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
            this.grdList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EmployeeID,
            this.EmployeeName,
            this.OfficeName,
            this.gLeaveOB});
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
            this.grdList.Location = new System.Drawing.Point(33, 34);
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
            this.grdList.Size = new System.Drawing.Size(507, 412);
            this.grdList.TabIndex = 4;
            this.grdList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdList_CellContentClick);
            // 
            // EmployeeID
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.EmployeeID.DefaultCellStyle = dataGridViewCellStyle8;
            this.EmployeeID.HeaderText = "Emp ID";
            this.EmployeeID.Name = "EmployeeID";
            this.EmployeeID.ReadOnly = true;
            this.EmployeeID.Width = 60;
            // 
            // EmployeeName
            // 
            this.EmployeeName.HeaderText = "Employee Name";
            this.EmployeeName.Name = "EmployeeName";
            this.EmployeeName.ReadOnly = true;
            this.EmployeeName.Width = 180;
            // 
            // OfficeName
            // 
            this.OfficeName.HeaderText = "Office Name";
            this.OfficeName.Name = "OfficeName";
            this.OfficeName.ReadOnly = true;
            this.OfficeName.Width = 150;
            // 
            // gLeaveOB
            // 
            this.gLeaveOB.HeaderText = "Leave OB";
            this.gLeaveOB.Name = "gLeaveOB";
            this.gLeaveOB.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gLeaveOB.Text = "Leave OB";
            this.gLeaveOB.ToolTipText = "Leave OB";
            this.gLeaveOB.UseColumnTextForButtonValue = true;
            this.gLeaveOB.Width = 70;
            // 
            // LeaveOB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "LeaveOB";
            this.Text = "ERP User";
            this.Load += new System.EventHandler(this.Region_Load);
            this.Enter += new System.EventHandler(this.LeaveOB_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlList.ResumeLayout(false);
            this.pnlList.PerformLayout();
            this.pnlShowLeaveOB.ResumeLayout(false);
            this.pnlShowLeaveOB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEmpWiseLeaveOB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Panel pnlList;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.TextBox txtSearchGrd;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Panel pnlShowLeaveOB;
        private System.Windows.Forms.Button btnCancelLeaveOB;
        private System.Windows.Forms.Button btnSaveLeaveOB;
        private System.Windows.Forms.ComboBox cmbLeaveYear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblEmployeename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblEmployeeiD;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView grdEmpWiseLeaveOB;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmployeeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmployeeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OfficeName;
        private System.Windows.Forms.DataGridViewButtonColumn gLeaveOB;
        private System.Windows.Forms.Button btnShowOBList;
        private System.Windows.Forms.DataGridViewTextBoxColumn LeaveID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn LeaveCount;
    }
}