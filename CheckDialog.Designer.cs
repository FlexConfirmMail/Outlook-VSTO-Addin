
namespace CheckMyMail
{
    partial class CheckDialog
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
            System.Windows.Forms.SplitContainer splitContainer1;
            System.Windows.Forms.SplitContainer splitContainer2;
            System.Windows.Forms.SplitContainer splitContainer3;
            System.Windows.Forms.GroupBox gbTrusted;
            System.Windows.Forms.ColumnHeader chTrustedType;
            System.Windows.Forms.GroupBox gbExt;
            System.Windows.Forms.ColumnHeader chExtType;
            System.Windows.Forms.GroupBox gbFile;
            this.lvTrusted = new System.Windows.Forms.ListView();
            this.chTrustedAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvExt = new System.Windows.Forms.ListView();
            this.chExtAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvFile = new System.Windows.Forms.ListView();
            this.chFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            splitContainer3 = new System.Windows.Forms.SplitContainer();
            gbTrusted = new System.Windows.Forms.GroupBox();
            chTrustedType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            gbExt = new System.Windows.Forms.GroupBox();
            chExtType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            gbFile = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer2)).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer3)).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            gbTrusted.SuspendLayout();
            gbExt.SuspendLayout();
            gbFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(this.btnOK);
            splitContainer1.Panel2.Controls.Add(this.btnCancel);
            splitContainer1.Size = new System.Drawing.Size(784, 511);
            splitContainer1.SplitterDistance = 458;
            splitContainer1.TabIndex = 3;
            splitContainer1.TabStop = false;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(gbFile);
            splitContainer2.Size = new System.Drawing.Size(784, 458);
            splitContainer2.SplitterDistance = 332;
            splitContainer2.TabIndex = 0;
            splitContainer2.TabStop = false;
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer3.Location = new System.Drawing.Point(0, 0);
            splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(gbTrusted);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(gbExt);
            splitContainer3.Size = new System.Drawing.Size(784, 332);
            splitContainer3.SplitterDistance = 389;
            splitContainer3.TabIndex = 0;
            splitContainer3.TabStop = false;
            // 
            // gbTrusted
            // 
            gbTrusted.Controls.Add(this.lvTrusted);
            gbTrusted.Cursor = System.Windows.Forms.Cursors.Default;
            gbTrusted.Dock = System.Windows.Forms.DockStyle.Fill;
            gbTrusted.Location = new System.Drawing.Point(0, 0);
            gbTrusted.Name = "gbTrusted";
            gbTrusted.Size = new System.Drawing.Size(389, 332);
            gbTrusted.TabIndex = 0;
            gbTrusted.TabStop = false;
            gbTrusted.Text = " 信頼済みの送信先";
            // 
            // lvTrusted
            // 
            this.lvTrusted.CheckBoxes = true;
            this.lvTrusted.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            chTrustedType,
            this.chTrustedAddress});
            this.lvTrusted.Cursor = System.Windows.Forms.Cursors.Default;
            this.lvTrusted.FullRowSelect = true;
            this.lvTrusted.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvTrusted.HideSelection = false;
            this.lvTrusted.Location = new System.Drawing.Point(10, 19);
            this.lvTrusted.MultiSelect = false;
            this.lvTrusted.Name = "lvTrusted";
            this.lvTrusted.Size = new System.Drawing.Size(373, 312);
            this.lvTrusted.TabIndex = 0;
            this.lvTrusted.UseCompatibleStateImageBehavior = false;
            this.lvTrusted.View = System.Windows.Forms.View.Details;
            this.lvTrusted.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lvTrusted_ItemCheck);
            this.lvTrusted.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvTrusted_ItemChecked);
            this.lvTrusted.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvTrusted_MouseDown);
            // 
            // chTrustedType
            // 
            chTrustedType.Width = 50;
            // 
            // chTrustedAddress
            // 
            this.chTrustedAddress.Width = 50;
            // 
            // gbExt
            // 
            gbExt.Controls.Add(this.lvExt);
            gbExt.Cursor = System.Windows.Forms.Cursors.Default;
            gbExt.Dock = System.Windows.Forms.DockStyle.Fill;
            gbExt.Location = new System.Drawing.Point(0, 0);
            gbExt.Name = "gbExt";
            gbExt.Size = new System.Drawing.Size(391, 332);
            gbExt.TabIndex = 1;
            gbExt.TabStop = false;
            gbExt.Text = "外部ドメインの送信先";
            // 
            // lvExt
            // 
            this.lvExt.CheckBoxes = true;
            this.lvExt.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            chExtType,
            this.chExtAddress});
            this.lvExt.FullRowSelect = true;
            this.lvExt.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvExt.HideSelection = false;
            this.lvExt.Location = new System.Drawing.Point(10, 19);
            this.lvExt.MultiSelect = false;
            this.lvExt.Name = "lvExt";
            this.lvExt.Size = new System.Drawing.Size(375, 312);
            this.lvExt.TabIndex = 1;
            this.lvExt.UseCompatibleStateImageBehavior = false;
            this.lvExt.View = System.Windows.Forms.View.Details;
            this.lvExt.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lvExt_ItemCheck);
            this.lvExt.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvExt_ItemChecked);
            this.lvExt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvExt_MouseDown);
            // 
            // chExtType
            // 
            chExtType.Width = 50;
            // 
            // chExtAddress
            // 
            this.chExtAddress.Width = 50;
            // 
            // gbFile
            // 
            gbFile.Controls.Add(this.lvFile);
            gbFile.Cursor = System.Windows.Forms.Cursors.Default;
            gbFile.Dock = System.Windows.Forms.DockStyle.Fill;
            gbFile.Location = new System.Drawing.Point(0, 0);
            gbFile.Name = "gbFile";
            gbFile.Size = new System.Drawing.Size(784, 122);
            gbFile.TabIndex = 2;
            gbFile.TabStop = false;
            gbFile.Text = "添付ファイル";
            // 
            // lvFile
            // 
            this.lvFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFile.CheckBoxes = true;
            this.lvFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFileName});
            this.lvFile.FullRowSelect = true;
            this.lvFile.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvFile.HideSelection = false;
            this.lvFile.Location = new System.Drawing.Point(3, 19);
            this.lvFile.MultiSelect = false;
            this.lvFile.Name = "lvFile";
            this.lvFile.Size = new System.Drawing.Size(778, 97);
            this.lvFile.TabIndex = 2;
            this.lvFile.UseCompatibleStateImageBehavior = false;
            this.lvFile.View = System.Windows.Forms.View.Details;
            this.lvFile.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lvFile_ItemCheck);
            this.lvFile.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvFile_ItemChecked);
            this.lvFile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvFile_MouseDown);
            // 
            // chFileName
            // 
            this.chFileName.Width = 268;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.SystemColors.Control;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOK.Location = new System.Drawing.Point(606, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 35);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "送信";
            this.btnOK.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(697, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 35);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // CheckDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 511);
            this.Controls.Add(splitContainer1);
            this.Name = "CheckDialog";
            this.Text = "メールの宛先を確認してください - CheckMyMail";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer2)).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer3)).EndInit();
            splitContainer3.ResumeLayout(false);
            gbTrusted.ResumeLayout(false);
            gbExt.ResumeLayout(false);
            gbFile.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ListView lvTrusted;
        private System.Windows.Forms.ListView lvExt;
        private System.Windows.Forms.ListView lvFile;
        private System.Windows.Forms.ColumnHeader chFileName;
        private System.Windows.Forms.ColumnHeader chTrustedAddress;
        private System.Windows.Forms.ColumnHeader chExtAddress;
    }
}