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
    public class TestJobsController : BaseController
    {
        [Dependency]
        public IDEF_TestJobsBLL m_BLL { get; set; }

        //错误集合
        ValidationErrors validationErrors = new ValidationErrors();
        //详细主页
        [SupportFilter]
        public ActionResult Index()
        {
            ViewBag.perm = GetPermission();
            return View();
        }
        private string CreateActions(string vercode)
        {
            string script;
            script = "<a href='#' title='删除' onclick=\"Delete('" + vercode + "');\"><img src='/Content/Images/icon/delete.png'/> </a>";
            script += "&nbsp;<a href='#' title='修改' onclick=\"Edit('" + vercode + "');\"> <img src='/Content/Images/icon/edit.png'/></a>";
            return script;

        }

       
        //JQGrid获取
        [HttpPost]
        public JsonResult GetList(GridPager pager, string queryStr, int selShow = 0)
        {
            List<DEF_TestJobsModel> list = m_BLL.GetList(ref pager,selShow, queryStr);
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new DEF_TestJobsModel()
                        {
                            VerCode = r.VerCode,
                            Name = r.Name,
                            Result = r.Result,
                            Description = r.Description,
                            Creator = r.CreatorTitle,
                            CrtDt = r.CrtDt,
                            CloseState = r.CloseState,
                            Closer = r.CloserTitle,
                            CloseDt = r.CloseDt,
                            Def = r.Def,
                            CheckFlag = r.CheckFlag
                        }).ToArray()
            };
            return Json(json);
        }

        //新增
        [SupportFilter]
        public ActionResult Create()
        {
            ViewBag.perm = GetPermission();
            return View();
        }
        /// <summary>
        /// 生成测试任务项目及测试步骤
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateTestJobs(string vercode)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }


                //新增
                m_BLL.CreateTestJobs(ref validationErrors, vercode);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，新增测试任务项目:" + vercode, "失败", "新增", "测试任务");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.InsertSucceed + "，新增测试任务项目:" + vercode, "成功", "新增", "测试任务");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.InsertFail));
            }
        }
        /// <summary>
        /// 设置一个测试任务为缺省任务
        /// </summary>
        /// <param name="vercode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetTestJobsDefault(string vercode)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }


                //新增
                m_BLL.SetTestJobsDefault(ref validationErrors, vercode);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), "设置一个测试任务为缺省任务:" + vercode, "失败", "编辑", "测试任务");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), "设置一个测试任务为缺省任务:" + vercode, "成功", "编辑", "测试任务");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateFail));
            }
        }
        
        //创建提交
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create(DEF_TestJobsModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }


                //新增
                model.Creator = GetUserId();
                model.CrtDt = DateTime.Now;
                m_BLL.Create(ref validationErrors, model);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，新增测试任务ID:" + model.VerCode, "失败", "新增", "测试任务");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.InsertSucceed + "，新增测试任务ID:" + model.VerCode, "成功", "新增", "测试任务");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.InsertFail));
            }
        }
        public JsonResult Check(string vercode, bool checkflag)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }


                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "测试任务的版本号不能为空"));
                }

                m_BLL.SetCheckFlag(ref validationErrors, vercode, checkflag);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，锁定测试任务ID:" + vercode, "失败", "锁定", "测试任务");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateSucceed + "，锁定测试任务ID:" + vercode, "成功", "锁定", "测试任务");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateFail));
            }
        }

        /// <summary>
        /// 关闭任务
        /// </summary>
        /// <param name="vercode"></param>
        /// <returns></returns>
        public JsonResult CloseTestJobs(string vercode,bool closestate)
        {
            return SetCloseState(vercode, closestate);
        }

        /// <summary>
        /// 设置关闭状态
        /// </summary>
        /// <param name="vercode"></param>
        /// <param name="closeState"></param>
        /// <returns></returns>
        private JsonResult SetCloseState(string vercode, bool closeState)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }


                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "测试任务的版本号不能为空"));
                }
                
                m_BLL.SetCloseTestJobsState(ref validationErrors, vercode, closeState,GetUserId());
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，关闭测试任务ID:" + vercode, "失败", "关闭", "测试任务");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateSucceed + "，关闭测试任务ID:" + vercode, "成功", "关闭", "测试任务");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateFail));
            }
        }
        //修改
        public ActionResult Edit(string vercode)
        {
            if (!ModelState.IsValid)
            {
                return View("数据验证不通过", true);
            }
            DEF_TestJobsModel model = m_BLL.GetModelById(vercode);

            if (model.CloseState == true)
            {
                return View("测试任务已关闭", true);
            }
            return View(model);

        }
        //修改
        [HttpPost]
        //[SupportFilter]
        [ValidateInput(false)]
        public ActionResult Edit(DEF_TestJobsModel model)
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
                    LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateFail + "，编辑测试任务ID:" + model.VerCode, "失败", "编辑", "测试任务");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateSucceed + "，编辑测试任务ID:" + model.VerCode, "成功", "编辑", "测试任务");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.UpdateFail));
            }

        }
        //详细
        public ActionResult Details(string vercode)
        {
            if (!ModelState.IsValid)
            {
                return View("数据验证不通过", true);
            }
            DEF_TestJobsModel model = m_BLL.GetModelById(vercode);

            return View(model);

        }

        // 删除 
        [HttpPost]
        public JsonResult Delete(string vercode)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择删除记录"));
                }



                m_BLL.Delete(ref validationErrors, vercode);

                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteFail + "，删除测试任务ID:" + vercode, "失败", "删除", "测试任务");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteSucceed + "，删除测试任务ID:" + vercode, "成功", "删除", "测试任务");
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteFail));
            }
        }
        [HttpPost]
        public JsonResult Copy(string vercode,string newvercode)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择要复制版本"));
                }
                if (string.IsNullOrEmpty(newvercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "新版本号不能为空"));
                }
                if (newvercode == vercode)
                {
                    return Json(JsonHandler.CreateMessage(0, "新版本号不能与原版本相同"));
                }
                m_BLL.CopyTestJobs(ref validationErrors, vercode, newvercode);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，新增测试任务ID:" + newvercode, "失败", "新增", "测试任务");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.InsertSucceed + "，新增测试任务ID:" + newvercode, "失败", "新增", "测试任务");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.InsertFail));
            }
        }
    }
}
