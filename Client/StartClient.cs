using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZbor.networking;
using Hashtable = System.Collections.Hashtable;
using CZbor.service;
using System.Windows.Forms;

namespace CZbor.client
{
    static class StartClient
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ServerProxy server = new ServerProxy("127.0.0.1", 55556);
            Login win = new Login(server);
            Application.Run(win);
        }
    }
}
