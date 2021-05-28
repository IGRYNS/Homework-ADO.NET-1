using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homework_ADO.NET_1
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string table="";
            string command="";
            if (cbType.SelectedItem!=null)
            {
                if (cbType.SelectedItem.ToString() == "Покупатель")
                {
                    table = "Byers";
                }
                else if (cbType.SelectedItem.ToString() == "Продавец")
                {
                    table = "Sellers";
                }

                if (string.IsNullOrEmpty(tbFirstName.Text) ==false && string.IsNullOrEmpty(tbLastName.Text) == false)
                {
                    command = $"INSERT INTO {table} (FirstName, LastName) VALUES ('{tbFirstName.Text}', '{tbLastName.Text}');";
                    dbConfig.Insert(command);
                }
                else
                {
                    MessageBox.Show("Не указано имя и\\или фамилия.");
                }

            }
            else
            {
                MessageBox.Show("Не выбран тип.");
            }


            
        }
    }
}
