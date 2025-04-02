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
    /// Логика взаимодействия для WorkerMainWindow.xaml
    /// </summary>
    public partial class WorkerMainWindow : Window
    {
        private readonly IUM2323DTeamlyEntities db;
        private readonly Users currentUser;
        public WorkerMainWindow()
        {
            InitializeComponent();
            db = new IUM2323DTeamlyEntities();
            currentUser = db
                        .Users
                        .FirstOrDefault(u => u.UserName == CurrentUserGlobal.Login && u.Password == CurrentUserGlobal.Password);

            LoadProjectData();
        }

        private void LoadProjectData()
        {
            try
            {
                DataGridWorkerProjects.ItemsSource = db.Projects.Where(q => q.Performers.Any(p => p.Workerid == currentUser.Id)).ToList();

                ComboBoxPriorities.ItemsSource = db.Priorities.ToList();
                ComboBoxStatuses.ItemsSource = db.Statuses.Where(t => t.Id != 1002).ToList();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void DataGridWorkerProjects_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Projects project = (Projects)DataGridWorkerProjects.SelectedItem;

            TextBoxProjectName.Text = project.Name;
            TextBoxProjectDescription.Text = project.Description;
            TextBoxStartDateTime.Text = project.StartTime.ToString("yyyy-MM-dd");
            TextBoxFinishDateTime.Text = project.FinishTime.ToString("yyyy-MM-dd");

            DataGridTasks.ItemsSource = null;
            DataGridTasks.ItemsSource = project.Tasks.Where(x => x.WorkerId == currentUser.Id);  

        }

        private void DataGridTasks_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DataGridTasks.SelectedItem != null)
            {
                Tasks tasks = (Tasks)DataGridTasks.SelectedItem;

                TextBoxTaskName.Text = tasks.Name;
                TextBoxTaskDescription.Text = tasks.Description;

                ComboBoxPriorities.Text = tasks.Priorities?.Name;
                ComboBoxStatuses.Text = tasks.Statuses?.Name;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonFinishTask_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridTasks.SelectedItem != null)
            {
                Projects project = (Projects)DataGridWorkerProjects.SelectedItem;

                Tasks finishedTask = (Tasks)DataGridTasks.SelectedItem;

                finishedTask.StatusId = 1002;

                db.SaveChanges();

                TextBoxTaskName.Clear();
                TextBoxTaskDescription.Clear();

                ComboBoxPriorities.SelectedIndex = -1;
                ComboBoxStatuses.SelectedIndex = -1;

                MessageBox.Show("Задача завершена.");

                try
                {
                    DataGridTasks.ItemsSource = null;
                    DataGridTasks.ItemsSource = db.Tasks
                            .Where(
                                    p => p.ProjectId == project.Id && p.StatusId != 1002
                                    )
                            .Select(t => t)
                            .ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void ComboBoxPriorities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxPriorities.SelectedItem != null && DataGridTasks.SelectedItem != null)
            {
                Tasks tasks = (Tasks)DataGridTasks.SelectedItem;

                tasks.Priorities = (Priorities)ComboBoxPriorities.SelectedItem;

                db.SaveChanges();

                ComboBoxPriorities.Text = tasks.Priorities?.Name;
            }
        }

        private void ComboBoxStatuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxStatuses.SelectedItem != null && DataGridTasks.SelectedItem != null)
            {
                Tasks tasks = (Tasks)DataGridTasks.SelectedItem;

                tasks.Statuses = (Statuses)ComboBoxStatuses.SelectedItem;

                db.SaveChanges();

                ComboBoxStatuses.Text = tasks.Statuses.Id != 1002 ? tasks.Statuses.Name : null;

            }
        }
    }
}
