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
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }

        public IEnumerable<User> SearchUser(string searchTerm)
        {
            var users = GetMany(e => e.FullName.Contains(searchTerm));
            return users;
        }
    }
}
