namespace CSLERP.FileManager
{
    partial class EditJV
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.dtINVTempDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.txtINVTempNo = new System.Windows.Forms.TextBox();
            this.txtTotalCreditAmnt = new System.Windows.Forms.TextBox();
            this.txtTotalDebitAmnt = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtAmountInWords = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtnarration = new System.Windows.Forms.TextBox();
            this.lblJournalDate = new System.Windows.Forms.Label();
            this.dtJournalDate = new System.Windows.Forms.DateTimePicker();
            this.lblJournalNo = new System.Windows.Forms.Label();
            this.txtJournalNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtTempDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTemporarryNo = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnAddLine = new System.Windows.Forms.Button();
            this.grdPRDetail = new System.Windows.Forms.DataGridView();
            this.LineNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SelAcDebit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.AmountDebit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AmountCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartyCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SelParty = new System.Windows.Forms.DataGridViewButtonColumn();
            this.gSLType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPRDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.grdPRDetail);
            this.panel1.Controls.Add(this.btnCalculate);
            this.panel1.Controls.Add(this.btnAddLine);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.dtINVTempDate);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtINVTempNo);
            this.panel1.Controls.Add(this.txtTotalCreditAmnt);
            this.panel1.Controls.Add(this.txtTotalDebitAmnt);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.txtAmountInWords);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.txtnarration);
            this.panel1.Controls.Add(this.lblJournalDate);
            this.panel1.Controls.Add(this.dtJournalDate);
            this.panel1.Controls.Add(this.lblJournalNo);
            this.panel1.Controls.Add(this.txtJournalNo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtTempDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtTemporarryNo);
            this.panel1.Location = new System.Drawing.Point(10, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1040, 386);
            this.panel1.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(720, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 192;
            this.label6.Text = "INV Temp Date";
            // 
            // dtINVTempDate
            // 
            this.dtINVTempDate.Enabled = false;
            this.dtINVTempDate.Location = new System.Drawing.Point(807, 45);
            this.dtINVTempDate.Name = "dtINVTempDate";
            this.dtINVTempDate.Size = new System.Drawing.Size(168, 20);
            this.dtINVTempDate.TabIndex = 190;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(729, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 191;
            this.label7.Text = "INV Temp No";
            // 
            // txtINVTempNo
            // 
            this.txtINVTempNo.Enabled = false;
            this.txtINVTempNo.Location = new System.Drawing.Point(807, 19);
            this.txtINVTempNo.Name = "txtINVTempNo";
            this.txtINVTempNo.ReadOnly = true;
            this.txtINVTempNo.Size = new System.Drawing.Size(168, 20);
            this.txtINVTempNo.TabIndex = 189;
            // 
            // txtTotalCreditAmnt
            // 
            this.txtTotalCreditAmnt.Location = new System.Drawing.Point(754, 94);
            this.txtTotalCreditAmnt.Name = "txtTotalCreditAmnt";
            this.txtTotalCreditAmnt.ReadOnly = true;
            this.txtTotalCreditAmnt.Size = new System.Drawing.Size(100, 20);
            this.txtTotalCreditAmnt.TabIndex = 186;
            // 
            // txtTotalDebitAmnt
            // 
            this.txtTotalDebitAmnt.Location = new System.Drawing.Point(541, 93);
            this.txtTotalDebitAmnt.Name = "txtTotalDebitAmnt";
            this.txtTotalDebitAmnt.ReadOnly = true;
            this.txtTotalDebitAmnt.Size = new System.Drawing.Size(100, 20);
            this.txtTotalDebitAmnt.TabIndex = 185;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(439, 96);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(98, 13);
            this.label16.TabIndex = 184;
            this.label16.Text = "Total Debit Amount";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(652, 98);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 13);
            this.label15.TabIndex = 183;
            this.label15.Text = "Total Credit Amount";
            // 
            // txtAmountInWords
            // 
            this.txtAmountInWords.Location = new System.Drawing.Point(679, 310);
            this.txtAmountInWords.Multiline = true;
            this.txtAmountInWords.Name = "txtAmountInWords";
            this.txtAmountInWords.ReadOnly = true;
            this.txtAmountInWords.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAmountInWords.Size = new System.Drawing.Size(282, 58);
            this.txtAmountInWords.TabIndex = 180;
            this.txtAmountInWords.Text = "Amount in Words";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(35, 310);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(50, 13);
            this.label18.TabIndex = 168;
            this.label18.Text = "Narration";
            // 
            // txtnarration
            // 
            this.txtnarration.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtnarration.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtnarration.Location = new System.Drawing.Point(91, 310);
            this.txtnarration.Multiline = true;
            this.txtnarration.Name = "txtnarration";
            this.txtnarration.ReadOnly = true;
            this.txtnarration.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtnarration.Size = new System.Drawing.Size(550, 58);
            this.txtnarration.TabIndex = 167;
            // 
            // lblJournalDate
            // 
            this.lblJournalDate.AutoSize = true;
            this.lblJournalDate.Location = new System.Drawing.Point(388, 46);
            this.lblJournalDate.Name = "lblJournalDate";
            this.lblJournalDate.Size = new System.Drawing.Size(67, 13);
            this.lblJournalDate.TabIndex = 160;
            this.lblJournalDate.Text = "Journal Date";
            // 
            // dtJournalDate
            // 
            this.dtJournalDate.Enabled = false;
            this.dtJournalDate.Location = new System.Drawing.Point(465, 43);
            this.dtJournalDate.Name = "dtJournalDate";
            this.dtJournalDate.Size = new System.Drawing.Size(168, 20);
            this.dtJournalDate.TabIndex = 152;
            // 
            // lblJournalNo
            // 
            this.lblJournalNo.AutoSize = true;
            this.lblJournalNo.Location = new System.Drawing.Point(397, 21);
            this.lblJournalNo.Name = "lblJournalNo";
            this.lblJournalNo.Size = new System.Drawing.Size(58, 13);
            this.lblJournalNo.TabIndex = 156;
            this.lblJournalNo.Text = "Journal No";
            // 
            // txtJournalNo
            // 
            this.txtJournalNo.Enabled = false;
            this.txtJournalNo.Location = new System.Drawing.Point(465, 18);
            this.txtJournalNo.Name = "txtJournalNo";
            this.txtJournalNo.ReadOnly = true;
            this.txtJournalNo.Size = new System.Drawing.Size(168, 20);
            this.txtJournalNo.TabIndex = 151;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 148;
            this.label2.Text = "Temporary Date";
            // 
            // dtTempDate
            // 
            this.dtTempDate.Enabled = false;
            this.dtTempDate.Location = new System.Drawing.Point(125, 43);
            this.dtTempDate.Name = "dtTempDate";
            this.dtTempDate.Size = new System.Drawing.Size(168, 20);
            this.dtTempDate.TabIndex = 147;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 146;
            this.label1.Text = "Temporary No";
            // 
            // txtTemporarryNo
            // 
            this.txtTemporarryNo.Enabled = false;
            this.txtTemporarryNo.Location = new System.Drawing.Point(125, 16);
            this.txtTemporarryNo.Name = "txtTemporarryNo";
            this.txtTemporarryNo.ReadOnly = true;
            this.txtTemporarryNo.Size = new System.Drawing.Size(168, 20);
            this.txtTemporarryNo.TabIndex = 145;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnClose.FlatAppearance.BorderSize = 2;
            this.btnClose.Location = new System.Drawing.Point(965, 404);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnUpdate.FlatAppearance.BorderSize = 2;
            this.btnUpdate.Location = new System.Drawing.Point(874, 404);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(85, 26);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(160, 90);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(97, 23);
            this.btnCalculate.TabIndex = 194;
            this.btnCalculate.Text = "Calculate";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // btnAddLine
            // 
            this.btnAddLine.Location = new System.Drawing.Point(19, 90);
            this.btnAddLine.Name = "btnAddLine";
            this.btnAddLine.Size = new System.Drawing.Size(100, 23);
            this.btnAddLine.TabIndex = 193;
            this.btnAddLine.Text = "Add New Line";
            this.btnAddLine.UseVisualStyleBackColor = true;
            this.btnAddLine.Click += new System.EventHandler(this.btnAddLine_Click);
            // 
            // grdPRDetail
            // 
            this.grdPRDetail.AllowUserToAddRows = false;
            this.grdPRDetail.AllowUserToDeleteRows = false;
            this.grdPRDetail.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdPRDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdPRDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdPRDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LineNo,
            this.AccountCode,
            this.AccountName,
            this.SelAcDebit,
            this.AmountDebit,
            this.AmountCredit,
            this.PartyCode,
            this.PartyName,
            this.SelParty,
            this.gSLType,
            this.Delete});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdPRDetail.DefaultCellStyle = dataGridViewCellStyle2;
            this.grdPRDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grdPRDetail.Location = new System.Drawing.Point(16, 122);
            this.grdPRDetail.Name = "grdPRDetail";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdPRDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grdPRDetail.RowHeadersVisible = false;
            this.grdPRDetail.Size = new System.Drawing.Size(1012, 184);
            this.grdPRDetail.TabIndex = 195;
            this.grdPRDetail.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdPRDetail_CellContentClick);
            // 
            // LineNo
            // 
            this.LineNo.HeaderText = "Line No";
            this.LineNo.Name = "LineNo";
            this.LineNo.Width = 30;
            // 
            // AccountCode
            // 
            this.AccountCode.HeaderText = "Account Code";
            this.AccountCode.Name = "AccountCode";
            this.AccountCode.ReadOnly = true;
            // 
            // AccountName
            // 
            this.AccountName.HeaderText = "Account Name";
            this.AccountName.Name = "AccountName";
            this.AccountName.ReadOnly = true;
            this.AccountName.Width = 230;
            // 
            // SelAcDebit
            // 
            this.SelAcDebit.HeaderText = "";
            this.SelAcDebit.Name = "SelAcDebit";
            this.SelAcDebit.Text = "Sel";
            this.SelAcDebit.UseColumnTextForButtonValue = true;
            this.SelAcDebit.Width = 30;
            // 
            // AmountDebit
            // 
            this.AmountDebit.HeaderText = "Amount Debit";
            this.AmountDebit.Name = "AmountDebit";
            this.AmountDebit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // AmountCredit
            // 
            this.AmountCredit.HeaderText = "Amount Credit";
            this.AmountCredit.Name = "AmountCredit";
            // 
            // PartyCode
            // 
            this.PartyCode.HeaderText = "Party Code";
            this.PartyCode.Name = "PartyCode";
            this.PartyCode.ReadOnly = true;
            // 
            // PartyName
            // 
            this.PartyName.HeaderText = "Party Name";
            this.PartyName.Name = "PartyName";
            this.PartyName.ReadOnly = true;
            this.PartyName.Width = 240;
            // 
            // SelParty
            // 
            this.SelParty.HeaderText = "";
            this.SelParty.Name = "SelParty";
            this.SelParty.Text = "sel";
            this.SelParty.UseColumnTextForButtonValue = true;
            this.SelParty.Width = 30;
            // 
            // gSLType
            // 
            this.gSLType.HeaderText = "SLType";
            this.gSLType.Name = "gSLType";
            this.gSLType.ReadOnly = true;
            this.gSLType.Width = 60;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "Del";
            this.Delete.Name = "Delete";
            this.Delete.Text = "Del";
            this.Delete.UseColumnTextForButtonValue = true;
            this.Delete.Width = 30;
            // 
            // EditJV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(1064, 443);
            this.ControlBox = false;
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditJV";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Voucher Details";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPRDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtTotalCreditAmnt;
        private System.Windows.Forms.TextBox txtTotalDebitAmnt;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtAmountInWords;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtnarration;
        private System.Windows.Forms.Label lblJournalDate;
        private System.Windows.Forms.DateTimePicker dtJournalDate;
        private System.Windows.Forms.Label lblJournalNo;
        private System.Windows.Forms.TextBox txtJournalNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtTempDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTemporarryNo;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtINVTempDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtINVTempNo;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button btnAddLine;
        private System.Windows.Forms.DataGridView grdPRDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountName;
        private System.Windows.Forms.DataGridViewButtonColumn SelAcDebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn AmountDebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn AmountCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartyCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartyName;
        private System.Windows.Forms.DataGridViewButtonColumn SelParty;
        private System.Windows.Forms.DataGridViewTextBoxColumn gSLType;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
    }
}