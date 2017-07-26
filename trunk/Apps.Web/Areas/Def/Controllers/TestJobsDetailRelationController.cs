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
    public class TestJobsDetailRelationController : BaseController
    {
        [Dependency]
        public IDEF_TestJobsDetailRelationBLL m_BLL { get; set; }
        [Dependency]
        public IDEF_TestJobsBLL m_testJobsBLL { get; set; }
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
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            
            List<DEF_TestJobsDetailRelationModel> list = m_BLL.GetList(ref pager, queryStr);
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new DEF_TestJobsDetailRelationModel()
                        {
                            Id = r.VerCode + "_" + r.PCode + "_" + r.CCode,//ID
		                    VerCode = r.VerCode,
		                    PCode = r.PCode,
		                    CCode = r.CCode,
		                    Name = r.Name,
		                    Description = r.Description,
		                    Result = r.Result,
		                    Sort = r.Sort,
		                    ExSort = r.ExSort

                        }).ToArray()

            };

            return Json(json);

        }
        [HttpPost]
        public JsonResult GetListByCode(GridPager pager, string vercode, string code)
        {
            if (string.IsNullOrEmpty(vercode))
            {
                return Json(JsonHandler.CreateMessage(0, "测试版本不能为空"));
            }
            if (string.IsNullOrEmpty(code))
            {
                return Json(JsonHandler.CreateMessage(0, "主测试用例不能为空"));
            }
            List<DEF_TestJobsDetailRelationModel> list = m_BLL.GetListByCode(ref pager, vercode, code);
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new DEF_TestJobsDetailRelationModel()
                        {
                            Id = r.VerCode + "_" + r.PCode + "_" + r.CCode,//ID
                            VerCode = r.VerCode,
                            PCode = r.PCode,
                            CCode = r.CCode,
                            Name = r.Name,
                            Description = r.Description,
                            Result = r.Result,
                            Sort = r.Sort,
                            ExSort = r.ExSort
                        }).ToArray()
            };
            return Json(json);
        }
        //新增
        public ActionResult Create()
        {
            return View();
        }

        //创建提交
        [HttpPost]
        public JsonResult Create(DEF_TestJobsDetailRelationModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "上传参数错误"));
                }


                //新增
                m_BLL.Create(ref validationErrors, model);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，新增任务用例关系ID:" + model.VerCode,  "失败", "新增", "测试步骤");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.InsertSucceed + "，新增任务用例关系ID:" + model.VerCode,  "成功", "新增", "测试步骤");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.InsertFail));
            }
        }
        //修改
        public ActionResult Edit(string vercode, string pcode, string ccode)
        {
            if (!ModelState.IsValid)
            {
                return View("上传参数有错误!", true);
            }
            DEF_TestJobsDetailRelationModel model = m_BLL.GetModelById(vercode, pcode, ccode);

            return View(model);

        }
        //修改
        [HttpPost]
        //[SupportFilter]
        public ActionResult Edit(DEF_TestJobsDetailRelationModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "上传参数错误"));
                }

                m_BLL.Edit(ref validationErrors, model);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateFail + "，编辑任务用例关系ID:" + model.VerCode, "失败", "编辑", "任务用例关系");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateSucceed + "，编辑任务用例关系ID:" + model.VerCode, "成功", "编辑", "任务用例关系");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.UpdateFail));
            }

        }
        //详细
        public ActionResult Details(string vercode, string pcode, string ccode)
        {
            if (!ModelState.IsValid)
            {
                return View("上传参数有错误!", true);
            }
            DEF_TestJobsDetailRelationModel model = m_BLL.GetModelById(vercode, pcode, ccode);

            return View(model);

        }

        // 删除 
        [HttpPost]
        public JsonResult Delete(string vercode, string pcode, string ccode)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "上传参有错误"));
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


                m_BLL.DeleteByVPCcode(ref validationErrors, vercode, pcode, ccode);

                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteFail + "，删除任务用例关系ID:" + vercode, "失败", "删除", "任务用例关系");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteSucceed + "，删除任务用例关系ID:" + vercode, "成功", "删除", "任务用例关系");
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteFail));
            }
        }
        [HttpPost]
        public JsonResult UpdateSort(string vercode, string pcode, string ccode, int sort)
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
                    return Json(JsonHandler.CreateMessage(0, "上传参有错误"));
                }
                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择记录不存在"));
                }

                if (string.IsNullOrEmpty(pcode))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择记录不存在"));
                }
                if (string.IsNullOrEmpty(ccode))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择记录不存在"));
                }

                DEF_TestJobsDetailRelationModel relationModel = m_BLL.GetModelById(vercode, pcode, ccode);
                if (relationModel == null)
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择记录不存在"));
                }

                relationModel.Sort = sort;
                m_BLL.Edit(ref validationErrors, relationModel);

                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(),  "测试明细ID:" + vercode, "失败", "编辑", "测试明细");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), "测试明细ID:" + vercode, "成功", "编辑", "测试明细");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.EditFail));
            }
        }
    }
}
