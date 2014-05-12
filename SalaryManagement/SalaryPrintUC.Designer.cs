namespace SalaryManagement
{
    partial class SalaryPrintUC
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
            this.tbId = new System.Windows.Forms.TextBox();
            this.lblId = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.lblYM = new System.Windows.Forms.Label();
            this.tbYM = new System.Windows.Forms.TextBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btPrint = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbId
            // 
            this.tbId.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbId.Location = new System.Drawing.Point(149, 62);
            this.tbId.Name = "tbId";
            this.tbId.Size = new System.Drawing.Size(203, 30);
            this.tbId.TabIndex = 7;
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblId.Location = new System.Drawing.Point(54, 65);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(89, 20);
            this.lblId.TabIndex = 6;
            this.lblId.Text = "身份证号";
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblNote.Location = new System.Drawing.Point(577, 68);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(69, 20);
            this.lblNote.TabIndex = 26;
            this.lblNote.Text = "YYYYMM";
            // 
            // lblYM
            // 
            this.lblYM.AutoSize = true;
            this.lblYM.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblYM.Location = new System.Drawing.Point(399, 68);
            this.lblYM.Name = "lblYM";
            this.lblYM.Size = new System.Drawing.Size(49, 20);
            this.lblYM.TabIndex = 25;
            this.lblYM.Text = "月份";
            // 
            // tbYM
            // 
            this.tbYM.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbYM.Location = new System.Drawing.Point(454, 65);
            this.tbYM.Name = "tbYM";
            this.tbYM.Size = new System.Drawing.Size(117, 30);
            this.tbYM.TabIndex = 24;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(54, 13);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 20);
            this.lblMsg.TabIndex = 27;
            // 
            // btPrint
            // 
            this.btPrint.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btPrint.Location = new System.Drawing.Point(679, 62);
            this.btPrint.Name = "btPrint";
            this.btPrint.Size = new System.Drawing.Size(109, 33);
            this.btPrint.TabIndex = 28;
            this.btPrint.Text = "打印";
            this.btPrint.UseVisualStyleBackColor = true;
            this.btPrint.Click += new System.EventHandler(this.btPrint_Click);
            // 
            // SalaryPrintUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btPrint);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.lblYM);
            this.Controls.Add(this.tbYM);
            this.Controls.Add(this.tbId);
            this.Controls.Add(this.lblId);
            this.Name = "SalaryPrintUC";
            this.Size = new System.Drawing.Size(1000, 700);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbId;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Label lblYM;
        private System.Windows.Forms.TextBox tbYM;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button btPrint;
    }
}
