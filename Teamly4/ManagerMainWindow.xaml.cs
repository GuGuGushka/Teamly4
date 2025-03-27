using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для ManagerMainWindow.xaml
    /// </summary>
    public partial class ManagerMainWindow : Window
    {
        private readonly IUM2323DTeamlyEntities db;
        public ManagerMainWindow()
        {
            InitializeComponent();
            try
            {
                db = new IUM2323DTeamlyEntities();
                var currentUser = db
                            .Users
                            .FirstOrDefault(u => u.UserName == CurrentUserGlobal.Login && u.Password == CurrentUserGlobal.Password);

                DataGridManagerProjects.ItemsSource = db.Projects.Where(x => x.ManagerId == currentUser.Id).ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DataGridManagerProjects_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DataGridManagerProjects.SelectedItem != null)
            {
                Projects project = (Projects)DataGridManagerProjects.SelectedItem;

                TextBoxProjectName.Text = project.Name;
                TextBoxProjectDescription.Text = project.Description;

                var performersUserNames = db.Performers
                                    .Where(p => p.Projectid == project.Id)
                                    .Join(db.Users,
                                            p => p.Workerid,
                                            u => u.Id,
                                            (p, u) => u.UserName)
                                    .ToList();

                var tasksNames

                DataGridPerformers.ItemsSource = null;
                DataGridTasks.ItemsSource = null;

                DataGridPerformers.ItemsSource = performersUserNames;
                DataGridTasks.ItemsSource = tasksNames;
            }
        }

        private void DataGridPerformers_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void DataGridTasks_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void ButtonAddProject_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDellProject_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDelTask_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ByttonAddTask_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFinishTask_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonChangeTask_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
