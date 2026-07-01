using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form3 : Form
    {
        private string Username;
        private Form1 form1;
        MongoUserService userService = new MongoUserService();
        public Form3(string username,Form1 form)
        {
            InitializeComponent();
            Username = username;
            form1=form;
        }
        private async void Form3_Load(object sender, EventArgs e)
        {//呼叫載入資料的方法
            await LoadDataAsync();
        }
        private async Task LoadDataAsync()
        {   //利用非同步尋找使用者帳號的所有電影票
            var user = await userService.FindMovieTiclet( Username);
            //將資料顯示在datagridview的格式
            var list = user.Select(u => new
            {
                id=u.Id,
                訂票人 = u.Username,
                電影票 = u.Title,
                時間=u.Time,
                位置 = ConvertSeat(u.Seat) // 將數字座位轉換為 A1 格式
            }).ToList();
            dataGridView1.DataSource = list;
            dataGridView1.Columns[0].Visible = false;
        }
        // 座位轉換方法
        private string ConvertSeat(int seatIndex, int seatsPerRow = 5)
        {
            int row = seatIndex / seatsPerRow;
            int column = seatIndex % seatsPerRow;
            char rowChar = (char)('A' + row);
            return $"{rowChar}{column + 1}";
        }
        
        private void button1_Click(object sender, EventArgs e)
        {//回到首頁
            form1.Show();
            Close();
        }
        //刪除按鈕
        private async void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1 != null)
            {//獲取使用者選擇的欄位的objectid
                var idvalue = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                //轉換成ObjectId形式
                var objectid = ObjectId.Parse(idvalue);            
                //刪除那個ObjectId的Bson
                var result = await userService.DeleteByIdAsync(objectid);
                if (result)
                {
                    MessageBox.Show("刪除成功");
                    await LoadDataAsync(); // 重新載入資料
                }
                else
                {
                    MessageBox.Show("刪除失敗");
                }
            }
        }
        
        private void button3_Click(object sender, EventArgs e)
        {//結束訂票
            Application.Exit();
        }
    }
}
