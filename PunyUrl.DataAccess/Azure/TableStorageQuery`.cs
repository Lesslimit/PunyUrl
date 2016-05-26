using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;

namespace PunyUrl.DataAccess.Azure
{
    public class TableStorageQuery<T> : ITableStorageQuery<T>
    {
        private IQueryable<T> queryable;
        private readonly TableQuery<T> tableQuery;

        private IQueryable<T> Queryable
        {
            get { return queryable ?? tableQuery.AsQueryable(); }
            set { queryable = value; }
        }

        public TableStorageQuery(TableQuery<T> tableQuery)
        {
            this.tableQuery = tableQuery;
        }

        public ITableStorageQuery<T> Where(Expression<Func<T, bool>> predicate)
        {
            Queryable = tableQuery.Where(predicate);

            return this;
        }

        public ITableStorageQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            Queryable = tableQuery.OrderByDescending(keySelector);

            return this;
        }

        public ITableStorageQuery<T> Take(int take)
        {
            Queryable = Queryable.Take(take);

            return this;
        }

        public async Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var token = new TableContinuationToken();

            var segmentResult = await Queryable.AsTableQuery()
                                               .ExecuteSegmentedAsync(token, cancellationToken)
                                               .ConfigureAwait(false);

            return segmentResult.Results.FirstOrDefault();
        }

        public async Task<T> SingleOrDefaultAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var token = new TableContinuationToken();

            var segmentResult = await Queryable.AsTableQuery()
                                               .ExecuteSegmentedAsync(token, cancellationToken)
                                               .ConfigureAwait(false);

            return segmentResult.Results.SingleOrDefault();
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var token = new TableContinuationToken();
            var query = Queryable.AsTableQuery();

            query.SelectColumns = new List<string> { "RowKey" };

            var segmentResult = await query.ExecuteSegmentedAsync(token, cancellationToken)
                                           .ConfigureAwait(false);

            return segmentResult.Results.Count;
        }

        public async Task<List<T>> ToListAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var results = await ExecuteSegmentedAsync(cancellationToken).ConfigureAwait(false);

            return results.ToList();
        }

        public async Task<T[]> ToArrayAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var results = await ExecuteSegmentedAsync(cancellationToken).ConfigureAwait(false);

            return results.ToArray();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return tableQuery.GetEnumerator();
        }

        private async Task<List<T>> ExecuteSegmentedAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var results = new List<T>();
            var storageQuery = Queryable.AsTableQuery();
            var continuationToken = new TableContinuationToken();

            do
            {
                var segment = await storageQuery.ExecuteSegmentedAsync(continuationToken, cancellationToken).ConfigureAwait(false);

                continuationToken = segment.ContinuationToken;

                results.AddRange(segment.Results);
            }
            while (continuationToken != null && !cancellationToken.IsCancellationRequested && results.Count < (storageQuery.TakeCount ?? int.MaxValue));

            return results;
        }
    }
}