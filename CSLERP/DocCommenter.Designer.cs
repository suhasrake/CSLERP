﻿namespace CSLERP
{
    partial class DocCommenter
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
            this.pnlDocumentOuter = new System.Windows.Forms.Panel();
            this.pnlDocumentInner = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbDocument = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbEmployee = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbDocumentStatus = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlDocumentList = new System.Windows.Forms.Panel();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.empID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.empName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FwdEmployee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.empStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlUI.SuspendLayout();
            this.pnlDocumentOuter.SuspendLayout();
            this.pnlDocumentInner.SuspendLayout();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlDocumentList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.pnlDocumentOuter);
            this.pnlUI.Controls.Add(this.pnlBottomButtons);
            this.pnlUI.Controls.Add(this.pnlDocumentList);
            this.pnlUI.Location = new System.Drawing.Point(15, 15);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(1100, 540);
            this.pnlUI.TabIndex = 8;
            // 
            // pnlDocumentOuter
            // 
            this.pnlDocumentOuter.BackColor = System.Drawing.Color.Black;
            this.pnlDocumentOuter.Controls.Add(this.pnlDocumentInner);
            this.pnlDocumentOuter.Location = new System.Drawing.Point(242, 38);
            this.pnlDocumentOuter.Name = "pnlDocumentOuter";
            this.pnlDocumentOuter.Size = new System.Drawing.Size(544, 428);
            this.pnlDocumentOuter.TabIndex = 8;
            this.pnlDocumentOuter.Visible = false;
            // 
            // pnlDocumentInner
            // 
            this.pnlDocumentInner.BackColor = System.Drawing.Color.White;
            this.pnlDocumentInner.Controls.Add(this.label2);
            this.pnlDocumentInner.Controls.Add(this.cmbDocument);
            this.pnlDocumentInner.Controls.Add(this.label1);
            this.pnlDocumentInner.Controls.Add(this.cmbEmployee);
            this.pnlDocumentInner.Controls.Add(this.btnSave);
            this.pnlDocumentInner.Controls.Add(this.btnCancel);
            this.pnlDocumentInner.Controls.Add(this.cmbDocumentStatus);
            this.pnlDocumentInner.Controls.Add(this.label7);
            this.pnlDocumentInner.Location = new System.Drawing.Point(13, 16);
            this.pnlDocumentInner.Name = "pnlDocumentInner";
            this.pnlDocumentInner.Size = new System.Drawing.Size(515, 393);
            this.pnlDocumentInner.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(86, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Document";
            // 
            // cmbDocument
            // 
            this.cmbDocument.FormattingEnabled = true;
            this.cmbDocument.Location = new System.Drawing.Point(167, 131);
            this.cmbDocument.Name = "cmbDocument";
            this.cmbDocument.Size = new System.Drawing.Size(154, 21);
            this.cmbDocument.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(89, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Employee";
            // 
            // cmbEmployee
            // 
            this.cmbEmployee.FormattingEnabled = true;
            this.cmbEmployee.Location = new System.Drawing.Point(167, 158);
            this.cmbEmployee.Name = "cmbEmployee";
            this.cmbEmployee.Size = new System.Drawing.Size(154, 21);
            this.cmbEmployee.TabIndex = 26;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::CSLERP.Properties.Resources.accept;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(246, 260);
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
            this.btnCancel.Location = new System.Drawing.Point(142, 260);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbDocumentStatus
            // 
            this.cmbDocumentStatus.FormattingEnabled = true;
            this.cmbDocumentStatus.Location = new System.Drawing.Point(167, 190);
            this.cmbDocumentStatus.Name = "cmbDocumentStatus";
            this.cmbDocumentStatus.Size = new System.Drawing.Size(100, 21);
            this.cmbDocumentStatus.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(105, 198);
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
            // pnlDocumentList
            // 
            this.pnlDocumentList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlDocumentList.Controls.Add(this.grdList);
            this.pnlDocumentList.Location = new System.Drawing.Point(108, 79);
            this.pnlDocumentList.Name = "pnlDocumentList";
            this.pnlDocumentList.Size = new System.Drawing.Size(866, 357);
            this.pnlDocumentList.TabIndex = 6;
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
            this.empID,
            this.empName,
            this.FwdEmployee,
            this.empStatus,
            this.Edit});
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
            this.grdList.Location = new System.Drawing.Point(33, 22);
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
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.AliceBlue;
            this.grdList.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.grdList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            this.grdList.Size = new System.Drawing.Size(794, 297);
            this.grdList.TabIndex = 4;
            this.grdList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdList_CellContentClick);
            // 
            // empID
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.empID.DefaultCellStyle = dataGridViewCellStyle3;
            this.empID.HeaderText = "Document ID";
            this.empID.Name = "empID";
            this.empID.Width = 150;
            // 
            // empName
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.empName.DefaultCellStyle = dataGridViewCellStyle4;
            this.empName.HeaderText = "Document Name";
            this.empName.Name = "empName";
            this.empName.Width = 300;
            // 
            // FwdEmployee
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.FwdEmployee.DefaultCellStyle = dataGridViewCellStyle5;
            this.FwdEmployee.HeaderText = "Employee Name";
            this.FwdEmployee.Name = "FwdEmployee";
            this.FwdEmployee.Width = 150;
            // 
            // empStatus
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.empStatus.DefaultCellStyle = dataGridViewCellStyle6;
            this.empStatus.HeaderText = "Status";
            this.empStatus.Name = "empStatus";
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
            // DocCommenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "DocCommenter";
            this.Text = "ERP User";
            this.Load += new System.EventHandler(this.DocCommenter_Load);
            this.Enter += new System.EventHandler(this.DocCommenter_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlDocumentOuter.ResumeLayout(false);
            this.pnlDocumentInner.ResumeLayout(false);
            this.pnlDocumentInner.PerformLayout();
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlDocumentList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Panel pnlDocumentOuter;
        private System.Windows.Forms.Panel pnlDocumentInner;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbDocumentStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pnlDocumentList;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbEmployee;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbDocument;
        private System.Windows.Forms.DataGridViewTextBoxColumn empID;
        private System.Windows.Forms.DataGridViewTextBoxColumn empName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FwdEmployee;
        private System.Windows.Forms.DataGridViewTextBoxColumn empStatus;
        private System.Windows.Forms.DataGridViewButtonColumn Edit;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
    }
}