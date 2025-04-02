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
    /// Логика взаимодействия для AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        private readonly IUM2323DTeamlyEntities db;

        public new String Name;
        public String Description;
        public Statuses status;
        public Priorities priority;
        public Users Performer;

        public AddTaskWindow()
        {
            InitializeComponent();
            db = new IUM2323DTeamlyEntities();
            ComboBoxStatuses.ItemsSource = db.Statuses.Where(t => t.Id != 1002).Select(t => t).ToList();
            ComboBoxPriorities.ItemsSource = db.Priorities.Select(t => t).ToList();

            ComboBoxPerformers.ItemsSource = CurrentProjectManagerGlobal.PerformersOfProject;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxName.Text == null)
            {
                MessageBox.Show("Имя не заполнено");
                return;
            }
            if (TextBoxDescription.Text == null)
            {
                MessageBox.Show("Описание не заполнено");
                return;
            }
            if (ComboBoxStatuses.SelectedItem == null)
            {
                MessageBox.Show("Статус не выбран");
                return;
            }
            if (ComboBoxPriorities.SelectedItem == null)
            {
                MessageBox.Show("Приоритет выполнения не выбран");
                return;
            }
            if (ComboBoxPerformers.SelectedItem == null)
            {
                MessageBox.Show("Исполнитель не выбран не выбран");
                return;
            }
            Name = TextBoxName.Text;
            Description = TextBoxDescription.Text;
            status = (Statuses)ComboBoxStatuses.SelectedItem;
            priority = (Priorities)ComboBoxPriorities.SelectedItem;
            Performer = (Users)ComboBoxPerformers.SelectedItem;

            DialogResult = true;
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }
    }
}
