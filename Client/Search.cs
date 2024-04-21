using System;
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
        ServerProxy ctrl;
        //ZboruriCtrl ctrl;
        Angajat angajat;
        public Search(ServerProxy ctrl, Angajat angajat)
        {
            this.ctrl = ctrl;
            this.angajat = angajat;
            InitializeComponent();
            this.ctrl.SetObsForm(this.angajat,this);
            FillDataGrid();

        }


        private void FillDataGrid()
        {

            try
            {
                IEnumerable<Zbor> zboruri = ctrl.FindAllAvailableFlights();

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
                List<Zbor> zboruri = ctrl.FindZborByDestinatieAndDate(destinatie, date).ToList();

                zboruriDataGridView.DataSource = zboruri;
                //zboruriDataGridView.Columns["Id"].Visible = false;

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



                int id = Int32.Parse(zboruriDataGridView.SelectedRows[0].Cells["Id"].Value.ToString());

                Zbor zbor = ctrl.FindZborById(id);

                Buy buy = new Buy(ctrl, zbor, angajat);
                buy.Text = "Buy flight to " + zbor.Destination + " for " + angajat.Username;
                buy.Show();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            ctrl.Logout(angajat);
            this.Close();
        }

        public void updateDataGrid()
        {
           
            List<Zbor>zb=ctrl.FindAllAvailableFlights().ToList();
            
            zboruriDataGridView.DataSource = zb;

        }

        public delegate void UpdateZborResponse();

        public void updateZbor()
        {

            zboruriDataGridView.BeginInvoke(new UpdateZborResponse(updateDataGrid), new object[] { });
        }
    }
}
