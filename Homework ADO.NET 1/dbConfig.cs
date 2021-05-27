using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homework_ADO.NET_1
{
    static public class dbConfig
    {
        static public string DBType { get; set; }
        static public string DBAddress { get; set; }
        static public string DBPort { get; set; }
        static public string DBNameBase { get; set; }
        static public bool DBAuth { get; set; }
        static public string DBUser { get; set; }
        static public string DBPassword { get; set; }
        static public string ConnectionStr { get; private set; }


        //true = only check DB name in Server, false = finalybase
        static public bool GenerateStringConnection(bool OnlyCheck = false)
        {
            if (string.IsNullOrWhiteSpace(DBType) == false)
            {
                if (string.IsNullOrWhiteSpace(DBAddress) == false)
                {
                    if (string.IsNullOrWhiteSpace(DBPort) == false)
                    {
                        if (DBType == "MS SQL Express")
                        {
                            if (DBPort != "1433")
                            {
                                ConnectionStr = $"Data Source={DBAddress}\\SQLEXPRESS,{DBPort};";                                
                                
                            }
                            else
                            {
                                ConnectionStr = $"Data Source={DBAddress}\\SQLEXPRESS;";
                            }
                        }

                        else if (DBType == "MS SQL")
                        {

                            if (DBPort != "1433")
                            {
                                ConnectionStr = $@"Data Source={DBAddress},{DBPort};";
                            }
                            else if (true)
                            {
                                ConnectionStr = $@"Data Source={DBAddress};";
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не задан порт БД");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Не задан адрес БД");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Не выбран тип БД");
                return false;
            }


            if (DBAuth == false)
            {
                ConnectionStr += "Integrated security=true;";
            }
            else if (DBAuth == true)
            {
                if (string.IsNullOrWhiteSpace(DBUser) == false)
                {
                    if (string.IsNullOrWhiteSpace(DBPassword) == false)
                    {
                        ConnectionStr += $@"Integrated security=false;User Id = {DBUser};Password={DBPassword}";
                    }
                    else
                    {
                        MessageBox.Show("Не задан пароль");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Не задан логин");
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(DBNameBase) == false)
            {
                if (OnlyCheck == true)
                {
                    ConnectionStr += $@"Initial Catalog=master";
                    return true;
                }
                else if (OnlyCheck == false)
                {
                    ConnectionStr += $@"Initial Catalog={DBNameBase}";
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Не задано имя базы");
                return false;
            }



            //database=master
            //SELECT * FROM master.dbo.sysdatabases
            //SELECT * FROM sys.tables WHERE name = 'YourTable' AND type = 'U'
            return false;
        }
    }
}
