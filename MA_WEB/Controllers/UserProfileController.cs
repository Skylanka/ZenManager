using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MA_WEB.Controllers.Parameters;
using MA_WEB.Managers;
using MA_WEB.Models;
using Model.Base;

namespace MA_WEB.Controllers
{
    [RoutePrefix("api/user")]
    public class UserProfileController : BaseController
    {
        private readonly UserManager _manager = new UserManager();

        [Route("getUser")]
        [Route("getUser/{Id}")]
        [HttpGet]
        public async Task<UserViewModel> GetUser([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);
            var u =  _manager.GetById(parameters.Id);
            UserViewModel user = u;
            user.ProjectCount = await _manager.GetCount(parameters.Id);
            user.IssueCount = await _manager.GetUserIssuesCount(parameters.Id);
            return user;
        }

        [Route("saveUser")]
        [HttpPost]
        public async Task<int> SaveUser(UserViewModel user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = await ValidateCurrentUser();

            var currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            string d = System.IO.Path.GetDirectoryName(currentPath) + "\\Content\\avatars\\";
            var path = d.Replace("file:\\", "");

            ApplicationUser us = _manager.GetById(user.Id);
            us.FirstName = user.FName;
            us.LastName = user.LName;
            us.Email = user.Email;
            us.UserName = user.Email;
            us.Phone = user.Phone;
            us.Hometown = user.Hometown;
            if (user.File != null)
            {
                string imageName = DateTime.Now.Ticks + ".jpg";
                byte[] bytes = Convert.FromBase64String(user.File);
                File.WriteAllBytes(path + imageName, Convert.FromBase64String(user.File));
                us.Avatar = imageName;
            }

            await _manager.Update(us);
            return us.Id;
        }


        /*[Route("saveUserAvatar")]
        [HttpPost]
        public async Task<int> SaveUserAvatar(string file)
        {

            var u = await ValidateCurrentUser();

            ApplicationUser us = _manager.GetById(u.Id);
            string imageName = DateTime.Now.Ticks + ".jpg";
            byte[] bytes = Convert.FromBase64String(file);
            File.WriteAllBytes("D:\\MA\\MA-project\\MA_WEB\\Content\\avatars" + imageName, Convert.FromBase64String(file));
            /*if (user.File != null)
            {
                string imageName = DateTime.Now.Ticks + ".jpg";
                // save image in folder
                user.File.SaveAs("D:\\MA\\MA-project\\MA_WEB\\Content\\avatars" + imageName);
                us.Avatar = imageName;
            }#1#
            await _manager.Update(us);
            return us.Id;
        }*/
    }
}
