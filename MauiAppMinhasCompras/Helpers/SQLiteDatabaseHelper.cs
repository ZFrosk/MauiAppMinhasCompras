using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        // INSERIR PRODUTO | INSERT A PRODUCT | INSERTAR PRODUCTO
        public Task<int> Insert(Produto p) 
        {
            return _conn.InsertAsync(p);
        }

        // ATUALIZAR PRODUTO | UPDATE A PRODUCT | ACTUALIZAR PRODUCTO
        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao = ?, Quantidade = ?, Preco = ? WHERE Id = ?";

            return _conn.QueryAsync<Produto>(sql, p.Descricao, p.Quantidade, p.Preco, p.Id);
        }

        // DELETAR PRODUTO | DELETE A PRODUCT | ELIMINAR PRODUCTO
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        // LISTAR TODOS OS PRODUTOS | LIST ALL PRODUCTS | LISTAR TODOS LOS PRODUCTOS
        public Task<List<Produto>> getAll() 
        { 
            return _conn.Table<Produto>().ToListAsync();
        }

        // PESQUISAR PRODUTO | SEARCH A PRODUCT | BUSCAR PRODUCTO
        public Task<List<Produto>> Search(string q) 
        {
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE '%" + q + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }

   
    }
}
