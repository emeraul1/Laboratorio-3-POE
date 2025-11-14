using Laboratorio_3_POE.Forms;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
//Emerson Raúl Ventura Castillo U20241017 
namespace Laboratorio_3_POE
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SqlServerBootstrap.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmLogin login = new frmLogin();

            if (login.ShowDialog() == DialogResult.OK)
            {

                Application.Run(new frmRegistrarProducto());
            }
        }
    }
}
