using System;
using System.Linq;
using Apps.Common;
using System.Web.Mvc;
using Apps.Locale;
using Microsoft.Practices.Unity;
using Apps.DEF.IBLL;
using Apps.DEF.IDAL;
using Apps.Models.DEF;
using Apps.Web.Core;
using System.Collections.Generic;

namespace Apps.Web.Areas.DEF.Controllers
{
    public class TestCaseStepsController : BaseController
    {
        [Dependency]
        public IDEF_TestCaseStepsBLL m_BLL { get; set; }
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
        private string CreateActions(string id)
        {
            string script;
            script = "<a href='#' title='删除' onclick=\"Delete('" + id + "');\"><img src='/Content/Images/icon/delete.png'/> </a>";
            script += "&nbsp;<a href='#' title='修改' onclick=\"Edit('" + id + "');\"> <img src='/Content/Images/icon/edit.png'/></a>";
           
            return script;

        }


        [HttpPost]
        [SupportFilter(ActionName = "Index")]
        public JsonResult GetListByTestCase(GridPager pager, string queryStr, string moduleId)
        {
            List<DEF_TestCaseModel> list = testCaseBLL.GetList(ref pager, queryStr, moduleId);
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new DEF_TestCaseModel()
                        {

                            Code = r.Code,
                            Name = r.Name,
                            Description = r.Description,
                            ModuleId = r.ModuleId,
                            Sort = r.Sort

                        }).ToArray()

            };

            return Json(json);
        }
      
        //JQGrid获取
        [HttpPost]
        [SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager,string code)
        {
            if (string.IsNullOrEmpty(code))
                return Json("");
            var colList = m_BLL.GetList(ref pager, code);
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in colList
                        select new DEF_TestCaseStepsModel()
                        {

                            ItemID = r.ItemID,
                            Code = r.Code,
                            Title = r.Title,
                            TestContent = r.TestContent,
                            state =r.state,
                            sort = r.sort

                        }).ToArray()

            };

            return Json(json);
        }

        //新增
        [SupportFilter]
        public ActionResult Create(string code)
        {
            ViewBag.perm = GetPermission();
            if (string.IsNullOrEmpty(code))
            {
                return View("用例不能为空");
            }
            var model = testCaseBLL.GetModelById(code);
            if (model == null)
            {
                return View("用例不能为空");
            }
            ViewBag.code = code;
            return View();
        }

        //创建提交
        [HttpPost]
        [ValidateInput(false)]
        [SupportFilter]
        public JsonResult Create(DEF_TestCaseStepsModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }


                //新增
                model.ItemID = Guid.NewGuid().ToString();
                model.state = true;
                m_BLL.Create(ref validationErrors, model);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，新增用例步骤ID:" + model.ItemID, "失败", "新增", "用例步骤");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.InsertSucceed + "，新增用例步骤ID:" + model.ItemID, "成功", "新增", "用例步骤");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.InsertFail));
            }
        }
        //修改
        public ActionResult Edit(string id)
        {
            if (!ModelState.IsValid)
            {
                return View("数据验证不通过", true);
            }
            DEF_TestCaseStepsModel model = m_BLL.GetModelById(id);

            return View(model);

        }
        //修改
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit(DEF_TestCaseStepsModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (model.state == null)
                {
                    model.state = false;
                }
                m_BLL.Edit(ref validationErrors, model);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateFail + "，编辑用例步骤ID:" + model.ItemID, "失败", "编辑", "用例步骤");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateSucceed + "，编辑用例步骤ID:" + model.ItemID, "成功", "编辑", "用例步骤");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.UpdateFail));
            }

        }

        // 删除 
        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "上传参数有错误"));
                }
                if (string.IsNullOrEmpty(id))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择删除记录"));
                }



                m_BLL.Delete(ref validationErrors, id);

                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteFail + "，删除用例步骤ID:" + id, "失败", "删除", "用例步骤");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteSucceed + "，删除用例步骤ID:" + id, "成功", "删除", "用例步骤");
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteFail));
            }
        }
    }
}
