namespace CSLERP
{
    partial class AutoJVAccMapping
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
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlDocumentOuter = new System.Windows.Forms.Panel();
            this.pnlDocumentInner = new System.Windows.Forms.Panel();
            this.txtAccountNameCredit = new System.Windows.Forms.TextBox();
            this.txtAccountNameDebit = new System.Windows.Forms.TextBox();
            this.txtDocName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtJVName = new System.Windows.Forms.TextBox();
            this.txtAccountCodeCredit = new System.Windows.Forms.TextBox();
            this.txtAccountCodeDebit = new System.Windows.Forms.TextBox();
            this.txtDocumentID = new System.Windows.Forms.TextBox();
            this.btnAccCredit = new System.Windows.Forms.Button();
            this.btnAccDebit = new System.Windows.Forms.Button();
            this.btnDocument = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlDocumentList = new System.Windows.Forms.Panel();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.RowID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JVName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountCodeDebit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountNameDebit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountCodeCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountNameCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlUI.SuspendLayout();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlDocumentOuter.SuspendLayout();
            this.pnlDocumentInner.SuspendLayout();
            this.pnlDocumentList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.pnlBottomButtons);
            this.pnlUI.Controls.Add(this.pnlDocumentOuter);
            this.pnlUI.Controls.Add(this.pnlDocumentList);
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
            // pnlDocumentOuter
            // 
            this.pnlDocumentOuter.BackColor = System.Drawing.Color.Black;
            this.pnlDocumentOuter.Controls.Add(this.pnlDocumentInner);
            this.pnlDocumentOuter.Location = new System.Drawing.Point(279, 38);
            this.pnlDocumentOuter.Name = "pnlDocumentOuter";
            this.pnlDocumentOuter.Size = new System.Drawing.Size(640, 356);
            this.pnlDocumentOuter.TabIndex = 8;
            this.pnlDocumentOuter.Visible = false;
            // 
            // pnlDocumentInner
            // 
            this.pnlDocumentInner.BackColor = System.Drawing.Color.White;
            this.pnlDocumentInner.Controls.Add(this.txtAccountNameCredit);
            this.pnlDocumentInner.Controls.Add(this.txtAccountNameDebit);
            this.pnlDocumentInner.Controls.Add(this.txtDocName);
            this.pnlDocumentInner.Controls.Add(this.label4);
            this.pnlDocumentInner.Controls.Add(this.txtJVName);
            this.pnlDocumentInner.Controls.Add(this.txtAccountCodeCredit);
            this.pnlDocumentInner.Controls.Add(this.txtAccountCodeDebit);
            this.pnlDocumentInner.Controls.Add(this.txtDocumentID);
            this.pnlDocumentInner.Controls.Add(this.btnAccCredit);
            this.pnlDocumentInner.Controls.Add(this.btnAccDebit);
            this.pnlDocumentInner.Controls.Add(this.btnDocument);
            this.pnlDocumentInner.Controls.Add(this.label2);
            this.pnlDocumentInner.Controls.Add(this.label3);
            this.pnlDocumentInner.Controls.Add(this.label1);
            this.pnlDocumentInner.Controls.Add(this.btnSave);
            this.pnlDocumentInner.Controls.Add(this.btnCancel);
            this.pnlDocumentInner.Controls.Add(this.cmbStatus);
            this.pnlDocumentInner.Controls.Add(this.label7);
            this.pnlDocumentInner.Location = new System.Drawing.Point(15, 14);
            this.pnlDocumentInner.Name = "pnlDocumentInner";
            this.pnlDocumentInner.Size = new System.Drawing.Size(608, 326);
            this.pnlDocumentInner.TabIndex = 0;
            // 
            // txtAccountNameCredit
            // 
            this.txtAccountNameCredit.Location = new System.Drawing.Point(185, 197);
            this.txtAccountNameCredit.Name = "txtAccountNameCredit";
            this.txtAccountNameCredit.ReadOnly = true;
            this.txtAccountNameCredit.Size = new System.Drawing.Size(266, 20);
            this.txtAccountNameCredit.TabIndex = 42;
            // 
            // txtAccountNameDebit
            // 
            this.txtAccountNameDebit.Location = new System.Drawing.Point(185, 147);
            this.txtAccountNameDebit.Name = "txtAccountNameDebit";
            this.txtAccountNameDebit.ReadOnly = true;
            this.txtAccountNameDebit.Size = new System.Drawing.Size(266, 20);
            this.txtAccountNameDebit.TabIndex = 41;
            // 
            // txtDocName
            // 
            this.txtDocName.Location = new System.Drawing.Point(185, 97);
            this.txtDocName.Name = "txtDocName";
            this.txtDocName.ReadOnly = true;
            this.txtDocName.Size = new System.Drawing.Size(266, 20);
            this.txtDocName.TabIndex = 40;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(131, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "JV Name";
            // 
            // txtJVName
            // 
            this.txtJVName.Location = new System.Drawing.Point(185, 45);
            this.txtJVName.Name = "txtJVName";
            this.txtJVName.Size = new System.Drawing.Size(223, 20);
            this.txtJVName.TabIndex = 38;
            // 
            // txtAccountCodeCredit
            // 
            this.txtAccountCodeCredit.Location = new System.Drawing.Point(185, 171);
            this.txtAccountCodeCredit.Name = "txtAccountCodeCredit";
            this.txtAccountCodeCredit.ReadOnly = true;
            this.txtAccountCodeCredit.Size = new System.Drawing.Size(223, 20);
            this.txtAccountCodeCredit.TabIndex = 37;
            // 
            // txtAccountCodeDebit
            // 
            this.txtAccountCodeDebit.Location = new System.Drawing.Point(185, 123);
            this.txtAccountCodeDebit.Name = "txtAccountCodeDebit";
            this.txtAccountCodeDebit.ReadOnly = true;
            this.txtAccountCodeDebit.Size = new System.Drawing.Size(223, 20);
            this.txtAccountCodeDebit.TabIndex = 36;
            // 
            // txtDocumentID
            // 
            this.txtDocumentID.Location = new System.Drawing.Point(185, 71);
            this.txtDocumentID.Name = "txtDocumentID";
            this.txtDocumentID.ReadOnly = true;
            this.txtDocumentID.Size = new System.Drawing.Size(223, 20);
            this.txtDocumentID.TabIndex = 35;
            // 
            // btnAccCredit
            // 
            this.btnAccCredit.Location = new System.Drawing.Point(414, 169);
            this.btnAccCredit.Name = "btnAccCredit";
            this.btnAccCredit.Size = new System.Drawing.Size(48, 23);
            this.btnAccCredit.TabIndex = 34;
            this.btnAccCredit.Text = "Select";
            this.btnAccCredit.UseVisualStyleBackColor = true;
            this.btnAccCredit.Click += new System.EventHandler(this.btnAccCredit_Click);
            // 
            // btnAccDebit
            // 
            this.btnAccDebit.Location = new System.Drawing.Point(414, 120);
            this.btnAccDebit.Name = "btnAccDebit";
            this.btnAccDebit.Size = new System.Drawing.Size(48, 23);
            this.btnAccDebit.TabIndex = 33;
            this.btnAccDebit.Text = "Select";
            this.btnAccDebit.UseVisualStyleBackColor = true;
            this.btnAccDebit.Click += new System.EventHandler(this.btnAccDebit_Click);
            // 
            // btnDocument
            // 
            this.btnDocument.Location = new System.Drawing.Point(414, 69);
            this.btnDocument.Name = "btnDocument";
            this.btnDocument.Size = new System.Drawing.Size(48, 23);
            this.btnDocument.TabIndex = 32;
            this.btnDocument.Text = "Select";
            this.btnDocument.UseVisualStyleBackColor = true;
            this.btnDocument.Click += new System.EventHandler(this.btnDocument_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(124, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Document ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(104, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Account Credit";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(108, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Account debit";
            // 
            // btnSave
            // 
            this.btnSave.Image = global::CSLERP.Properties.Resources.accept;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(183, 281);
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
            this.btnCancel.Location = new System.Drawing.Point(97, 281);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(185, 223);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(168, 21);
            this.cmbStatus.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(144, 225);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Status";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pnlDocumentList
            // 
            this.pnlDocumentList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlDocumentList.Controls.Add(this.grdList);
            this.pnlDocumentList.Location = new System.Drawing.Point(31, 21);
            this.pnlDocumentList.Name = "pnlDocumentList";
            this.pnlDocumentList.Size = new System.Drawing.Size(1054, 439);
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
            this.RowID,
            this.JVName,
            this.DocumentID,
            this.DocumentName,
            this.AccountCodeDebit,
            this.AccountNameDebit,
            this.AccountCodeCredit,
            this.AccountNameCredit,
            this.Status,
            this.StatusString,
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
            this.grdList.Location = new System.Drawing.Point(27, 31);
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
            this.grdList.Size = new System.Drawing.Size(1021, 383);
            this.grdList.TabIndex = 4;
            this.grdList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdList_CellContentClick);
            // 
            // RowID
            // 
            this.RowID.HeaderText = "RowID";
            this.RowID.Name = "RowID";
            this.RowID.Visible = false;
            this.RowID.Width = 50;
            // 
            // JVName
            // 
            this.JVName.HeaderText = "JVName";
            this.JVName.Name = "JVName";
            // 
            // DocumentID
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DocumentID.DefaultCellStyle = dataGridViewCellStyle3;
            this.DocumentID.HeaderText = "Document ID";
            this.DocumentID.Name = "DocumentID";
            // 
            // DocumentName
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DocumentName.DefaultCellStyle = dataGridViewCellStyle4;
            this.DocumentName.HeaderText = "Document Name";
            this.DocumentName.Name = "DocumentName";
            this.DocumentName.Width = 150;
            // 
            // AccountCodeDebit
            // 
            this.AccountCodeDebit.HeaderText = "Acc Code Debit";
            this.AccountCodeDebit.Name = "AccountCodeDebit";
            // 
            // AccountNameDebit
            // 
            this.AccountNameDebit.HeaderText = "Acc Name Debit";
            this.AccountNameDebit.Name = "AccountNameDebit";
            this.AccountNameDebit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.AccountNameDebit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AccountNameDebit.Width = 150;
            // 
            // AccountCodeCredit
            // 
            this.AccountCodeCredit.HeaderText = "Acc Code Credit";
            this.AccountCodeCredit.Name = "AccountCodeCredit";
            // 
            // AccountNameCredit
            // 
            this.AccountNameCredit.HeaderText = "Acc Name Credit";
            this.AccountNameCredit.Name = "AccountNameCredit";
            this.AccountNameCredit.Width = 150;
            // 
            // Status
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Status.DefaultCellStyle = dataGridViewCellStyle5;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.Visible = false;
            this.Status.Width = 70;
            // 
            // StatusString
            // 
            this.StatusString.HeaderText = "Status";
            this.StatusString.Name = "StatusString";
            this.StatusString.Width = 70;
            // 
            // Edit
            // 
            this.Edit.HeaderText = "Edit";
            this.Edit.Name = "Edit";
            this.Edit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Edit.Text = "Edit";
            this.Edit.ToolTipText = "Edit Employee";
            this.Edit.UseColumnTextForButtonValue = true;
            this.Edit.Width = 70;
            // 
            // AutoJVAccMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "AutoJVAccMapping";
            this.Text = "ERP User";
            this.Load += new System.EventHandler(this.DocEmpMapping_Load);
            this.Enter += new System.EventHandler(this.AutoJVAccMapping_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlDocumentOuter.ResumeLayout(false);
            this.pnlDocumentInner.ResumeLayout(false);
            this.pnlDocumentInner.PerformLayout();
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
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pnlDocumentList;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.TextBox txtAccountCodeCredit;
        private System.Windows.Forms.TextBox txtAccountCodeDebit;
        private System.Windows.Forms.TextBox txtDocumentID;
        private System.Windows.Forms.Button btnAccCredit;
        private System.Windows.Forms.Button btnAccDebit;
        private System.Windows.Forms.Button btnDocument;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtJVName;
        private System.Windows.Forms.TextBox txtAccountNameCredit;
        private System.Windows.Forms.TextBox txtAccountNameDebit;
        private System.Windows.Forms.TextBox txtDocName;
        private System.Windows.Forms.DataGridViewTextBoxColumn RowID;
        private System.Windows.Forms.DataGridViewTextBoxColumn JVName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountCodeDebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountNameDebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountCodeCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountNameCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusString;
        private System.Windows.Forms.DataGridViewButtonColumn Edit;
    }
}