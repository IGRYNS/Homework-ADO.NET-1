using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homework_ADO.NET_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }

        private void SettingsDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings();
            DialogResult dialogResult = formSettings.ShowDialog();
            if (dialogResult==DialogResult.OK)
            {
                try
                {
                    if (dbConfig.GenerateStringConnection(true))
                    {
                        statusLbl.Text = "Connecting...";
                        using (SqlConnection sqlConnection = new SqlConnection(dbConfig.ConnectionStr))
                        {
                            sqlConnection.Open();
                            statusLbl.Text = "Connected";
                            string command = $"SELECT Name FROM master.dbo.sysdatabases WHERE name='{dbConfig.DBNameBase}'";
                            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

                            SqlDataReader reader = sqlCommand.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read()) // построчно считываем данные
                                {
                                    object name = reader["name"];
                                    if (name.ToString() == dbConfig.DBNameBase)
                                    {
                                        MessageBox.Show("База найдена");
                                        return;
                                    }
                                    MessageBox.Show("База не найдена");
                                }
                            }
                        }                        
                    }
                    statusLbl.Text = "Disconnect";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"Проверка базы данных.");
                }
            }
        }

        private void FirstDBdataToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
