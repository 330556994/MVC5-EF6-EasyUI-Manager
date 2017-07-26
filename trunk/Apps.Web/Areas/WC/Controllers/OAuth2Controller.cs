using Apps.Models.WC;
using Apps.WC.IBLL;
using Microsoft.Practices.Unity;
using Senparc.Weixin.MP;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;

namespace Apps.Web.Areas.WC.Controllers
{
    public class OAuth2Controller : Controller
    {
        [Dependency]
        public IWC_OfficalAccountsBLL account_BLL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl">用户尝试进入的需要登录的页面</param>
        /// <returns></returns>
        public ActionResult Index(string returnUrl)
        {
            WC_OfficalAccountsModel model = account_BLL.GetCurrentAccount();

            //获取用户保存的cookie
            //判断是否已经授权过

            var state = "YMNETS-" + DateTime.Now.Millisecond;//随机数，用于识别请求可靠性
            Session["State"] = state;//储存随机数到Session

            ViewData["returnUrl"] = returnUrl;

            //此页面引导用户点击授权
            ViewData["UrlUserInfo"] =
                OAuthApi.GetAuthorizeUrl(model.AppId,
                "http://ymnets.imwork.net/WC/OAuth2/UserInfoCallback?returnUrl=" + returnUrl.UrlEncode(),
                state, OAuthScope.snsapi_userinfo);
            ViewData["UrlBase"] =
                OAuthApi.GetAuthorizeUrl(model.AppSecret,
                "http://ymnets.imwork.net/WC/OAuth2/BaseCallback?returnUrl=" + returnUrl.UrlEncode(),
                state, OAuthScope.snsapi_base);
            return View();
        }

        /// <summary>
        /// OAuthScope.snsapi_userinfo方式回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="returnUrl">用户最初尝试进入的页面</param>
        /// <returns></returns>
        public ActionResult UserInfoCallback(string code, string state, string returnUrl)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }

            if (state != Session["State"] as string)
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下，
                //建议用完之后就清空，将其一次性使用
                //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                return Content("验证失败！请从正规途径进入！");
            }

            OAuthAccessTokenResult result = null;

            //通过，用code换取access_token
            try
            {
                WC_OfficalAccountsModel model = account_BLL.GetCurrentAccount();
                result = OAuthApi.GetAccessToken(model.AppId, model.AppSecret, code);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }
            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            Session["OAuthAccessTokenStartTime"] = DateTime.Now;
            Session["OAuthAccessToken"] = result;

            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            try
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                return View(userInfo);
            }
            catch (ErrorJsonResultException ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// OAuthScope.snsapi_base方式回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="returnUrl">用户最初尝试进入的页面</param>
        /// <returns></returns>
        public ActionResult BaseCallback(string code, string state, string returnUrl)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }

            if (state != Session["State"] as string)
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下，
                //建议用完之后就清空，将其一次性使用
                //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                return Content("验证失败！请从正规途径进入！");
            }
            WC_OfficalAccountsModel model = account_BLL.GetCurrentAccount();
            //通过，用code换取access_token
            var result = OAuthApi.GetAccessToken(model.AppId, model.AppSecret, code);
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }

            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            Session["OAuthAccessTokenStartTime"] = DateTime.Now;
            Session["OAuthAccessToken"] = result;

            //因为这里还不确定用户是否关注本微信，所以只能试探性地获取一下
            OAuthUserInfo userInfo = null;
            try
            {
                //已关注，可以得到详细信息
                userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }


                ViewData["ByBase"] = true;
                return View("UserInfoCallback", userInfo);
            }
            catch (ErrorJsonResultException ex)
            {
                //未关注，只能授权，无法得到详细信息
                //这里的 ex.JsonResult 可能为："{\"errcode\":40003,\"errmsg\":\"invalid openid\"}"
                return Content("用户已授权，授权Token：" + result);
            }
        }

        /// <summary>
        /// 测试ReturnUrl
        /// </summary>
        /// <returns></returns>
        public ActionResult TestReturnUrl()
        {
            string msg = "OAuthAccessTokenStartTime：" + Session["OAuthAccessTokenStartTime"];
            //注意：OAuthAccessTokenStartTime这里只是为了方便识别和演示，
            //OAuthAccessToken千万千万不能传输到客户端！

            msg += "<br /><br />" +
                   "此页面为returnUrl功能测试页面，可以进行刷新（或后退），不会得到code不可用的错误。<br />测试不带returnUrl效果，请" +
                   string.Format("<a href=\"{0}\">点击这里</a>。", Url.Action("Index"));

            return Content(msg);
        }
    }
}