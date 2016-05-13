using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Instagram.Model.EDM;
using Instagram.Model.Common;

namespace Instagram.Model.Repository
{
    public class UserFollowRepository : BaseRepository<UserFollow>, IUserFollowRepository
    {
        public UserFollowRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }

    }
}
