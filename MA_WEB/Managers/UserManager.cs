using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MA_WEB.Models;
using Model.Base;
using Model.BusinessObjects;

namespace MA_WEB.Managers
{
    public class UserManager: BaseManager
    {
        public ApplicationUser Get(string name)
        {
            return Context.Users.FirstOrDefault(user => user.UserName == name);
        }

        /// <summary>
        /// Возвращает пользователя по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ApplicationUser GetByEmail(string email)
        {
            return Context.Users.FirstOrDefault(user => user.Email == email);
        }

        public ApplicationUser GetById(int id)
        {
            return Context.Users.Include(u=>u.Invitations).FirstOrDefault(user => user.Id == id);
        }


        public async Task<int> GetUserIssuesCount(int id)
        {
            return await Context.Issues.Where(p => p.AssignedTo.Id == id && !p.IsDeleted).CountAsync();
        }

        public async Task<int> GetCount(int id)
        {
            return await Context.Projects.Where(p => p.Users.FirstOrDefault(u => u.Id == id) != null && !p.IsDeleted).CountAsync();
        }
        public Task Update(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (user.Id == 0)
                throw new ArgumentException("InvalidId");
            /*if (String.IsNullOrWhiteSpace(user.FirstName))
                throw new ArgumentNullException("user name");*/

            Context.Users.AddOrUpdate(user);
            return Context.SaveChangesAsync();
        }

        }
}