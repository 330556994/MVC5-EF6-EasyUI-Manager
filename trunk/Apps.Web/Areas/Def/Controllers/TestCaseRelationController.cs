using System;
using System.Linq;
using Apps.Common;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Apps.DEF.IBLL;
using Apps.DEF.IDAL;
using Apps.Models.DEF;
using Apps.Web.Core;
using System.Collections.Generic;
using Apps.Locale;

namespace Apps.Web.Areas.DEF.Controllers
{
    public class TestCaseRelationController : BaseController
    {
        [Dependency]
        public IDEF_TestCaseRelationBLL m_BLL { get; set; }
        [Dependency]
        public IDEF_TestCaseBLL testCaseBLL { get; set; }
        //错误集合
        ValidationErrors validationErrors = new ValidationErrors();
        //详细主页
        [SupportFilter]
        public ActionResult Index()
        {
            ViewBag.perm = GetPermission();
            return View();
        }
     

       
        //JQGrid获取
        [HttpPost]
        public JsonResult GetList(GridPager pager,string code)
        {
            if (string.IsNullOrEmpty(code))
                return Json("");
            List<DEF_TestCaseRelationModel> list = m_BLL.GetList(ref pager, code);
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new DEF_TestCaseRelationModel()
                        {
                            Id = r.PCode + "_" + r.CCode,
                            PCode = r.PCode,
                            CCode = r.CCode,
                            Name = testCaseBLL.GetById(r.CCode).Name,
                            ReMark = r.ReMark,
                            Sort = r.Sort

                        }).ToArray()

            };
            return Json(json);
        }

        //新增
        [SupportFilter]
        public ActionResult Create(string code )
        {
            ViewBag.perm = GetPermission();
            if (string.IsNullOrEmpty(code))
            {
                return View("用例不能为空");
            }
            var model =testCaseBLL.GetModelById(code);
            if (model == null)
            {
                return View("用例不能为空");
            }
            ViewBag.code = code;
            return View();
        }

        //创建提交
        [HttpPost]
        public JsonResult Create(string code, string codeIds)
        {
            try
            {

                //新增
                if (m_BLL.Create(ref validationErrors, code, codeIds))
                {
                    //成功写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertSucceed + "，新增用例关系ID:" + code + "," + codeIds, "失败", "新增", "用例关系");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));

                }
                else
                {
                        //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，新增用例关系ID:" + code + "," + codeIds, "成功", "新增", "用例关系");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
              
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.InsertFail));
            }
        }
        //创建提交
        [HttpPost]
        public JsonResult Edit(string id,int Sort)
        {
            try
            {

                //新增
                if (m_BLL.Edit(ref validationErrors, id, Sort))
                {
                    //成功写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertSucceed + "，修改用例关系ID:" + id + ",排序" + Sort, "成功", "修改", "用例关系");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));

                }
                else
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，修改用例关系ID:" + id + ",排序" + Sort, "失败", "修改", "用例关系");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }

            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
            }
        }
                     
        // 删除 
        [HttpPost]
        public JsonResult Delete(string pcode,string ccode)
        {
            try
            {
                if (string.IsNullOrEmpty(pcode))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择删除记录"));
                }
                if (string.IsNullOrEmpty(ccode))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择删除记录"));
                }



                m_BLL.Delete(ref validationErrors, pcode,ccode);

                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteFail + "，删除用例关系ID:" + pcode, "失败", "删除", "用例关系");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteSucceed + "，删除用例关系ID:" + pcode, "成功", "删除", "用例关系");
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteFail));
            }
        }
    }
}
