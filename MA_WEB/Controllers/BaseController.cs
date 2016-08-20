using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MA_WEB.Controllers.Parameters;
using Microsoft.AspNet.Identity.Owin;
using Model.Base;


namespace MA_WEB.Controllers
{
    
    public class BaseController : ApiController
    {

       /// <summary>
        /// Возвращает текущего пользователя
        /// </summary>
        /// <returns></returns>
        protected async virtual Task<ApplicationUser> ValidateCurrentUser()
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);
   
            //если не найден, то ошибка
            if (currentUser == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Access forbidden"));

            return currentUser;
        }

        protected async virtual Task<ApplicationUser> ValidateCurrentUser(string email)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var currentUser = await userManager.FindByEmailAsync(email);

            //если не найден, то ошибка
            if (currentUser == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Access forbidden"));

            return currentUser;
        }

        /// <summary>
        /// Возвращает текущего пользователя
        /// </summary>
        /// <returns></returns>
        protected async virtual Task<ApplicationUser> ValidateCurrentUser(int id)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var currentUser = await userManager.FindByIdAsync(id);

            //если не найден, то ошибка
            if (currentUser == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Access forbidden"));

            return currentUser;
        }
        /// <summary>
        /// Возвращает менеджер
        /// </summary>
        /// <returns></returns>
        protected ApplicationUserManager GetApplicationUserManager()
        {
            return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }


        /// <summary>
        /// Валидирует параметры
        /// </summary>
        /// <param name="parameters"></param>
        protected void ValidateParameters(IParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            string error;
            if (parameters.IsNotValid(out error))
                throw new ArgumentException(error);
        }


    }
}
