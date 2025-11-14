using Laboratorio_3_POE.UoW;
using RepoDb;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratorio_3_POE.Forms
{
    public partial class frmRegistrarProductoCmd : Form
    {
        private readonly string _connectionString;
        private readonly int _productoId;


        public frmRegistrarProductoCmd(int productoId, string ConnectionString)
        {
            InitializeComponent();
            _productoId = productoId;
            _connectionString = ConnectionString;
        }

        private void frmRegistrarProductoCmd_Load(object sender, EventArgs e)
        {
            chbActivo.Checked = true;

            using (var uow = new UnitOfWork(_connectionString))
            {
                var producto = uow.Producto.GetById(_productoId);

                if (producto == null)
                {
                    MessageBox.Show("Producto no encontrado.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                txtNombre.Text = producto.Nombre;
                txtCodigo.Text = producto.Codigo;
                txtCategoria.Text = producto.Categoria;
                txtPrecioUnitario.Text = producto.PrecioUnitario.ToString();
                txtCantidad.Text = producto.Cantidad.ToString();
                txtStockMinimo.Text = producto.StockMinimo.ToString();
                chbActivo.Checked = producto.Activo;

            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtCodigo.Text) || string.IsNullOrWhiteSpace(txtCategoria.Text) ||
              string.IsNullOrWhiteSpace(txtPrecioUnitario.Text) || string.IsNullOrWhiteSpace(txtCantidad.Text) || string.IsNullOrWhiteSpace(txtStockMinimo.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            var productoActualizado = new Clases.Producto
            {
                IdProducto = _productoId,
                Nombre = txtNombre.Text.Trim(),
                Codigo = txtCodigo.Text.Trim(),
                Categoria = txtCategoria.Text.Trim(),
                PrecioUnitario = decimal.Parse(txtPrecioUnitario.Text.Trim()),
                Cantidad = int.Parse(txtCantidad.Text.Trim()),
                StockMinimo = int.Parse(txtStockMinimo.Text.Trim()),
                Activo = chbActivo.Checked
            };

            var camposActualizar = new List<Field>
            {
                new Field("Nombre"),
                new Field("Codigo"),
                new Field("Categoria"),
                new Field("PrecioUnitario"),
                new Field("Cantidad"),
                new Field("StockMinimo"),
                new Field("Activo")
            };

            using (var uow = new UnitOfWork(_connectionString))
            {
                uow.Producto.Update(productoActualizado, camposActualizar);
                uow.Commit();
                MessageBox.Show("Producto actualizado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void txtCategoria_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Validar si solo es letra 
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
    }
}
