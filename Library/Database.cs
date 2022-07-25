using System.IO;
using System.Data.SQLite;
using System.Windows;

namespace Library
{
    class Database
    {
        public SQLiteConnection myConnection;

        public Database()
        {
            myConnection = new SQLiteConnection("Data Source=database.sqlite3");

            if(!File.Exists("./database.sqlite3"))
            {
                MessageBox.Show("Database created");
                SQLiteConnection.CreateFile("database.sqlite3");
            }
        }
        
        public void OpenConnection()
        {
            if(myConnection.State != System.Data.ConnectionState.Open)
            {
                myConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if(myConnection.State != System.Data.ConnectionState.Closed)
            {
                myConnection.Close();
            }
        }
    }
}
