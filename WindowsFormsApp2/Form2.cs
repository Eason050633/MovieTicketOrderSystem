using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        private string choose;//電影名稱
        private string username;//帳戶名
        int[] ticket;//票卷張數
        int no = 0;//目前是選擇第幾張票卷
        DateTime dateTime;//電影時間
        Form1 form1;
        MongoUserService userService = new MongoUserService();
        public Form2(string choose, int ticket, string username, DateTime date, Form1 fo)
        {
            InitializeComponent();
            this.choose = choose;
            this.username = username;
            this.ticket = new int[ticket];
            this.dateTime = date.ToLocalTime();
            form1 = fo;
            label1.Text = $"請選擇{ticket}個位置";
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            //將tableLayoutPanel加上按鈕當作座位
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Button seat = new Button();
                    //座位名稱為(A1~A5)~(E1~E5)
                    seat.Text = $"{(char)('A' + i)}{j + 1}";
                    //如果有在Movie資料庫找到同時符合電影名稱、時間的座位
                    if (userService.FindMovieSeat(choose, j*10 + i,dateTime))
                    {//就將該座位變成藍色且不可按
                        seat.Enabled = false;
                        seat.BackColor = Color.LightBlue;
                    }
                    else
                    {//否則為綠色
                        seat.BackColor = Color.LightGreen;
                    }
                    seat.Click += new EventHandler(click);
                    tableLayoutPanel1.Controls.Add(seat, j, i);
                }
            }
        }
        private void click(object sender, EventArgs e)
        {//座位按鈕的觸發事件
            Button button = (Button)sender;
            int index = tableLayoutPanel1.Controls.IndexOf(button);
            //當使用者點擊綠色的按鈕時，將該按鈕的位置存到ticket陣列，並加按鈕顏色改為紅色
            if (button.BackColor == Color.LightGreen)
                {
                if (no < ticket.Length)
                {
                    ticket[no] = index;
                    button.BackColor = Color.Red;
                    //再將現在還需要選擇幾個位置的數字減一
                    no++;                
                }
                else
                {
                    MessageBox.Show("已選購足夠張數的票卷");
                }
            }//當使用者點擊紅色按鈕時，將現在需要選擇幾個位置的數字加一，並將按鈕變成綠色
            else if (button.BackColor == Color.Red)
            {
                no--;
                button.BackColor = Color.LightGreen;
            }
            
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //當使用者選擇足夠的位置時
            //將剛剛儲存的位置資訊一一儲存成不同的Bson檔並寫進資料庫
            if (no == ticket.Length) {                 
                for (int i = 0; i < ticket.Length; i++) { 
                    Movie shrek = new Movie(); 
                    shrek.Seat = ticket[i];
                    shrek.Username = username;
                    shrek.Time=dateTime;
                    shrek.Title = choose;
                    userService.InsertMovie(shrek);
                }
                MessageBox.Show("訂票成功");
                //打開我的票卷頁面
                Form3 form = new Form3(username,form1);
                form.Show();
                Close();
            }
            else
            {
                MessageBox.Show("請選擇足夠的票數");
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {//回上一頁
            form1.Show();
            Close();
        }
    }
}
