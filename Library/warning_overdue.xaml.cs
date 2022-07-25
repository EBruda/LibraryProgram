using System.Windows;

namespace Library
{
    /// <summary>
    /// Interaction logic for warning_overdue.xaml
    /// </summary>
    public partial class warning_overdue : Window
    {
        MainWindow MW;
       

        public warning_overdue(MainWindow MW)
        {
            InitializeComponent();
        }

        private void Return_over_btn_Click(object sender, RoutedEventArgs e)
        {
            string id = id_warning.Content.ToString();
            string isbn = isbn_warning.Content.ToString();
            string grbc_code = grbc_code_warning.Content.ToString();
            string title = title_warning.Content.ToString();
            string author = author_warning.Content.ToString();
            string holder = holder_warning.Content.ToString();
            ((MainWindow)Application.Current.MainWindow).Return_book(id, isbn, grbc_code, title, author, holder);
            
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Ignore_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
