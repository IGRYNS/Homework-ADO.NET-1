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
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            settingsTypeDB.SelectedItem = dbConfig.DBType;
            settingsAdressServer.Text = dbConfig.DBAddress;
            settingsPortServer.Text = dbConfig.DBPort;
            settingsNameDatabase.Text = dbConfig.DBNameBase;
            if (dbConfig.DBAuth==true)
            {
                radioButton1.Checked = true;
            }
            else if (dbConfig.DBAuth==false)
            {
                radioButton2.Checked = true;
            }
            textBox1.Text = dbConfig.DBUser;
            textBox2.Text = dbConfig.DBPassword;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
        }

        private void settingsTypeDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (settingsTypeDB.SelectedItem.ToString() == "MS SQL")
            {
                settingsAdressServer.Text = "localhost";
                settingsPortServer.Text = "1433";
            }
            else if (settingsTypeDB.SelectedItem.ToString() == "MS SQL Express")
            {
                settingsAdressServer.Text = "localhost";
                settingsPortServer.Text = "1433";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dbConfig.DBType = settingsTypeDB.Text;
            dbConfig.DBAddress = settingsAdressServer.Text;
            dbConfig.DBPort = settingsPortServer.Text;
            dbConfig.DBNameBase = settingsNameDatabase.Text;
            if (radioButton1.Checked)
            {
                dbConfig.DBAuth = false;
                dbConfig.DBUser = "";
                dbConfig.DBPassword = "";
            }
            else if (radioButton2.Checked)
            {
                dbConfig.DBAuth = true;
                dbConfig.DBUser = textBox1.Text;
                dbConfig.DBPassword = textBox2.Text;
            }
        }

        private void settingsPortServer_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
