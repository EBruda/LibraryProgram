using System.Data.SQLite;
using System.Windows;

namespace Library
{
    /// <summary>
    /// Interaction logic for edit_window.xaml
    /// </summary>
    public partial class edit_window : Window
    {
        MainWindow MW;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Hidden;
            //TO DO: If this window is closed without clicking cancel or submit (click the red x) then it must somehow be set to hidden

        }

        public edit_window(MainWindow MW)
        {
            this.MW = MW;
            InitializeComponent();

        }

        private void Cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Submit_btn_Click(object sender, RoutedEventArgs e)
        {
            string updated_id = id_label.Content.ToString();
            string updated_isbn = new_isbn.Text;
            string updated_grbc_code = new_grbc_code.Text;
            string updated_title = new_title.Text;
            string updated_author = new_author.Text;
            string updated_status = new_status.Content.ToString();
            string updated_holder = new_holder.Content.ToString();

            //BEFORE YOU COULD CHANGE STATUS FROM EDIT WINDOW,,,,BUT THAT IS REDUNDANT AND MESSY
         /*   if(edit_able_borrow.SelectionBoxItem.ToString() == "Available")
            {
                updated_status = "Available";
            }

            else if(edit_able_borrow.SelectionBoxItem.ToString() == "Borrowed")
            {
                updated_status = "Borrowed";
            }

            else
            {
                MessageBox.Show("ERROR: " + edit_able_borrow.SelectionBoxItem.ToString());
            }

    */
            Database edit_database = new Database();

            //inserts data into sqlite database
            string update = "UPDATE books SET ISBN=@newisbn, GRBC_code=@newcode, title=@newtitle, author=@newauthor, status=@status, holder=@holder" +
            " WHERE id=@newid"; //TOSO: edit by id 

            SQLiteCommand myCommand_edit = new SQLiteCommand(update, edit_database.myConnection);
            myCommand_edit.Parameters.AddWithValue("@newid", updated_id);
            myCommand_edit.Parameters.AddWithValue("@newisbn", updated_isbn);
            myCommand_edit.Parameters.AddWithValue("@newcode", updated_grbc_code);
            myCommand_edit.Parameters.AddWithValue("@newtitle", updated_title);
            myCommand_edit.Parameters.AddWithValue("@newauthor", updated_author);
            myCommand_edit.Parameters.AddWithValue("@status", updated_status);
            myCommand_edit.Parameters.AddWithValue("@holder", updated_holder);
            edit_database.OpenConnection();

            myCommand_edit.ExecuteNonQuery();
            edit_database.CloseConnection();

            ((MainWindow)Application.Current.MainWindow).Read_database();
            this.Visibility = System.Windows.Visibility.Hidden;


        }
    }
}
