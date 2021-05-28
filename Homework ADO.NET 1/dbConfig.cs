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
        static public List<string> TablesBase = new List<string>();
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

        //true = check, false = create
        static public bool CheckOrCreateTables(bool check)
        {
            try
            {
                if (check == false)
                {
                    using (SqlConnection sqlConnection = new SqlConnection(ConnectionStr))
                    {
                        sqlConnection.Open();

                        //sellers
                        string command = $"CREATE TABLE {TablesBase[0]}" +
                            $"(" +
                            $"id INT NOT NULL PRIMARY KEY IDENTITY," +
                            $"FirstName nvarchar(20) NOT NULL," +
                            $"LastName nvarchar(20) NOT NULL" +
                            $");";
                        SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                        sqlCommand.ExecuteNonQuery();

                        //Byers
                        command = $"CREATE TABLE {TablesBase[1]}" +
                            $"(" +
                            $"id INT NOT NULL PRIMARY KEY IDENTITY," +
                            $"FirstName nvarchar(20) NOT NULL," +
                            $"LastName nvarchar(20) NOT NULL" +
                            $");";
                        sqlCommand = new SqlCommand(command, sqlConnection);
                        sqlCommand.ExecuteNonQuery();

                        //Sales
                        command = $"CREATE TABLE {TablesBase[2]}" +
                            $"(" +
                            $"id INT NOT NULL PRIMARY KEY IDENTITY," +
                            $"idByers int NOT NULL REFERENCES {TablesBase[1]}(id)," +
                            $"idSellers int NOT NULL REFERENCES {TablesBase[0]}(id)," +
                            $"SummSale MONEY NOT NULL," +
                            $"DateSale DATETIME NOT NULL default GETDATE()" +
                            $");";
                        sqlCommand = new SqlCommand(command, sqlConnection);
                        sqlCommand.ExecuteNonQuery();                       
                    }
                    return true;
                }
                else
                {
                    using (SqlConnection sqlConnection = new SqlConnection(ConnectionStr))
                    {
                        sqlConnection.Open();
                        //sellers
                        string command = $"SELECT * FROM {TablesBase[0]}";
                        SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                        SqlDataReader reader = sqlCommand.ExecuteReader();
                        if (true)
                        {
                            //Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));
                            if (reader.GetName(0) != "id")
                                return false;
                            if (reader.GetName(1) != "FirstName")
                                return false;
                            if (reader.GetName(2) != "LastName")
                                return false;
                        }
                        reader.Close();

                        command = $"SELECT * FROM {TablesBase[1]}";
                        sqlCommand = new SqlCommand(command, sqlConnection);
                        reader = sqlCommand.ExecuteReader();
                        if (true)
                        {
                            if (reader.GetName(0) != "id")
                                return false;
                            if (reader.GetName(1) != "FirstName")
                                return false;
                            if (reader.GetName(2) != "LastName")
                                return false;
                        }
                        reader.Close();

                        command = $"SELECT * FROM {TablesBase[2]}";
                        sqlCommand = new SqlCommand(command, sqlConnection);
                        reader = sqlCommand.ExecuteReader();
                        if (true)
                        {
                            if (reader.GetName(0) != "id")
                                return false;
                            if (reader.GetName(1) != "idByers")
                                return false;
                            if (reader.GetName(2) != "idSellers")
                                return false;
                            if (reader.GetName(3) != "SummSale")
                                return false;
                            if (reader.GetName(4) != "DateSale")
                                return false;
                        }
                        reader.Close();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;

        }

        static public bool FirstAddData()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionStr))
                {
                    sqlConnection.Open();
                    string command = $"INSERT INTO {TablesBase[0]} (FirstName, LastName) VALUES " +
                        $"('Marti', 'Davidson')," +
                        $"('John', 'Martinez')" +
                        $";";
                     SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                    sqlCommand.ExecuteNonQuery();

                    command = $"INSERT INTO {TablesBase[1]} (FirstName, LastName) VALUES " +
                        $"('Некий', 'Покупатель')," +
                        $"('Иван', 'Попов')" +
                        $";";
                    sqlCommand = new SqlCommand(command, sqlConnection);
                    sqlCommand.ExecuteNonQuery();

                    command = $"INSERT INTO {TablesBase[2]} (idByers, idSellers, SummSale) VALUES " +
                        $"(1, 1, 800)," +
                        $"(1, 2, 950)," +
                        $"(2, 2, 1000)," +
                        $"(2, 1, 981)" +
                        $";";
                    sqlCommand = new SqlCommand(command, sqlConnection);
                    sqlCommand.ExecuteNonQuery();                    
                }
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        static public void Insert(string command)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionStr))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        static public List<string> GetSales()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionStr))
                {
                    sqlConnection.Open();
                    string command = $"SELECT id FROM Sales;";
                    SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<string> list = new List<string>();
                        while (reader.Read())
                        {
                            list.Add(reader.GetValue(0).ToString());
                        }
                        return list;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        static public List<string> GetInfo(string id)
        {
            List<string> list = new List<string>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionStr))
                {
                    sqlConnection.Open();
                    string command = $"select Byers.FirstName AS BFName,Byers.LastName AS BLName,Sellers.FirstName AS SFname,Sellers.LastName AS SLName,SummSale,DateSale from sales JOIN Byers ON Byers.id = sales.idByers JOIN Sellers ON Sellers.id = sales.idSellers where Sales.id ={id};";
                    SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            object FNameByer = reader["BFName"];
                            object LNameByer = reader["BLName"];
                            object FNameSallers = reader["SFName"];
                            object LNameSallers = reader["SLName"];
                            object Summ = reader["SummSale"];
                            object date = reader["DateSale"];

                            list.Add(FNameByer.ToString() + LNameByer.ToString());
                            list.Add(FNameSallers.ToString() + LNameSallers.ToString());
                            list.Add(String.Format("{0:0.##}", Summ));
                            list.Add(date.ToString());
                        }
                    }
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return list;
        }

        static public List<string> GetFIO(string table)
        {
            List<string> list = new List<string>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionStr))
                {
                    sqlConnection.Open();
                    string command = $"SELECT FirstName,LastName from {table};";
                    SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            object Fname = reader["FirstName"];
                            object Lname = reader["LastName"];

                            list.Add(Lname.ToString() + " " + Fname.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return list;
        }

        static public void AcceptSale(string Sallers, string Byers, string summ)
        {
            try
            {
                string idSaller = "";
                string idByers = "";

                //double _summ = Convert.ToDouble(summ);

                using (SqlConnection sqlConnection = new SqlConnection(ConnectionStr))
                {
                    sqlConnection.Open();
                    string command = "Select id,(CONCAT(LastName, ' ', FirstName)) from sellers;";
                    SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                   // if (reader.HasRows)
                   // {
                        while (reader.Read())
                        {
                            object id = reader.GetValue(0);
                            object FIO = reader.GetValue(1);
                            if (FIO.ToString()==Sallers)
                            {
                                idSaller = id.ToString();
                            break;
                            }
                        }
                    //}
                }
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionStr))
                {
                    sqlConnection.Open();
                    string command = "Select id,(CONCAT(LastName, ' ', FirstName)) from Byers;";
                    SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                   // if (reader.HasRows)
                   // {
                        while (reader.Read())
                        {
                            object id = reader.GetValue(0);
                            object FIO = reader.GetValue(1);
                            if (FIO.ToString() == Byers)
                            {
                                idByers = id.ToString();
                                break;
                            }
                        }
                   // }
                }
                //Select CONCAT(FirstName, ' ', LastName) from sellers AS FIO
                
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionStr))
                {
                    sqlConnection.Open();
                    string command = $"INSERT INTO sales (idByers, idSellers, SummSale) VALUES ({idByers}, {idSaller}, {summ});";
                    SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                    sqlCommand.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //распарсить ФИО 1
            //распарсить ФИО 2
            //распарсить сумму
            //добавить в таблицу
        }
    }
}
