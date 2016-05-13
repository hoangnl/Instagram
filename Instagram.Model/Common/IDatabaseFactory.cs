using Instagram.Model.EDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Model.Common
{
    public interface IDatabaseFactory
    {
         InstagramEntities Get();
         void Dispose();
    }
}
