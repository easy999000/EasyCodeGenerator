using EasyCodeGeneratorForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class EasyCodeGenerator : Form
    {
        /// <summary>
        /// 生成控制器
        /// </summary>
        ClientControl BuildControl = new ClientControl();
        /// <summary>
        /// 显示模板
        /// </summary>
        void ShowTemplateList()
        {
            BuildControl.GetAllTemplate();
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = BuildControl.TemplateShowList;
        }
        /// <summary>
        /// 显示数据库列表
        /// </summary>
        void ShowDBList()
        {
            this.comboBox1.DataSource = null;
            this.comboBox1.DisplayMember = "DBName";
            BuildControl.GetAllDB();
            this.comboBox1.DataSource = BuildControl.DBList;
        }
        /// <summary>
        /// 显示表列表
        /// </summary>
        void ShowTableList()
        {
            string where = this.textBox1.Text;
            BuildControl.SelectTable(where);
            this.dataGridView2.DataSource = null;
            this.dataGridView2.DataSource = BuildControl.CurrentTableList;
        }
        /// <summary>
        /// 选择数据库
        /// </summary>
        void SelectDB()
        {
            BuildControl.DBListSelectChange((DBShow)this.comboBox1.SelectedItem);

            this.textBox1.Text = "";

            ShowTableList();
        }

        public EasyCodeGenerator()
        {
            InitializeComponent();
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Shuaxin();
        }

        private void splitContainer4_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void buttonOnekey_Click(object sender, EventArgs e)
        {
            string s = BuildControl.Build();
            showDir(BuildControl.CodeBuild.CodePath);
            MessageBox.Show(s);


        }

        private void buttonBuxuan_Click(object sender, EventArgs e)
        {
            foreach (var item in BuildControl.CurrentTableList)
            {
                item.Check = false;
            }
            this.dataGridView2.Refresh();
        }

        private void buttonQuanxuan_Click(object sender, EventArgs e)
        {

            foreach (var item in BuildControl.CurrentTableList)
            {
                item.Check = true;
            }
            this.dataGridView2.Refresh();
        }

        void showDir(string path)
        { 
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildControl.DBListSelectChange((DBShow)this.comboBox1.SelectedItem);
            this.textBox1.Text = "";
            ShowTableList();


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ShowTableList();

        }

        public class showView
        {
            public bool Check { get; set; }

            public string File { get; set; }
            public string TableName { get; set; }
            public string FileNamespace { get; set; }
            public string DBName { get; set; }

        }

        List<showView> l = new List<showView>();
        List<showView> l2 = new List<showView>();
        List<showView> l3 = new List<showView>();
        private void button1_Click_1(object sender, EventArgs e)
        {

            this.dataGridView1.DataSource = null;

            for (int i = 0; i < 4; i++)
            {
                showView s = new showView();
                s.Check = i % 3 == 1 ? true : false;
                s.File = "name1" + i;
                s.FileNamespace = "fileNamespace" + i;
                l.Add(s);

            }

            this.dataGridView1.DataSource = l;


            for (int i = 0; i < 4; i++)
            {
                showView s = new showView();
                s.Check = i % 3 == 1 ? true : false;
                s.File = "name1" + i;
                s.FileNamespace = "fileNamespace" + i;
                s.DBName = "DBName" + i;
                l2.Add(s);

            }
            this.comboBox1.DataSource = l2;

            for (int i = 0; i < 4; i++)
            {
                showView s = new showView();
                s.Check = i % 3 == 1 ? true : false;
                s.File = "name1" + i;
                s.TableName = "fileNamespace" + i;
                l3.Add(s);

            }
            this.dataGridView2.DataSource = l3;


        }

        private void EasyCodeGenerator_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView2.AutoGenerateColumns = false;
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var item in l)
            {
                item.Check = true;
            }
            foreach (var item in l3)
            {
                item.Check = true;
            }
            dataGridView1.Refresh();
        }

        private void EasyCodeGenerator_Shown(object sender, EventArgs e)
        {
            Shuaxin();
        }

        void Shuaxin()
        {

            ShowTemplateList();
            ShowDBList();
            SelectDB();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void splitContainer2_SplitterMoved_1(object sender, SplitterEventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
