using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MA_WEB.Controllers.Parameters
{
    public interface IParameters
    {
        bool IsNotValid(out string error);
    }
}