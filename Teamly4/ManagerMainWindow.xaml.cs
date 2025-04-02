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
        private readonly Users currentUser;
        public ManagerMainWindow()
        {
            
            InitializeComponent();

            try
            {
                db = new IUM2323DTeamlyEntities();
                currentUser = db
                            .Users
                            .FirstOrDefault(u => u.UserName == CurrentUserGlobal.Login && u.Password == CurrentUserGlobal.Password);

                LoadProjectData();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void LoadProjectData()
        {
            try
            {
                DataGridManagerProjects.ItemsSource = db.Projects.Where(x => x.ManagerId == currentUser.Id).ToList();

                ComboBoxPriorities.ItemsSource = db.Priorities.ToList();
                ComboBoxStatuses.ItemsSource = db.Statuses.Where(t => t.Id != 1002).ToList();
                
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

                CurrentProjectManagerGlobal.Name = project.Name;
                CurrentProjectManagerGlobal.Description = project.Description;
                CurrentProjectManagerGlobal.StartDateTime = project.StartTime;
                CurrentProjectManagerGlobal.FinishDateTime = project.FinishTime;

                TextBoxProjectName.Text = CurrentProjectManagerGlobal.Name;
                TextBoxProjectDescription.Text = CurrentProjectManagerGlobal.Description;
                TextBoxStartDateTime.Text = CurrentProjectManagerGlobal.StartDateTime.ToString("yyyy-MM-dd");
                TextBoxFinishDateTime.Text = CurrentProjectManagerGlobal.FinishDateTime.ToString("yyyy-MM-dd");

                CurrentProjectManagerGlobal.PerformersOfProject = db.Performers
                                    .Where(p => p.Projectid == project.Id)
                                    .Join(db.Users,
                                            p => p.Workerid,
                                            u => u.Id,
                                            (p, u) => u
                                            )
                                    .ToList();

                
                CurrentProjectManagerGlobal.TasksNames = db.Tasks
                            .Where(
                                    p => p.ProjectId == project.Id && p.StatusId != 1002
                                    )
                            .Select(t => t)
                            .ToList();

                DataGridPerformers.ItemsSource = null;
                DataGridTasks.ItemsSource = null;

                DataGridPerformers.ItemsSource = CurrentProjectManagerGlobal.PerformersOfProject;
                DataGridTasks.ItemsSource = CurrentProjectManagerGlobal.TasksNames;
            }
        }

        

        private void DataGridTasks_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            
            if (DataGridTasks.SelectedItem != null) 
            { 
                Tasks tasks = (Tasks)DataGridTasks.SelectedItem;

                CurrentTaskManagerGlobal.Name = tasks.Name;
                CurrentTaskManagerGlobal.Performer = tasks.Users;
                CurrentTaskManagerGlobal.Description = tasks.Description;

                TextBoxTaskName.Text = CurrentTaskManagerGlobal.Name;
                TextBoxPerformer.Text = CurrentTaskManagerGlobal.Performer.UserName;
                TextBoxTaskDescription.Text = CurrentTaskManagerGlobal.Description;

                ComboBoxPriorities.Text = tasks.Priorities?.Name;
                ComboBoxStatuses.Text = tasks.Statuses?.Name;
            }
        }

        

        private void ButtonDellProject_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridManagerProjects.SelectedItem != null)
            {
                Projects project = (Projects)DataGridManagerProjects.SelectedItem;

                Tasks delTasks = (Tasks)db.Tasks.Where(t => t.ProjectId == project.Id);
                Performers delPerformers = (Performers)db.Performers.Where(t => t.Projectid == project.Id);
                UsersProjects delUsersInProject = (UsersProjects)db.UsersProjects.Where(t => t.ProjectId == project.Id);

                db.Projects.Remove(project);
                db.Tasks.Remove(delTasks);
                db.Performers.Remove(delPerformers);
                db.UsersProjects.Remove(delUsersInProject);

                db.SaveChanges();

                if (DataGridTasks.SelectedItem != null)
                {
                    TextBoxTaskName.Clear();
                    TextBoxTaskDescription.Clear();
                    TextBoxPerformer.Clear();

                    ComboBoxPriorities.SelectedIndex = -1;
                    ComboBoxStatuses.SelectedIndex = -1;
                }

                TextBoxProjectName.Clear();
                TextBoxProjectDescription.Clear();

                DataGridPerformers.ItemsSource = null;
                DataGridTasks.ItemsSource = null;

                MessageBox.Show("Проект удален.");

                try
                {
                    DataGridManagerProjects.ItemsSource = null;
                    DataGridManagerProjects.ItemsSource = db.Projects.Where(x => x.ManagerId == currentUser.Id).ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void ButtonDelTask_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridTasks.SelectedItem != null)
            {
                Projects project = (Projects)DataGridManagerProjects.SelectedItem;

                Tasks delTask = (Tasks)DataGridTasks.SelectedItem;
                
                db.Tasks.Remove(delTask);

                db.SaveChanges();

                TextBoxTaskName.Clear();
                TextBoxTaskDescription.Clear();
                TextBoxPerformer.Clear();

                ComboBoxPriorities.SelectedIndex = -1;
                ComboBoxStatuses.SelectedIndex = -1;

                try
                {
                    DataGridTasks.ItemsSource = null;
                    DataGridTasks.ItemsSource = db.Tasks
                            .Where(
                                    p => p.ProjectId == project.Id && p.StatusId != 1002
                                    )
                            .Select(t => t)
                            .ToList();
                    MessageBox.Show("Задача удалена.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void ButtonFinishTask_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridTasks.SelectedItem != null)
            {
                Projects project = (Projects)DataGridManagerProjects.SelectedItem;

                Tasks finishedTask = (Tasks)DataGridTasks.SelectedItem;

                finishedTask.StatusId = 1002;

                db.SaveChanges();

                TextBoxTaskName.Clear();
                TextBoxTaskDescription.Clear();
                TextBoxPerformer.Clear();

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
        private void ButtonAddProject_Click(object sender, RoutedEventArgs e)
        {   
            var addProjectWindow = new AddProjectWindow();

            if (addProjectWindow.ShowDialog() == true)
            {
                Projects newProject = new Projects()
                {
                    Name = addProjectWindow.Name,
                    Description = addProjectWindow.Description,
                    StartTime = DateTime.Now,
                    FinishTime = addProjectWindow.FinishDate,
                    ManagerId = currentUser.Id,
                };

                db.Projects.Add(newProject);
                db.SaveChanges();

                UsersProjects newManagerUserProject = new UsersProjects()
                {
                    ProjectId = newProject.Id,
                    UserId = currentUser.Id,
                };
                db.UsersProjects.Add(newManagerUserProject);
                db.SaveChanges();

                foreach (Users user in addProjectWindow.ListBoxUsers.SelectedItems)
                {
                    UsersProjects newUsersProjects = new UsersProjects()
                    {
                        ProjectId = newProject.Id,
                        UserId = user.Id,
                    };
                    Performers performers = new Performers()
                    {
                        Projectid = newProject.Id,
                        Workerid = user.Id,
                    };
                    db.UsersProjects.Add(newUsersProjects);
                    db.Performers.Add(performers);
                }
                db.SaveChanges();

                LoadProjectData();
            }
            else
            {
                MessageBox.Show("Проект не добавлен");
            }       
        }

        private void ByttonAddTask_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridManagerProjects.SelectedItem != null)
            {
                Projects project = (Projects)DataGridManagerProjects.SelectedItem;

                var addTaskWindow = new AddTaskWindow();

                if (addTaskWindow.ShowDialog() == true)
                {
                    Tasks newTask = new Tasks()
                    {
                        Name = addTaskWindow.Name,
                        Description = addTaskWindow.Description,
                        StatusId = addTaskWindow.status.Id,
                        PriorityId = addTaskWindow.priority.Id,
                        CreatedAt = DateTime.Now,
                        ProjectId = project.Id,
                        WorkerId = addTaskWindow.Performer.Id,
                        ManagerId = currentUser.Id,
                    };

                    try
                    {
                        db.Tasks.Add(newTask);
                        db.SaveChanges();

                        DataGridTasks.ItemsSource = null;
                        DataGridTasks.ItemsSource = db.Tasks
                                .Where(
                                        p => p.ProjectId == project.Id && p.StatusId != 1002
                                        )
                                .Select(t => t)
                                .ToList();
                        MessageBox.Show("Задача добавлена");
                    }
                    catch (Exception ex) 
                    { 
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Задача не добавдена");
                }
            }
        }

        private void ButtonChangeTask_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridTasks.SelectedItem != null)
            {
                Tasks currentTask = (Tasks)DataGridTasks.SelectedItem;
                Projects project = (Projects)DataGridManagerProjects.SelectedItem;

                var changeTaskWindow = new ChangeTaskWindow();

                if(changeTaskWindow.ShowDialog() == true)
                {
                    currentTask.Name = changeTaskWindow.Name;
                    currentTask.Description = changeTaskWindow.Description;
                    currentTask.WorkerId = changeTaskWindow.PerformerOfTask.Id;

                    db.SaveChanges();

                    try
                    {
                        DataGridTasks.ItemsSource = null;

                        TextBoxTaskName.Text = null;
                        TextBoxTaskDescription.Text = null;
                        TextBoxPerformer.Text = null;

                        TextBoxTaskName.Text = currentTask.Name;
                        TextBoxTaskDescription.Text = currentTask.Description;
                        TextBoxPerformer.Text = currentTask.Users.UserName;

                        DataGridTasks.ItemsSource = db.Tasks
                            .Where(
                                    p => p.ProjectId == project.Id && p.StatusId != 1002
                                    )
                            .Select(t => t)
                            .ToList();
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show("Ошибка: " + ex);
                    }

                } 
                else
                {
                    MessageBox.Show("Данные не сохранены");
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите задачу");
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
