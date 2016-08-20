using System;
using System.ComponentModel;

namespace Model.BusinessModels
{
    public enum Role
    {
        [Description("Администратор")]
        Admin,

        [Description("Администратор компании")]
        ProjectAdmin,

        [Description("Инженер")]
        Engineer
    }
}
