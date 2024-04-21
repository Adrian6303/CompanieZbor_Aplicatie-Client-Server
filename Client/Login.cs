using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CZbor.model;
using CZbor.service;
using CZbor.networking;

namespace CZbor.client
{
    public partial class Login : Form, IObserver
    {
        private ServerProxy server;

        public Login(ServerProxy ctrl)
        {
            
            this.server = ctrl;
            InitializeComponent();

        }

        public void updateZbor()
        {
            throw new NotImplementedException();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string user = userTextBox.Text;
            string password = passwordTextBox.Text;
            Angajat angajat = server.FindAngajat(user, password, this);
            if (angajat != null)
            {
                Search search = new Search(server, angajat);
                search.Text = "Search flight for: " + user;
                search.Show();
                userTextBox.Clear();
                passwordTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Invalid username or password");
            }
        }
        

    }
        
}
