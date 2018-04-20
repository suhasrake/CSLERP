namespace CSLERP
{
    partial class MenuItem
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlUI = new System.Windows.Forms.Panel();
            this.pnlMenuItemOuter = new System.Windows.Forms.Panel();
            this.pnlMenuItemInner = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVersionRequired = new System.Windows.Forms.TextBox();
            this.cmbDocument = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMenuItemUIName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDocumentName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMenuItemShortDescription = new System.Windows.Forms.TextBox();
            this.cmbMenuItemType = new System.Windows.Forms.ComboBox();
            this.txtMenuItemDescription = new System.Windows.Forms.TextBox();
            this.btnMenuItemSave = new System.Windows.Forms.Button();
            this.btnMenuItemCancel = new System.Windows.Forms.Button();
            this.cmbMenuItemStatus = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMenuItemID = new System.Windows.Forms.TextBox();
            this.pnlBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlMenuList = new System.Windows.Forms.Panel();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.cmbMenuGroup = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.empID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.empName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.empStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Document = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UIName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VersionRequired = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MenuGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlUI.SuspendLayout();
            this.pnlMenuItemOuter.SuspendLayout();
            this.pnlMenuItemInner.SuspendLayout();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlMenuList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.pnlBottomButtons);
            this.pnlUI.Controls.Add(this.pnlMenuItemOuter);
            this.pnlUI.Controls.Add(this.pnlMenuList);
            this.pnlUI.Location = new System.Drawing.Point(15, 15);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(1100, 540);
            this.pnlUI.TabIndex = 8;
            // 
            // pnlMenuItemOuter
            // 
            this.pnlMenuItemOuter.BackColor = System.Drawing.Color.Black;
            this.pnlMenuItemOuter.Controls.Add(this.pnlMenuItemInner);
            this.pnlMenuItemOuter.Location = new System.Drawing.Point(191, 26);
            this.pnlMenuItemOuter.Name = "pnlMenuItemOuter";
            this.pnlMenuItemOuter.Size = new System.Drawing.Size(544, 428);
            this.pnlMenuItemOuter.TabIndex = 8;
            this.pnlMenuItemOuter.Visible = false;
            // 
            // pnlMenuItemInner
            // 
            this.pnlMenuItemInner.BackColor = System.Drawing.Color.White;
            this.pnlMenuItemInner.Controls.Add(this.cmbMenuGroup);
            this.pnlMenuItemInner.Controls.Add(this.label8);
            this.pnlMenuItemInner.Controls.Add(this.textBox1);
            this.pnlMenuItemInner.Controls.Add(this.label3);
            this.pnlMenuItemInner.Controls.Add(this.txtVersionRequired);
            this.pnlMenuItemInner.Controls.Add(this.cmbDocument);
            this.pnlMenuItemInner.Controls.Add(this.label6);
            this.pnlMenuItemInner.Controls.Add(this.txtMenuItemUIName);
            this.pnlMenuItemInner.Controls.Add(this.label5);
            this.pnlMenuItemInner.Controls.Add(this.txtDocumentName);
            this.pnlMenuItemInner.Controls.Add(this.label1);
            this.pnlMenuItemInner.Controls.Add(this.txtMenuItemShortDescription);
            this.pnlMenuItemInner.Controls.Add(this.cmbMenuItemType);
            this.pnlMenuItemInner.Controls.Add(this.txtMenuItemDescription);
            this.pnlMenuItemInner.Controls.Add(this.btnMenuItemSave);
            this.pnlMenuItemInner.Controls.Add(this.btnMenuItemCancel);
            this.pnlMenuItemInner.Controls.Add(this.cmbMenuItemStatus);
            this.pnlMenuItemInner.Controls.Add(this.label10);
            this.pnlMenuItemInner.Controls.Add(this.label7);
            this.pnlMenuItemInner.Controls.Add(this.label4);
            this.pnlMenuItemInner.Controls.Add(this.label2);
            this.pnlMenuItemInner.Controls.Add(this.txtMenuItemID);
            this.pnlMenuItemInner.Location = new System.Drawing.Point(13, 16);
            this.pnlMenuItemInner.Name = "pnlMenuItemInner";
            this.pnlMenuItemInner.Size = new System.Drawing.Size(515, 393);
            this.pnlMenuItemInner.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(62, 218);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "Version Required";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtVersionRequired
            // 
            this.txtVersionRequired.Location = new System.Drawing.Point(167, 213);
            this.txtVersionRequired.Name = "txtVersionRequired";
            this.txtVersionRequired.Size = new System.Drawing.Size(154, 20);
            this.txtVersionRequired.TabIndex = 35;
            // 
            // cmbDocument
            // 
            this.cmbDocument.FormattingEnabled = true;
            this.cmbDocument.Location = new System.Drawing.Point(167, 161);
            this.cmbDocument.Name = "cmbDocument";
            this.cmbDocument.Size = new System.Drawing.Size(180, 21);
            this.cmbDocument.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(131, 194);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "UI";
            // 
            // txtMenuItemUIName
            // 
            this.txtMenuItemUIName.Location = new System.Drawing.Point(167, 187);
            this.txtMenuItemUIName.Name = "txtMenuItemUIName";
            this.txtMenuItemUIName.Size = new System.Drawing.Size(154, 20);
            this.txtMenuItemUIName.TabIndex = 32;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(93, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Document";
            // 
            // txtDocumentName
            // 
            this.txtDocumentName.Location = new System.Drawing.Point(167, 161);
            this.txtDocumentName.Name = "txtDocumentName";
            this.txtDocumentName.Size = new System.Drawing.Size(154, 20);
            this.txtDocumentName.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Short Description";
            // 
            // txtMenuItemShortDescription
            // 
            this.txtMenuItemShortDescription.Location = new System.Drawing.Point(167, 108);
            this.txtMenuItemShortDescription.Name = "txtMenuItemShortDescription";
            this.txtMenuItemShortDescription.Size = new System.Drawing.Size(154, 20);
            this.txtMenuItemShortDescription.TabIndex = 28;
            // 
            // cmbMenuItemType
            // 
            this.cmbMenuItemType.FormattingEnabled = true;
            this.cmbMenuItemType.Location = new System.Drawing.Point(167, 134);
            this.cmbMenuItemType.Name = "cmbMenuItemType";
            this.cmbMenuItemType.Size = new System.Drawing.Size(100, 21);
            this.cmbMenuItemType.TabIndex = 26;
            // 
            // txtMenuItemDescription
            // 
            this.txtMenuItemDescription.Location = new System.Drawing.Point(167, 82);
            this.txtMenuItemDescription.Name = "txtMenuItemDescription";
            this.txtMenuItemDescription.Size = new System.Drawing.Size(154, 20);
            this.txtMenuItemDescription.TabIndex = 25;
            // 
            // btnMenuItemSave
            // 
            this.btnMenuItemSave.Image = global::CSLERP.Properties.Resources.accept;
            this.btnMenuItemSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuItemSave.Location = new System.Drawing.Point(192, 318);
            this.btnMenuItemSave.Name = "btnMenuItemSave";
            this.btnMenuItemSave.Size = new System.Drawing.Size(75, 23);
            this.btnMenuItemSave.TabIndex = 22;
            this.btnMenuItemSave.Text = "Save";
            this.btnMenuItemSave.UseVisualStyleBackColor = true;
            this.btnMenuItemSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnMenuItemCancel
            // 
            this.btnMenuItemCancel.Image = global::CSLERP.Properties.Resources.cancel;
            this.btnMenuItemCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuItemCancel.Location = new System.Drawing.Point(88, 318);
            this.btnMenuItemCancel.Name = "btnMenuItemCancel";
            this.btnMenuItemCancel.Size = new System.Drawing.Size(75, 23);
            this.btnMenuItemCancel.TabIndex = 21;
            this.btnMenuItemCancel.Text = "Cancel";
            this.btnMenuItemCancel.UseVisualStyleBackColor = true;
            this.btnMenuItemCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbMenuItemStatus
            // 
            this.cmbMenuItemStatus.FormattingEnabled = true;
            this.cmbMenuItemStatus.Location = new System.Drawing.Point(167, 273);
            this.cmbMenuItemStatus.Name = "cmbMenuItemStatus";
            this.cmbMenuItemStatus.Size = new System.Drawing.Size(100, 21);
            this.cmbMenuItemStatus.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(89, 89);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Description";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(113, 281);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Status";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(65, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Menu Item Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(131, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "ID";
            // 
            // txtMenuItemID
            // 
            this.txtMenuItemID.Location = new System.Drawing.Point(167, 56);
            this.txtMenuItemID.Name = "txtMenuItemID";
            this.txtMenuItemID.Size = new System.Drawing.Size(71, 20);
            this.txtMenuItemID.TabIndex = 0;
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
            this.btnExit.Click += new System.EventHandler(this.btnMenuItemListExit_Click);
            // 
            // pnlMenuList
            // 
            this.pnlMenuList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlMenuList.Controls.Add(this.grdList);
            this.pnlMenuList.Location = new System.Drawing.Point(3, 57);
            this.pnlMenuList.Name = "pnlMenuList";
            this.pnlMenuList.Size = new System.Drawing.Size(1094, 357);
            this.pnlMenuList.TabIndex = 6;
            // 
            // grdList
            // 
            this.grdList.AllowUserToAddRows = false;
            this.grdList.AllowUserToDeleteRows = false;
            this.grdList.AllowUserToOrderColumns = true;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grdList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.grdList.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grdList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.grdList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.empID,
            this.empName,
            this.UserType,
            this.empStatus,
            this.Document,
            this.UIName,
            this.VersionRequired,
            this.MenuGroup,
            this.Status,
            this.Edit});
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdList.DefaultCellStyle = dataGridViewCellStyle22;
            this.grdList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdList.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdList.Location = new System.Drawing.Point(3, 19);
            this.grdList.Name = "grdList";
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle23.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle23.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle23.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle23.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdList.RowHeadersDefaultCellStyle = dataGridViewCellStyle23;
            this.grdList.RowHeadersVisible = false;
            dataGridViewCellStyle24.BackColor = System.Drawing.Color.AliceBlue;
            this.grdList.RowsDefaultCellStyle = dataGridViewCellStyle24;
            this.grdList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            this.grdList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdList.Size = new System.Drawing.Size(1088, 297);
            this.grdList.TabIndex = 4;
            this.grdList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdList_CellContentClick);
            // 
            // cmbMenuGroup
            // 
            this.cmbMenuGroup.FormattingEnabled = true;
            this.cmbMenuGroup.Location = new System.Drawing.Point(167, 239);
            this.cmbMenuGroup.Name = "cmbMenuGroup";
            this.cmbMenuGroup.Size = new System.Drawing.Size(180, 21);
            this.cmbMenuGroup.TabIndex = 39;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(93, 246);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 38;
            this.label8.Text = "MenuGroup";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(167, 239);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(154, 20);
            this.textBox1.TabIndex = 37;
            // 
            // empID
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.empID.DefaultCellStyle = dataGridViewCellStyle15;
            this.empID.HeaderText = "ID";
            this.empID.Name = "empID";
            // 
            // empName
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.empName.DefaultCellStyle = dataGridViewCellStyle16;
            this.empName.HeaderText = "Description";
            this.empName.Name = "empName";
            this.empName.Width = 200;
            // 
            // UserType
            // 
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.UserType.DefaultCellStyle = dataGridViewCellStyle17;
            this.UserType.HeaderText = "Short Description";
            this.UserType.Name = "UserType";
            // 
            // empStatus
            // 
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.empStatus.DefaultCellStyle = dataGridViewCellStyle18;
            this.empStatus.HeaderText = "Type";
            this.empStatus.Name = "empStatus";
            // 
            // Document
            // 
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Document.DefaultCellStyle = dataGridViewCellStyle19;
            this.Document.HeaderText = "Document";
            this.Document.Name = "Document";
            // 
            // UIName
            // 
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.UIName.DefaultCellStyle = dataGridViewCellStyle20;
            this.UIName.HeaderText = "UI Name";
            this.UIName.Name = "UIName";
            // 
            // VersionRequired
            // 
            this.VersionRequired.HeaderText = "Version Required";
            this.VersionRequired.Name = "VersionRequired";
            // 
            // MenuGroup
            // 
            this.MenuGroup.HeaderText = "MenuGroup";
            this.MenuGroup.Name = "MenuGroup";
            // 
            // Status
            // 
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Status.DefaultCellStyle = dataGridViewCellStyle21;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
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
            // MenuItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "MenuItem";
            this.Text = "ERP User";
            this.Load += new System.EventHandler(this.MenuItem_Load);
            this.pnlUI.ResumeLayout(false);
            this.pnlMenuItemOuter.ResumeLayout(false);
            this.pnlMenuItemInner.ResumeLayout(false);
            this.pnlMenuItemInner.PerformLayout();
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlMenuList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Panel pnlMenuItemOuter;
        private System.Windows.Forms.Panel pnlMenuItemInner;
        private System.Windows.Forms.Button btnMenuItemSave;
        private System.Windows.Forms.Button btnMenuItemCancel;
        private System.Windows.Forms.ComboBox cmbMenuItemStatus;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMenuItemID;
        private System.Windows.Forms.Panel pnlMenuList;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.TextBox txtMenuItemDescription;
        private System.Windows.Forms.ComboBox cmbMenuItemType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMenuItemShortDescription;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDocumentName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMenuItemUIName;
        private System.Windows.Forms.ComboBox cmbDocument;
        private System.Windows.Forms.FlowLayoutPanel pnlBottomButtons;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVersionRequired;
        private System.Windows.Forms.ComboBox cmbMenuGroup;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn empID;
        private System.Windows.Forms.DataGridViewTextBoxColumn empName;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserType;
        private System.Windows.Forms.DataGridViewTextBoxColumn empStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Document;
        private System.Windows.Forms.DataGridViewTextBoxColumn UIName;
        private System.Windows.Forms.DataGridViewTextBoxColumn VersionRequired;
        private System.Windows.Forms.DataGridViewTextBoxColumn MenuGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewButtonColumn Edit;
    }
}