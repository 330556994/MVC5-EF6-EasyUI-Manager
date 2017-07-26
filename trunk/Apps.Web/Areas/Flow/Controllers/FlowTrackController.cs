﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;
using Apps.IBLL;
using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.Flow.IBLL;
using Apps.Models.Flow;
using System.Text;
using Apps.Flow.BLL;
using System;
using Apps.Web.Core;
using Apps.Models.Enum;
namespace Apps.Web.Areas.Flow.Controllers
{
    public class FlowTrackController : BaseController
    {
        [Dependency]
        public ISysUserBLL userBLL { get; set; }
        [Dependency]
        public IFlow_TypeBLL m_BLL { get; set; }
        [Dependency]
        public IFlow_FormBLL formBLL { get; set; }
        [Dependency]
        public IFlow_FormAttrBLL formAttrBLL { get; set; }
        [Dependency]
        public IFlow_FormContentBLL formContentBLL { get; set; }
        [Dependency]
        public IFlow_StepBLL stepBLL { get; set; }
        [Dependency]
        public IFlow_StepRuleBLL stepRuleBLL { get; set; }
        [Dependency]
        public IFlow_FormContentStepCheckBLL stepCheckBLL { get; set; }
        [Dependency]
        public IFlow_FormContentStepCheckStateBLL stepCheckStateBLL { get; set; }

        ValidationErrors errors = new ValidationErrors();


        [SupportFilter]
        public ActionResult Index()
        {
            

            List<Flow_FormContentModel> list = formContentBLL.GeExaminetList(ref setNoPagerAscById, "");
            foreach (var model in list)
            {
                List<Flow_FormContentStepCheckModel> stepCheckModelList = stepCheckBLL.GetListByFormId(model.FormId, model.Id);
                model.CurrentState = formContentBLL.GetCurrentFormState(model);
            }
            FlowStateCountModel stateModel = new FlowStateCountModel();
            stateModel.requestCount = list.Count();
            stateModel.passCount = list.Where(a => a.CurrentState == (int)FlowStateEnum.Pass).Count();
            stateModel.rejectCount = list.Where(a => a.CurrentState == (int)FlowStateEnum.Reject).Count();
            stateModel.processCount = list.Where(a => a.CurrentState == (int)FlowStateEnum.Progress).Count();
            stateModel.closedCount = list.Where(a => a.TimeOut < DateTime.Now).Count();
            return View(stateModel);
        }
        [HttpPost]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<Flow_FormContentModel> list = formContentBLL.GetList(ref pager, queryStr);
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new Flow_FormContentModel()
                        {

                            Id = r.Id,
                            Title = r.Title,
                            UserId = r.UserId,
                            FormId = r.FormId,
                            FormLevel = r.FormLevel,
                            CreateTime = r.CreateTime,
                            TimeOut = r.TimeOut,
                            CurrentStep = formContentBLL.GetCurrentFormStep(r),
                            CurrentState = formContentBLL.GetCurrentFormState(r)
                           
                        }).ToArray()

            };
            return Json(json);
        }

        public string GetCurrentStep(Flow_FormContentModel model)
        {
            string str = "结束";
            List<Flow_FormContentStepCheckModel> stepCheckModelList = stepCheckBLL.GetListByFormId(model.FormId, model.Id);
            for (int i = stepCheckModelList.Count() - 1; i >= 0; i--)
            {
                if (stepCheckModelList[i].State == 2)
                {
                    str = stepBLL.GetById(stepCheckModelList[i].StepId).Name;
                }
            }
            return str;
        }


        #region 详细
        [SupportFilter(ActionName = "Index")]
        public ActionResult Details(string id)
        {
            
            Flow_FormModel flowFormModel = formBLL.GetById(id);
            //获取现有的步骤
            GridPager pager = new GridPager()
            {
                rows = 1000,
                page = 1,
                sort = "Id",
                order = "asc"
            };
            flowFormModel.stepList = new List<Flow_StepModel>();
            flowFormModel.stepList = stepBLL.GetList(ref pager, flowFormModel.Id);
            for (int i = 0; i < flowFormModel.stepList.Count; i++)//获取步骤下面的步骤规则
            {
                flowFormModel.stepList[i].stepRuleList = new List<Flow_StepRuleModel>();
                flowFormModel.stepList[i].stepRuleList = GetStepRuleListByStepId(flowFormModel.stepList[i].Id);
            }

            return View(flowFormModel);
        }
        //获取步骤下面的规则
        private List<Flow_StepRuleModel> GetStepRuleListByStepId(string stepId)
        {
            List<Flow_StepRuleModel> list = stepRuleBLL.GetList(stepId);
            return list;
        }
        #endregion


        [SupportFilter(ActionName = "Index")]
        public ActionResult Edit(string formId, string id)
        {

            
            Flow_FormModel formModel = formBLL.GetById(formId);
            //是否已经设置布局
            if (!string.IsNullOrEmpty(formModel.HtmlForm))
            {
                ViewBag.Html = formModel.HtmlForm;
            }
            else
            {
                ViewBag.Html = ExceHtmlJs(formId);
            }
            ViewBag.StepCheckMes = formContentBLL.GetCurrentStepCheckMes(ref setNoPagerAscById, formId, id, GetUserId());
            Flow_FormContentModel model = formContentBLL.GetById(id);
            return View(model);
        }

        //根据设定公文，生成表单及控制条件
        private string ExceHtmlJs(string id)
        {
            //定义一个sb为生成HTML表单
            StringBuilder sbHtml = new StringBuilder();
            StringBuilder sbJS = new StringBuilder();
            sbJS.Append("<script type='text/javascript'>function CheckForm(){");
            Flow_FormModel model = formBLL.GetById(id);
            #region 判断流程是否有字段,有就生成HTML表单
            Type formType = model.GetType();
            //查找名称为"A-Z"的属性
            string[] arrStr = { "AttrA", "AttrB", "AttrC", "AttrD", "AttrE", "AttrF", "AttrG", "AttrH", "AttrI", "AttrJ", "AttrK"
                                  , "AttrL", "AttrM", "AttrN", "AttrO", "AttrP", "AttrQ", "AttrR", "AttrS", "AttrT", "AttrU"
                                  , "AttrV", "AttrW", "AttrX", "AttrY", "AttrZ"};
            foreach (string str in arrStr)
            {
                object o = formType.GetProperty(str).GetValue(model, null);
                if (o != null)
                {
                    //查找model类的Class对象的"str"属性的值
                    sbHtml.Append(GetHtml(o.ToString(), str, ref sbJS));
                }
            }


            #endregion
            sbJS.Append("return true}</script>");
            ViewBag.HtmlJS = sbJS.ToString();
            return sbHtml.ToString();
        }

        //对比
        private bool JudgeVal(string attrId, string rVal, string cVal, string lVal)
        {
            string attrType = formAttrBLL.GetById(attrId).AttrType;
            return new FlowHelper().Judge(attrType, rVal, cVal, lVal);
        }





        //获取指定名称的HTML表单
        private string GetHtml(string id, string no, ref StringBuilder sbJS)
        {
            StringBuilder sb = new StringBuilder();
            Flow_FormAttrModel attrModel = formAttrBLL.GetById(id);
            sb.AppendFormat("<tr><td style='width:100px; text-align:right;'>{0} :</td>", attrModel.Title);
            //获取指定类型的HTML表单
            sb.AppendFormat("<td>{0}</td></tr>", new FlowHelper().GetInput(attrModel.AttrType, attrModel.Name, no));
            sbJS.Append(attrModel.CheckJS);
            return sb.ToString();
        }

        
	}
}