﻿namespace CSLERP
{
    partial class State
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
            this.pnlStateOuter = new System.Windows.Forms.Panel();
            this.pnlStateInner = new System.Windows.Forms.Panel();
            this.txtStateName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStateID = new System.Windows.Forms.TextBox();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlStateList = new System.Windows.Forms.Panel();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.StateID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlUI.SuspendLayout();
            this.pnlStateOuter.SuspendLayout();
            this.pnlStateInner.SuspendLayout();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlStateList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.pnlStateOuter);
            this.pnlUI.Controls.Add(this.pnlBottomButtons);
            this.pnlUI.Controls.Add(this.pnlStateList);
            this.pnlUI.Location = new System.Drawing.Point(15, 15);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(1100, 540);
            this.pnlUI.TabIndex = 8;
            // 
            // pnlStateOuter
            // 
            this.pnlStateOuter.BackColor = System.Drawing.Color.Black;
            this.pnlStateOuter.Controls.Add(this.pnlStateInner);
            this.pnlStateOuter.Location = new System.Drawing.Point(248, 55);
            this.pnlStateOuter.Name = "pnlStateOuter";
            this.pnlStateOuter.Size = new System.Drawing.Size(511, 360);
            this.pnlStateOuter.TabIndex = 11;
            this.pnlStateOuter.Visible = false;
            // 
            // pnlStateInner
            // 
            this.pnlStateInner.BackColor = System.Drawing.Color.White;
            this.pnlStateInner.Controls.Add(this.txtStateName);
            this.pnlStateInner.Controls.Add(this.btnSave);
            this.pnlStateInner.Controls.Add(this.btnCancel);
            this.pnlStateInner.Controls.Add(this.cmbStatus);
            this.pnlStateInner.Controls.Add(this.label10);
            this.pnlStateInner.Controls.Add(this.label7);
            this.pnlStateInner.Controls.Add(this.label2);
            this.pnlStateInner.Controls.Add(this.txtStateID);
            this.pnlStateInner.Location = new System.Drawing.Point(14, 15);
            this.pnlStateInner.Name = "pnlStateInner";
            this.pnlStateInner.Size = new System.Drawing.Size(483, 327);
            this.pnlStateInner.TabIndex = 0;
            // 
            // txtStateName
            // 
            this.txtStateName.Location = new System.Drawing.Point(175, 124);
            this.txtStateName.Name = "txtStateName";
            this.txtStateName.Size = new System.Drawing.Size(200, 20);
            this.txtStateName.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::CSLERP.Properties.Resources.accept;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(200, 206);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::CSLERP.Properties.Resources.cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(116, 206);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.ItemHeight = 13;
            this.cmbStatus.Items.AddRange(new object[] {
            "Active",
            "Deactive"});
            this.cmbStatus.Location = new System.Drawing.Point(175, 150);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(100, 21);
            this.cmbStatus.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(109, 127);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "State Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(135, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Status";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(112, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "State Code";
            // 
            // txtStateID
            // 
            this.txtStateID.Location = new System.Drawing.Point(175, 98);
            this.txtStateID.Name = "txtStateID";
            this.txtStateID.Size = new System.Drawing.Size(154, 20);
            this.txtStateID.TabIndex = 0;
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
            // pnlStateList
            // 
            this.pnlStateList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlStateList.Controls.Add(this.grdList);
            this.pnlStateList.Location = new System.Drawing.Point(90, 57);
            this.pnlStateList.Name = "pnlStateList";
            this.pnlStateList.Size = new System.Drawing.Size(839, 322);
            this.pnlStateList.TabIndex = 6;
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
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.SkyBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grdList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StateID,
            this.StateName,
            this.Status,
            this.CreateTime,
            this.CreateUser,
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
            this.grdList.EnableHeadersVisualStyles = false;
            this.grdList.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdList.Location = new System.Drawing.Point(179, 14);
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
            this.grdList.Size = new System.Drawing.Size(505, 297);
            this.grdList.TabIndex = 4;
            this.grdList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdList_CellContentClick);
            // 
            // StateID
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.StateID.DefaultCellStyle = dataGridViewCellStyle3;
            this.StateID.HeaderText = "State ID";
            this.StateID.Name = "StateID";
            // 
            // StateName
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.StateName.DefaultCellStyle = dataGridViewCellStyle4;
            this.StateName.HeaderText = "State Name";
            this.StateName.Name = "StateName";
            this.StateName.Width = 200;
            // 
            // Status
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Status.DefaultCellStyle = dataGridViewCellStyle5;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            // 
            // CreateTime
            // 
            dataGridViewCellStyle6.Format = "dd-MM-yyyy HH:mm:ss";
            this.CreateTime.DefaultCellStyle = dataGridViewCellStyle6;
            this.CreateTime.HeaderText = "Create Time";
            this.CreateTime.Name = "CreateTime";
            this.CreateTime.Visible = false;
            // 
            // CreateUser
            // 
            this.CreateUser.HeaderText = "Create User";
            this.CreateUser.Name = "CreateUser";
            this.CreateUser.Visible = false;
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
            // State
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "State";
            this.Text = "ERP User";
            this.Load += new System.EventHandler(this.Region_Load);
            this.Enter += new System.EventHandler(this.State_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlStateOuter.ResumeLayout(false);
            this.pnlStateInner.ResumeLayout(false);
            this.pnlStateInner.PerformLayout();
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlStateList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Panel pnlStateList;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.Panel pnlStateOuter;
        private System.Windows.Forms.Panel pnlStateInner;
        private System.Windows.Forms.TextBox txtStateName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStateID;
        private System.Windows.Forms.DataGridViewTextBoxColumn StateID;
        private System.Windows.Forms.DataGridViewTextBoxColumn StateName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateUser;
        private System.Windows.Forms.DataGridViewButtonColumn Edit;
    }
}