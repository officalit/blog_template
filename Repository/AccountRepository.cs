using BLOG_CORE.Entity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLOG_CORE.Repository
{

    public class AccountRepository : BaseRepository
    {
        public static AccountRepository accountRepository = new AccountRepository();
        public int RegisterUser(User user)
        {
            return Execute("INSERT INTO USERS(FIRSTNAME,LASTNAME,USERNAME,PASSWORDHASH,PASSWORDSALT) " +
                "VALUES(:FIRSTNAME,:LASTNAME,:USERNAME,:PASSWORDHASH,:PASSWORDSALT)", user);
        }

        public IEnumerable<User> FindAll()
        {
            return Query<User>("SELECT * FROM USERS");
        }

        public IEnumerable<User_Role> UserRoleGetByID(User user)
        {
            return Query<User_Role>("SELECT * FROM USERS_ROLES WHERE USER_ID = :ID", user);
        }

        public Role RoleGetByID(User_Role userRole)
        {
            return QueryFirstOrDefault<Role>("SELECT * FROM ROLES WHERE ID = :ROLE_ID", userRole);
        }
    }
}
