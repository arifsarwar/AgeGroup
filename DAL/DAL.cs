using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Reflection;
using DAL;

namespace DataAccess
{
    public class DAL : IDisposable
    {

/* new comments added to the DAL*/


        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private SQLiteDataAdapter DB;
        private SQLiteDataReader DR;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();

        private void SetConnection()
        {
            sql_con = new SQLiteConnection();
        }

        private SQLiteConnection getConnection()
        {
            string str = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = null;
            if (System.IO.File.Exists(str + "AgeRanger.db"))
            {
                path = "Data Source= " + str + "AgeRanger.db;Version=3;Enlist=N";

            }
            else
                path = ConfigurationManager.ConnectionStrings["AgeRanger"].ConnectionString;

            return new SQLiteConnection(path);
            //Data Source=C:\\Users\\Arif\\Downloads\\Ageranger.db;Version=3;New=False;Compress=True;
        }


        public Person getPerson(int id)
        {
            //return listEmp.First(e => e.ID == id);  
            DR = null;
            sql_con = getConnection();

            SQLiteCommand sqlCmd = sql_con.CreateCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Select firstname,lastname,age from person where Id=" + id + "";
            sqlCmd.Connection = sql_con;
            sql_con.Open();
            DR = sqlCmd.ExecuteReader();
            Person p = null;
            while (DR.Read())
            {
                p = new Person();
                p.FName = DR.GetValue(0).ToString();
                p.LName = DR.GetValue(1).ToString();
                p.Age = Convert.ToInt32(DR.GetValue(2));
            }

            sql_con.Close();
            return p;

        }

        public DataTable getAll(string sCritera, string sValue)
        {
            //return listEmp.First(e => e.ID == id);  
            DR = null;
            sql_con = getConnection();

            SQLiteCommand sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandType = CommandType.Text;
            string CommandText = "SELECT P.*,AG.Description as AgeGroup FROM Person p LEFT JOIN AgeGroup AG on p.AGe >= AG.MinAge and p.Age <= AG.MaxAge ";
            if (!string.IsNullOrEmpty(sCritera))
            {
                switch (sCritera)
                {
                    case "First Name":
                        CommandText += " where FirstName = '" + sValue + "'";
                        break;
                    case "Last Name":
                        CommandText += " Where LastName = '" + sValue + "'";
                        break;
                }

            }

            //sCritera + sValue;

            sql_cmd.Connection = sql_con;
            sql_con.Open();

            DB = new SQLiteDataAdapter(CommandText, sql_con);
            DS.Reset();
            DB.Fill(DS);
            DT = DS.Tables[0];

            sql_con.Close();
            return DT;

        }

        public int AddPerson(Person p)
        {
            //sql_con = new SqlConnection();
            sql_con = getConnection();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandType = CommandType.Text;
            sql_cmd.CommandText = "INSERT INTO person (firstname,LastName,Age) Values (@firstname,@LastName,@Age)";
            sql_cmd.Connection = sql_con;
            int newID = 0;
            using (sql_cmd)
            {
                sql_cmd.Parameters.AddWithValue("@firstname", p.FName);
                sql_cmd.Parameters.AddWithValue("@LastName", p.LName);
                sql_cmd.Parameters.AddWithValue("@Age", p.Age);
                sql_con.Open();
                newID = (int)sql_cmd.ExecuteNonQuery();
                sql_con.Close();

            }
            return newID;
        }

        public void Dispose()
        {
            if (sql_con != null)
                sql_con.Close();
        }
    }

}
