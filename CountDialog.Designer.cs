namespace CheckMyMail
{
    partial class CountDialog
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Button btnCancel;
            this.btnOK = new System.Windows.Forms.Button();
            this.labelCount = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label2.Location = new System.Drawing.Point(89, 81);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(230, 31);
            label2.TabIndex = 2;
            label2.Text = "秒後に送信します...";
            // 
            // btnCancel
            // 
            btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            btnCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancel.Location = new System.Drawing.Point(249, 146);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 22);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "キャンセル";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(147, 146);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(86, 22);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "いますぐ送信";
            this.btnOK.UseVisualStyleBackColor = false;
            // 
            // labelCount
            // 
            this.labelCount.AutoSize = true;
            this.labelCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCount.ForeColor = System.Drawing.Color.Red;
            this.labelCount.Location = new System.Drawing.Point(12, 33);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(83, 91);
            this.labelCount.TabIndex = 1;
            this.labelCount.Text = "5";
            // 
            // CountDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 180);
            this.Controls.Add(btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(label2);
            this.Controls.Add(this.labelCount);
            this.Name = "CountDialog";
            this.Text = "送信カウントダウン - CheckMyMail";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.CountDialog_Closing);
            this.Shown += new System.EventHandler(this.CountDialog_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Button btnOK;
    }
}