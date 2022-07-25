using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.IO;
using System.Windows.Controls.Primitives;
using System.Data.SQLite;

namespace Library
{
    /// <summary>
    /// Interaction logic for borrow_window.xaml
    /// </summary>
    public partial class borrow_window : Window
    {
        Database borrow_database = new Database();
        MainWindow MW;
        bool read_list = true;
        List<string> people_list = new List<string>();


        public borrow_window(MainWindow MW)
        {
            InitializeComponent();
            this.MW = MW;
            Read_people();

            names_combo.Loaded += delegate
            {
                System.Windows.Controls.TextBox textBox = names_combo.Template.FindName("PART_EditableTextBox", names_combo) as System.Windows.Controls.TextBox;
                Popup popup = names_combo.Template.FindName("PART_Popup", names_combo) as Popup;
                if (textBox != null)
                {
                    textBox.TextChanged += delegate
                    {
                        names_combo.Items.Filter += (item) =>
                        {
                            if (item.ToString().StartsWith(textBox.Text))
                            {
                                popup.IsOpen = true;
                                return true;

                            }
                            else
                            {
                                // popup.IsOpen = false;
                                return false;
                            }
                        };

                    };
                }
            };

            names_combo.KeyDown += new System.Windows.Input.KeyEventHandler(comboBox1_KeyDown);
        }

        public static string PrintXML(string xml)
        {
            string result = "";

            MemoryStream mStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode);
            XmlDocument document = new XmlDocument();

            
            try
            {
                // Load the XmlDocument with the XML.
                document.LoadXml(xml);

                writer.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                document.WriteContentTo(writer);
                writer.Flush();
                mStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                mStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader sReader = new StreamReader(mStream);

                // Extract the text from the StreamReader.
                string formattedXml = sReader.ReadToEnd();

                result = formattedXml;
            }
            catch (XmlException)
            {
                // Handle the exception
            }

            mStream.Close();
            writer.Close();

            return result;
        }

        public void Read_people()
        {
            XmlDocument xmlDoc = new XmlDocument();

            if (read_list == true)
            {
                xmlDoc.Load("C:\\Users\\Elizabeth\\source\\repos\\Library\\Library\\bin\\Debug\\people.xml");

                XmlNodeList actual_list = xmlDoc.DocumentElement.ChildNodes;
                foreach (XmlNode xmlNode in actual_list)
                {
                    String name = xmlNode.Attributes["name"].Value;
                    people_list.Add(name);
                }
            }

            names_combo.DataContext = people_list;

            read_list = false;
        }

        private void comboBox1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == Key.Down)
            {
                e.Handled = true;
                names_combo.Items.MoveCurrentToNext();
            }
        }

        private void Submit_borrow_btn_Click(object sender, RoutedEventArgs e)
        {
            DateTime now = DateTime.Now;
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.Year.ToString();
            string day = DateTime.Now.Day.ToString();

            string name = names_combo.Text;
            borrow_database.OpenConnection();

            string id_set = show_id.Content.ToString();
            string isbn_set = show_isbn.Content.ToString();
            string grbc_code_set = show_grbc_code.Content.ToString();
            string title_set = show_title.Content.ToString();
            string author_set = show_author.Content.ToString();
            //status is [5]
            string holder_set = names_combo.Text.ToString();

            string return_cmd = "UPDATE books SET ISBN=@isbn, GRBC_code=@grbc, title=@title, author=@author, status=@status, holder=@holder, borrow_year=@year, " +
                "borrow_month=@month, borrow_day=@day " +
            " WHERE id=@id";

            SQLiteCommand myCommand_borrow = new SQLiteCommand(return_cmd, borrow_database.myConnection);
            myCommand_borrow.Parameters.AddWithValue("@id", id_set);
            myCommand_borrow.Parameters.AddWithValue("@isbn", isbn_set);
            myCommand_borrow.Parameters.AddWithValue("@grbc", grbc_code_set);
            myCommand_borrow.Parameters.AddWithValue("@title", title_set);
            myCommand_borrow.Parameters.AddWithValue("@author", author_set);
            myCommand_borrow.Parameters.AddWithValue("@holder", name);
            myCommand_borrow.Parameters.AddWithValue("@year", Int32.Parse(year));
            myCommand_borrow.Parameters.AddWithValue("@month", Int32.Parse(month));
            myCommand_borrow.Parameters.AddWithValue("@day", Int32.Parse(day));
            myCommand_borrow.Parameters.AddWithValue("@status", "Borrowed"); //this is borrowed because I want it to be borrowed
            borrow_database.OpenConnection();

            myCommand_borrow.ExecuteNonQuery();
            borrow_database.CloseConnection();
            XmlDocument xmlDoc = new XmlDocument();

            bool new_person = false;
            for(int i = 0; i < people_list.Count(); i++)
            {
                if(people_list.Contains(names_combo.Text))
                {
                    new_person = false;
                }

                else
                {
                    new_person = true;
                    i = people_list.Count();
                }
            }


            if (new_person == true)
            {
                MessageBox.Show("new person!!");
                xmlDoc.Load("C:\\Users\\Elizabeth\\source\\repos\\Library\\Library\\bin\\Debug\\people.xml");


                XmlElement id_person = xmlDoc.CreateElement("Person");
                id_person.SetAttribute("name", name);
                xmlDoc.DocumentElement.AppendChild(id_person);
                xmlDoc.Save("C:\\Users\\Elizabeth\\source\\repos\\Library\\Library\\bin\\Debug\\people.xml");
            }

            xmlDoc.Load("C:\\Users\\Elizabeth\\source\\repos\\Library\\Library\\bin\\Debug\\people.xml");

            XmlNodeList actual_list = xmlDoc.DocumentElement.ChildNodes;
            foreach (XmlNode xmlNode in actual_list)
            {
                String name_xml = xmlNode.Attributes["name"].Value;
                if(name_xml == name)
                {
                    XmlElement id_borrow = xmlDoc.CreateElement("book");
                    //        <book id="" action="Returned">
                    id_borrow.SetAttribute("id", show_id.Content.ToString());
                    id_borrow.SetAttribute("ISBN", isbn_set);
                    id_borrow.SetAttribute("GRBC_Code", grbc_code_set);
                    id_borrow.SetAttribute("title", title_set);
                    id_borrow.SetAttribute("author", author_set);
                    id_borrow.SetAttribute("action", "Borrowed");
                    xmlNode.AppendChild(id_borrow);

                    
                    XmlElement id_date = xmlDoc.CreateElement("date");
                    id_date.SetAttribute("month", month);
                    id_date.SetAttribute("year", year);
                    id_date.SetAttribute("day", day);
                    id_borrow.AppendChild(id_date);

                    // xmlDoc.PreserveWhitespace = true;
                    xmlDoc.Save("C:\\Users\\Elizabeth\\source\\repos\\Library\\Library\\bin\\Debug\\people.xml");
                }
            }

            names_combo.Text = "";
            ((MainWindow)Application.Current.MainWindow).Read_database();
            this.Visibility = System.Windows.Visibility.Hidden;


        }
    }
}
