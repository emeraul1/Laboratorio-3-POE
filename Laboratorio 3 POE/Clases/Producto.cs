using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio_3_POE.Clases
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Categoria { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public int StockMinimo { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
