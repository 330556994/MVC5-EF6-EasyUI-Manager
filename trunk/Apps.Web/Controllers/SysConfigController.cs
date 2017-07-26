﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Apps.IBLL;
using Apps.Common;
using Apps.Models.Sys;
using Apps.BLL;
using Apps.Web.Core;
using Apps.Locale;


namespace Apps.Web.Controllers
{
    public class SysConfigController : BaseController
    {
        //
        // GET: /SysConfig/

        ValidationErrors errors = new ValidationErrors();

        [SupportFilter]
        public ActionResult Index()
        {
            
            Apps.BLL.SysConfigBLL bll = new Apps.BLL.SysConfigBLL();
            SysConfigModel model = bll.loadConfig(Utils.GetXmlMapPath(ContextKeys.FILE_SITE_XML_CONFING));
            return View(model);
        }
        [HttpPost]
        [SupportFilter]
        [ValidateInput(false)]
        public JsonResult Edit(SysConfigModel model)
        {
            Apps.BLL.SysConfigBLL bll = new Apps.BLL.SysConfigBLL();
            try
            {
                bll.saveConifg(model, Utils.GetXmlMapPath(ContextKeys.FILE_SITE_XML_CONFING));
                return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
        }
    }
}
