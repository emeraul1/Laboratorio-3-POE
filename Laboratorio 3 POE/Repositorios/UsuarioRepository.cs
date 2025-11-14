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
    public class UsuarioRepository
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;

        public UsuarioRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public Usuario GetById(int id)
        {
            return _connection.Query<Usuario>(where: e => e.IdUsuario == id, transaction: _transaction).FirstOrDefault();
        }

        public int Insert(Usuario usuario)
        {
            return (int)_connection.Insert(usuario, transaction: _transaction);
        }

        public IEnumerable<Usuario> GetAll()
        {
            return _connection.QueryAll<Usuario>(transaction: _transaction);
        }

        public int Update(Usuario usuario, IEnumerable<Field> campos)
        {
            return _connection.Update<Usuario>(usuario, fields: campos, transaction: _transaction);
        }

        public IEnumerable<Usuario> GetByEstado(bool estado)
        {
            return _connection.Query<Usuario>(where: e => e.Activo == estado, transaction: _transaction);
        }

        //metodo poara validar usuario sea correcto
        public Usuario ObtenerPorCredenciales(string codigo, string claveEncriptada)
        {
            return _connection.Query<Usuario>(
                where: u => u.CodigoUsuario == codigo && u.Clave == claveEncriptada,
                transaction: _transaction).FirstOrDefault();
        }
    }
}
