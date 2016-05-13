using Instagram.Model.EDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Model.Common
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private InstagramEntities DbContext;
        private bool Disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
            Disposed = true;
        }

        public InstagramEntities Get()
        {
            return DbContext ?? (DbContext = new InstagramEntities());
        }

    }
}
