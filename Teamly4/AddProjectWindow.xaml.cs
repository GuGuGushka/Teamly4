using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Teamly4
{
    /// <summary>
    /// Логика взаимодействия для AddProjectWindow.xaml
    /// </summary>
    public partial class AddProjectWindow : Window
    {
        private readonly IUM2323DTeamlyEntities db;

        public new String Name;
        public String Description;
        public DateTime FinishDate;
        public List<Users> performers;

        public AddProjectWindow()
        {
            InitializeComponent();
            db = new IUM2323DTeamlyEntities();

            ListBoxUsers.Items.Clear();
            ListBoxUsers.ItemsSource = db.Users.Where(i => i.RoleId == 3).ToList();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxName.Text != null && TextBoxDescription.Text != null && TextBoxFinishTime.Text != null && ListBoxUsers.SelectedItems != null)
            {


                Name = TextBoxName.Text;
                Description = TextBoxDescription.Text;
                FinishDate = DateTime.Parse(TextBoxFinishTime.Text);
                performers = ListBoxUsers.SelectedItems.Cast<Users>().ToList();
                

                DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Не все данные заполнены");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }
    }
}
