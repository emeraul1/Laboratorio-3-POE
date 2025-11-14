using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Laboratorio_3_POE
{
    public partial class frmLogin : Form
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["conexionBD"].ConnectionString;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtClave.UseSystemPasswordChar = true; 
            txtClave.CharacterCasing = CharacterCasing.Lower; 
            txtCodigoUsuario.CharacterCasing = CharacterCasing.Upper; 
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtClave.Text) || string.IsNullOrWhiteSpace(txtCodigoUsuario.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos, usuario y contraseña.");
            }

            //Buscar en bd

            using (var uow = new UoW.UnitOfWork(_connectionString))
            {
                string codigo = txtCodigoUsuario.Text.Trim();
                var clave = txtClave.Text.Trim();
                var claveEncriptada = Encriptador.EncriptarClave(clave);

                var usuario = uow.Usuario.ObtenerPorCredenciales(codigo, claveEncriptada);

                if (usuario != null)
                {
                    //usuario encontrado
                    MessageBox.Show($"Bienvenido {usuario.Nombre} {usuario.Apellido} ", "Acceso permitido.",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Credenciales incorrectas. Intente nuevamente.", "Error de acceso",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
