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

        }

        private void DataGridTasks_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonFinishTask_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxPriorities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxStatuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
