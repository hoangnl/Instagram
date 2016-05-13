using Instagram.Model.Common;
using Instagram.Model.EDM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Instagram.Model.Repository
{
    /// <summary>
    /// Lớp Repository base cho entity kiểu T
    /// </summary>
    /// <typeparam name="T">Kiểu Entity</typeparam>
    public class BaseRepository<T> where T : class
    {
        #region Declaration

        /// <summary>
        /// DBContext chung của Repository
        /// </summary>
        private InstagramEntities DbContext;

        /// <summary>
        /// Tập Entity kiểu T
        /// </summary>
        private readonly IDbSet<T> DbSet;

        #endregion

        #region Property

        /// <summary>
        /// Factory khởi tạo Database
        /// </summary>
        protected IDatabaseFactory DatabaseFactory { get; set; }

        /// <summary>
        /// DBContext chung của Repository
        /// </summary>
        protected InstagramEntities DataContext
        {
            get { return DbContext ?? (DbContext = DatabaseFactory.Get()); }
        }

        #endregion

        #region Constructor

        protected BaseRepository(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            DbSet = DataContext.Set<T>();
        }

        #endregion

        #region Function

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Update(T entity)
        {
            DbSet.Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = DbSet.Where(where).AsEnumerable();
            foreach (var obj in objects)
            {
                Delete(obj);
            }
        }

        public T GetById(long id)
        {
            return DbSet.Find(id);
        }

        public T GetById(string id)
        {
            return DbSet.Find(id);
        }

        public T GetBy(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).ToList();
        }

        public IEnumerable<T> GetWithInclude(Expression<Func<T, bool>> where, params string[] include)
        {
            IQueryable<T> query = DbSet;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));

            return query.Where(where);
        }

        public bool Exist(object primaryKey)
        {
            return DbSet.Find(primaryKey) != null;
        }

        public T GetFirst(Expression<Func<T, bool>> where)
        {
            return DbSet.First(where);
        }

        #endregion
    }
}
