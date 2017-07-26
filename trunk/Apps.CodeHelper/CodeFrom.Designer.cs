namespace Apps.CodeHelper
{
    partial class CodeFrom
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_Tables = new System.Windows.Forms.ListBox();
            this.tab_CodeList = new System.Windows.Forms.TabControl();
            this.tp_Controller = new System.Windows.Forms.TabPage();
            this.txt_Controller = new System.Windows.Forms.TextBox();
            this.tp_Index = new System.Windows.Forms.TabPage();
            this.txt_Index = new System.Windows.Forms.TextBox();
            this.tp_Create = new System.Windows.Forms.TabPage();
            this.txt_Create = new System.Windows.Forms.TextBox();
            this.tp_Edit = new System.Windows.Forms.TabPage();
            this.txt_Edit = new System.Windows.Forms.TextBox();
            this.tp_Details = new System.Windows.Forms.TabPage();
            this.txt_Details = new System.Windows.Forms.TextBox();
            this.tp_Common = new System.Windows.Forms.TabPage();
            this.txt_Common = new System.Windows.Forms.TextBox();
            this.txt_SQL = new System.Windows.Forms.TextBox();
            this.btn_EditSQLCon = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_prefix = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_ModelName = new System.Windows.Forms.TextBox();
            this.tab_CodeList.SuspendLayout();
            this.tp_Controller.SuspendLayout();
            this.tp_Index.SuspendLayout();
            this.tp_Create.SuspendLayout();
            this.tp_Edit.SuspendLayout();
            this.tp_Details.SuspendLayout();
            this.tp_Common.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb_Tables
            // 
            this.lb_Tables.FormattingEnabled = true;
            this.lb_Tables.ItemHeight = 15;
            this.lb_Tables.Location = new System.Drawing.Point(16, 55);
            this.lb_Tables.Margin = new System.Windows.Forms.Padding(4);
            this.lb_Tables.Name = "lb_Tables";
            this.lb_Tables.Size = new System.Drawing.Size(344, 544);
            this.lb_Tables.TabIndex = 0;
            this.lb_Tables.SelectedIndexChanged += new System.EventHandler(this.lb_Tables_SelectedIndexChanged);
            // 
            // tab_CodeList
            // 
            this.tab_CodeList.Controls.Add(this.tp_Controller);
            this.tab_CodeList.Controls.Add(this.tp_Index);
            this.tab_CodeList.Controls.Add(this.tp_Create);
            this.tab_CodeList.Controls.Add(this.tp_Edit);
            this.tab_CodeList.Controls.Add(this.tp_Details);
            this.tab_CodeList.Controls.Add(this.tp_Common);
            this.tab_CodeList.Location = new System.Drawing.Point(369, 15);
            this.tab_CodeList.Margin = new System.Windows.Forms.Padding(4);
            this.tab_CodeList.Name = "tab_CodeList";
            this.tab_CodeList.SelectedIndex = 0;
            this.tab_CodeList.Size = new System.Drawing.Size(977, 589);
            this.tab_CodeList.TabIndex = 1;
            // 
            // tp_Controller
            // 
            this.tp_Controller.Controls.Add(this.txt_Controller);
            this.tp_Controller.Location = new System.Drawing.Point(4, 25);
            this.tp_Controller.Margin = new System.Windows.Forms.Padding(4);
            this.tp_Controller.Name = "tp_Controller";
            this.tp_Controller.Padding = new System.Windows.Forms.Padding(4);
            this.tp_Controller.Size = new System.Drawing.Size(969, 560);
            this.tp_Controller.TabIndex = 5;
            this.tp_Controller.Text = "Controller";
            this.tp_Controller.UseVisualStyleBackColor = true;
            // 
            // txt_Controller
            // 
            this.txt_Controller.Location = new System.Drawing.Point(0, 1);
            this.txt_Controller.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Controller.Multiline = true;
            this.txt_Controller.Name = "txt_Controller";
            this.txt_Controller.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Controller.Size = new System.Drawing.Size(965, 553);
            this.txt_Controller.TabIndex = 4;
            this.txt_Controller.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.anyTextBox_KeyPress);
            // 
            // tp_Index
            // 
            this.tp_Index.Controls.Add(this.txt_Index);
            this.tp_Index.Location = new System.Drawing.Point(4, 25);
            this.tp_Index.Margin = new System.Windows.Forms.Padding(4);
            this.tp_Index.Name = "tp_Index";
            this.tp_Index.Padding = new System.Windows.Forms.Padding(4);
            this.tp_Index.Size = new System.Drawing.Size(969, 560);
            this.tp_Index.TabIndex = 6;
            this.tp_Index.Text = "Index";
            this.tp_Index.UseVisualStyleBackColor = true;
            // 
            // txt_Index
            // 
            this.txt_Index.Location = new System.Drawing.Point(0, 1);
            this.txt_Index.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Index.Multiline = true;
            this.txt_Index.Name = "txt_Index";
            this.txt_Index.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Index.Size = new System.Drawing.Size(965, 553);
            this.txt_Index.TabIndex = 4;
            this.txt_Index.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.anyTextBox_KeyPress);
            // 
            // tp_Create
            // 
            this.tp_Create.Controls.Add(this.txt_Create);
            this.tp_Create.Location = new System.Drawing.Point(4, 25);
            this.tp_Create.Margin = new System.Windows.Forms.Padding(4);
            this.tp_Create.Name = "tp_Create";
            this.tp_Create.Padding = new System.Windows.Forms.Padding(4);
            this.tp_Create.Size = new System.Drawing.Size(969, 560);
            this.tp_Create.TabIndex = 7;
            this.tp_Create.Text = "Create";
            this.tp_Create.UseVisualStyleBackColor = true;
            // 
            // txt_Create
            // 
            this.txt_Create.Location = new System.Drawing.Point(0, 1);
            this.txt_Create.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Create.Multiline = true;
            this.txt_Create.Name = "txt_Create";
            this.txt_Create.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Create.Size = new System.Drawing.Size(965, 553);
            this.txt_Create.TabIndex = 3;
            this.txt_Create.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.anyTextBox_KeyPress);
            // 
            // tp_Edit
            // 
            this.tp_Edit.Controls.Add(this.txt_Edit);
            this.tp_Edit.Location = new System.Drawing.Point(4, 25);
            this.tp_Edit.Margin = new System.Windows.Forms.Padding(4);
            this.tp_Edit.Name = "tp_Edit";
            this.tp_Edit.Padding = new System.Windows.Forms.Padding(4);
            this.tp_Edit.Size = new System.Drawing.Size(969, 560);
            this.tp_Edit.TabIndex = 8;
            this.tp_Edit.Text = "Edit";
            this.tp_Edit.UseVisualStyleBackColor = true;
            // 
            // txt_Edit
            // 
            this.txt_Edit.Location = new System.Drawing.Point(0, 1);
            this.txt_Edit.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Edit.Multiline = true;
            this.txt_Edit.Name = "txt_Edit";
            this.txt_Edit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Edit.Size = new System.Drawing.Size(965, 553);
            this.txt_Edit.TabIndex = 4;
            this.txt_Edit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.anyTextBox_KeyPress);
            // 
            // tp_Details
            // 
            this.tp_Details.Controls.Add(this.txt_Details);
            this.tp_Details.Location = new System.Drawing.Point(4, 25);
            this.tp_Details.Margin = new System.Windows.Forms.Padding(4);
            this.tp_Details.Name = "tp_Details";
            this.tp_Details.Padding = new System.Windows.Forms.Padding(4);
            this.tp_Details.Size = new System.Drawing.Size(969, 560);
            this.tp_Details.TabIndex = 9;
            this.tp_Details.Text = "Details";
            this.tp_Details.UseVisualStyleBackColor = true;
            // 
            // txt_Details
            // 
            this.txt_Details.Location = new System.Drawing.Point(0, 1);
            this.txt_Details.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Details.Multiline = true;
            this.txt_Details.Name = "txt_Details";
            this.txt_Details.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Details.Size = new System.Drawing.Size(965, 553);
            this.txt_Details.TabIndex = 4;
            this.txt_Details.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.anyTextBox_KeyPress);
            // 
            // tp_Common
            // 
            this.tp_Common.Controls.Add(this.txt_Common);
            this.tp_Common.Location = new System.Drawing.Point(4, 25);
            this.tp_Common.Margin = new System.Windows.Forms.Padding(4);
            this.tp_Common.Name = "tp_Common";
            this.tp_Common.Padding = new System.Windows.Forms.Padding(4);
            this.tp_Common.Size = new System.Drawing.Size(969, 560);
            this.tp_Common.TabIndex = 10;
            this.tp_Common.Text = "Common";
            this.tp_Common.UseVisualStyleBackColor = true;
            // 
            // txt_Common
            // 
            this.txt_Common.Location = new System.Drawing.Point(0, 1);
            this.txt_Common.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Common.Multiline = true;
            this.txt_Common.Name = "txt_Common";
            this.txt_Common.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Common.Size = new System.Drawing.Size(965, 553);
            this.txt_Common.TabIndex = 6;
            this.txt_Common.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.anyTextBox_KeyPress);
            // 
            // txt_SQL
            // 
            this.txt_SQL.Location = new System.Drawing.Point(143, 609);
            this.txt_SQL.Margin = new System.Windows.Forms.Padding(4);
            this.txt_SQL.Name = "txt_SQL";
            this.txt_SQL.Size = new System.Drawing.Size(1091, 25);
            this.txt_SQL.TabIndex = 4;
            // 
            // btn_EditSQLCon
            // 
            this.btn_EditSQLCon.Location = new System.Drawing.Point(1243, 606);
            this.btn_EditSQLCon.Margin = new System.Windows.Forms.Padding(4);
            this.btn_EditSQLCon.Name = "btn_EditSQLCon";
            this.btn_EditSQLCon.Size = new System.Drawing.Size(100, 29);
            this.btn_EditSQLCon.TabIndex = 6;
            this.btn_EditSQLCon.Text = "确定";
            this.btn_EditSQLCon.UseVisualStyleBackColor = true;
            this.btn_EditSQLCon.Click += new System.EventHandler(this.btn_EditSQLCon_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 616);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "SQL Connection：";
            // 
            // txt_prefix
            // 
            this.txt_prefix.Location = new System.Drawing.Point(143, 642);
            this.txt_prefix.Margin = new System.Windows.Forms.Padding(4);
            this.txt_prefix.Name = "txt_prefix";
            this.txt_prefix.Size = new System.Drawing.Size(132, 25);
            this.txt_prefix.TabIndex = 8;
            this.txt_prefix.Text = "Apps";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 646);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "命名空间：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(307, 646);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "模块名称：";
            // 
            // txt_ModelName
            // 
            this.txt_ModelName.Location = new System.Drawing.Point(385, 642);
            this.txt_ModelName.Margin = new System.Windows.Forms.Padding(4);
            this.txt_ModelName.Name = "txt_ModelName";
            this.txt_ModelName.Size = new System.Drawing.Size(132, 25);
            this.txt_ModelName.TabIndex = 11;
            this.txt_ModelName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_ModelName_KeyUp);
            // 
            // CodeFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1351, 680);
            this.Controls.Add(this.txt_ModelName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_prefix);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_EditSQLCon);
            this.Controls.Add(this.txt_SQL);
            this.Controls.Add(this.tab_CodeList);
            this.Controls.Add(this.lb_Tables);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CodeFrom";
            this.Text = "MVC模版生成byYmNets";
            this.Load += new System.EventHandler(this.CodeFrom_Load);
            this.tab_CodeList.ResumeLayout(false);
            this.tp_Controller.ResumeLayout(false);
            this.tp_Controller.PerformLayout();
            this.tp_Index.ResumeLayout(false);
            this.tp_Index.PerformLayout();
            this.tp_Create.ResumeLayout(false);
            this.tp_Create.PerformLayout();
            this.tp_Edit.ResumeLayout(false);
            this.tp_Edit.PerformLayout();
            this.tp_Details.ResumeLayout(false);
            this.tp_Details.PerformLayout();
            this.tp_Common.ResumeLayout(false);
            this.tp_Common.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lb_Tables;
        private System.Windows.Forms.TabControl tab_CodeList;
        private System.Windows.Forms.TabPage tp_Controller;
        private System.Windows.Forms.TabPage tp_Index;
        private System.Windows.Forms.TabPage tp_Create;
        private System.Windows.Forms.TabPage tp_Edit;
        private System.Windows.Forms.TabPage tp_Details;
        private System.Windows.Forms.TextBox txt_SQL;
        private System.Windows.Forms.Button btn_EditSQLCon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_prefix;
        private System.Windows.Forms.TextBox txt_Create;
        private System.Windows.Forms.TextBox txt_Controller;
        private System.Windows.Forms.TextBox txt_Index;
        private System.Windows.Forms.TextBox txt_Edit;
        private System.Windows.Forms.TextBox txt_Details;
        private System.Windows.Forms.TabPage tp_Common;
        private System.Windows.Forms.TextBox txt_Common;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_ModelName;
    }
}

