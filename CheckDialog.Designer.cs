
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
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.trustedGB = new System.Windows.Forms.GroupBox();
            this.extGB = new System.Windows.Forms.GroupBox();
            this.attachGB = new System.Windows.Forms.GroupBox();
            this.ButtonOK = new System.Windows.Forms.Button();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            splitContainer3 = new System.Windows.Forms.SplitContainer();
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
            this.SuspendLayout();
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(697, 8);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 35);
            this.ButtonCancel.TabIndex = 2;
            this.ButtonCancel.Text = "キャンセル";
            this.ButtonCancel.UseVisualStyleBackColor = false;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
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
            splitContainer1.Panel2.Controls.Add(this.ButtonOK);
            splitContainer1.Panel2.Controls.Add(this.ButtonCancel);
            splitContainer1.Size = new System.Drawing.Size(784, 461);
            splitContainer1.SplitterDistance = 408;
            splitContainer1.TabIndex = 3;
            splitContainer1.TabStop = false;
            splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
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
            splitContainer2.Panel2.Controls.Add(this.attachGB);
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
            splitContainer3.Panel1.Controls.Add(this.trustedGB);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(this.extGB);
            splitContainer3.Size = new System.Drawing.Size(502, 408);
            splitContainer3.SplitterDistance = 190;
            splitContainer3.TabIndex = 0;
            splitContainer3.TabStop = false;
            // 
            // trustedGB
            // 
            this.trustedGB.Cursor = System.Windows.Forms.Cursors.Default;
            this.trustedGB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trustedGB.Location = new System.Drawing.Point(0, 0);
            this.trustedGB.Name = "trustedGB";
            this.trustedGB.Size = new System.Drawing.Size(502, 190);
            this.trustedGB.TabIndex = 0;
            this.trustedGB.TabStop = false;
            this.trustedGB.Text = " 信頼済みの送信先";
            this.trustedGB.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // extGB
            // 
            this.extGB.Cursor = System.Windows.Forms.Cursors.Default;
            this.extGB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extGB.Location = new System.Drawing.Point(0, 0);
            this.extGB.Name = "extGB";
            this.extGB.Size = new System.Drawing.Size(502, 214);
            this.extGB.TabIndex = 1;
            this.extGB.TabStop = false;
            this.extGB.Text = "外部ドメインの送信先";
            this.extGB.Enter += new System.EventHandler(this.groupBox1_Enter_1);
            // 
            // attachGB
            // 
            this.attachGB.Cursor = System.Windows.Forms.Cursors.Default;
            this.attachGB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attachGB.Location = new System.Drawing.Point(0, 0);
            this.attachGB.Name = "attachGB";
            this.attachGB.Size = new System.Drawing.Size(278, 408);
            this.attachGB.TabIndex = 2;
            this.attachGB.TabStop = false;
            this.attachGB.Text = "添付ファイル";
            this.attachGB.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // ButtonOK
            // 
            this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOK.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.Enabled = false;
            this.ButtonOK.Location = new System.Drawing.Point(606, 8);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 35);
            this.ButtonOK.TabIndex = 1;
            this.ButtonOK.Text = "送信";
            this.ButtonOK.UseVisualStyleBackColor = false;
            this.ButtonOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // CheckDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(splitContainer1);
            this.Name = "CheckDialog";
            this.Text = "メールを送信しますか？ - CheckMyMail";
            this.Load += new System.EventHandler(this.ChecDialog_Load);
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
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.GroupBox trustedGB;
        private System.Windows.Forms.GroupBox extGB;
        private System.Windows.Forms.GroupBox attachGB;
        private System.Windows.Forms.Button ButtonOK;
    }
}