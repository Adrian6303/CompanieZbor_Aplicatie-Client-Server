﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CZbor.model;
using CZbor.networking;

namespace CZbor.client
{
    public partial class Buy : Form
    {
        ServerProxy server;
        private Zbor zbor;
        private Angajat angajat;

        public Buy(ServerProxy server,Zbor zbor, Angajat angajat)
        {
            this.server = server;
            this.zbor = zbor;
            this.angajat = angajat;
            InitializeComponent();
            initialize();
        }

        private void initialize()
        {
            nrAltiTuristiNumericUpDown.Value = 0;
            Turist1TextBox.Visible = false;
            Turist2TextBox.Visible = false;
            Turist3TextBox.Visible = false;
            Turist4TextBox.Visible = false;
            Turist5TextBox.Visible = false;
            Turist1Label.Visible = false;
            Turist2Label.Visible = false;
            Turist3Label.Visible = false;
            Turist4Label.Visible = false;
            Turist5Label.Visible = false;
            nrAltiTuristiNumericUpDown.Minimum = 0;
            nrAltiTuristiNumericUpDown.Maximum = 5;
            if (zbor.NoTotalSeats < 5)
            {
                nrAltiTuristiNumericUpDown.Maximum = zbor.NoTotalSeats;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buyTicketsButton_Click(object sender, EventArgs e)
        {
            try
            {
                int nrTuristi = (int)nrAltiTuristiNumericUpDown.Value;
                string numeClient = NumeClientTextBox.Text;
                string adresaClient = AdresaClientTextBox.Text;
                List<Turist> turisti = new List<Turist>();
                Turist client = server.findOrAddTurist(numeClient);
                turisti.Add(client);
                if (nrTuristi >= 1)
                {
                    Turist turist1 = server.findOrAddTurist(Turist1TextBox.Text);
                    turisti.Add(turist1);
                }
                if (nrTuristi >= 2)
                {
                    Turist turist2 = server.findOrAddTurist(Turist2TextBox.Text);
                    turisti.Add(turist2);
                }
                if (nrTuristi >= 3)
                {
                    Turist turist3 = server.findOrAddTurist(Turist3TextBox.Text);
                    turisti.Add(turist3);
                }
                if (nrTuristi >= 4)
                {
                    Turist turist4 = server.findOrAddTurist(Turist4TextBox.Text);
                    turisti.Add(turist4);
                }
                if (nrTuristi == 5)
                {
                    Turist turist5 = server.findOrAddTurist(Turist5TextBox.Text);
                    turisti.Add(turist5);
                }
                int nrLocuri = nrTuristi + 1;
                zbor.NoTotalSeats -= nrLocuri;
                server.UpdateZbor(zbor);
                Bilet bilet = new Bilet(angajat, zbor, client, turisti, adresaClient, nrLocuri);
                server.SaveBilet(bilet);

                MessageBox.Show("Bilet cumparat cu succes!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void nrAltiTuristiNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            int nr = (int)nrAltiTuristiNumericUpDown.Value;

            if (nr == 0)
            {
                Turist1TextBox.Visible = false;
                Turist2TextBox.Visible = false;
                Turist3TextBox.Visible = false;
                Turist4TextBox.Visible = false;
                Turist5TextBox.Visible = false;
                Turist1Label.Visible = false;
                Turist2Label.Visible = false;
                Turist3Label.Visible = false;
                Turist4Label.Visible = false;
                Turist5Label.Visible = false;
            }
            else if (nr == 1)
            {
                Turist1TextBox.Visible = true;
                Turist2TextBox.Visible = false;
                Turist3TextBox.Visible = false;
                Turist4TextBox.Visible = false;
                Turist5TextBox.Visible = false;
                Turist1Label.Visible = true;
                Turist2Label.Visible = false;
                Turist3Label.Visible = false;
                Turist4Label.Visible = false;
                Turist5Label.Visible = false;
            }
            else if (nr == 2)
            {
                Turist1TextBox.Visible = true;
                Turist2TextBox.Visible = true;
                Turist3TextBox.Visible = false;
                Turist4TextBox.Visible = false;
                Turist5TextBox.Visible = false;
                Turist1Label.Visible = true;
                Turist2Label.Visible = true;
                Turist3Label.Visible = false;
                Turist4Label.Visible = false;
                Turist5Label.Visible = false;
            }
            else if (nr == 3)
            {
                Turist1TextBox.Visible = true;
                Turist2TextBox.Visible = true;
                Turist3TextBox.Visible = true;
                Turist4TextBox.Visible = false;
                Turist5TextBox.Visible = false;
                Turist1Label.Visible = true;
                Turist2Label.Visible = true;
                Turist3Label.Visible = true;
                Turist4Label.Visible = false;
                Turist5Label.Visible = false;

            }
            else if (nr == 4)
            {
                Turist1TextBox.Visible = true;
                Turist2TextBox.Visible = true;
                Turist3TextBox.Visible = true;
                Turist4TextBox.Visible = true;
                Turist5TextBox.Visible = false;
                Turist1Label.Visible = true;
                Turist2Label.Visible = true;
                Turist3Label.Visible = true;
                Turist4Label.Visible = true;
                Turist5Label.Visible = false;
            }
            else if (nr == 5)
            {
                Turist1TextBox.Visible = true;
                Turist2TextBox.Visible = true;
                Turist3TextBox.Visible = true;
                Turist4TextBox.Visible = true;
                Turist5TextBox.Visible = true;
                Turist1Label.Visible = true;
                Turist2Label.Visible = true;
                Turist3Label.Visible = true;
                Turist4Label.Visible = true;
                Turist5Label.Visible = true;
            }
        }
    }
}
