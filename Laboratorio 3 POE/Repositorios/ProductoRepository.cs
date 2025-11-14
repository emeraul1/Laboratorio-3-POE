using Laboratorio_3_POE.Clases;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Laboratorio_3_POE.Repositorios
{
    public class ProductoRepository
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;

        public ProductoRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public Producto GetById(int id)
        {
            return _connection.Query<Producto>(where: e => e.IdProducto == id, transaction: _transaction).FirstOrDefault();
        }

        public int Insert(Producto producto)
        {
            return (int)_connection.Insert(producto, transaction: _transaction);
        }

        public IEnumerable<Producto> GetAll()
        {
            return _connection.QueryAll<Producto>(transaction: _transaction);
        }

        public int Update(Producto producto, IEnumerable<Field> campos)
        {
            return _connection.Update<Producto>(producto, fields: campos, transaction: _transaction);
        }

        public IEnumerable<Producto> GetByEstado(bool estado)
        {
            return _connection.Query<Producto>(where: e => e.Activo == estado, transaction: _transaction);
        }
    }
}
