using Laboratorio_3_POE.UoW;
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
using Laboratorio_3_POE.Clases;

namespace Laboratorio_3_POE.Forms
{
    public partial class frmRegistrarProducto : Form
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["conexionBD"].ConnectionString;
        public frmRegistrarProducto()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void frmRegistrarProducto_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }


        //metodo para cargar datos en data grid view
        private void CargarProductos()
        {
            using (var uow = new UnitOfWork (_connectionString))
            {
                var lista = uow.Producto.GetByEstado(true).ToList();
                dgvRegistro.DataSource = lista;
                dgvRegistro.Columns["IdProducto"].Visible = false;
                dgvRegistro.Columns["Activo"].Visible = false;
                dgvRegistro.Refresh();

            }
        }

        //metodo para limpiar campos
        private void LimpiarControles()
        {
            txtNombre.Clear();
            txtCodigo.Clear();
            txtCategoria.Clear();
            txtPrecioUnitario.Clear();
            txtCantidad.Clear();
            txtStockMinimo.Clear();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            

            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtCodigo.Text) || string.IsNullOrWhiteSpace(txtCategoria.Text) ||
                string.IsNullOrWhiteSpace(txtPrecioUnitario.Text) || string.IsNullOrWhiteSpace(txtCantidad.Text) || string.IsNullOrWhiteSpace(txtStockMinimo.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            var producto = new Producto 
            {
                Nombre = txtNombre.Text.Trim(),
                Codigo = txtCodigo.Text.Trim(),
                Categoria = txtCategoria.Text.Trim(),
                PrecioUnitario = decimal.Parse(txtPrecioUnitario.Text.Trim()),
                Cantidad = int.Parse(txtCantidad.Text.Trim()),
                StockMinimo = int.Parse(txtStockMinimo.Text.Trim()),
                Activo = true,
                FechaRegistro = DateTime.Now

            };

            using (var uow = new UnitOfWork (_connectionString))
            {
                uow.Producto.Insert(producto);
                uow.Commit();
            }

            MessageBox.Show("Producto guardado correctamente.");
            LimpiarControles();
            CargarProductos();
        }

        private void dgvRegistro_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dgvRegistro_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var producto = (Producto)dgvRegistro.Rows[e.RowIndex].DataBoundItem;

                using (var frmEditarProducto = new frmRegistrarProductoCmd (producto.IdProducto, _connectionString))
                {
                    var result = frmEditarProducto.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        CargarProductos();
                    }
                       
                }
            }
        }

        private void txtCategoria_KeyPress(object sender, KeyPressEventArgs e)
        {
            //VAlidar si solo es letra 
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten letras", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtPrecioUnitario_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos y un punto decimal
            TextBox txt = sender as TextBox;
            if (char.IsControl(e.KeyChar))
                return;
            if (char.IsDigit(e.KeyChar))
                return;

            // Permitir un solo punto decimal
            if (e.KeyChar == '.' && !txt.Text.Contains("."))
                return;

            // Bloquear todo lo demás
            e.Handled = true;
            MessageBox.Show("Solo se permiten números y un punto decimal", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            //Validar que solo sea número
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Bloquea cualquier otra tecla
                MessageBox.Show("Solo se permiten números", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            SoloNumeros(sender, e);
        }

        private void txtStockMinimo_KeyPress(object sender, KeyPressEventArgs e)
        {
            SoloNumeros(sender, e);
        }

        private void dgvRegistro_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvRegistro.Columns[e.ColumnIndex].Name == "Cantidad")
            {
                int cantidad = Convert.ToInt32(dgvRegistro.Rows[e.RowIndex].Cells["Cantidad"].Value);
                int stockMinimo = Convert.ToInt32(dgvRegistro.Rows[e.RowIndex].Cells["StockMinimo"].Value);

                if (cantidad <= stockMinimo)
                {
                    dgvRegistro.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                }
                else
                {
                    dgvRegistro.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }
    }
}
