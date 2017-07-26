using System;
using System.Linq;
using Apps.Common;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Apps.DEF.IBLL;
using Apps.Models.DEF;
using Apps.Web.Core;
using System.Collections.Generic;
using Apps.Locale;

namespace Apps.Web.Areas.DEF.Controllers
{
    public class TestCaseController : BaseController
    {
        [Dependency]
        public IDEF_CaseTypeBLL m_caseTypeBll { get; set; }
        [Dependency]
        public IDEF_TestCaseBLL m_BLL { get; set; }
        [Dependency]
        public IDEF_TestCaseRelationBLL relationBLL { get; set; }
        //错误集合
        ValidationErrors validationErrors = new ValidationErrors();
        
        [SupportFilter]
        public ActionResult Index()
        {
            ViewBag.perm = GetPermission();
            return View();
        }

        public ActionResult ModuleLookup()
        {
            return View();
        }
        private string CreateActions(string code)
        {
            string script;
            script = "<a href='#' title='删除' onclick=\"Delete('" + code + "');\"><img src='/Content/Images/icon/delete.png'/></a>";
            script += "&nbsp;<a href='#' title='修改' onclick=\"Edit('" + code + "');\"><img src='/Content/Images/icon/edit.png'/></a>";
            return script;

        }



        //JQGrid获取
        [HttpPost]
        public JsonResult GetListByModuleId(GridPager pager, string moduleId)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(moduleId))
                {
                    return Json(JsonHandler.CreateMessage(0, "分类不能为空，请选择一个用例分类"));
                }
                var colList = m_BLL.GetListByModuleId(ref pager, moduleId);
                var jsonData = new
                {
                    total = 10,//pager.totalPages,
                    page = pager.page,
                    records = pager.totalRows,
                    rows = (
                        from r in colList
                        select new
                        {
                            i = r.Code,
                            cell = new string[]
							{
								CreateActions(r.Code),//操作
                                r.Code,//ID
								//输出结果列表
								r.Name,
								r.Description,
								r.Sort.ToString(),
								r.ModuleId,
  
							}
                        }
                    ).ToArray()

                };
                return Json(jsonData);

            }
            catch (Exception ex)
            {
                LogHandler.WriteServiceLog(GetUserId(), "读取出错,查询参数:" + moduleId + " 错误：" + ex.Message, "失败", "读取", "测试用例");
                return null;
            }

        }

        [HttpPost]
        [SupportFilter(ActionName="Index")]
        public JsonResult GetList(GridPager pager, string queryStr, string moduleId)
        {
            List<DEF_TestCaseModel> list = m_BLL.GetList(ref pager, queryStr, moduleId);
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

        [HttpPost]
        [SupportFilter(ActionName = "Index")]
        public JsonResult GetListByRelation(GridPager pager, string code)
        {
            if (string.IsNullOrEmpty(code))
                return Json("");
            List<DEF_TestCaseRelationModel> list = relationBLL.GetList(ref pager, code);
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new DEF_TestCaseRelationModel()
                        {
                            Id = r.PCode + "_" + r.CCode,
                            PCode = r.PCode,
                            CCode = r.CCode,
                            Name = m_BLL.GetById(r.CCode).Name,
                            ReMark = r.ReMark,
                            Sort = r.Sort

                        }).ToArray()

            };
            return Json(json);
        }



        [HttpPost]
        public JsonResult GetListNoAction(GridPager pager, string querystr,string moduelId)
        {

            try
            {
                var colList = m_BLL.GetList(ref pager, querystr,moduelId);
                var jsonData = new
                {
                    total = 10,//pager.totalPages,
                    page = pager.page,
                    records = pager.totalRows,
                    rows = (
                        from r in colList
                        select new
                        {
                            i = r.Code,
                            cell = new string[]
							{
                                r.Code,//ID
								//输出结果列表
								r.Name,
								r.Description,
								r.Sort.ToString(),
								r.ModuleId,
  
							}
                        }
                    ).ToArray()

                };
                return Json(jsonData);

            }
            catch (Exception ex)
            {
                LogHandler.WriteServiceLog(GetUserId(), "读取出错,查询参数:" + querystr + " 错误：" + ex.Message, "失败", "读取", "测试用例");
                return null;
            }

        }
        [HttpPost]
        public JsonResult GetListNoActionByModuleId(GridPager pager, string moduleId)
        {

            try
            {
                var colList = m_BLL.GetListByModuleId(ref pager, moduleId);
                var jsonData = new
                {
                    total = 10,//pager.totalPages,
                    page = pager.page,
                    records = pager.totalRows,
                    rows = (
                        from r in colList
                        select new
                        {
                            i = r.Code,
                            cell = new string[]
							{
                                r.Code,//ID
								//输出结果列表
								r.Name,
								r.Description,
								r.Sort.ToString(),
								r.ModuleId,
  
							}
                        }
                    ).ToArray()

                };
                return Json(jsonData);

            }
            catch (Exception ex)
            {
                LogHandler.WriteServiceLog(GetUserId(), "读取出错,查询参数:" + moduleId + " 错误：" + ex.Message, "失败", "读取", "测试用例");
                return null;
            }

        }
        //新增
        [SupportFilter]
        public ActionResult Create()
        {
            ViewBag.perm = GetPermission();
            return View();
        }


         //新增
         [SupportFilter(ActionName = "Create")]
         public ActionResult CreateRelation(string code)
         {
             ViewBag.perm = GetPermission();
             if (string.IsNullOrEmpty(code))
             {
                 return View("用例不能为空");
             }
             var model = m_BLL.GetModelById(code);
             if (model == null)
             {
                 return View("用例不能为空");
             }
             ViewBag.code = code;
             return View();
         }

         //创建提交
         [HttpPost]
         [SupportFilter(ActionName = "Create")]
         public JsonResult CreateRelation(string code, string codeIds)
         {
             try
             {

                 //新增
                 if (relationBLL.Create(ref validationErrors, code, codeIds))
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

         // 删除 
         [HttpPost]
         [SupportFilter(ActionName = "Delete")]
         public JsonResult DeleteRelation(string pcode, string ccode)
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

                 relationBLL.Delete(ref validationErrors, pcode, ccode);

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

        //创建提交
        [HttpPost]
        [SupportFilter]
        [ValidateInput(false)]
        public JsonResult Create(DEF_TestCaseModel info)
        {

            if (info != null && ModelState.IsValid)
            {

                if (m_BLL.Create(ref validationErrors, info))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Code:" + info.Code + ",Name:" + info.Name, "成功", "创建", "测试用例");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = validationErrors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Code:" + info.Code + ",Name:" + info.Name + "," + ErrorCol, "失败", "创建", "测试用例");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail), JsonRequestBehavior.AllowGet);
            }

        }

        //查看测试用例是否存在
        [HttpPost]
        public JsonResult Checkcode(string code)
        {
            DEF_TestCaseModel model = m_BLL.GetModelById(code);
            if (model != null)
            {
                return Json("1",JsonRequestBehavior.AllowGet);
            }
            return Json("0", JsonRequestBehavior.AllowGet);
        }
        //修改
        [SupportFilter]
        public ActionResult Edit(string code)
        {
            ViewBag.perm = GetPermission();
            if (!ModelState.IsValid)
            {
                return View("数据验证不通过", true);
            }
            DEF_TestCaseModel model = m_BLL.GetModelById(code);

            return View(model);

        }
        //修改
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(DEF_TestCaseModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }

                m_BLL.Edit(ref validationErrors, model);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateFail + "，编辑测试用例ID:" + model.Code, "失败", "编辑", "测试用例");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateSucceed + "，编辑测试用例ID:" + model.Code, "失败", "编辑", "测试用例");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.UpdateFail));
            }

        }
        // 删除 
        [HttpPost]
        public JsonResult Delete(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择删除记录"));
                }



                if (m_BLL.Delete(ref validationErrors, code))
                {

                    //成功写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteSucceed + "，删除测试用例ID:" + code, "失败", "删除", "测试用例");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {

                    
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteFail + "，删除测试用例ID:" + code, "成功", "删除", "测试用例");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteFail));
            }
        }
    }
}
