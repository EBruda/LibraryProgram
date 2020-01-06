using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Library
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        readonly Database databaseObject = new Database(); //creates a database object 
        readonly DataTable dt = new DataTable(); //for the datagrid 


        edit_window editW;
        borrow_window borrowW;
        warning_overdue warning;

        public MainWindow()
        {
            this.Visibility = Visibility.Visible;
            //passes in the entire windows (including methods)
            editW = new edit_window(this);
            borrowW = new borrow_window(this);
            warning = new warning_overdue(this);

            InitializeComponent();
            Read_database();  //automatically fills in datagrid   
            select_datagrid.SelectedItem = all_combo_item; //User friendly don't have to click all because it is already all

            Info(); //shows the extra info bar under each selected row  - TODO: Buggy when multiple are selected


            Count_records(); //counts the records 
            //Calc_Overdue();

        }

        //show the overdue books
        private void Calculate_Overdue()
        {
            databaseObject.OpenConnection();

            string b_select_cmd = "SELECT id, ISBN, GRBC_code, title, author, holder,  borrow_year, borrow_month, borrow_day from books WHERE status='Borrowed' and status NOTNULL and holder NOTNULL and borrow_year NOTNULL and borrow_month NOTNULL and borrow_day NOTNULL";
            SQLiteCommand myCommand_b_select = new SQLiteCommand(b_select_cmd, databaseObject.myConnection);


            using (myCommand_b_select)
            {
                using (SQLiteDataReader rdr = myCommand_b_select.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        int id_check = rdr.GetInt32(0);
                        int isbn_check = rdr.GetInt32(1);
                        int grbc_code_check = rdr.GetInt32(2);

                        string title_check = rdr.GetString(3);
                        string author_check = rdr.GetString(4);
                        string holder_check = rdr.GetString(5);

                        int year_check = rdr.GetInt32(6);
                        int month_check = rdr.GetInt32(7);
                        int day_check = rdr.GetInt32(8);


                        //year, month, day
                        DateTime date_checked_out = new DateTime(year_check, month_check, day_check);
                        DateTime today = DateTime.Today;
                        int daysDiff = ((TimeSpan)(today - date_checked_out)).Days;

                        if (daysDiff > 14) //check out limit: 2 weeks
                        {
                            warning_overdue warning_Overdue = new warning_overdue(this);
                            warning_Overdue.id_warning.Content = id_check.ToString();
                            warning_Overdue.isbn_warning.Content = isbn_check.ToString();
                            warning_Overdue.grbc_code_warning.Content = grbc_code_check.ToString();
                            warning_Overdue.title_warning.Content = title_check;
                            warning_Overdue.author_warning.Content = author_check;
                            warning_Overdue.holder_warning.Content = holder_check;

                            string full_date = month_check.ToString() + "/" + day_check.ToString() + "/" + year_check.ToString();
                            warning_Overdue.date_warning.Content = full_date;
                            warning_Overdue.Visibility = System.Windows.Visibility.Visible; //new window per overdue book

                        }
                    }
                }
            }
            databaseObject.CloseConnection();

        }


        private void Overdue_book_btn_Click(object sender, RoutedEventArgs e)
        {
            Calculate_Overdue();
            //TODO: Show "there are no overdue books" message somehow

        }


        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            //inserts data into sqlite database
            string query = "INSERT INTO books ('ISBN', 'GRBC_code' , 'title', 'author', 'status', 'holder', 'borrow_year', 'borrow_month', 'borrow_day') VALUES (@ISBN, @GRBC_code, @title, @author, @status, @holder, @year, @month, @day)";  //sqlite command 
            SQLiteCommand myCommand_add = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();

            myCommand_add.Parameters.AddWithValue("@ISBN", Int32.Parse(isbn_txt.Text));
            myCommand_add.Parameters.AddWithValue("@GRBC_code", Int32.Parse(grbc_code_txt.Text));
            myCommand_add.Parameters.AddWithValue("@title", title_txt.Text); //adds using input
            myCommand_add.Parameters.AddWithValue("@author", author_txt.Text); //adds using input 
            myCommand_add.Parameters.AddWithValue("@status", "Available"); //adds available because it is new
            myCommand_add.Parameters.AddWithValue("@holder", null);  //for now , no holder 
            myCommand_add.Parameters.AddWithValue("@year", null);
            myCommand_add.Parameters.AddWithValue("@month", null);
            myCommand_add.Parameters.AddWithValue("@day", null);

            myCommand_add.ExecuteNonQuery();
            //TODO: Unique GRBC_Code


            //resets everything
            isbn_txt.Text = "";
            grbc_code_txt.Text = "";
            title_txt.Text = "";
            author_txt.Text = "";

            Read_database();
            databaseObject.CloseConnection();
        }

        private void Refresh_btn_Click(object sender, RoutedEventArgs e)
        {
            Read_database(); //fills in the datagrid 
            Count_records(); //how many books
        }

        public void Read_database()
        {
            //select from database
            string select = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books"; //books is my table 
            SQLiteCommand myCommand_select = new SQLiteCommand(select, databaseObject.myConnection);

            var con = new SQLiteConnection("Data Source=database.sqlite3"); //connect to database 
            using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_select))
            {
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable); //fills in the datagrid (book_grid)

                book_grid.ItemsSource = dataTable.AsDataView();
            }

        }

        public void Read_borrowed()
        {
            //used when comboBox is borrowed 
            databaseObject.OpenConnection();
            string borrowed_cmd = "SELECT id, ISBN, GRBC_code, title, author, status, holder from books WHERE status = 'Borrowed'";
            SQLiteCommand myCommand_borrow = new SQLiteCommand(borrowed_cmd, databaseObject.myConnection);

            myCommand_borrow.ExecuteNonQuery();
            var con = new SQLiteConnection("Data Source=database.sqlite3");
            using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_borrow))
            {
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                book_grid.ItemsSource = dataTable.AsDataView();

            }
            databaseObject.CloseConnection();
        }

        public void Read_available()
        {
            //used when comboBox is available 
            databaseObject.OpenConnection();
            string borrowed_cmd = "SELECT id, ISBN, GRBC_code, title, author, status, holder from books WHERE status = 'Available'";
            SQLiteCommand myCommand_borrow = new SQLiteCommand(borrowed_cmd, databaseObject.myConnection);

            myCommand_borrow.ExecuteNonQuery();
            var con = new SQLiteConnection("Data Source=database.sqlite3");
            using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_borrow))
            {
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                book_grid.ItemsSource = dataTable.AsDataView();

            }
            databaseObject.CloseConnection();
        }

        private void Count_records()
        {
            int result = 0;
            databaseObject.OpenConnection();
            string count_cmd = "select count(id) from books"; //count cmd from sqlite...used to count records 
            SQLiteCommand myCommand_count = new SQLiteCommand(count_cmd, databaseObject.myConnection);

            result = Convert.ToInt32(myCommand_count.ExecuteScalar());
            num_records.Content = result.ToString();
            databaseObject.CloseConnection();

        }


        public void Info() //content text next to table
        {
            try
            {
                //TODO: This was declared and used twice fix this???
                string id_msg = ((DataRowView)book_grid.SelectedItem).Row[0].ToString();
                string ISBN = ((DataRowView)book_grid.SelectedItem).Row[1].ToString();
                string GRBC = ((DataRowView)book_grid.SelectedItem).Row[2].ToString();
                string title_msg = ((DataRowView)book_grid.SelectedItem).Row[3].ToString();
                string author_msg = ((DataRowView)book_grid.SelectedItem).Row[4].ToString();
                string status_msg = ((DataRowView)book_grid.SelectedItem).Row[5].ToString();
                string holder_msg = ((DataRowView)book_grid.SelectedItem).Row[6].ToString();

                //TODO: null vs "";
                if ((id_msg != null) && (ISBN != "") && (GRBC != "") && (title_msg != "") && (author_msg != null) && (status_msg != null) && (holder_msg != null))
                {
                    id_output.Content = id_msg;
                    isbn_output.Content = ISBN;
                    grbc_output.Content = GRBC;
                    title_output.Content = title_msg;
                    author_output.Content = author_msg;
                    status_output.Content = status_msg;
                    holder_output.Content = holder_msg;
                }
            }

            catch (Exception e_info)
            {
                MessageBox.Show("You did not select a valid item.+" +
                    "" + "\n" + "Error message: " + e_info.Message);

            }
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // deteles selected data 
                string selected_id = ((DataRowView)book_grid.SelectedItem).Row[0].ToString();
                string selected_status = ((DataRowView)book_grid.SelectedItem).Row[5].ToString();
                if (selected_status == "Borrowed")
                {
                    MessageBox.Show("Sorry this item can not be returned because it is currently borrowed. Please first return it");
                }

                else if (selected_status == "Available")
                {
                    string query = "DELETE FROM books WHERE id = " + Int32.Parse(selected_id); //TODO: delete by id 
                    SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
                    databaseObject.OpenConnection();

                    myCommand.ExecuteNonQuery();
                    databaseObject.CloseConnection();


                    databaseObject.OpenConnection();
                    InitializeComponent();
                    //NOTE: In order for it to show up read_database must be called, but it is so the person must also click save button
                    Count_records();

                    databaseObject.CloseConnection();
                }

                else
                {
                    MessageBox.Show("Error");
                }
            }

            catch(Exception e_delete)
            {
                MessageBox.Show("Error: " + e_delete.Message);
            }
        }

        private void Book_grid_RowDetailsVisibilityChanged(object sender, DataGridRowDetailsEventArgs e)
        {
            Info();
            //shows the little bar under the selection with title and author 
        }

        private void Edit_btn_Click(object sender, RoutedEventArgs e)
        {
            string id_msg = ((DataRowView)book_grid.SelectedItem).Row[0].ToString();
            string isbn_edit = ((DataRowView)book_grid.SelectedItem).Row[1].ToString();
            string grbc_edit = ((DataRowView)book_grid.SelectedItem).Row[2].ToString();
            string title_msg = ((DataRowView)book_grid.SelectedItem).Row[3].ToString();
            string author_msg = ((DataRowView)book_grid.SelectedItem).Row[4].ToString();
            string status = ((DataRowView)book_grid.SelectedItem).Row[5].ToString();
            string holder = ((DataRowView)book_grid.SelectedItem).Row[6].ToString();

            //shows book info in the edit window 
            editW.id_label.Content = id_msg;
            editW.new_isbn.Text = isbn_edit;
            editW.new_grbc_code.Text = grbc_edit;
            editW.new_title.Text = title_msg;
            editW.new_author.Text = author_msg;
            editW.new_status.Content = status;
            editW.new_holder.Content = holder;

            editW.Visibility = System.Windows.Visibility.Visible; //works better than close/show because it allows the window to be opened multiple times

        }

        private void Search_btn_Click(object sender, RoutedEventArgs e)
        {
            
            if (select_datagrid.SelectionBoxItem.ToString() == "All")
            {
                Search_all(); //search all of the books
            }

            else if (select_datagrid.SelectionBoxItem.ToString() == "Borrowed")
            {
                Search_borrowed(); //only search the borrowed books 
            }

            else if (select_datagrid.SelectionBoxItem.ToString() == "Available")
            {
                Search_available(); //only search the available books 
            }

            else
            {
                MessageBox.Show("ERROR WHEN SEARCHING!!!! Please select what you want to search"); //one must be selected - default all
            }
        }

        private void Search_all()
        {
            string search;
            string choice = select_search.SelectionBoxItem.ToString();

            //searching by id 
            #region
            if (choice == "Id")
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true) //like search 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE id LIKE '%" + search_txt.Text + "%'";
                }

                else //word for word search
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE id='" + search_txt.Text + "'";
                }

                //execute command 
                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView(); //fill datatable based on search 

                }

                databaseObject.CloseConnection();
            }
            #endregion

            //search by isbn
            #region
            else if (choice == "ISBN")
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true) //contains search 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE ISBN LIKE '%" + search_txt.Text + "%'";
                }

                else //word for word search 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE ISBN ='" + search_txt.Text + "'";
                }

                //execute command 
                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView(); //fill datatable based on search 

                }

                databaseObject.CloseConnection();
            }
            #endregion

            //search by grbc code 
            #region
            else if (choice == "GRBC Code")
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true) //contains search 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE GRBC_code LIKE '%" + search_txt.Text + "%'";
                }

                else //word for word search 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE GRBC_code ='" + search_txt.Text + "'";
                }

                //execute command 
                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView(); //fill datatable based on search 

                }

                databaseObject.CloseConnection();
            }
            #endregion

            //search by title
            #region
            else if (choice == "Title")
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true) //contains search 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE title LIKE '%" + search_txt.Text + "%'";
                }

                else //word for word search 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE title='" + search_txt.Text + "'";
                }

                //execute command 
                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView(); //fill datatable based on search 

                }

                databaseObject.CloseConnection();
            }
            #endregion

            //search by author 
            #region
            else if (choice == "Author")
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true) //contains search
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE author LIKE '%" + search_txt.Text + "%'";
                }

                else //search word for word 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE author='" + search_txt.Text + "'";
                }

                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView(); //fill datagrid based on search

                }

                databaseObject.CloseConnection();
            }
            #endregion

            //search by borrower
            #region
            else if (choice == "Borrower")
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true) //contains search
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE holder LIKE '%" + search_txt.Text + "%'";
                }

                else //word for word search 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE holder='" + search_txt.Text + "'";
                }

                //execute search
                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView(); //fill datagrid based on search 

                }

                databaseObject.CloseConnection();
            }
            #endregion

            else
            {
                MessageBox.Show("Not a valid selection");
            }
        }

        private void Search_borrowed()
        {
            string search;
            string choice = select_search.SelectionBoxItem.ToString();

            if (choice == "Id") //search based on id 
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true) //contains search
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE id LIKE '%" + search_txt.Text + "%' and status='Borrowed' ";
                }

                else //word for word search 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE id='" + search_txt.Text + "'" + " and status='Borrowed'";
                }

                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView(); //fill datagrid based on search

                }

                databaseObject.CloseConnection();
            }

            //search by isbn
            else if (choice == "ISBN") 
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true) //contains search
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE ISBN LIKE '%" + search_txt.Text + "%' and status='Borrowed' ";
                }

                else //word for word search 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE ISBN='" + search_txt.Text + "'" + " and status='Borrowed'";
                }

                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView(); //fill datagrid based on search

                }

                databaseObject.CloseConnection();
            }

            //search by grbc code
            else if (choice == "GRBC Code")
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true) //contains search
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE GRBC_code LIKE '%" + search_txt.Text + "%' and status='Borrowed' ";
                }

                else //word for word search 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE GRBC_code='" + search_txt.Text + "'" + " and status='Borrowed'";
                }

                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView(); //fill datagrid based on search

                }

                databaseObject.CloseConnection();
            }

            else if (choice == "Title") //search based on title
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true) //contains search
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE title LIKE '%" + search_txt.Text + "%' and status = 'Borrowed'";
                }

                else //word for word search 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE title='" + search_txt.Text + "' and status = 'Borrowed'";
                }

                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView();

                }

                databaseObject.CloseConnection();
            }

            else if (choice == "Author")
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true)
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE author LIKE '%" + search_txt.Text + "%' and status = 'Borrowed'";
                }

                else
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE author='" + search_txt.Text + "' and status = 'Borrowed'";
                }

                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView();

                }

                databaseObject.CloseConnection();
            }

            else if (choice == "Borrower")
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true)
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE holder LIKE '%" + search_txt.Text + "%'";
                }

                else
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE holder='" + search_txt.Text + "'";
                }

                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView();

                }

                databaseObject.CloseConnection();
            }

            else
            {
                MessageBox.Show("Not a valid selection");
            }
        }

        private void Search_available()
        {
            string search;
            string choice = select_search.SelectionBoxItem.ToString();

            if (choice == "Id")
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true)
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE id LIKE '%" + search_txt.Text + "%' and status='Available'";
                }

                else
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE id=" + "'" + search_txt.Text + "'" + " and status='Available'";
                }

                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView();

                }

                databaseObject.CloseConnection();
            }

            else if (choice == "ISBN") //search based on isbn
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true) //contains search
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE ISBN LIKE '%" + search_txt.Text + "%' and status='Available' ";
                }

                else //word for word search 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE ISBN='" + search_txt.Text + "'" + " and status='Available'";
                }

                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView(); //fill datagrid based on search

                }

                databaseObject.CloseConnection();
            }

            //search based on grbc code
            else if (choice == "GRBC Code") 
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true) //contains search
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE GRBC_code LIKE '%" + search_txt.Text + "%' and status='Available' ";
                }

                else //word for word search 
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE GRBC_code='" + search_txt.Text + "'" + " and status='Available'";
                }

                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView(); //fill datagrid based on search

                }

                databaseObject.CloseConnection();
            }

            else if (choice == "Title")
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true)
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE title LIKE '%" + search_txt.Text + "%' and status = 'Available'";
                }

                else
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE title='" + search_txt.Text + "' and status = 'Available'";
                }

                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView();

                }

                databaseObject.CloseConnection();
            }

            else if (choice == "Author")
            {
                databaseObject.OpenConnection();

                if (contains_check.IsChecked == true)
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE author LIKE '%" + search_txt.Text + "%' and status = 'Available'";
                }

                else
                {
                    search = "SELECT id, ISBN, GRBC_code, title, author, status, holder FROM books WHERE author='" + search_txt.Text + "' and status = 'Available'";
                }

                SQLiteCommand myCommand_search = new SQLiteCommand(search, databaseObject.myConnection);

                myCommand_search.ExecuteNonQuery();
                var con = new SQLiteConnection("Data Source=database.sqlite3");
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(myCommand_search))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    book_grid.ItemsSource = dataTable.AsDataView();

                }

                databaseObject.CloseConnection();
            }

            else if (choice == "Borrower")
            {
                MessageBox.Show("Sorry, all of these books are available. The holder is the library");
            }

            else
            {
                MessageBox.Show("Not a valid selection");
            }
        }

        private void Clear_btn_Click(object sender, RoutedEventArgs e)
        {
            Read_database();
            select_datagrid.SelectedItem = all_combo_item;
            search_txt.Text = "";
            contains_check.IsChecked = false;
            select_search.SelectedIndex = -1;
        }

        public void All_Selected(object sender, RoutedEventArgs e)
        {
            Read_database();
        }

        public void Borrow_Selected(object sender, RoutedEventArgs e)
        {
            Read_borrowed();
        }

        public void Available_Selected(object sender, RoutedEventArgs e)
        {
            Read_available();
        }

        private void Borrow_btn_Click(object sender, RoutedEventArgs e)
        {
            if (((DataRowView)book_grid.SelectedItem).Row[5].ToString() == "Available")
            {
                string id_main = ((DataRowView)book_grid.SelectedItem).Row[0].ToString();
                string isbn_main = ((DataRowView)book_grid.SelectedItem).Row[1].ToString();
                string grbc_code_main = ((DataRowView)book_grid.SelectedItem).Row[2].ToString();
                string title_main = ((DataRowView)book_grid.SelectedItem).Row[3].ToString();
                string author_main = ((DataRowView)book_grid.SelectedItem).Row[4].ToString();

                borrowW.show_id.Content = id_main;
                borrowW.show_isbn.Content = isbn_main;
                borrowW.show_grbc_code.Content = grbc_code_main;
                borrowW.show_title.Content = title_main;
                borrowW.show_author.Content = author_main;

                borrowW.Visibility = System.Windows.Visibility.Visible;

                databaseObject.OpenConnection();
            }

            else if (((DataRowView)book_grid.SelectedItem).Row[5].ToString() == "Borrowed")
            {
                MessageBox.Show("This item is currently already borrowed");
            }

        }

        private void Return_btn_Click(object sender, RoutedEventArgs e)
        {
            if (((DataRowView)book_grid.SelectedItem).Row[5].ToString() == "Borrowed")
            {
                string id = ((DataRowView)book_grid.SelectedItem).Row[0].ToString();
                string isbn = ((DataRowView)book_grid.SelectedItem).Row[1].ToString();
                string grbc = ((DataRowView)book_grid.SelectedItem).Row[2].ToString();
                string title = ((DataRowView)book_grid.SelectedItem).Row[3].ToString();
                string author = ((DataRowView)book_grid.SelectedItem).Row[4].ToString();
                string status = ((DataRowView)book_grid.SelectedItem).Row[5].ToString();
                string holder = ((DataRowView)book_grid.SelectedItem).Row[6].ToString();

                Return_book(id, isbn, grbc, title, author, holder);
            }

            else if (((DataRowView)book_grid.SelectedItem).Row[5].ToString() == "Available")
            {
                MessageBox.Show("This item is currently already returned");
            }

            else
            {
                MessageBox.Show("Borrowing issue");
            }
        }

        public void Return_book(string id,string isbn, string grbc_code ,string title, string author, string holder)
        {
            databaseObject.OpenConnection();
            string return_cmd = "UPDATE books SET ISBN=@isbn, GRBC_code=@grbc, title=@title, author=@author, status=@status, holder=@holder, borrow_year=@year, " +
            "borrow_month=@month, borrow_day=@day " +
            " WHERE id=@id";

            SQLiteCommand myCommand_return = new SQLiteCommand(return_cmd, databaseObject.myConnection);
            myCommand_return.Parameters.AddWithValue("@id", id);
            myCommand_return.Parameters.AddWithValue("@isbn", isbn);
            myCommand_return.Parameters.AddWithValue("@grbc", grbc_code);
            myCommand_return.Parameters.AddWithValue("@title",title);
            myCommand_return.Parameters.AddWithValue("@author", author);
            myCommand_return.Parameters.AddWithValue("@holder", null);
            myCommand_return.Parameters.AddWithValue("@year", null);
            myCommand_return.Parameters.AddWithValue("@month", null);
            myCommand_return.Parameters.AddWithValue("@day", null);
            myCommand_return.Parameters.AddWithValue("@status", "Available"); //this is available because I want it to be returned
            databaseObject.OpenConnection();

            myCommand_return.ExecuteNonQuery();
            databaseObject.CloseConnection();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("C:\\Users\\Elizabeth\\source\\repos\\Library\\Library\\bin\\Debug\\people.xml");

            XmlNodeList actual_list = xmlDoc.DocumentElement.ChildNodes;
            foreach (XmlNode xmlNode in actual_list)
            {
                String name_xml = xmlNode.Attributes["name"].Value;
                if (name_xml == holder)
                {
                    XmlElement id_return = xmlDoc.CreateElement("book");
                    //        <book id="" action="Returned">
                    id_return.SetAttribute("id", id);
                    id_return.SetAttribute("ISBN", isbn);
                    id_return.SetAttribute("GRBC_Code", grbc_code);
                    id_return.SetAttribute("title", title);
                    id_return.SetAttribute("author", author);
                    id_return.SetAttribute("action", "Returned");
                    xmlNode.AppendChild(id_return);

                    DateTime now = DateTime.Now;
                    string month = now.ToString("MM");
                    string year = DateTime.Now.Year.ToString();
                    string day = DateTime.Now.Day.ToString();


                    XmlElement id_date = xmlDoc.CreateElement("date");
                    id_date.SetAttribute("month", month);
                    id_date.SetAttribute("year", year);
                    id_date.SetAttribute("day", day);
                    id_return.AppendChild(id_date);                   

                    // xmlDoc.PreserveWhitespace = true;
                    xmlDoc.Save("C:\\Users\\Elizabeth\\source\\repos\\Library\\Library\\bin\\Debug\\people.xml");
                }
            }
            Read_database();
        }


    }
}
