using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Utilities;

namespace Platform.Commonn
{
    public class AuthenticationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 在  Action方法之前 调用  
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            HttpRequest request = HttpContext.Current.Request;
            string token = request.Params["token"];//当前工程访问token为空外部访问验证cookie
            string paths = request.Path.ToString();
            /**
            获取url的token，若不为空则验证token信息
            */
            if (token != null)
            {
                string reqStr = Encryption.DESDeCode(token, Encryption.DESKey);
                string[] strArray = reqStr.Split('|');  //0为用户Guid  1为时间戳
                bool r_req = OSData.TimeUtcToCompare(strArray[1]);//判断时间戳是否在五分钟以内
                if (!r_req)
                {
                    string returnUrl = ConfigurationManager.AppSettings["ReturnLoginUrl"];  //获取登录要跳转的url
                    filterContext.HttpContext.Response.Redirect(returnUrl);//跳转到指定路径
                    return;
                }
                else
                {
                    var powerinfo = CacheHelper.GetCache("power" + strArray[0]);
                    if (powerinfo == null)
                    {
                        UserOAuth.UserToPowerPre(strArray[0]);//权限为空，获取权限内容并写入缓存
                    }
                    request.Cookies.Remove("User"); //移除用户信息cookic
                    UserOAuth.UserInfoToCookie(strArray[0]);//重新保存cooick
                    CacheHelper.SetCache(strArray[0] + "Controll", paths);//保存

                }
            }

            HttpCookie cookie = StorageHelper.Cookie.GetCookie("User");
            string testBool = ConfigurationManager.AppSettings["test"];  //是否为测试环境
            if (testBool == "true")
            {
                CacheHelper.SetCache("f4b2b995ac414623b779467f8a8f5946" + "Controll", paths);
                var PuserGuid = "f4b2b995ac414623b779467f8a8f5946";
                var powerinfo = CacheHelper.GetCache("power" + "f4b2b995ac414623b779467f8a8f5946");
                if (powerinfo == null)
                {
                    UserOAuth.UserToPowerPre(PuserGuid);
                }
                Dictionary<string, object> cDic = new Dictionary<string, object>();
                cDic.Add("uguid", "f4b2b995ac414623b779467f8a8f5946");
                cDic.Add("sguid", "30E0E3D1341049D39AD513E854836AC1");
                cDic.Add("username", "13547937936");
                cDic.Add("name", "夏小平");
                StorageHelper.Cookie.SetCookie("User", cDic);
            }
            else
            {
                /**
                 先判断cooick的值是否为null,若为null则登录，否则判断权限是否为null，若为空，去cookie的userid获取权限
                 */
                if (cookie == null)
                {
                    string _RetUrl = ConfigurationManager.AppSettings["ReturnLoginUrl"];  //获取默认登录页的路径 
                    filterContext.HttpContext.Response.Redirect(_RetUrl);
                    return;
                }
                var powerinfo = CacheHelper.GetCache("power" + StorageHelper.Cookie.GetCookieValue("User", "uguid"));//获取当前登录人的权限
                if (powerinfo == null)//权限为空则调取权限接口，加载缓存
                {
                    UserOAuth.UserToPowerPre(StorageHelper.Cookie.GetCookieValue("User", "uguid"));
                }
            }
        }
    }

    public static class UserOAuth
    {
        /// <summary>
        /// 将用户权限从接口中获取后保存至缓存中
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public static void UserToPowerPre(string userGuid)
        {
            try
            {
                string power = OSData.UserToPwore(userGuid);//获取用户权限接口信息
                Utilities.CacheHelper.SetCache("power" + userGuid, power, 43200);//将权限信息写入缓存.12小时过期
            }
            catch (Exception ex)
            {
                //Log4netHelper.WriteLog("将权限保存如缓存中时出错：", ex);
            }
        }
        /// <summary>
        /// 保存用户cookie信息
        /// </summary>
        /// <param name="userGuid"></param>
        public static void UserInfoToCookie(string userGuid)
        {
            try
            {
                BLL.ShareBll bll = new BLL.ShareBll();
                var userModel = bll.GetUserById(userGuid);
                Dictionary<string, object> cDic = new Dictionary<string, object>();
                cDic.Add("uguid", userModel.RI_UserId);
                cDic.Add("sguid", userModel.ri_schoolId);
                cDic.Add("username", userModel.RI_UserName);
                cDic.Add("name", userModel.RI_RealName);
                StorageHelper.Cookie.SetCookie("User", cDic);
            }
            catch (Exception ex)
            {
                //Log4netHelper.WriteLog("权限认证时保存用户cookie信息出错：",ex);
                throw;
            }
        }
    }
}