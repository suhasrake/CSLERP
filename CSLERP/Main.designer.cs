namespace CSLERP
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.pnlMainHeader = new System.Windows.Forms.Panel();
            this.lblEmployeeName = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picMainLogo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlMainOptionButtons = new System.Windows.Forms.Panel();
            this.pnlMainContent = new System.Windows.Forms.Panel();
            this.txtVersionUpdate = new System.Windows.Forms.TextBox();
            this.MSuserAction = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrMain = new System.Windows.Forms.Timer(this.components);
            this.pnlMainHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMainLogo)).BeginInit();
            this.pnlMainContent.SuspendLayout();
            this.MSuserAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMainHeader
            // 
            this.pnlMainHeader.BackColor = System.Drawing.Color.White;
            this.pnlMainHeader.Controls.Add(this.lblEmployeeName);
            this.pnlMainHeader.Controls.Add(this.lblVersion);
            this.pnlMainHeader.Controls.Add(this.lblTime);
            this.pnlMainHeader.Controls.Add(this.pictureBox1);
            this.pnlMainHeader.Controls.Add(this.picMainLogo);
            this.pnlMainHeader.Controls.Add(this.label1);
            this.pnlMainHeader.Location = new System.Drawing.Point(10, 5);
            this.pnlMainHeader.Name = "pnlMainHeader";
            this.pnlMainHeader.Size = new System.Drawing.Size(1342, 72);
            this.pnlMainHeader.TabIndex = 2;
            // 
            // lblEmployeeName
            // 
            this.lblEmployeeName.AutoSize = true;
            this.lblEmployeeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeeName.Location = new System.Drawing.Point(1166, 17);
            this.lblEmployeeName.Name = "lblEmployeeName";
            this.lblEmployeeName.Size = new System.Drawing.Size(76, 16);
            this.lblEmployeeName.TabIndex = 9;
            this.lblEmployeeName.Text = "Emp Name";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(95, 55);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(31, 9);
            this.lblVersion.TabIndex = 8;
            this.lblVersion.Text = "Version";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(1166, 40);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(72, 16);
            this.lblTime.TabIndex = 6;
            this.lblTime.Text = "Time Here";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CSLERP.Properties.Resources.man_2;
            this.pictureBox1.Location = new System.Drawing.Point(1287, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(52, 65);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // picMainLogo
            // 
            this.picMainLogo.Image = ((System.Drawing.Image)(resources.GetObject("picMainLogo.Image")));
            this.picMainLogo.Location = new System.Drawing.Point(5, 3);
            this.picMainLogo.Name = "picMainLogo";
            this.picMainLogo.Size = new System.Drawing.Size(68, 72);
            this.picMainLogo.TabIndex = 2;
            this.picMainLogo.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Elephant", 24.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(77, 18);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(109, 42);
            this.label1.TabIndex = 0;
            this.label1.Text = "ERP";
            // 
            // pnlMainOptionButtons
            // 
            this.pnlMainOptionButtons.AutoScroll = true;
            this.pnlMainOptionButtons.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlMainOptionButtons.Location = new System.Drawing.Point(10, 82);
            this.pnlMainOptionButtons.Name = "pnlMainOptionButtons";
            this.pnlMainOptionButtons.Size = new System.Drawing.Size(200, 600);
            this.pnlMainOptionButtons.TabIndex = 3;
            this.pnlMainOptionButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMainOptionButtons_Paint);
            // 
            // pnlMainContent
            // 
            this.pnlMainContent.BackColor = System.Drawing.Color.LightGray;
            this.pnlMainContent.Controls.Add(this.txtVersionUpdate);
            this.pnlMainContent.Location = new System.Drawing.Point(216, 83);
            this.pnlMainContent.Name = "pnlMainContent";
            this.pnlMainContent.Size = new System.Drawing.Size(1135, 600);
            this.pnlMainContent.TabIndex = 7;
            // 
            // txtVersionUpdate
            // 
            this.txtVersionUpdate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtVersionUpdate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtVersionUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVersionUpdate.ForeColor = System.Drawing.Color.Red;
            this.txtVersionUpdate.Location = new System.Drawing.Point(216, 578);
            this.txtVersionUpdate.Name = "txtVersionUpdate";
            this.txtVersionUpdate.ReadOnly = true;
            this.txtVersionUpdate.Size = new System.Drawing.Size(681, 14);
            this.txtVersionUpdate.TabIndex = 0;
            // 
            // MSuserAction
            // 
            this.MSuserAction.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu1ToolStripMenuItem,
            this.menu2ToolStripMenuItem,
            this.menu3ToolStripMenuItem});
            this.MSuserAction.Name = "MSuserAction";
            this.MSuserAction.Size = new System.Drawing.Size(169, 70);
            // 
            // menu1ToolStripMenuItem
            // 
            this.menu1ToolStripMenuItem.Name = "menu1ToolStripMenuItem";
            this.menu1ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.menu1ToolStripMenuItem.Text = "Change Password";
            this.menu1ToolStripMenuItem.Click += new System.EventHandler(this.menu1ToolStripMenuItem_Click);
            // 
            // menu2ToolStripMenuItem
            // 
            this.menu2ToolStripMenuItem.Name = "menu2ToolStripMenuItem";
            this.menu2ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.menu2ToolStripMenuItem.Text = "Personal Info";
            // 
            // menu3ToolStripMenuItem
            // 
            this.menu3ToolStripMenuItem.Name = "menu3ToolStripMenuItem";
            this.menu3ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.menu3ToolStripMenuItem.Text = "Logout";
            this.menu3ToolStripMenuItem.Click += new System.EventHandler(this.menu3ToolStripMenuItem_Click);
            // 
            // tmrMain
            // 
            this.tmrMain.Interval = 60000;
            this.tmrMain.Tick += new System.EventHandler(this.tmrMain_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ClientSize = new System.Drawing.Size(1350, 712);
            this.Controls.Add(this.pnlMainContent);
            this.Controls.Add(this.pnlMainOptionButtons);
            this.Controls.Add(this.pnlMainHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "ERP";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.pnlMainHeader.ResumeLayout(false);
            this.pnlMainHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMainLogo)).EndInit();
            this.pnlMainContent.ResumeLayout(false);
            this.pnlMainContent.PerformLayout();
            this.MSuserAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMainHeader;
        private System.Windows.Forms.PictureBox picMainLogo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlMainOptionButtons;
        private System.Windows.Forms.Panel pnlMainContent;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip MSuserAction;
        private System.Windows.Forms.ToolStripMenuItem menu1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menu2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menu3ToolStripMenuItem;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer tmrMain;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblEmployeeName;
        private System.Windows.Forms.TextBox txtVersionUpdate;
    }
}