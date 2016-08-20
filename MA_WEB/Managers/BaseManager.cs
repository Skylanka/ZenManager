using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Model.Base;

namespace MA_WEB.Managers
{
    public abstract class BaseManager
    {

        protected ApplicationDbContext Context
        {
            get { return GetContext(); }
        }

        private ApplicationDbContext GetContext()
        {
            ApplicationDbContext context = null;
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.GetOwinContext() != null)
                    context = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();

                if (context == null)
                {
                    var dbContext = HttpContext.Current.Items["DBContext"];
                    if (dbContext == null)
                    {
                        context = new ApplicationDbContext();
                        HttpContext.Current.Items["DBContext"] = context;
                    }
                    else
                    {
                        context = (ApplicationDbContext)dbContext;
                    }
                }

            }

            if (context == null)
                context = new ApplicationDbContext();

            return context;
        }
    }
}