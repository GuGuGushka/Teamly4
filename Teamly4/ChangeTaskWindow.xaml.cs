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
    /// Логика взаимодействия для ChangeTaskWindow.xaml
    /// </summary>
    public partial class ChangeTaskWindow : Window
    {
        public new String Name;
        public String Description;
        public Users PerformerOfTask;
        public ChangeTaskWindow()
        {
            InitializeComponent();          
            TextBoxName.Text = CurrentTaskManagerGlobal.Name;
            TextBoxDescription.Text = CurrentTaskManagerGlobal.Description;
            ComboBoxPerformers.ItemsSource = CurrentProjectManagerGlobal.PerformersOfProject;

            ComboBoxPerformers.SelectedItem = CurrentTaskManagerGlobal.Performer;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Name = TextBoxName.Text;
            Description = TextBoxDescription.Text;
            PerformerOfTask = (Users)ComboBoxPerformers.SelectedItem;

            if (Name.Length == 0)
            {
                MessageBox.Show("Имя не заполнено");
                return;
            }
            if (Description.Length == 0)
            {
                MessageBox.Show("Описание не заполнено");
                return;
            }
            if (PerformerOfTask == null)
            {
                MessageBox.Show("Исполнитель не назначен");
                return;
            }
            if (Name.Length != 0 && Description.Length != 0 && PerformerOfTask != null)
            {
                DialogResult = true;
                this.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }
    }
}
