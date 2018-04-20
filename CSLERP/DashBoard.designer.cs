namespace CSLERP
{
    partial class DashBoard
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlUI = new System.Windows.Forms.Panel();
            this.pnlMovementReg = new System.Windows.Forms.Panel();
            this.grdMovementReg = new System.Windows.Forms.DataGridView();
            this.Sno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moveDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Purpose = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlannedExitTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlannedReturnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.pnlTapal = new System.Windows.Forms.Panel();
            this.grdTapaList = new System.Windows.Forms.DataGridView();
            this.SINo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RowId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TapalReference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InwardDocumentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceivedFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.View = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Move = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnTapalAlert = new System.Windows.Forms.Button();
            this.pnlShowCurrentDocument = new System.Windows.Forms.Panel();
            this.grdDocumentList = new System.Windows.Forms.DataGridView();
            this.LineNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemporaryNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemporaryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActivityType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FromUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Acknowledge = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Close = new System.Windows.Forms.DataGridViewLinkColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.tmrDashBoard = new System.Windows.Forms.Timer(this.components);
            this.pnlUI.SuspendLayout();
            this.pnlMovementReg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMovementReg)).BeginInit();
            this.pnlTapal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTapaList)).BeginInit();
            this.pnlShowCurrentDocument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDocumentList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlUI
            // 
            this.pnlUI.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlUI.Controls.Add(this.pnlMovementReg);
            this.pnlUI.Controls.Add(this.pnlTapal);
            this.pnlUI.Controls.Add(this.pnlShowCurrentDocument);
            this.pnlUI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlUI.Location = new System.Drawing.Point(15, 15);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(1100, 540);
            this.pnlUI.TabIndex = 8;
            // 
            // pnlMovementReg
            // 
            this.pnlMovementReg.Controls.Add(this.grdMovementReg);
            this.pnlMovementReg.Controls.Add(this.button2);
            this.pnlMovementReg.Location = new System.Drawing.Point(9, 341);
            this.pnlMovementReg.Name = "pnlMovementReg";
            this.pnlMovementReg.Size = new System.Drawing.Size(1081, 181);
            this.pnlMovementReg.TabIndex = 4;
            // 
            // grdMovementReg
            // 
            this.grdMovementReg.AllowUserToAddRows = false;
            this.grdMovementReg.AllowUserToDeleteRows = false;
            this.grdMovementReg.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Aqua;
            this.grdMovementReg.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdMovementReg.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grdMovementReg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdMovementReg.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grdMovementReg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMovementReg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Sno,
            this.moveDate,
            this.From,
            this.Purpose,
            this.PlannedExitTime,
            this.PlannedReturnTime,
            this.Status,
            this.DocumentStatus});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdMovementReg.DefaultCellStyle = dataGridViewCellStyle7;
            this.grdMovementReg.Location = new System.Drawing.Point(3, 33);
            this.grdMovementReg.Name = "grdMovementReg";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdMovementReg.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.grdMovementReg.RowHeadersVisible = false;
            this.grdMovementReg.RowHeadersWidth = 30;
            this.grdMovementReg.Size = new System.Drawing.Size(1075, 143);
            this.grdMovementReg.TabIndex = 1;
            this.grdMovementReg.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdMovementReg_CellContentClick);
            // 
            // Sno
            // 
            this.Sno.HeaderText = "SI No";
            this.Sno.Name = "Sno";
            this.Sno.ReadOnly = true;
            this.Sno.Width = 50;
            // 
            // moveDate
            // 
            dataGridViewCellStyle3.Format = "dd-MM-yyyy HH:mm:ss";
            dataGridViewCellStyle3.NullValue = null;
            this.moveDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.moveDate.HeaderText = "Request Time";
            this.moveDate.Name = "moveDate";
            this.moveDate.ReadOnly = true;
            this.moveDate.Width = 150;
            // 
            // From
            // 
            this.From.HeaderText = "From";
            this.From.Name = "From";
            this.From.ReadOnly = true;
            this.From.Width = 120;
            // 
            // Purpose
            // 
            this.Purpose.HeaderText = "Purpose";
            this.Purpose.Name = "Purpose";
            this.Purpose.ReadOnly = true;
            this.Purpose.Width = 300;
            // 
            // PlannedExitTime
            // 
            dataGridViewCellStyle4.Format = "dd-MM-yyyy HH:mm:ss";
            this.PlannedExitTime.DefaultCellStyle = dataGridViewCellStyle4;
            this.PlannedExitTime.HeaderText = "Planned Exit Time";
            this.PlannedExitTime.Name = "PlannedExitTime";
            this.PlannedExitTime.ReadOnly = true;
            this.PlannedExitTime.Width = 150;
            // 
            // PlannedReturnTime
            // 
            dataGridViewCellStyle5.Format = "dd-MM-yyyy HH:mm:ss";
            this.PlannedReturnTime.DefaultCellStyle = dataGridViewCellStyle5;
            this.PlannedReturnTime.HeaderText = "Planned Return Time";
            this.PlannedReturnTime.Name = "PlannedReturnTime";
            this.PlannedReturnTime.ReadOnly = true;
            this.PlannedReturnTime.Width = 150;
            // 
            // Status
            // 
            dataGridViewCellStyle6.Format = "d";
            dataGridViewCellStyle6.NullValue = null;
            this.Status.DefaultCellStyle = dataGridViewCellStyle6;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 150;
            // 
            // DocumentStatus
            // 
            this.DocumentStatus.HeaderText = "Document Status";
            this.DocumentStatus.Name = "DocumentStatus";
            this.DocumentStatus.ReadOnly = true;
            this.DocumentStatus.Visible = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.MediumPurple;
            this.button2.Location = new System.Drawing.Point(3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(1075, 27);
            this.button2.TabIndex = 0;
            this.button2.Text = "Employee Movement";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // pnlTapal
            // 
            this.pnlTapal.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlTapal.Controls.Add(this.grdTapaList);
            this.pnlTapal.Controls.Add(this.btnTapalAlert);
            this.pnlTapal.Location = new System.Drawing.Point(9, 172);
            this.pnlTapal.Name = "pnlTapal";
            this.pnlTapal.Size = new System.Drawing.Size(1081, 163);
            this.pnlTapal.TabIndex = 3;
            // 
            // grdTapaList
            // 
            this.grdTapaList.AllowUserToAddRows = false;
            this.grdTapaList.AllowUserToDeleteRows = false;
            this.grdTapaList.AllowUserToOrderColumns = true;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.Aqua;
            this.grdTapaList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.grdTapaList.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grdTapaList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdTapaList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.grdTapaList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTapaList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SINo,
            this.RowId,
            this.DocID,
            this.TapalReference,
            this.Date,
            this.FileName,
            this.InwardDocumentType,
            this.ReceivedFrom,
            this.Description,
            this.Sender,
            this.View,
            this.Move,
            this.Delete});
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdTapaList.DefaultCellStyle = dataGridViewCellStyle13;
            this.grdTapaList.Location = new System.Drawing.Point(3, 27);
            this.grdTapaList.Name = "grdTapaList";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdTapaList.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.grdTapaList.RowHeadersVisible = false;
            this.grdTapaList.RowHeadersWidth = 30;
            this.grdTapaList.Size = new System.Drawing.Size(1075, 131);
            this.grdTapaList.TabIndex = 0;
            this.grdTapaList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdTapaList_CellContentClick);
            // 
            // SINo
            // 
            this.SINo.HeaderText = "SI No";
            this.SINo.Name = "SINo";
            this.SINo.Width = 50;
            // 
            // RowId
            // 
            this.RowId.HeaderText = "RowId";
            this.RowId.Name = "RowId";
            this.RowId.Visible = false;
            // 
            // DocID
            // 
            this.DocID.HeaderText = "DocumentID";
            this.DocID.Name = "DocID";
            this.DocID.Visible = false;
            // 
            // TapalReference
            // 
            this.TapalReference.HeaderText = "TapalReference";
            this.TapalReference.Name = "TapalReference";
            this.TapalReference.Visible = false;
            // 
            // Date
            // 
            dataGridViewCellStyle11.Format = "dd-MM-yyyy";
            dataGridViewCellStyle11.NullValue = null;
            this.Date.DefaultCellStyle = dataGridViewCellStyle11;
            this.Date.HeaderText = "Date";
            this.Date.Name = "Date";
            this.Date.Width = 80;
            // 
            // FileName
            // 
            this.FileName.HeaderText = "File Name";
            this.FileName.Name = "FileName";
            this.FileName.Visible = false;
            // 
            // InwardDocumentType
            // 
            this.InwardDocumentType.HeaderText = "InwardDocumentType";
            this.InwardDocumentType.Name = "InwardDocumentType";
            this.InwardDocumentType.Width = 190;
            // 
            // ReceivedFrom
            // 
            this.ReceivedFrom.HeaderText = "ReceivedFrom";
            this.ReceivedFrom.Name = "ReceivedFrom";
            this.ReceivedFrom.Width = 200;
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 190;
            // 
            // Sender
            // 
            dataGridViewCellStyle12.Format = "d";
            dataGridViewCellStyle12.NullValue = null;
            this.Sender.DefaultCellStyle = dataGridViewCellStyle12;
            this.Sender.HeaderText = "Sent by";
            this.Sender.Name = "Sender";
            this.Sender.Width = 150;
            // 
            // View
            // 
            this.View.HeaderText = "";
            this.View.Name = "View";
            this.View.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.View.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.View.Text = "View";
            this.View.UseColumnTextForButtonValue = true;
            this.View.Width = 80;
            // 
            // Move
            // 
            this.Move.HeaderText = "";
            this.Move.Name = "Move";
            this.Move.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Move.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Move.Text = "Move";
            this.Move.UseColumnTextForButtonValue = true;
            this.Move.Width = 80;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "";
            this.Delete.Name = "Delete";
            this.Delete.Text = "Del";
            this.Delete.UseColumnTextForButtonValue = true;
            this.Delete.Width = 50;
            // 
            // btnTapalAlert
            // 
            this.btnTapalAlert.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnTapalAlert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTapalAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTapalAlert.Location = new System.Drawing.Point(3, 0);
            this.btnTapalAlert.Name = "btnTapalAlert";
            this.btnTapalAlert.Size = new System.Drawing.Size(1075, 23);
            this.btnTapalAlert.TabIndex = 0;
            this.btnTapalAlert.Text = "Tapal Documents";
            this.btnTapalAlert.UseVisualStyleBackColor = false;
            // 
            // pnlShowCurrentDocument
            // 
            this.pnlShowCurrentDocument.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlShowCurrentDocument.Controls.Add(this.grdDocumentList);
            this.pnlShowCurrentDocument.Controls.Add(this.button1);
            this.pnlShowCurrentDocument.Location = new System.Drawing.Point(9, 14);
            this.pnlShowCurrentDocument.Name = "pnlShowCurrentDocument";
            this.pnlShowCurrentDocument.Size = new System.Drawing.Size(1081, 152);
            this.pnlShowCurrentDocument.TabIndex = 2;
            // 
            // grdDocumentList
            // 
            this.grdDocumentList.AllowUserToAddRows = false;
            this.grdDocumentList.AllowUserToDeleteRows = false;
            this.grdDocumentList.AllowUserToOrderColumns = true;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.Aqua;
            this.grdDocumentList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle15;
            this.grdDocumentList.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grdDocumentList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grdDocumentList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDocumentList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LineNo,
            this.DocumentID,
            this.DocumentName,
            this.TemporaryNo,
            this.TemporaryDate,
            this.DocumentNo,
            this.DocumentDate,
            this.ActivityType,
            this.FromUser,
            this.Acknowledge,
            this.Close});
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdDocumentList.DefaultCellStyle = dataGridViewCellStyle18;
            this.grdDocumentList.Location = new System.Drawing.Point(3, 29);
            this.grdDocumentList.Name = "grdDocumentList";
            this.grdDocumentList.RowHeadersVisible = false;
            this.grdDocumentList.RowHeadersWidth = 30;
            this.grdDocumentList.Size = new System.Drawing.Size(1075, 119);
            this.grdDocumentList.TabIndex = 0;
            this.grdDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdDocumentList_CellContentClick);
            this.grdDocumentList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdDocumentList_DataError);
            // 
            // LineNo
            // 
            this.LineNo.HeaderText = "SI No";
            this.LineNo.Name = "LineNo";
            this.LineNo.Width = 50;
            // 
            // DocumentID
            // 
            this.DocumentID.HeaderText = "Doc ID";
            this.DocumentID.Name = "DocumentID";
            // 
            // DocumentName
            // 
            this.DocumentName.HeaderText = "Doc Name";
            this.DocumentName.Name = "DocumentName";
            // 
            // TemporaryNo
            // 
            this.TemporaryNo.HeaderText = "Temp No";
            this.TemporaryNo.Name = "TemporaryNo";
            // 
            // TemporaryDate
            // 
            dataGridViewCellStyle16.Format = "dd-MM-yyyy";
            dataGridViewCellStyle16.NullValue = null;
            this.TemporaryDate.DefaultCellStyle = dataGridViewCellStyle16;
            this.TemporaryDate.HeaderText = "Temp Date";
            this.TemporaryDate.Name = "TemporaryDate";
            // 
            // DocumentNo
            // 
            this.DocumentNo.HeaderText = "Doc No";
            this.DocumentNo.Name = "DocumentNo";
            // 
            // DocumentDate
            // 
            dataGridViewCellStyle17.Format = "dd-MM-yyyy";
            dataGridViewCellStyle17.NullValue = null;
            this.DocumentDate.DefaultCellStyle = dataGridViewCellStyle17;
            this.DocumentDate.HeaderText = "Doc Date";
            this.DocumentDate.Name = "DocumentDate";
            // 
            // ActivityType
            // 
            this.ActivityType.HeaderText = "Activity Type";
            this.ActivityType.Name = "ActivityType";
            // 
            // FromUser
            // 
            this.FromUser.HeaderText = "From User";
            this.FromUser.Name = "FromUser";
            // 
            // Acknowledge
            // 
            this.Acknowledge.HeaderText = "";
            this.Acknowledge.Name = "Acknowledge";
            this.Acknowledge.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Acknowledge.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Acknowledge.Text = "Acknowledge";
            this.Acknowledge.UseColumnTextForLinkValue = true;
            this.Acknowledge.Visible = false;
            // 
            // Close
            // 
            this.Close.HeaderText = "";
            this.Close.Name = "Close";
            this.Close.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Close.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Close.Text = "Hide";
            this.Close.UseColumnTextForLinkValue = true;
            this.Close.Width = 123;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Maroon;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(3, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(1075, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Alerts";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // tmrDashBoard
            // 
            this.tmrDashBoard.Enabled = true;
            this.tmrDashBoard.Interval = 60000;
            this.tmrDashBoard.Tick += new System.EventHandler(this.tmrDashBoard_Tick);
            // 
            // DashBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1134, 597);
            this.Controls.Add(this.pnlUI);
            this.Name = "DashBoard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.DashBoard_Load);
            this.Enter += new System.EventHandler(this.DashBoard_Enter);
            this.pnlUI.ResumeLayout(false);
            this.pnlMovementReg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMovementReg)).EndInit();
            this.pnlTapal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTapaList)).EndInit();
            this.pnlShowCurrentDocument.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDocumentList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.DataGridView grdDocumentList;
        private System.Windows.Forms.Panel pnlShowCurrentDocument;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer tmrDashBoard;
        private System.Windows.Forms.Panel pnlTapal;
        private System.Windows.Forms.DataGridView grdTapaList;
        private System.Windows.Forms.Button btnTapalAlert;
        private System.Windows.Forms.Panel pnlMovementReg;
        private System.Windows.Forms.DataGridView grdMovementReg;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sno;
        private System.Windows.Forms.DataGridViewTextBoxColumn moveDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn From;
        private System.Windows.Forms.DataGridViewTextBoxColumn Purpose;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlannedExitTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlannedReturnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn SINo;
        private System.Windows.Forms.DataGridViewTextBoxColumn RowId;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TapalReference;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn InwardDocumentType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceivedFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sender;
        private System.Windows.Forms.DataGridViewButtonColumn View;
        private System.Windows.Forms.DataGridViewButtonColumn Move;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemporaryNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemporaryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActivityType;
        private System.Windows.Forms.DataGridViewTextBoxColumn FromUser;
        private System.Windows.Forms.DataGridViewLinkColumn Acknowledge;
        private System.Windows.Forms.DataGridViewLinkColumn Close;
    }
}