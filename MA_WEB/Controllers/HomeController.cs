using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MA_WEB.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Model.BusinessModels;

namespace MA_WEB.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;
        public ActionResult Index()
        {
            var model = GetUserModel();
            return View(model);
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private UserViewModel GetUserModel()
        {
            var appUser = UserManager.FindByName(User.Identity.Name);
            var projects = new ProjectsController().GetProjectsForUser(appUser.Id);

            string since = CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat.GetAbbreviatedMonthName(appUser.CreatedDate.Month) + ". " + appUser.CreatedDate.Year;
           
            var user = new UserViewModel { Id = appUser.Id,FName = appUser.FirstName, LName = appUser.LastName, IsAuthenticated = Request.IsAuthenticated, Projects = projects, Avatar = appUser.Avatar, MemberSince = since};

        
                      /*  if (User.IsInRole(Role.Admin.ToString()))
                            user.Role = Role.Admin;
                        if (User.IsInRole(Role.ProjectAdmin.ToString()))
                            user.Role = Role.ProjectAdmin;
                        if (User.IsInRole(Role.Engineer.ToString()))
                            user.Role = Role.Engineer;*/

            /* //если у пользователя есть роль, то определим кол-во аварийного оборудования
             if (user.HasRole)
             {
                 
             }
             else //иначе найдем открытую заявку на регистрацию
             {
                 //получим открытую заявку
                 var openedClaim = IdentityServiceProxy.GetUserOpenedRegisterClaim(User.Identity.Name);
                 user.OpenedClaim = openedClaim != null ? new RegisterClaimViewModel { Id = openedClaim.Id, CreatedDate = openedClaim.CreatedDate } : null;
             }*/

            return user;
        }
    }
}
