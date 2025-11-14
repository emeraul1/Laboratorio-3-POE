using Laboratorio_3_POE.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio_3_POE.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        UsuarioRepository Usuario { get; }
        ProductoRepository Producto { get; }
        void Commit();
    }
}
