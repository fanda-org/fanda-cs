using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fanda.Shared
{
    public static class WhereAnyExtension
    {
        public static IQueryable<T> WhereAny<T>(
            this IQueryable<T> source,
            params Expression<Func<T, bool>>[] predicates)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (predicates == null)
            {
                throw new ArgumentNullException("predicates");
            }

            if (predicates.Length == 0)
            {
                return source.Where(x => false);    // no matches!
            }

            if (predicates.Length == 1)
            {
                return source.Where(predicates[0]); // simple
            }

            var param = Expression.Parameter(typeof(T));
            Expression body = Expression.Invoke(predicates[0], param);
            for (int i = 1; i < predicates.Length; i++)
            {
                body = Expression.OrElse(body, Expression.Invoke(predicates[i], param));
            }
            var lambda = Expression.Lambda<Func<T, bool>>(body, param);
            return source.Where(lambda);
        }
    }
}
