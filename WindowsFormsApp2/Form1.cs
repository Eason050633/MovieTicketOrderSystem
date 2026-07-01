using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }
        MongoUserService userService= new MongoUserService();
        
        private void registerbutton_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text.Trim();
            //查找要註冊的帳號是否存在
            if (!string.IsNullOrEmpty(str)) { 
                if (userService.FindUsersByUsername(str)>0) {
                    MessageBox.Show("帳號已存在");
                }
                else
                {
                    //若不存在則創建一個User物件
                    User user = new User();
                    user.Username = str;
                    user.Password = textBox2.Text.Trim();
                    //再將物件存入資料庫
                    userService.InsertUser(user);
                    MessageBox.Show("註冊成功");
                    textBox1.Clear();
                    textBox2.Clear();
                }
            }
            else
            {
                MessageBox.Show("欄位不可為空白");
            }
        }
        private void loginbutton_Click(object sender, EventArgs e)
        {
            string str=textBox1.Text.Trim();
            string str2 = textBox2.Text.Trim();
            if (!string.IsNullOrEmpty(str2)) {
                //驗證使用者輸入的帳號密碼是否正確
                var userid = userService.ValidateLogin(str, str2);
                if (userid != null)
                {
                    MessageBox.Show("登錄成功");
                    groupBox1.Enabled = false;//登錄成功關閉帳號選項
                    groupBox2.Enabled = true;//打開電影票選項
                }
                else
                {
                    MessageBox.Show("請檢查帳號密碼是否正確");
                }
            }
            else
            {
                MessageBox.Show("請檢查帳號密碼是否正確");
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void Sendbutton_Click(object sender, EventArgs e)
        {          
            string type="";
            //尋找現在聚焦的是哪一個radiobutton
            foreach(Control control in groupBox2.Controls)
            {
                if(control is RadioButton rb && rb.Checked == true)
                {
                    type = rb.Text;
                    break;
                }
            }
            //如果聚焦的是"我的票卷"則前往Form3並隱藏Form1
            if (type.Equals("我的票卷"))
            {
                Form3 form = new Form3(textBox1.Text,this);
                form.Show();
                Hide();
            }
            //如果聚焦的是其他的選項則將radiobutton的文字傳到form2並打開
            else
            {//建構子Form2(電影名稱,票卷張數,帳戶名稱,電影時間)
                Form2 form= new Form2(type, comboBox1.SelectedIndex + 1, textBox1.Text.Trim(),dateTimePicker1.Value,this);
                form.Show();
                Hide();
            }
        }
       

    }
}
