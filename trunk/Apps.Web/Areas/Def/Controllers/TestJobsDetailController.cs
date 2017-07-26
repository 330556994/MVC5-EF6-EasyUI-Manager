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
    public class TestJobsDetailController : BaseController
    {
        [Dependency]
        public IDEF_TestJobsDetailBLL m_BLL { get; set; }
        [Dependency]
        public IDEF_TestJobsBLL m_testJobsBLL { get; set; }
        [Dependency]
        public IDEF_TestJobsDetailRelationBLL m_testJobsRelationBLL { get; set; }
        //错误集合
        ValidationErrors validationErrors = new ValidationErrors();
        //详细主页
        [SupportFilter]
        public ActionResult Index()
        {
            ViewBag.perm = GetPermission();
            var testJobs = m_testJobsBLL.GetDefaultTestJobs(ref validationErrors);
             if (testJobs != null)
             {
                 ViewBag.vercode = testJobs.VerCode;
                 ViewBag.name = testJobs.Name;
                 if (testJobs.Result != null)
                 {
                     ViewBag.testing = "已测试";
                     ViewBag.testflag = 1;
                 }
                 else
                 {
                     ViewBag.testing = "未测试";
                     ViewBag.testflag = 0;
                 }
             }
            return View();
        }
        [HttpPost]
        public JsonResult GetSelVer(string id)
        {
            var testJobs = m_testJobsBLL.GetById(id);
            if (testJobs.Result != null)
            {
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }
        //JQGrid获取
        [HttpPost]
        public JsonResult GetList(GridPager pager, string queryStr, string vercode)
        {
            List<DEF_TestJobsDetailModel> list = m_BLL.GetList(ref pager, queryStr, vercode);
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new DEF_TestJobsDetailModel()
                        {
                            Id = r.VerCode + "_" + r.Code,
                            VerCode = r.VerCode,
                            Code = r.Code,
                            Name = r.Name,
                            Description = r.Description,
                            Result = r.Result,
                            Sort = r.Sort

                        }).ToArray()

            };
            return Json(json);
        }

        //新增
        [SupportFilter]
        public ActionResult Create(string vercode)
        {
            ViewBag.perm = GetPermission();
            if (string.IsNullOrEmpty(vercode))
            {
                return View("版本不能为空",true);
            }
            var model = m_testJobsBLL.GetModelById(vercode);
            if (model == null)
            {
                return View("版本不存在",true);
            }

            if (model.Result != null)
            {
                return View("已进行测试，不能新增测试用例",true);
            }
            ViewBag.vercode = vercode;
            return View();
        }

        //创建提交
        [HttpPost]
        public JsonResult Create(string vercode, string codes)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "没有选择测试任务"));
                }
                if (string.IsNullOrEmpty(codes))
                {
                    return Json(JsonHandler.CreateMessage(0, "没有测试明细用例"));
                }


                //新增
                m_BLL.Create(ref validationErrors, vercode, codes);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，新增测试明细ID:" +vercode +">"+codes, "失败", "新增", "测试明细");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.InsertSucceed + "，新增测试明细ID:" + vercode + ">" + codes, "失败", "新增", "测试明细");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.InsertFail));
            }
        }


        //新增
        public ActionResult CreateRelation(string vercode,string code)
        {
            if (string.IsNullOrEmpty(vercode))
            {
                return View("没有选择测试任务");
            }
            if (string.IsNullOrEmpty(code))
            {
                return View("没有选择测试任务");
            } 
            var jobsModel = m_testJobsBLL.GetModelById(vercode);
            if (jobsModel == null)
            {
                return View("版本不存在",true);
            }
            if (jobsModel.Result != null)
            {
                return View("已进行测试，不能新增测试用例", true);
            }
            var model = m_BLL.GetModelById(vercode, code);
            if (model == null)
            {
                return View("主用例不存在",true);
            }
            ViewBag.vercode = vercode;
            ViewBag.code = code;

            return View();
        }

        /// <summary>
        /// 新增子关系用例
        /// </summary>
        /// <param name="vercode"></param>
        /// <param name="codes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateRelation(string vercode, string pcode,string ccode)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "没有选择测试任务"));
                }
                if (string.IsNullOrEmpty(pcode))
                {
                    return Json(JsonHandler.CreateMessage(0, "没有测试明细用例"));
                }
                if (string.IsNullOrEmpty(ccode))
                {
                    return Json(JsonHandler.CreateMessage(0, "没有测试子用例"));
                }
                var jobsModel = m_testJobsBLL.GetModelById(vercode);
                if (jobsModel == null)
                {
                    return Json(JsonHandler.CreateMessage(0, "版本不存在"));
                }
                if (jobsModel.Result != null)
                {
                    return Json(JsonHandler.CreateMessage(0, "已进行测试，不能新增测试用例"));
                }
                //新增
                m_testJobsRelationBLL.CreateRelation(ref validationErrors, vercode, pcode,ccode);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，新增测试子用例ID:" + vercode + ">" + pcode, "失败", "新增", "测试明细");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.InsertSucceed + "，新增测试子用例ID:" + vercode + ">" + pcode, "成功", "新增", "测试明细");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.InsertFail));
            }
        }
        //修改
        public ActionResult Edit(string vercode,string code)
        {
       
            if (!ModelState.IsValid)
            {
                return View("数据验证不通过", true);
            }
           
            DEF_TestJobsModel testJobsModel = m_testJobsBLL.GetModelById(vercode);

            if (testJobsModel.CloseState == true)
            {
                return View("测试任务已关闭", true);
            }
            DEF_TestJobsDetailModel model = m_BLL.GetModelById(vercode, code);
            return View(model);

        }
        //修改
        [HttpPost]
        //[SupportFilter]
        [ValidateInput(false)]
        public ActionResult Edit(DEF_TestJobsDetailModel model)
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
                    LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateFail + "，编辑测试明细ID:" + model.VerCode, "失败", "编辑", "测试明细");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateSucceed + "，编辑测试明细ID:" + model.VerCode, "成功", "编辑", "测试明细");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.UpdateFail));
            }

        }

        [HttpPost]
        public JsonResult CreateTestJobs(string vercode)
        {
            try
            {
                DEF_TestJobsModel model = m_testJobsBLL.GetModelById(vercode);

                if (model.CloseState == true)
                {
                    return Json(JsonHandler.CreateMessage(0, "测试任务已关闭，不能操作"));
                }
                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择要生成的测试"));
                }
                if (model.Result!=null)
                {
                    return Json(JsonHandler.CreateMessage(0, "任务已进行测试，不能再生成执行项目"));
                }
                m_BLL.CreateTestJobs(ref validationErrors, vercode);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteFail + "，生成测试项目ID:" + vercode, "失败", "新增", "测试明细");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteSucceed + "，生成测试项目ID:" + vercode, "成功", "新增", "测试明细");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateFail));
            }
        }
        // 删除 
        [HttpPost]
        public JsonResult Delete(string vercode,string code)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                DEF_TestJobsModel model = m_testJobsBLL.GetModelById(vercode);

                if (model.CloseState == true)
                {
                    return Json(JsonHandler.CreateMessage(0, "测试任务已关闭，不能删除明细"));
                }
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择删除记录"));
                }

                if (string.IsNullOrEmpty(code))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择删除记录"));
                }


                m_BLL.Delete(ref validationErrors, vercode,code);

                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteFail + "，删除测试明细ID:" + vercode, "失败", "删除", "测试明细");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteSucceed + "，删除测试明细ID:" + vercode, "成功", "删除", "测试明细");
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteFail));
            }
        }
        [HttpPost]
        public JsonResult DeleteRelation(string vercode, string pcode,string ccode)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                DEF_TestJobsModel model = m_testJobsBLL.GetModelById(vercode);

                if (model.CloseState == true)
                {
                    return Json(JsonHandler.CreateMessage(0, "测试任务已关闭，不能删除明细"));
                }
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择删除记录"));
                }

                if (string.IsNullOrEmpty(pcode))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择删除记录"));
                }

                if (string.IsNullOrEmpty(ccode))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择删除记录"));
                }

                m_testJobsRelationBLL.DeleteByVPCcode(ref validationErrors, vercode, pcode, ccode);

                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteFail + "，删除测试明细ID:" + vercode, "失败", "新增", "测试明细");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteSucceed + "，删除测试明细ID:" + vercode, "成功", "新增", "测试明细");
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteFail));
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateSort(string vercode, string code, int sort, string description)
        {
            try
            {
                DEF_TestJobsModel model = m_testJobsBLL.GetModelById(vercode);

                if (model.CloseState == true)
                {
                    return Json(JsonHandler.CreateMessage(0, "测试任务已关闭，不能删除明细"));
                }
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择记录不存在"));
                }

                if (string.IsNullOrEmpty(code))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择记录不存在"));
                }

                DEF_TestJobsDetailModel detailModel = m_BLL.GetModelById(vercode, code);
                if (detailModel == null)
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择记录不存在"));
                }

                detailModel.Sort = sort;
                detailModel.Description = description;
                m_BLL.Edit(ref validationErrors, detailModel);

                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), "更新排序号测试明细ID:" + vercode, "失败", "更新", "测试明细");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), "更新排序号测试明细ID:" + vercode, "成功", "更新", "测试明细");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.EditFail));
            }
        }
    }
}
