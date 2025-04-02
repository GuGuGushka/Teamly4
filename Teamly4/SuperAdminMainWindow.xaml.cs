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
    /// Логика взаимодействия для SuperAdminMainWindow.xaml
    /// </summary>
    public partial class SuperAdminMainWindow : Window
    {
        private readonly IUM2323DTeamlyEntities db;

        public SuperAdminMainWindow()
        {
            InitializeComponent();
            try
            {
                db = new IUM2323DTeamlyEntities();
                DataGridEmploees.ItemsSource = db.Users.ToList();
                ComboBoxRoles.ItemsSource = db.Roles.ToList();
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

        private void DataGridEmploees_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if(DataGridEmploees.SelectedItem != null)
            {
                Users user = (Users)DataGridEmploees.SelectedItem;
                

                TextBoxId.Text = user.Id.ToString();
                TextBoxUsername.Text = user.UserName;
                TextBoxFirstName.Text = user.FirstName;
                TextBoxSecondName.Text = user.SecondName;
                TextBoxPassword.Text = user.Password;
                ComboBoxRoles.Text = user.Roles?.Name;
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if(TextBoxUsername.Text.Length == 0)
            {
                MessageBox.Show("Ошибка. Имя должно быть заполнено.");
                return;
            }

            if (TextBoxFirstName.Text.Length == 0)
            {
                MessageBox.Show("Ошибка. Имя должно быть заполнено.");
                return;
            }

            if (TextBoxSecondName.Text.Length == 0)
            {
                MessageBox.Show("Ошибка. Имя должно быть заполнено.");
                return;
            }

            if (TextBoxPassword.Text.Length == 0)
            {
                MessageBox.Show("Ошибка. Имя должно быть заполнено.");
                return;
            }

            Users newUser = new Users()
            {
                UserName = TextBoxUsername.Text,
                Password = TextBoxPassword.Text,
                Roles = (Roles)ComboBoxRoles.SelectedItem,
                FirstName = TextBoxFirstName.Text,
                SecondName = TextBoxSecondName.Text,
            }; 
            
            try
            {
                db.Users.Add(newUser);
                db.SaveChanges();

                TextBoxUsername.Clear();
                TextBoxFirstName.Clear();
                TextBoxSecondName.Clear();
                TextBoxPassword.Clear();
                ComboBoxRoles.SelectedIndex = -1;

                DataGridEmploees.ItemsSource = null;
                DataGridEmploees.ItemsSource = db.Users.ToList();

                ComboBoxRoles.ItemsSource = db.Roles.ToList();

                MessageBox.Show("Успех, сотрудник добавлен.");
            }
            catch
            {
                MessageBox.Show("Ошибка, не удалось добавить сотрудника в БД.");
            }
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {

            if (TextBoxUsername.Text.Length == 0)
            {
                MessageBox.Show("Ошибка. Имя должно быть заполнено.");
                return;
            }

            if (TextBoxFirstName.Text.Length == 0)
            {
                MessageBox.Show("Ошибка. Имя должно быть заполнено.");
                return;
            }

            if (TextBoxSecondName.Text.Length == 0)
            {
                MessageBox.Show("Ошибка. Имя должно быть заполнено.");
                return;
            }

            if (TextBoxPassword.Text.Length == 0)
            {
                MessageBox.Show("Ошибка. Имя должно быть заполнено.");
                return;
            }

            int updatedId = int.Parse(TextBoxId.Text);

            Users updatedUser = db.Users.FirstOrDefault(user => user.Id == updatedId);

            updatedUser.UserName = TextBoxUsername.Text;
            updatedUser.Password = TextBoxPassword.Text;
            updatedUser.Roles = (Roles)ComboBoxRoles.SelectedItem;
            updatedUser.FirstName = TextBoxFirstName.Text;
            updatedUser.SecondName = TextBoxSecondName.Text;

            db.SaveChanges();

            try
            {
                TextBoxUsername.Clear();
                TextBoxFirstName.Clear();
                TextBoxSecondName.Clear();
                TextBoxPassword.Clear();
                ComboBoxRoles.SelectedIndex = -1;

                DataGridEmploees.ItemsSource = null;
                DataGridEmploees.ItemsSource = db.Users.ToList();

                ComboBoxRoles.ItemsSource = db.Roles.ToList();

                MessageBox.Show("Успех, сотрудник обновлен.");
            }
            catch
            {
                MessageBox.Show("Ошибка, не удалось добавить сотрудника в БД.");
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            Users user = (Users)DataGridEmploees.SelectedItem;
            db.Users.Remove(user);

            db.SaveChanges();
            try
            {
                DataGridEmploees.ItemsSource = null;
                DataGridEmploees.ItemsSource = db.Users.ToList();
            }
            catch (Exception ex) 
            { 
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            TextBoxId.Clear();
            TextBoxUsername.Clear();
            TextBoxFirstName.Clear();
            TextBoxSecondName.Clear();
            TextBoxPassword.Clear();
            ComboBoxRoles.SelectedIndex = -1;

            DataGridEmploees.ItemsSource = null;
            DataGridEmploees.ItemsSource= db.Users.ToList();

            MessageBox.Show("Успех, сотрудник удален.");
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            TextBoxId.Clear();
            TextBoxUsername.Clear();
            TextBoxFirstName.Clear();
            TextBoxSecondName.Clear();
            TextBoxPassword.Clear();
            ComboBoxRoles.SelectedIndex = -1;
        }
    }
}
