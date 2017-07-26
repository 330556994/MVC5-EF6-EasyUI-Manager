using System;
using System.Linq;
using Apps.Common;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Apps.DEF.IBLL;
using Apps.DEF.IDAL;
using Apps.Models.DEF;
using System.Collections.Generic;
using Apps.Web.Core;
using Apps.Locale;


namespace Apps.Web.Areas.DEF.Controllers
{
    public class TestJobsDetailItemController : BaseController
    {
        [Dependency]
        public IDEF_TestJobsDetailItemBLL m_BLL { get; set; }
        [Dependency]
        public IDEF_TestJobsBLL m_testJobsBLL { get; set; }
        //错误集合
        ValidationErrors validationErrors = new ValidationErrors();
        //新增
        [SupportFilter(ActionName="Create")]
        public ActionResult AddTestCase(string vercode)
        {
            ViewBag.perm = GetPermission();
            if (string.IsNullOrEmpty(vercode))
            {
                return View("没有选择测试任务");
            }
         
            var jobsModel = m_testJobsBLL.GetModelById(vercode);
            if (jobsModel == null)
            {
                return View("版本不存在", true);
            }
            
            ViewBag.vercode = vercode;
  
            return View();
        }

        /// <summary>
        /// 新增子关系用例
        /// </summary>
        /// <param name="vercode"></param>
        /// <param name="codes"></param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter(ActionName = "Create")]
        public JsonResult AddTestCase(string vercode,string code)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonHandler.CreateMessage(0, "上传参数错误"));
                }
                if (string.IsNullOrEmpty(vercode))
                {
                    return Json(JsonHandler.CreateMessage(0, "没有选择测试任务"));
                }
                if (string.IsNullOrEmpty(code))
                {
                    return Json(JsonHandler.CreateMessage(0, "没有测试用例"));
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
                string[] arr = code.Split(',');
                foreach (string str in arr)
                {
                    //新增
                    m_BLL.AddTestCase(ref validationErrors, vercode, str);
                }

             
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，新增测试子用例ID:" + vercode + ">" + code, "失败", "新增", "测试明细");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.InsertSucceed + "，新增测试子用例ID:" + vercode + ">" + code, "成功", "新增", "测试明细");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.InsertFail));
            }
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
       

        //JQGrid获取
        [HttpPost]
        public JsonResult GetList(GridPager pager, string queryStr, string vercode, string code)
        {

            List<DEF_TestJobsDetailItemModel> list = m_BLL.GetList(ref pager, queryStr, vercode);
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
        public JsonResult GetListByVerCode(GridPager pager, string vercode)
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

                var colList = m_BLL.GetListByVerCode(ref pager,vercode);
                return CreateJson(ref pager, ref colList);

            }
            catch (Exception ex)
            {
                LogHandler.WriteServiceLog(GetUserId(), "读取出错,查询参数:" + vercode + " 错误：" + ex.Message, "失败", "读取", "测试项目");
                return null;
            }

        }
        [HttpPost]
        public JsonResult GetListByCode(GridPager pager, string vercode, string code)
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

                var colList = m_BLL.GetListByCode(ref pager, vercode,code);
                return CreateJson(ref pager, ref colList);

            }
            catch (Exception ex)
            {
                LogHandler.WriteServiceLog(GetUserId(), "读取出错,查询参数:" + vercode+">"+code + " 错误：" + ex.Message, "失败", "读取", "测试项目");
                return null;
            }

        }
        
   
        private JsonResult CreateJson(ref GridPager pager, ref List<DEF_TestJobsDetailItemModel> colList)
        {
            var jsonData = new
            {
                total = 10,//pager.totalPages,
                page = pager.page,
                records = pager.totalRows,
                rows = (
                    from r in colList
                    select new
                    {
                        i = r.VerCode + "_" + r.Code,
                        cell = new string[]
							{
                                "act",
								//输出结果列表
                                r.VerCode+"_"+r.Code,
								r.VerCode,
								r.Code,
								r.Name,
								r.Description,
								r.Result==null?"":r.Result==true?"true":"false",
								r.Sort.ToString(),
                                r.ExSort.ToString(),
                                r.Lock==true?"true":"false",
				
  
							}
                    }
                ).ToArray()

            };
            return Json(jsonData);
        }
        //新增
        public ActionResult Create()
        {
            return View();
        }

        //创建提交
        [HttpPost]
        public JsonResult Create(DEF_TestJobsDetailItemModel model)
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
                    LogHandler.WriteServiceLog(GetUserId(), Resource.InsertFail + "，新增测试项目ID:" + model.VerCode, "失败", "新增", "测试项目");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.InsertSucceed + "，新增测试项目ID:" + model.VerCode, "成功", "新增", "测试项目");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.InsertFail));
            }
        }
        //修改
        public ActionResult Edit(string vercode, string code)
        {
            if (!ModelState.IsValid)
            {
                return View("数据验证不通过", true);
            }
            DEF_TestJobsDetailItemModel model = m_BLL.GetModelById(vercode, code);

            return View(model);

        }
        //修改
        [HttpPost]
        //[SupportFilter]
        [ValidateInput(false)]
        public ActionResult Edit(DEF_TestJobsDetailItemModel model)
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
                    LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateFail + "，编辑测试项目ID:" + model.VerCode, "失败", "编辑", "测试项目");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.UpdateSucceed + "，编辑测试项目ID:" + model.VerCode, "成功", "编辑", "测试项目");
                return Json(JsonHandler.CreateMessage(1, Resource.UpdateSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, Resource.UpdateFail));
            }

        }
       

        // 删除 
        [HttpPost]
        public JsonResult Delete(string vercode, string code)
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
                if (string.IsNullOrEmpty(code))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择删除记录"));
                }

                m_BLL.Delete(ref validationErrors, vercode, code);

                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteFail + "，删除测试项目ID:" + vercode, "失败", "删除", "测试项目");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), Resource.DeleteSucceed + "，删除测试项目ID:" + vercode, "成功", "删除", "测试项目");
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(1, Resource.DeleteFail));
            }
        }
        /// <summary>
        /// 批量设置开发或测试人员
        /// </summary>
        /// <param name="ids">选中的项目</param>
        /// <param name="member">要分配给谁</param>
        /// <param name="isDev">是否是设置开发</param>
        /// <returns>成功或失败信息</returns>
        [HttpPost]
        public JsonResult SetMember(string id, string member, bool isDev)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Json(JsonHandler.CreateMessage(0, Resource.PlaseChooseToOperatingRecords));
                }


                m_BLL.SetMember(ref validationErrors, member, isDev, id);
                //写日志
                if (validationErrors.Count > 0)
                {
                    //错误写入日志
                    LogHandler.WriteServiceLog(GetUserId(), "批量设置测试负责人/开发负责人出错:" + id, "失败", "批量处理", "测试/开发项目");
                    return Json(JsonHandler.CreateMessage(0, validationErrors.Error));
                }
                //成功写入日志
                LogHandler.WriteServiceLog(GetUserId(), "批量设置测试负责人/开发负责人成功:" + id, "成功", "批量处理", "测试/开发项目");

                return Json(JsonHandler.CreateMessage(1, "设置成功"));
            }
            catch
            {
                return Json(JsonHandler.CreateMessage(0, "分配失败"));
            }
        }
    }
}
