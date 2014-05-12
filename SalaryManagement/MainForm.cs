using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SalaryManagement
{
    public partial class MainForm : Form
    {
        private StaffUC staffUC = new StaffUC();
        private SalaryUC salaryUC = new SalaryUC();
        private SalarySettingUC salarySettingUC = new SalarySettingUC();
        private SalaryPrintUC salaryPrintUC = new SalaryPrintUC();
        private SalaryDeleteUC salaryDeleteUC = new SalaryDeleteUC();

        public MainForm()
        {
            InitializeComponent();
        }

        private void 员工基本信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveOldControls();
            this.plMain.Controls.Add(staffUC);
            staffUC.Size = new System.Drawing.Size(this.Width, this.Height);
            staffUC.Focus();
        }

        private void 员工工资导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveOldControls();
            this.plMain.Controls.Add(salaryUC);
            salaryUC.Size = new System.Drawing.Size(this.Width, this.Height);
            salaryUC.Focus();
        }

        private void RemoveOldControls()
        {
            while (this.plMain.Controls.Count > 0)
            {
                this.plMain.Controls.RemoveAt(0);
            }
        }

        private void 应发扣发项维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveOldControls();
            this.plMain.Controls.Add(salarySettingUC);
            salarySettingUC.Size = new System.Drawing.Size(this.Width, this.Height);
            salarySettingUC.Focus();
        }

        private void 员工工资打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveOldControls();
            this.plMain.Controls.Add(salaryPrintUC);
            salaryPrintUC.Size = new System.Drawing.Size(this.Width, this.Height);
            salaryPrintUC.Focus();
        }

        private void 员工工资删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveOldControls();
            this.plMain.Controls.Add(salaryDeleteUC);
            salaryDeleteUC.Size = new System.Drawing.Size(this.Width, this.Height);
            salaryDeleteUC.Focus();
        }
    }
}
