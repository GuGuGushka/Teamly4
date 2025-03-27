using System;
using System.Collections.Generic;
using System.Data.Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Teamly4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IUM2323DTeamlyEntities db;
        
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                db = new IUM2323DTeamlyEntities();
            } 
            catch(Exception e) 
            {
                MessageBox.Show(e.Message);
            }
        }

        

        private void ButtonAuth_Click(object sender, RoutedEventArgs e)
        {
            CurrentUserGlobal.Login = TextBoxLogin.Text;
            CurrentUserGlobal.Password= TextBoxPassword.Text;
            Roles roles = null;

            if (CurrentUserGlobal.Login.Length != 0 && CurrentUserGlobal.Password.Length != 0)
            {
                try
                {
                    roles = db
                            .Users
                            .FirstOrDefault(u => u.UserName == CurrentUserGlobal.Login && u.Password == CurrentUserGlobal.Password)
                            .Roles;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                if (roles == null)
                {
                        TextBoxLogin.Clear();
                        TextBoxPassword.Clear();
                        MessageBox.Show("Ошибка, пользователь не найден.");
                }

                    switch (roles.Name)
                    {

                        case "Superadmin":

                            this.Hide();
                            new SuperAdminMainWindow().ShowDialog();

                            break;
                        case "Manager":

                            this.Hide();
                            new ManagerMainWindow().ShowDialog();

                            break;
                        case "Worker":

                            this.Hide();
                            new WorkerMainWindow().ShowDialog();

                            break;
                    }
                
            }
            else
            {
                MessageBox.Show("Ошибка, данные не заполнены.");
            }

        }
    }
}
