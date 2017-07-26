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
    public class FollowPlanController : BaseController
    {
        [Dependency]
        public IDEF_TestJobsDetailStepsBLL m_BLL { get; set; }
        [Dependency]
        public IDEF_TestJobsDetailItemBLL Item_BLL { get; set; }
        [Dependency]
        public IDEF_TestJobsBLL m_testJobsBLL { get; set; }
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
            }
            return View();
        }



        private string CreateActions(string id)
        {
            string script;
            script = "<a href='#' title='删除' onclick=\"Delete('" + id + "');\"><img src='/Content/Images/icon/delete.png'/> </a>";
            script += "&nbsp;<a href='#' title='修改' onclick=\"Edit('" + id + "');\"> <img src='/Content/Images/icon/edit.png'/></a>";
            script += "&nbsp;<a href='#' title='详细' onclick=\"Details('" + id + "');\"><img src='/Content/Images/icon/details.png'/></a>";
            return script;

        }

        //JQGrid获取
        [HttpPost]
        public JsonResult GetList(GridPager pager, string querystr, string vercode)
        {

            try
            {
                var colList = m_BLL.GetList(ref pager, querystr, vercode);
                var jsonData = new
                {
                    total = 10,//pager.totalPages,
                    page = pager.page,
                    records = pager.totalRows,
                    rows = (
                        from r in colList
                        select new
                        {
                            i = r.ItemID,
                            cell = new string[]
							{
								CreateActions(r.ItemID),//操作
								//输出结果列表
								r.ItemID.ToString(),
								r.VerCode,
								r.Code,
								r.Title,
								r.TestContent,
								r.Result==null?"":r.Result==true?"true":"false",
								r.Sort.ToString(),
								r.ResultContent,
                                r.StepType==0?"测试步骤":"缺陷记录",
				
  
							}
                        }
                    ).ToArray()

                };
                return Json(jsonData);

            }
            catch (Exception ex)
            {
                LogHandler.WriteServiceLog(GetUserId(), "读取出错,查询参数:" + querystr + " 错误：" + ex.Message, "失败", "删除", "测试步骤");
                return null;
            }

        }

        /// <summary>
        /// 执行开发
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[SupportFilter]
        [ValidateInput(false)]
        public ActionResult RunDev(DEF_TestJobsDetailStepsModel model)
        {
            try
            {
            
                model.Developer = GetUserId();
                if (m_BLL.RunDev(ref validationErrors, model))
                {
                    //生成缺陷报告
                    Item_BLL.DevUpdateState(ref validationErrors, model.VerCode, model.Code);

                }
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateFail + "，开发步骤ID:" + model.ItemID, "失败", "新增", "测试步骤");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateFail + "，开发步骤ID:" + model.ItemID, "成功", "新增", "测试步骤");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.UpdateFail));
            }

        }
        //新增
        public ActionResult Create()
        {
            return View();
        }

        //创建提交
        [HttpPost]
        public JsonResult Create(DEF_TestJobsDetailStepsModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }


                //新增
                m_BLL.Create(ref validationErrors, model);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，新增测试步骤ID:" + model.ItemID, "失败", "新增", "测试步骤");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.InsertSucceed + "，新增测试步骤ID:" + model.ItemID, "成功", "新增", "测试步骤");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.InsertFail));
            }
        }
        //修改
        public ActionResult Edit(string itemId, string vercode, string code)
        {
            if (!ModelState.IsValid)
            {
                return View("数据验证不通过", true);
            }
            DEF_TestJobsDetailStepsModel model = m_BLL.GetModelById(itemId, vercode, code);

            return View(model);

        }
        //修改
        [HttpPost]
        //[SupportFilter]
        public ActionResult Edit(DEF_TestJobsDetailStepsModel model)
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
                    LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateFail + "，编辑测试步骤ID:" + model.ItemID, "失败", "编辑", "测试步骤");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateSucceed + "，编辑测试步骤ID:" + model.ItemID, "成功", "编辑", "测试步骤");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.UpdateFail));
            }

        }
        //详细
        public ActionResult Details(string itemid, string vercode, string code)
        {
            if (!ModelState.IsValid)
            {
                return View("数据验证不通过", true);
            }
            DEF_TestJobsDetailStepsModel model = m_BLL.GetModelById(itemid, vercode, code);

            return View(model);

        }

        // 删除由用户新建的缺陷记录 

        [HttpPost]
        public JsonResult Delete(string itemid, string vercode, string code)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(itemid))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择删除记录"));
                }
                if (string.IsNullOrEmpty(code))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择删除记录"));
                }
                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择版本"));
                }
                //
                var model = m_BLL.GetModelById(itemid, vercode, code);
                if (model == null)
                {
                    return Json(JsonHandler.CreateMessage(0, "步骤记录不存在"));
                }
                if (model.StepType == 0)
                {
                    return Json(JsonHandler.CreateMessage(0, "当前测记录是系统自建步骤，不能删除!"));
                }

                m_BLL.DeleteDefect(ref validationErrors, itemid, vercode, code, GetUserId());

                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteFail + "，删除测试步骤ID:" + itemid, "失败", "删除", "测试步骤");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteSucceed + "，删除测试步骤ID:" + itemid, "成功", "删除", "测试步骤");
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteFail));
            }
        }

        /// <summary>
        /// 获取开发步骤
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="querystr"></param>
        /// <param name="vercode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetListNoAction(GridPager pager, string querystr, string vercode)
        {

            List<DEF_TestJobsDetailItemModel> list = Item_BLL.GetList(ref pager, querystr, vercode);
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new DEF_TestJobsDetailItemModel()
                        {
                            Id = r.VerCode + "_" + r.Code,
                            VerCode = r.VerCode,
                            Code = r.Code,
                            Name = r.Name,
                            Description = r.Description,
                            Result = r.Result,
                            Sort = r.Sort,
                            ExSort = r.ExSort,
                            Lock = r.Lock,
                            Developer = r.Developer,
                            Tester = r.Tester,
                            FinDt = r.FinDt,
                            TestRequestFlag = r.TestRequestFlag,
                            DevFinFlag = r.DevFinFlag

                        }).ToArray()

            };

            return Json(json);


          

        }
        [HttpPost]
        public JsonResult GetListByCode(GridPager pager, string vercode, string code, string querystr)
        {

            List<DEF_TestJobsDetailStepsModel> list = m_BLL.GetListByCode(ref pager, vercode, code, querystr);
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new DEF_TestJobsDetailStepsModel()
                        {
                            Id = r.ItemID + "_" + r.VerCode + "_" + r.Code,
                            ItemID = r.ItemID,
                            VerCode = r.VerCode,
                            Code = r.Code,
                            Title = r.Title,
                            TestContent = r.TestContent,
                            Result = r.Result,
                            Sort = r.Sort,
                            ResultContent = r.ResultContent,
                            ExSort = r.ExSort,
                            StepType = r.StepType,
                            Tester = r.Tester,
                            TestDt = r.TestDt,
                            Developer = r.Developer,
                            PlanStartDt = r.PlanStartDt,
                            PlanEndDt = r.PlanEndDt,
                            FinDt = r.FinDt,
                            TestRequestFlag = r.TestRequestFlag,
                            DevFinFlag = r.DevFinFlag

                        }).ToArray()

            };

            return Json(json);

        }

    }
}
