using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace PunyUrl.DataAccess.Azure
{
    public interface ITableStorageQuery<T>
    {
        ITableStorageQuery<T> Where(Expression<Func<T, bool>> predicate);
        ITableStorageQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector);
        ITableStorageQuery<T> Take(int take);

        Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<T> SingleOrDefaultAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<List<T>> ToListAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<T[]> ToArrayAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}