namespace SalaryManagement
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.菜单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.员工基本信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.员工工资导入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plMain = new System.Windows.Forms.Panel();
            this.应发扣发项维护ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.员工工资打印ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.员工工资删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.菜单ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1016, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 菜单ToolStripMenuItem
            // 
            this.菜单ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.员工基本信息ToolStripMenuItem,
            this.员工工资导入ToolStripMenuItem,
            this.应发扣发项维护ToolStripMenuItem,
            this.员工工资打印ToolStripMenuItem,
            this.员工工资删除ToolStripMenuItem});
            this.菜单ToolStripMenuItem.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.菜单ToolStripMenuItem.Name = "菜单ToolStripMenuItem";
            this.菜单ToolStripMenuItem.Size = new System.Drawing.Size(61, 24);
            this.菜单ToolStripMenuItem.Text = "菜单";
            // 
            // 员工基本信息ToolStripMenuItem
            // 
            this.员工基本信息ToolStripMenuItem.Name = "员工基本信息ToolStripMenuItem";
            this.员工基本信息ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.员工基本信息ToolStripMenuItem.Text = "员工基本信息";
            this.员工基本信息ToolStripMenuItem.Click += new System.EventHandler(this.员工基本信息ToolStripMenuItem_Click);
            // 
            // 员工工资导入ToolStripMenuItem
            // 
            this.员工工资导入ToolStripMenuItem.Name = "员工工资导入ToolStripMenuItem";
            this.员工工资导入ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.员工工资导入ToolStripMenuItem.Text = "员工工资导入";
            this.员工工资导入ToolStripMenuItem.Click += new System.EventHandler(this.员工工资导入ToolStripMenuItem_Click);
            // 
            // plMain
            // 
            this.plMain.Location = new System.Drawing.Point(0, 31);
            this.plMain.Name = "plMain";
            this.plMain.Size = new System.Drawing.Size(1016, 707);
            this.plMain.TabIndex = 1;
            // 
            // 应发扣发项维护ToolStripMenuItem
            // 
            this.应发扣发项维护ToolStripMenuItem.Name = "应发扣发项维护ToolStripMenuItem";
            this.应发扣发项维护ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.应发扣发项维护ToolStripMenuItem.Text = "应发/扣发项";
            this.应发扣发项维护ToolStripMenuItem.Click += new System.EventHandler(this.应发扣发项维护ToolStripMenuItem_Click);
            // 
            // 员工工资打印ToolStripMenuItem
            // 
            this.员工工资打印ToolStripMenuItem.Name = "员工工资打印ToolStripMenuItem";
            this.员工工资打印ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.员工工资打印ToolStripMenuItem.Text = "员工工资打印";
            this.员工工资打印ToolStripMenuItem.Click += new System.EventHandler(this.员工工资打印ToolStripMenuItem_Click);
            // 
            // 员工工资删除ToolStripMenuItem
            // 
            this.员工工资删除ToolStripMenuItem.Name = "员工工资删除ToolStripMenuItem";
            this.员工工资删除ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.员工工资删除ToolStripMenuItem.Text = "员工工资删除";
            this.员工工资删除ToolStripMenuItem.Click += new System.EventHandler(this.员工工资删除ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 736);
            this.Controls.Add(this.plMain);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "薪资管理系统";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 菜单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 员工基本信息ToolStripMenuItem;
        private System.Windows.Forms.Panel plMain;
        private System.Windows.Forms.ToolStripMenuItem 员工工资导入ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 应发扣发项维护ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 员工工资打印ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 员工工资删除ToolStripMenuItem;
    }
}

