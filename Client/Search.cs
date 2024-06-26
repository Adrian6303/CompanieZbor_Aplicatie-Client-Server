﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CZbor.model;
using CZbor.networking;
using CZbor.service;

namespace CZbor.client
{
    [Serializable]
    public partial class Search : Form ,IObserver
    {
        ServerProxy server;
        Angajat angajat;
        public Search(ServerProxy server, Angajat angajat)
        {
            this.server = server;
            this.angajat = angajat;
            InitializeComponent();
            this.server.SetObsForm(this.angajat,this);
            FillDataGrid();

        }

        private void FillDataGrid()
        {
            try
            {
                IEnumerable<Zbor> zboruri = server.FindAllAvailableFlights();

                zboruriDataGridView.DataSource = zboruri;
                List<string> destinatii = new List<string>();
                foreach (Zbor zbor in zboruri)
                {
                    if (!destinatii.Contains(zbor.Destination))
                    {
                        destinatii.Add(zbor.Destination);
                    }
                }
                destinationComboBox.DataSource = destinatii;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                string destinatie = destinationComboBox.Text;
                DateTime date = Convert.ToDateTime(dataPlecareDateTimePicker.Text);
                List<Zbor> zboruri = server.FindZborByDestinatieAndDate(destinatie, date).ToList();

                zboruriDataGridView.DataSource = zboruri;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buyButton_Click(object sender, EventArgs e)
        {
            try
            {
                Zbor zbor = null;
                if (zboruriDataGridView.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = zboruriDataGridView.SelectedRows[0];
                    zbor = row.DataBoundItem as Zbor;

                    Buy buy = new Buy(server, zbor, angajat);
                    buy.Text = "Buy flight to " + zbor.Destination + " for " + angajat.Username;
                    buy.Show();
                }
                else
                {
                    MessageBox.Show("Please select a flight");
                }

     
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            server.Logout(angajat);
            this.Close();
        }

        public void updateZbor()
        {
            BeginInvoke((MethodInvoker)delegate
            {
                FillDataGrid();
            });
        }
    }
}
