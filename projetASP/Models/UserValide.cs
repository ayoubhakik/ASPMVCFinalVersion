using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projetASP.Models
{
    public class UserValide
    {
        public static bool IsValid()
        {
            HttpContext context =  HttpContext.Current;
            if (context.Session["userId"] != null)
            {
                return true;
            }
            else
                return false;
        }
    }
}