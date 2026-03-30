using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umfgcloud.infraestrutura.service.Context;
using umfgcloud.loja.dominio.service.Entidades;
using umfgcloud.loja.dominio.service.Interfaces.Repositorios;

namespace umfgcloud.infraestrutura.service.Classes
{
    public abstract class AbstractRepositorio<T> : IAbstractRepositorio<T> where T : AbstractEntity
    {
        private readonly MySqlDataBaseContext _context;

        // basicamente ele aponta qual tabela você quer carregar do database x classe que ela pertence

        public DbSet<T> Entity => _context.Set<T>();

        // injeção de dependência
        protected AbstractRepositorio(MySqlDataBaseContext context)
            => _context = context ?? throw new ArgumentNullException(nameof(context));

        // retorna todos os dados da tabela, se não possuir retorna uma lista vazia
        public virtual async Task<ICollection<T>> ObterTodosAsync() 
            => await Entity.Where(x => x.IsActive).ToListAsync() ?? [];

        // procura um registro pelo id e se está ativo
        // se não gera uma excessão dizendo que o registro não foi encontrado
        public virtual async Task<T> ObterPorIdAsync(Guid id)
            => await Entity.FirstOrDefaultAsync(x => x.Id == id && x.IsActive) ?? throw new ApplicationException($"Registro nao encontrado | {this.GetType()}");

        public async Task AdicionarAsync(T entity)
        {
            await Entity.AddAsync(entity);
            await _context.SaveChangesAsync(); //commita os dados no database
        }

        public async Task RemoverAsync(T entity)
        {
            Entity.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(T entity)
        {
            Entity.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
