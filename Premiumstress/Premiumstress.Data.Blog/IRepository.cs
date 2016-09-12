using System;
using System.Linq;
using System.Linq.Expressions;

namespace Premiumstress.Data.Blog
{
    public interface IRepository<T>
    {
        /// <summary>
        ///     Returns the queryable entity set for the given type {T}.
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        ///     Returns an untracked queryable entity set for the given type {T}.
        ///     The entities returned will not be cached in the object context thus increasing performance.
        /// </summary>
        IQueryable<T> TableUntracked { get; }


        /// <summary>
        ///     Gets or sets a value indicating whether database write operations
        ///     such as insert, delete or update should be committed immediately.
        /// </summary>
        /// <remarks>
        ///     Set this to <c>true</c> or <c>false</c> to supersede the global <c>AutoCommitEnabled</c>
        ///     on <see cref="IDbContext" /> level for this repository instance only.
        /// </remarks>
        bool? AutoCommitEnabled { get; set; }

        /// <summary>
        ///     Creates a new instance of an entity of type {T}
        /// </summary>
        /// <returns>The new entity instance.</returns>
        T Create();

        /// <summary>
        ///     Gets an entity by id from the database or the local change tracker.
        /// </summary>
        /// <param name="id">The id of the entity. This can also be a composite key.</param>
        /// <returns>The resolved entity</returns>
        T GetById(object id);

        /// <summary>
        ///     Attaches an entity to the context
        /// </summary>
        /// <param name="entity">The entity to attach</param>
        /// <returns>The entity</returns>
        T Attach(T entity);

        /// <summary>
        ///     Marks the entity instance to be saved to the store.
        /// </summary>
        /// <param name="entity">An entity instance that should be saved to the database.</param>
        /// <remarks>Implementors should delegate this to the current <see cref="IDbContext" /></remarks>
        bool Insert(T entity);

        /// <summary>
        ///     Marks the changes of an existing entity to be saved to the store.
        /// </summary>
        /// <param name="entity">An instance that should be updated in the database.</param>
        /// <remarks>Implementors should delegate this to the current <see cref="IDbContext" /></remarks>
        bool Update(T entity);


        /// <summary>
        ///     Marks an existing entity to be deleted from the store.
        /// </summary>
        /// <param name="entity">An entity instance that should be deleted from the database.</param>
        /// <remarks>Implementors should delegate this to the current <see cref="IDbContext" /></remarks>
        bool Delete(T entity);


        [Obsolete("Use the extension method from 'SmartStore.Core, SmartStore.Core.Data' instead")]
        IQueryable<T> Expand(IQueryable<T> query, string path);

        [Obsolete("Use the extension method from 'SmartStore.Core, SmartStore.Core.Data' instead")]
        IQueryable<T> Expand<TProperty>(IQueryable<T> query, Expression<Func<T, TProperty>> path);

        /// <summary>
        ///     Gets a value indicating whether the given entity was modified since it has been attached to the context
        /// </summary>
        /// <param name="entity">The entity to check</param>
        /// <returns><c>true</c> if the entity was modified, <c>false</c> otherwise</returns>
        bool IsModified(T entity);
    }
}