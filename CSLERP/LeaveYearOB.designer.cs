namespace CSLERP
{
    partial class LeaveYearOB
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
            this.pnlUI = new System.Windows.Forms.Panel();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlList = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnShowOBList = new System.Windows.Forms.Button();
            this.pnlShowLeaveOB = new System.Windows.Forms.Panel();
            this.btnCancelLeaveOB = new System.Windows.Forms.Button();
            this.cmbLeaveYear = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlUI.SuspendLayout();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlList.SuspendLayout();
            this.pnlShowLeaveOB.SuspendLayout();
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
            this.pnlList.Controls.Add(this.button1);
            this.pnlList.Controls.Add(this.btnShowOBList);
            this.pnlList.Controls.Add(this.pnlShowLeaveOB);
            this.pnlList.Location = new System.Drawing.Point(10, 12);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(1082, 492);
            this.pnlList.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(138, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 43;
            this.button1.Text = "Create OB";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnShowOBList
            // 
            this.btnShowOBList.Location = new System.Drawing.Point(8, 10);
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
            this.pnlShowLeaveOB.Controls.Add(this.btnCancelLeaveOB);
            this.pnlShowLeaveOB.Controls.Add(this.cmbLeaveYear);
            this.pnlShowLeaveOB.Controls.Add(this.label2);
            this.pnlShowLeaveOB.Location = new System.Drawing.Point(308, 72);
            this.pnlShowLeaveOB.Name = "pnlShowLeaveOB";
            this.pnlShowLeaveOB.Size = new System.Drawing.Size(338, 110);
            this.pnlShowLeaveOB.TabIndex = 41;
            this.pnlShowLeaveOB.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlShowLeaveOB_Paint);
            // 
            // btnCancelLeaveOB
            // 
            this.btnCancelLeaveOB.Image = global::CSLERP.Properties.Resources.cancel;
            this.btnCancelLeaveOB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelLeaveOB.Location = new System.Drawing.Point(3, 82);
            this.btnCancelLeaveOB.Name = "btnCancelLeaveOB";
            this.btnCancelLeaveOB.Size = new System.Drawing.Size(75, 23);
            this.btnCancelLeaveOB.TabIndex = 11;
            this.btnCancelLeaveOB.Text = "Cancel";
            this.btnCancelLeaveOB.UseVisualStyleBackColor = true;
            this.btnCancelLeaveOB.Click += new System.EventHandler(this.btnCancelLeaveOB_Click);
            // 
            // cmbLeaveYear
            // 
            this.cmbLeaveYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLeaveYear.FormattingEnabled = true;
            this.cmbLeaveYear.Location = new System.Drawing.Point(173, 33);
            this.cmbLeaveYear.Name = "cmbLeaveYear";
            this.cmbLeaveYear.Size = new System.Drawing.Size(121, 21);
            this.cmbLeaveYear.TabIndex = 9;
            this.cmbLeaveYear.SelectedIndexChanged += new System.EventHandler(this.cmbLeaveYear_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(52, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Create Leave OB for :";
            // 
            // LeaveYearOB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "LeaveYearOB";
            this.Text = "ERP User";
            this.Load += new System.EventHandler(this.Region_Load);
            this.Enter += new System.EventHandler(this.LeaveYearOB_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlList.ResumeLayout(false);
            this.pnlShowLeaveOB.ResumeLayout(false);
            this.pnlShowLeaveOB.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Panel pnlList;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.Panel pnlShowLeaveOB;
        private System.Windows.Forms.Button btnCancelLeaveOB;
        private System.Windows.Forms.ComboBox cmbLeaveYear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnShowOBList;
        private System.Windows.Forms.Button button1;
    }
}