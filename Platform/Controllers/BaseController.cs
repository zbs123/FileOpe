using Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using Utilities;

namespace Platform.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetPower()
        {

            string json = Server.UrlDecode(Utilities.CacheHelper.GetCache("power" + StorageHelper.Cookie.GetCookieValue("User", "uguid")).ToString());
            return json;
        }

        /// <summary>
        /// 获取左侧菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetInnerPower()
        {
            string json = Server.UrlDecode(Utilities.CacheHelper.GetCache("power" + StorageHelper.Cookie.GetCookieValue("User", "uguid")).ToString());
            string collStr = Utilities.CacheHelper.GetCache(StorageHelper.Cookie.GetCookieValue("User", "uguid") + "Controll").ToString().ToLower();
            if (collStr == "/home/getfile")
            {
                collStr = "/home/pageone";
            }
            PowerModel pmodel = JsonConvert.DeserializeObject<PowerModel>(json);
            List<PModel> plist = pmodel.menu;
            List<CModel> oList = new List<CModel>();
            if (plist!=null)
            {
                foreach (var item in plist)
                {
                    List<CModel> clist = item.child;
                    foreach (var cmodel in clist)
                    {
                        if (cmodel.url.ToLower().IndexOf(collStr) > -1)
                        {
                            oList = clist;
                        }
                    }
                }
            }
            
            return JsonConvert.SerializeObject(oList);
        }

    }
}