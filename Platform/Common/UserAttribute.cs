using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace Platform.Commonn
{
    public class UserAttribute : ActionFilterAttribute

    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
           
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //filterContext.HttpContext.Response.Redirect("./Error.html");
            HttpCookie cookie = StorageHelper.Cookie.GetCookie("UserInfo");
            if (cookie == null)
            {
                if (ConfigHelper.GetConfigString("test") == "true")
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("username", "ruiyi_admin");
                    dic.Add("uguid", "3F86CE57694F4498923AB7566C88A51D");
                    dic.Add("sguid", "30E0E3D1341049D39AD513E854836AC1");
                    dic.Add("rolestr", "2");/*登录用户的身份*/
                    dic.Add("head", "/upload/201711211042415724.jpg");
                    dic.Add("name", "%e7%ae%a1%e7%90%86%e5%91%98");

                    //StorageHelper.Cookie.SetCookie("UserInfo", dic, DateTime.Now.AddDays(1.0));
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect(ConfigHelper.GetConfigString("out"));

                }

            }
            
        }

        
    }
}