namespace SalaryManagement
{
    partial class SalaryUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btUpload = new System.Windows.Forms.Button();
            this.lblYM = new System.Windows.Forms.Label();
            this.tbYM = new System.Windows.Forms.TextBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btUpload
            // 
            this.btUpload.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btUpload.Location = new System.Drawing.Point(319, 56);
            this.btUpload.Name = "btUpload";
            this.btUpload.Size = new System.Drawing.Size(109, 33);
            this.btUpload.TabIndex = 21;
            this.btUpload.Text = "导入";
            this.btUpload.UseVisualStyleBackColor = true;
            this.btUpload.Click += new System.EventHandler(this.btUpload_Click);
            // 
            // lblYM
            // 
            this.lblYM.AutoSize = true;
            this.lblYM.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblYM.Location = new System.Drawing.Point(57, 62);
            this.lblYM.Name = "lblYM";
            this.lblYM.Size = new System.Drawing.Size(49, 20);
            this.lblYM.TabIndex = 16;
            this.lblYM.Text = "月份";
            // 
            // tbYM
            // 
            this.tbYM.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbYM.Location = new System.Drawing.Point(112, 59);
            this.tbYM.Name = "tbYM";
            this.tbYM.Size = new System.Drawing.Size(117, 30);
            this.tbYM.TabIndex = 15;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(26, 14);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 20);
            this.lblMsg.TabIndex = 22;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblNote.Location = new System.Drawing.Point(235, 62);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(69, 20);
            this.lblNote.TabIndex = 23;
            this.lblNote.Text = "YYYYMM";
            // 
            // SalaryUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.btUpload);
            this.Controls.Add(this.lblYM);
            this.Controls.Add(this.tbYM);
            this.Name = "SalaryUC";
            this.Size = new System.Drawing.Size(1000, 700);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btUpload;
        private System.Windows.Forms.Label lblYM;
        private System.Windows.Forms.TextBox tbYM;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Label lblNote;
    }
}
