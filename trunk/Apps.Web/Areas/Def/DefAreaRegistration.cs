using System.Web.Mvc;

namespace Apps.Web.Areas.DEF
{
    public class DefAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Def";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "defGlobalization", // 路由名称
                "{lang}/def/{controller}/{action}/{id}", // 带有参数的 URL
                new { lang = "zh", controller = "Home", action = "Index", id = UrlParameter.Optional }, // 参数默认值
                new { lang = "^[a-zA-Z]{2}(-[a-zA-Z]{2})?$" }    //参数约束
            );
            context.MapRoute(
                "def_default",
                "def/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
