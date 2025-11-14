using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoDb;
using Microsoft.Data.SqlClient;
using Laboratorio_3_POE.Repositorios;

namespace Laboratorio_3_POE.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;
        private UsuarioRepository _usuarioRepo;
        private ProductoRepository _productoRepo;

        public UnitOfWork(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public UsuarioRepository Usuario
        {
            get
            {
                if (_usuarioRepo == null)
                {
                    _usuarioRepo = new UsuarioRepository(_connection, _transaction);
                }
                return _usuarioRepo;
            }
        }

        public ProductoRepository Producto  
        {
            get
            {
                if (_productoRepo == null)
                {
                    _productoRepo = new ProductoRepository(_connection, _transaction);
                }
                return _productoRepo;
            }
        }
        public void Commit()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
            _transaction = null;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}
