using System;
using System.Linq;
using Apps.Common;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Apps.DEF.IBLL;
using Apps.DEF.IDAL;
using Apps.Models.DEF;
using System.Collections.Generic;
using Apps.MIS.BLL;
using Apps.IBLL;
using Apps.Web.Core;
using Apps.Locale;
using Apps.MIS.IBLL;


namespace Apps.Web.Areas.DEF.Controllers
{
    public class DefectController : BaseController
    {
        public const int MAX_LENGTH = 8000;
        [Dependency]
        public IDEF_DefectBLL m_BLL { get; set; }
        [Dependency]
        public IDEF_TestJobsBLL m_testJobsBLL { get; set; }

        [Dependency]
        public IMIS_WebIM_MessageBLL m_msgBLL { get; set; }
        [Dependency]
        public ISysUserBLL m_userBLL { get; set; }
        //错误集合
        ValidationErrors validationErrors = new ValidationErrors();
        /// <summary>
        /// 全能查询
        /// </summary>
        [HttpPost]
        public JsonResult Query(GridPager pager, string vercode ,string sql)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(sql))
                    {
                        return Json(JsonHandler.CreateMessage(0, "查询参数不能为空"));
                    }
                    string msg = "";
                    if (!ResultHelper.ValidateSQL(sql, ref msg))
                    {
                        return Json(msg);
                    }
                    if (string.IsNullOrEmpty(vercode))
                    {
                        return Json(JsonHandler.CreateMessage(0, "没有选择测试任务"));
                    }

                    sql= sql + " and vercode='" + vercode + "'";
                    var colList = m_BLL.Query(ref pager,vercode, sql);
                    return CreateJsonList(ref pager, ref colList);
                }

            }
            catch
            {
                return Json(0);
            }
            return Json(0);

        }
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
        [HttpPost]
        //[SupportFilter]
        public JsonResult UpdateExecutor(string id, string executor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }

                if (string.IsNullOrEmpty(id))
                {
                    return Json(JsonHandler.CreateMessage(0, "记录编码无效"));
                }
                if (string.IsNullOrEmpty(executor))
                {
                    return Json(JsonHandler.CreateMessage(0, "用户不能为空"));
                }
                var defect = m_BLL.GetModelByComplexId(id);
                if (defect == null)
                {
                    return Json(JsonHandler.CreateMessage(0, "缺陷记录不存在"));
                }


                var user = m_userBLL.GetById(executor);
                if (user == null)
                {
                    return Json(JsonHandler.CreateMessage(0, "用户不存在"));
                }
                //更新内容
                defect.Executor = executor;


                m_BLL.Edit(ref validationErrors, defect);

                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), "修改出错:" + id, "失败", "读取", "缺陷报告");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), "修改成功:" + id, "成功", "读取", "缺陷报告");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.UpdateFail));
            }

        }
        [HttpPost]
        //[SupportFilter]
        [ValidateInput(false)]
        public JsonResult UpdateRemark(string id, string remark)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }

                if (string.IsNullOrEmpty(id))
                {
                    return Json(JsonHandler.CreateMessage(0, "记录编码无效"));
                }
                if (!string.IsNullOrEmpty(remark))
                {
                    if (remark.Length > MAX_LENGTH)
                    {
                        return Json(JsonHandler.CreateMessage(0, "内容字数不能超过" + MAX_LENGTH));
                    }
                }
                m_BLL.UpdateRemark(ref validationErrors, id, remark);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), "关闭缺陷状态出错:" + id, "失败", "新增", "缺陷报告");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), "关闭缺陷状态成功:" + id,  "成功", "新增", "缺陷报告");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.UpdateFail));
            }

        }
        [HttpPost]
        //[SupportFilter]
        [ValidateInput(false)]
        public JsonResult SendMessage(string ids, string message, string receiver, string receiverTitle)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                string messageId =m_msgBLL.CreateMessage(ref validationErrors, message, GetUserId(), receiver, receiverTitle);
                if (!string.IsNullOrEmpty(messageId))
                {
                    //更新记录的信息ID及Receiver
                    m_BLL.SetMessageId(ref validationErrors, ids, messageId, receiverTitle);
                }
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), "发送消息:" + receiverTitle, "失败", "新增", "消息系统");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), "发送消息:" + receiverTitle, "成功", "新增", "消息系统");
                return Json(JsonHandler.CreateMessage(1, "发送消息成功"));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, "发送消息异常"));
            }

        }
        [HttpPost]
        //[SupportFilter]
        public JsonResult SetErrorLevel(string ids, int errorlevel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(ids))
                {
                    return Json(JsonHandler.CreateMessage(0, Resource.PlaseChooseToOperatingRecords));
                }


                m_BLL.SetErrorLevel(ref validationErrors, errorlevel, ids);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), "设置错误级别出错:" + ids, "失败", "修改", "缺陷报告");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), "设置错误级别成功:" + ids, "成功", "修改", "缺陷报告");

                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.UpdateFail));
            }

        }

        [HttpPost]
        //[SupportFilter]
        public JsonResult CheckAll(string vercode)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, Resource.PlaseChooseToOperatingRecords));
                }


                m_BLL.CheckAll(ref validationErrors, vercode, GetUserId());
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), "设置全部审核出错:" + vercode, "失败", "修改", "缺陷报告");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), "设置全部审核成功:" + vercode, "成功", "修改", "缺陷报告");

                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.UpdateFail));
            }

        }


        [HttpPost]
        //[SupportFilter]
        public JsonResult SetProcessState(string ids, bool state)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(ids))
                {
                    return Json(JsonHandler.CreateMessage(0, Resource.PlaseChooseToOperatingRecords));
                }


                m_BLL.SetProcessState(ref validationErrors, state, ids, GetUserId());
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), "处理状态出错:" + ids, "失败", "处理状态", "缺陷报告");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), "处理状态成功:" + ids, "成功", "处理状态", "缺陷报告");

                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }

        }
        [HttpPost]
        //[SupportFilter]
        public JsonResult SetDefectCloseState(string ids, bool closeState)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(ids))
                {
                    return Json(JsonHandler.CreateMessage(0, Resource.PlaseChooseToOperatingRecords));
                }


                m_BLL.SetDefectCloseState(ref validationErrors, closeState, ids, GetUserId());
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), "关闭缺陷状态出错:" + ids, "失败", "新增", "缺陷报告");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), "关闭缺陷状态成功:" + ids, "成功", "新增", "缺陷报告");

                return Json(JsonHandler.CreateMessage(1, closeState ? "审核成功" :"反审核成功"));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, closeState ? "审核异常" : "反审核异常"));
            }

        }

        private JsonResult CreateJsonList(ref GridPager pager, ref List<DEF_DefectModel> list)
        {


            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new DEF_DefectModel()
                        {
                            Id = r.ItemID + "_" + r.VerCode + "_" + r.Code,
                            ItemID = r.ItemID,
                            VerCode = r.VerCode,
                            Code = r.Code,
                            CaseName = r.CaseName,
                            Title = r.Title,
                            Creator = r.Creator,
                            CrtDt = r.CrtDt,
                            Receiver = r.Receiver,
                            SendDt = r.SendDt,
                            CloseState = r.CloseState,
                            Closer = r.Closer,
                            CloseDt = r.CloseDt,
                            TestContent = r.TestContent,
                            ResultContent = r.ResultContent,
                            Remark = r.Remark,
                            MessageId = r.MessageId,
                            Sort = r.Sort,
                            ProcessState = r.ProcessState,
                            Processor = r.Processor,
                            ProcessDt = r.ProcessDt,
                            ErrorLevel = r.ErrorLevel,
                            Complex = r.Complex,
                            PStartDt = r.PStartDt,
                            PEndDt = r.PEndDt,
                            Executor = r.Executor

                        }).ToArray()

            };

            return Json(json);

        }
        //JQGrid获取
        [HttpPost]
        public JsonResult GetList(GridPager pager, string querystr, string vercode, bool ok, bool no)
        {

            try
            {
                var colList = m_BLL.GetList(ref pager, querystr, vercode, ok, no);
                return CreateJsonList(ref pager, ref colList);

            }
            catch (Exception ex)
            {
                LogHandler.WriteServiceLog(GetUserId(), "读取出错,查询参数:" + querystr + " 错误：" + ex.Message, "失败", "读取", "缺陷报告");
                return null;
            }

        }
        [HttpPost]
        public JsonResult GetListByVerCode(GridPager pager, string vercode)
        {

            try
            {
                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "没有选择测试任务"));
                }

                var colList = m_BLL.GetListByVerCode(ref pager, vercode);
                return CreateJsonList(ref pager, ref colList);

            }
            catch (Exception ex)
            {
                LogHandler.WriteServiceLog(GetUserId(), "读取出错,查询参数:" + vercode + " 错误：" + ex.Message, "失败", "读取", "缺陷报告");
                return null;
            }

        }
        [HttpPost]
        public JsonResult GetListByVerCode3(GridPager pager, string vercode, bool ok, bool no)
        {

            try
            {
                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "没有选择测试任务"));
                }

                var colList = m_BLL.GetListByVerCode(ref pager, vercode, ok, no);
                return CreateJsonList(ref pager, ref colList);

            }
            catch (Exception ex)
            {
                LogHandler.WriteServiceLog(GetUserId(), "读取出错,查询参数:" + vercode + " 错误：" + ex.Message, "失败", "读取", "缺陷报告");
                return null;
            }

        }
        //新增
        public ActionResult Create()
        {
            return View();
        }

        //创建提交
        [HttpPost]
        public JsonResult Create(DEF_DefectModel model)
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
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，新增缺陷报告ID:" + model.ItemID, "失败", "新增", "缺陷报告");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.InsertSucceed + "，新增缺陷报告ID:" + model.ItemID, "成功", "新增", "缺陷报告");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.InsertFail));
            }
        }

        /// <summary>
        /// 生成缺陷报告
        /// </summary>
        /// <param name="vercode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateDefectReport(string vercode)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }

                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(1, "没有选择测试任务"));
                }

                m_BLL.CreateDefectReport(ref validationErrors, vercode, GetUserId());
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，新增缺陷报告ID:" + vercode, "失败", "新增", "缺陷报告");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.InsertSucceed + "，新增缺陷报告ID:" + vercode, "成功", "新增", "缺陷报告");
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
            DEF_DefectModel model = m_BLL.GetModelByComplexId(id);

            return View(model);

        }
        //修改
        [HttpPost]
        //[SupportFilter]
        public ActionResult Edit(DEF_DefectModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }

                var defect = m_BLL.GetModelById(model.ItemID, model.VerCode, model.Code);
                if (defect == null)
                {
                    return Json(JsonHandler.CreateMessage(0, "缺陷记录不存在"));
                }

                //更新内容
                defect.Complex = model.Complex;
                defect.PStartDt = model.PStartDt;
                defect.PEndDt = model.PEndDt;
                defect.Remark = model.Remark;

                m_BLL.Edit(ref validationErrors, defect);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateFail + "，编辑缺陷报告ID:" + model.ItemID, "失败", "编辑", "缺陷报告");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateSucceed + "，编辑缺陷报告ID:" + model.ItemID, "成功", "编辑", "缺陷报告");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.UpdateFail));
            }

        }
        //详细
        public ActionResult Details(string id)
        {
            if (!ModelState.IsValid)
            {
                return View("数据验证不通过", true);
            }
            DEF_DefectModel model = m_BLL.GetModelByComplexId(id);

            return View(model);

        }

        // 删除 
        [HttpPost]
        public JsonResult Delete(string ids)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(ids))
                {
                    return Json(JsonHandler.CreateMessage(0, Resource.PlaseChooseToOperatingRecords));
                }


                m_BLL.DeleteCollection(ref validationErrors, ids);

                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteFail + "，删除缺陷报告ID:" + ids, "失败", "新增", "缺陷报告");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteSucceed + "，删除缺陷报告ID:" + ids, "成功", "新增", "缺陷报告");
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteFail));
            }
        }

        [HttpPost]
        //[SupportFilter]
        public JsonResult AllSet(string ids,string begintime,string endtime, string member)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "数据验证不通过"));
                }
                if (string.IsNullOrEmpty(ids))
                {
                    return Json(JsonHandler.CreateMessage(0, Resource.PlaseChooseToOperatingRecords));
                }


                m_BLL.AllSet(ref validationErrors,begintime,endtime, member, ids);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), "批量设置出错:" + ids, "失败", "批量处理", "缺陷报告");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), "批量设置成功:" + ids, "成功", "批量处理", "缺陷报告");

                return Json(JsonHandler.CreateMessage(1, "设置成功"));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, "分配失败"));
            }

        }

    }
}
