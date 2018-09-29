using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace Platform.Controllers
{
    public class ApiController : Controller
    {
        /// <summary>
        /// 生成当前时间下的token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetUrlToken()
        {

            string uguid = StorageHelper.Cookie.GetCookieValue("User", "uguid");
            string token = Utilities.Encryption.DESEnCode(uguid + "|" + DateTime.Now.ToFileTimeUtc(), Utilities.Encryption.DESKey);
            return token;

        }
        /// <summary>
        /// 退出登录接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string outLogin()
        {
            var cookie = StorageHelper.Cookie.GetCookie("User");
            OutLogingModel model = new OutLogingModel();
            var info = "";
            if (cookie != null)
            {
                try
                {
                    string uguid = StorageHelper.Cookie.GetCookieValue("User", "uguid");
                    CacheHelper.RemoveAllCache("power" + uguid);
                    CacheHelper.RemoveAllCache(uguid + "Controll");
                    Utilities.StorageHelper.Cookie.ClearCookie("User");
                    model.code = "200";
                    model.date = DateTime.Now.ToString();
                    model.msg = "成功";
                }
                catch (Exception ex)
                {
                    model.code = "500";
                    model.date = DateTime.Now.ToString();
                    model.msg = "失败";
                }
                finally
                {

                    info = JsonConvert.SerializeObject(model);
                }
            }
            else
            {
                model.code = "200";
                model.date = DateTime.Now.ToString();
                model.msg = "成功";
                info = JsonConvert.SerializeObject(model);
            }
            return info;
        }

    }
    public class OutLogingModel
    {
        public string code { get; set; }
        public string msg { get; set; }
        public string date { get; set; }
    }
}
