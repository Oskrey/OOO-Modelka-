using ООО__Моделька_.Classes;
using ООО__Моделька_.Forms;
using ООО__Моделька_.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ООО__Моделька_
{
    public partial class FormAuth : Form
    {
        public FormAuth()
        {
            InitializeComponent();
        }
        int sec;
        bool vis = true;
        int attempt = 3;
        private void buttonAuth_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;
            using (LegoStoreEntities db = new LegoStoreEntities())
            {
                List<Employee> users = db.Employee.Where(x => x.EmployeeMail == login && x.EmployeePassword == password).ToList();
                Employee user = users.FirstOrDefault();
                if (user != null)
                {
                    Helper.idRole = user.EmployeeRoleID;
                    if (Helper.idRole == 1)
                    {
                        MessageBox.Show("Вы одмен");
                        FormCatalog formCatalog = new FormCatalog();
                        Hide();
                        formCatalog.ShowDialog();
                        Show();
                        textBoxLogin.Text = "";
                        textBoxPassword.Text = "";
                    }
                    else if (Helper.idRole == 2)
                    {
                        FormCatalog formCatalog = new FormCatalog();
                        Hide();
                        formCatalog.ShowDialog();
                        Show();
                        textBoxLogin.Text = "";
                        textBoxPassword.Text = "";
                    }
                }
                else
                {
                    attempt--;
                    textBoxLogin.Text = "";
                    textBoxPassword.Text = "";
                    MessageBox.Show("Неверно введен логин или пароль", "Ошибка авторизации");
                    if (attempt == 0)
                    {
                        sec = 10;
                        timerBlock.Enabled = true;
                        timerBlock.Start();
                        buttonAuth.Visible = false;
                        buttonGuest.Visible = false;
                        textBoxLogin.Text = "";
                        textBoxPassword.Text = "";
                        MessageBox.Show("Система заблокирована на 10 секунд", "Ошибка авторизации");
                    }
                }
            }

        }

        private void buttonGuest_Click(object sender, EventArgs e)
        {
            Helper.idRole = -1;
            FormCatalog formCatalog = new FormCatalog();
            Hide();
            formCatalog.ShowDialog();
            Show();
        }

        private void timerBlock_Tick(object sender, EventArgs e)
        {
            if (sec == 0)
            {
                timerBlock.Enabled = false;
                timerBlock.Stop();
                buttonAuth.Visible = true;
                buttonGuest.Visible = true;
            }
            if (sec > 0)
            {
                sec--;
            }
        }

        private void checkBoxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (vis)
            {
                textBoxPassword.UseSystemPasswordChar = false;
                vis = false;
            }
            else
            {
                textBoxPassword.UseSystemPasswordChar = true;
                vis = true;
            }
        }

        private void clientAuth_Click(object sender, EventArgs e)
        {
            string login = textBoxClientLogin.Text;
            string password = textBoxClientPasswd.Text;
            using (LegoStoreEntities db = new LegoStoreEntities())
            {
                List<Client> clients = db.Client.Where(x => x.ClientMail == login && x.ClientPassord == password).ToList();
                Client client = clients.FirstOrDefault();
                if (client != null)
                {
                    Helper.idRole = 3;
                    FormCatalog formCatalog = new FormCatalog(client.ClientFullName);
                    Hide();
                    formCatalog.ShowDialog();
                    Show();
                    textBoxLogin.Text = "";
                    textBoxPassword.Text = "";

                }
                else
                {
                    attempt--;
                    textBoxLogin.Text = "";
                    textBoxPassword.Text = "";
                    MessageBox.Show("Неверно введен логин или пароль", "Ошибка авторизации");
                    if (attempt == 0)
                    {
                        sec = 10;
                        timerBlock.Enabled = true;
                        timerBlock.Start();
                        buttonAuth.Visible = false;
                        buttonGuest.Visible = false;
                        textBoxLogin.Text = "";
                        textBoxPassword.Text = "";
                        MessageBox.Show("Система заблокирована на 10 секунд", "Ошибка авторизации");
                    }
                }
            }
        }
    }
}
