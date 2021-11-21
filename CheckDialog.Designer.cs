
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
            System.Windows.Forms.GroupBox gbExt;
            System.Windows.Forms.GroupBox gbFile;
            this.clbTrusted = new System.Windows.Forms.CheckedListBox();
            this.clbExt = new System.Windows.Forms.CheckedListBox();
            this.clbFile = new System.Windows.Forms.CheckedListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            splitContainer3 = new System.Windows.Forms.SplitContainer();
            gbTrusted = new System.Windows.Forms.GroupBox();
            gbExt = new System.Windows.Forms.GroupBox();
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
            splitContainer1.Size = new System.Drawing.Size(784, 461);
            splitContainer1.SplitterDistance = 408;
            splitContainer1.TabIndex = 3;
            splitContainer1.TabStop = false;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.IsSplitterFixed = true;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(gbFile);
            splitContainer2.Size = new System.Drawing.Size(784, 408);
            splitContainer2.SplitterDistance = 502;
            splitContainer2.TabIndex = 0;
            splitContainer2.TabStop = false;
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer3.IsSplitterFixed = true;
            splitContainer3.Location = new System.Drawing.Point(0, 0);
            splitContainer3.Name = "splitContainer3";
            splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(gbTrusted);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(gbExt);
            splitContainer3.Size = new System.Drawing.Size(502, 408);
            splitContainer3.SplitterDistance = 190;
            splitContainer3.TabIndex = 0;
            splitContainer3.TabStop = false;
            // 
            // gbTrusted
            // 
            gbTrusted.Controls.Add(this.clbTrusted);
            gbTrusted.Cursor = System.Windows.Forms.Cursors.Default;
            gbTrusted.Dock = System.Windows.Forms.DockStyle.Fill;
            gbTrusted.Location = new System.Drawing.Point(0, 0);
            gbTrusted.Name = "gbTrusted";
            gbTrusted.Size = new System.Drawing.Size(502, 190);
            gbTrusted.TabIndex = 0;
            gbTrusted.TabStop = false;
            gbTrusted.Text = " 信頼済みの送信先";
            // 
            // clbTrusted
            // 
            this.clbTrusted.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbTrusted.CheckOnClick = true;
            this.clbTrusted.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clbTrusted.FormattingEnabled = true;
            this.clbTrusted.HorizontalScrollbar = true;
            this.clbTrusted.Location = new System.Drawing.Point(12, 19);
            this.clbTrusted.Name = "clbTrusted";
            this.clbTrusted.Size = new System.Drawing.Size(480, 154);
            this.clbTrusted.TabIndex = 0;
            this.clbTrusted.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // gbExt
            // 
            gbExt.Controls.Add(this.clbExt);
            gbExt.Cursor = System.Windows.Forms.Cursors.Default;
            gbExt.Dock = System.Windows.Forms.DockStyle.Fill;
            gbExt.Location = new System.Drawing.Point(0, 0);
            gbExt.Name = "gbExt";
            gbExt.Size = new System.Drawing.Size(502, 214);
            gbExt.TabIndex = 1;
            gbExt.TabStop = false;
            gbExt.Text = "外部ドメインの送信先";
            // 
            // clbExt
            // 
            this.clbExt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbExt.CheckOnClick = true;
            this.clbExt.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clbExt.ForeColor = System.Drawing.Color.Red;
            this.clbExt.FormattingEnabled = true;
            this.clbExt.HorizontalScrollbar = true;
            this.clbExt.Location = new System.Drawing.Point(16, 27);
            this.clbExt.Name = "clbExt";
            this.clbExt.Size = new System.Drawing.Size(480, 184);
            this.clbExt.TabIndex = 1;
            this.clbExt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // gbFile
            // 
            gbFile.Controls.Add(this.clbFile);
            gbFile.Cursor = System.Windows.Forms.Cursors.Default;
            gbFile.Dock = System.Windows.Forms.DockStyle.Fill;
            gbFile.Location = new System.Drawing.Point(0, 0);
            gbFile.Name = "gbFile";
            gbFile.Size = new System.Drawing.Size(278, 408);
            gbFile.TabIndex = 2;
            gbFile.TabStop = false;
            gbFile.Text = "添付ファイル";
            // 
            // clbFile
            // 
            this.clbFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbFile.CheckOnClick = true;
            this.clbFile.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clbFile.FormattingEnabled = true;
            this.clbFile.HorizontalScrollbar = true;
            this.clbFile.Location = new System.Drawing.Point(6, 19);
            this.clbFile.Name = "clbFile";
            this.clbFile.Size = new System.Drawing.Size(260, 379);
            this.clbFile.TabIndex = 2;
            this.clbFile.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
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
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(splitContainer1);
            this.Name = "CheckDialog";
            this.Text = "メールを送信しますか？ - CheckMyMail";
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
        private System.Windows.Forms.CheckedListBox clbTrusted;
        private System.Windows.Forms.CheckedListBox clbExt;
        private System.Windows.Forms.CheckedListBox clbFile;
    }
}