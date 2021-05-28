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
            dbConfig.TablesBase.Add("Sellers");
            dbConfig.TablesBase.Add("Byers");
            dbConfig.TablesBase.Add("Sales");
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
                                /*while (reader.Read()) // построчно считываем данные
                                {
                                    object name = reader["name"];
                                    if (name.ToString() == dbConfig.DBNameBase)
                                    {
                                        MessageBox.Show("База найдена");
                                        return;
                                    }
                                }*/

                                //SELECT* FROM sys.tables WHERE name = 'YourTable' AND type = 'U'

                                statusLbl.Text = "База найдена. Проверка наличия таблиц...";
                                dbConfig.GenerateStringConnection();
                                using (SqlConnection sqlConnectionTestTable = new SqlConnection(dbConfig.ConnectionStr))
                                {
                                    sqlConnectionTestTable.Open();
                                    command = $"SELECT Count(Name) FROM sys.tables where Name = 'Sellers' or name ='Byers' or name ='Sales'";
                                    sqlCommand = new SqlCommand(command, sqlConnectionTestTable);
                                    object count = sqlCommand.ExecuteScalar();
                                    if ((int)count == 3)
                                    {
                                        statusLbl.Text = "Таблицы таблицы. Проверка структуры таблиц.";
                                        if (dbConfig.CheckOrCreateTables(true) == true)
                                        {
                                            statusLbl.Text = "Таблицы прошли проверку.";
                                        }
                                        else
                                        {
                                            /*DialogResult Rezult =  MessageBox.Show("Таблицы не прошли проверку. Пересоздать базу данных?", "Ошибка проверки таблиц.", MessageBoxButtons.OKCancel);
                                            if (Rezult == DialogResult.OK)
                                            {
                                                dbConfig.GenerateStringConnection(true);

                                                using (SqlConnection sqlConnection2 = new SqlConnection(dbConfig.ConnectionStr))
                                                {
                                                    sqlConnection2.Open();
                                                    statusLbl.Text = "Пересоздание базы данных";
                                                    command = "DROP DATABASE " + $"{dbConfig.DBNameBase};";                                                     
                                                    sqlCommand = new SqlCommand(command, sqlConnection2);
                                                    sqlCommand.ExecuteNonQuery();

                                                    command = "CREATE DATABASE " + $"{dbConfig.DBNameBase};";
                                                    sqlCommand = new SqlCommand(command, sqlConnection2);
                                                    sqlCommand.ExecuteNonQuery();

                                                    dbConfig.GenerateStringConnection();
                                                    if (dbConfig.CheckOrCreateTables(false))
                                                    {
                                                        statusLbl.Text = "База создана.";
                                                    }
                                                }
                                            }*/
                                            MessageBox.Show("Таблицы не прошли проверку. Выбрете другое имя базы данных.");
                                            statusLbl.Text = "Таблицы не прошли проверку.";
                                        }
                                    }
                                    else
                                    {
                                        statusLbl.Text = "Требуемые таблицы не найдены.";
                                        DialogResult rez =  MessageBox.Show("Требуемые таблицы не найдены. Создать таблицы?", "Ошибка проверки существования таблиц.", MessageBoxButtons.OKCancel);
                                        if (rez==DialogResult.OK)
                                        {
                                            if (dbConfig.CheckOrCreateTables(false) == true)
                                            {
                                                statusLbl.Text = "База готова к работе";
                                            }     
                                        }

                                    }
                                }
                                //command = $"USE '{dbConfig.DBNameBase}';SELECT * FROM sys.tables";


                            }
                            else
                            {
                                DialogResult rezult =  MessageBox.Show("Создать базу данных?","База не найдена.", MessageBoxButtons.OKCancel);
                                if (rezult == DialogResult.OK)
                                {
                                    dbConfig.GenerateStringConnection(true);
                                    statusLbl.Text = "Connecting...";
                                    using (SqlConnection sqlConnection2 = new SqlConnection(dbConfig.ConnectionStr))
                                    {
                                        sqlConnection2.Open();
                                        statusLbl.Text = "Connected";
                                        string temp = "CREATE DATABASE " + $"{dbConfig.DBNameBase};";
                                        command = temp;
                                        sqlCommand = new SqlCommand(command, sqlConnection2);
                                        sqlCommand.ExecuteNonQuery();
                                    }
                                    statusLbl.Text="База данных создана. Создание таблиц.";
                                    dbConfig.GenerateStringConnection();
                                    if (dbConfig.CheckOrCreateTables(false))
                                    {
                                        statusLbl.Text = "Таблицы созданы";
                                    }
                                    

                                }
                            }
                        }   
                        
                    }
                    //statusLbl.Text = "Disconnect";
                    dbConfig.GenerateStringConnection();
                    lbSalles.Items.Clear();
                    List<string> list = dbConfig.GetSales();
                    if (list != null)
                    {
                        foreach (string item in list)
                        {
                            lbSalles.Items.Add(item);
                        }
                    }



                    cbByer.Items.Clear();
                    cbSeller.Items.Clear();
                    //Byers
                    List<string> Byers = dbConfig.GetFIO($"{dbConfig.TablesBase[1]}");

                    //Sellers
                    List<string> Sellers = dbConfig.GetFIO($"{dbConfig.TablesBase[0]}");

                    foreach (string item in Byers)
                    {
                        cbByer.Items.Add(item);
                    }
                    foreach (string item in Sellers)
                    {
                        cbSeller.Items.Add(item);
                    }
                    button1.Enabled = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"Проверка базы данных.");
                    statusLbl.Text = "Disconnect";
                }
            }
        }

        private void FirstDBdataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLbl.Text = "Заполнение начальных данных.";
            if (dbConfig.FirstAddData() == true)
            {
                statusLbl.Text = "Начальные данные внесены.";
            }
            statusLbl.Text = "Ошибка заполнения начальными данными. Рекомендуется пересоздать БД";
        }

        private void RegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            registration.ShowDialog();
            cbByer.Items.Clear();
            cbSeller.Items.Clear();
            //Byers
            List<string> Byers = dbConfig.GetFIO($"{dbConfig.TablesBase[1]}");

            //Sellers
            List<string> Sellers = dbConfig.GetFIO($"{dbConfig.TablesBase[0]}");

            foreach (string item in Byers)
            {
                cbByer.Items.Add(item);
            }
            foreach (string item in Sellers)
            {
                cbSeller.Items.Add(item);
            }
        }

        private void lbSalles_SelectedIndexChanged(object sender, EventArgs e)
        {
            statusLbl.Text = "Получение данных.";
            List<string> list = dbConfig.GetInfo(lbSalles.SelectedItem.ToString());
            if (list.Count!=0)
            {
                lblByer.Text = list[0];
                lblSeller.Text = list[1];
                lblSumm.Text = list[2];
                lblDate.Text = list[3];
            }
            statusLbl.Text = "Данные получены.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            dbConfig.AcceptSale(cbSeller.SelectedItem.ToString(), cbByer.SelectedItem.ToString(), textBox1.Text);

            lbSalles.Items.Clear();
            List<string> list = dbConfig.GetSales();
            if (list != null)
            {
                foreach (string item in list)
                {
                    lbSalles.Items.Add(item);
                }
            }

        }
    }
}
