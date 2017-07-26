using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;
using Apps.IBLL;
using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.Flow.IBLL;
using Apps.Models.Flow;
using System.Text;
using System;
using Apps.Web.Core;
using Apps.Models.Enum;
using Apps.Locale;
using Apps.Flow.BLL;

namespace Apps.Web.Areas.Flow.Controllers
{
       
        
    public class DrafController : BaseController
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

        public ActionResult Index()
        {
            List<Flow_TypeModel> list = m_BLL.GetList(ref setNoPagerAscBySort, "");
            foreach (var v in list)
            {
                v.formList = new List<Flow_FormModel>();
                List<Flow_FormModel> formList = formBLL.GetListByTypeId(v.Id);
                v.formList = formList;
            }
            ViewBag.DrafList = list;
            return View();

            
        }
       

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(Flow_FormContentModel model)
        {
            //当前的Form模版
            Flow_FormModel formModel = formBLL.GetById(model.FormId);
            //初始化部分数据
            model.Id = ResultHelper.NewId;
            model.CreateTime = ResultHelper.NowTime;
            model.UserId = GetUserId();
            model.Title = formModel.Name;
            model.TimeOut = DateTime.Now.AddDays(30);
            if (model != null && ModelState.IsValid)
            {

                if (formContentBLL.Create(ref errors, model))
                {
                    //创建成功后把步骤取出
                    List<Flow_StepModel> stepModelList = stepBLL.GetList(ref setNoPagerAscBySort,model.FormId);
                    //查询步骤
                    int listCount = stepModelList.Count();
                    bool IsEnd = false;
                    for (int i = 0; i < listCount; i++)
                    {
                        string nextStep  ="";
                        Flow_StepModel stepModel = stepModelList[i];
                        List<Flow_StepRuleModel> stepRuleModelList = stepRuleBLL.GetList(stepModel.Id);
                        //获取规则判断流转方向
                        foreach (Flow_StepRuleModel stepRuleModel in stepRuleModelList)
                        {
                           string val =new FlowHelper().GetFormAttrVal(stepRuleModel.AttrId, formModel, model);
                            //有满足流程结束的条件
                           if (!JudgeVal(stepRuleModel.AttrId, val, stepRuleModel.Operator, stepRuleModel.Result))
                           {
                               if (stepRuleModel.NextStep != "0")
                               {
                                   //获得跳转的步骤
                                   nextStep = stepRuleModel.NextStep;
                                   //跳到跳转后的下一步
                                   for (int j = 0; j < listCount; j++)
                                   {
                                       if (stepModelList[j].Id == nextStep)
                                       {
                                           i = j;//跳到分支后的下一步
                                       }
                                   }
                               }
                               else
                               {
                                   //nextStep=0流程结束
                                   IsEnd = true;
                               }
                           }
                          
                        }
                        
                        #region 插入步骤
                        //插入步骤审核表
                        Flow_FormContentStepCheckModel stepCheckModel = new Flow_FormContentStepCheckModel();
                        stepCheckModel.Id = ResultHelper.NewId;
                        stepCheckModel.ContentId = model.Id;
                        stepCheckModel.StepId = stepModel.Id;
                        stepCheckModel.State = (int)FlowStateEnum.Progress;
                        stepCheckModel.StateFlag = false;//true此步骤审核完成
                        stepCheckModel.CreateTime = ResultHelper.NowTime;
                        if (nextStep != "" && nextStep != "0")
                        {
                            stepCheckModel.IsEnd = (i == listCount) ? true : IsEnd;//是否流程的最后一步
                        }
                        else
                        {
                            stepCheckModel.IsEnd = (i == listCount-1) ? true : IsEnd;//是否流程的最后一步
                        }
                        
                        stepCheckModel.IsCustom = stepModel.FlowRule == (int)FlowRuleEnum.Customer ? true : false;
                        if (stepCheckBLL.Create(ref errors, stepCheckModel))//新建步骤成功
                        {
                            InsertChecker(model, i, stepModel, stepCheckModel);
                        }
                        #endregion
                        #region 插入分支步骤
                        if (nextStep != "" && nextStep != "0")
                        {
                            //不是最后一个审核人
                            if (listCount < 1 || i != listCount - 1)
                            {
                                IsEnd = false;
                            }
                            else
                            {
                                IsEnd = true;
                            }

                            stepCheckModel = new Flow_FormContentStepCheckModel();
                            stepCheckModel.Id = ResultHelper.NewId;
                            stepCheckModel.ContentId = model.Id;
                            stepCheckModel.StepId = stepModel.Id;
                            stepCheckModel.State = (int)FlowStateEnum.Progress;
                            stepCheckModel.StateFlag = false;//true此步骤审核完成
                            stepCheckModel.CreateTime = ResultHelper.NowTime;
                            stepCheckModel.IsEnd = IsEnd;//是否流程的最后一步
                            

                            stepCheckModel.IsCustom = stepModel.FlowRule == (int)FlowRuleEnum.Customer ? true : false;
                            if (stepCheckBLL.Create(ref errors, stepCheckModel))//新建步骤成功
                            {
                                InsertChecker(model, i, stepModel, stepCheckModel);
                            }

                        }
                        #endregion

                        if (IsEnd)//如果是最后一步就无需要下面继续了
                        {
                            break;
                        }
                        IsEnd = true;
                    }



                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",AttrA" + model.AttrA, "成功", "创建", "Flow_FormContent");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",AttrA" + model.AttrA + "," + ErrorCol, "失败", "创建", "Flow_FormContent");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail));
            }
        }

        private void InsertChecker(Flow_FormContentModel model, int i, Flow_StepModel stepModel, Flow_FormContentStepCheckModel stepCheckModel)
        {
            //获得流转规则下的审核人员
            List<string> userIdList = new List<string>();
            if (stepModel.FlowRule == (int)FlowRuleEnum.Customer)
            {
                string[] arrUserList = model.CustomMember.Split(',');
                foreach (string s in arrUserList)
                {
                    userIdList.Add(s);
                }
            }
            else
            {
                userIdList = GetStepCheckMemberList(stepModel.Id, model.Id);
            }

                foreach (string userId in userIdList)
                {
                    //批量建立步骤审核人表
                    Flow_FormContentStepCheckStateModel stepCheckModelState = new Flow_FormContentStepCheckStateModel();
                    stepCheckModelState.Id = ResultHelper.NewId;
                    stepCheckModelState.StepCheckId = stepCheckModel.Id;
                    stepCheckModelState.UserId = userId;
                    stepCheckModelState.CheckFlag = 2;
                    stepCheckModelState.Reamrk = "";
                    stepCheckModelState.TheSeal = "";
                    stepCheckModelState.CreateTime = ResultHelper.NowTime;
                    stepCheckStateBLL.Create(ref errors, stepCheckModelState);
                }
        }

        public List<string> GetStepCheckMemberList(string stepId,string formContentId)
        {
            List<string> userModelList = new List<string>();
            Flow_StepModel model = stepBLL.GetById(stepId);
            if (model.FlowRule == (int)FlowRuleEnum.Lead)
            {
                SysUserModel userModel = userBLL.GetById(GetUserId());
                string[] array = userModel.Lead.Split(',');//获得领导，可能有多个领导
                foreach (string str in array)
                {
                    userModelList.Add(str);
                }
            }
            else if (model.FlowRule == (int)FlowRuleEnum.Position)
            {
                string[] array = model.Execution.Split(',');//获得领导，可能有多个领导
                foreach (string str in array)
                {
                    List<SysUserModel> userList = userBLL.GetListByPosId(str);
                    foreach (SysUserModel userModel in userList)
                    {
                        userModelList.Add(userModel.Id);
                    }
                }
            }
            else if (model.FlowRule == (int)FlowRuleEnum.Department)
            {
                GridPager pager = new GridPager()
                {
                    rows = 10000,
                    page = 1,
                    sort = "Id",
                    order = "desc"
                };
                string[] array = model.Execution.Split(',');//获得领导，可能有多个领导
                foreach (string str in array)
                {
                    List<SysUserModel> userList = userBLL.GetUserByDepId(ref pager, str, "");
                    foreach (SysUserModel userModel in userList)
                    {
                        userModelList.Add(userModel.Id);
                    }
                }
            }
            else if (model.FlowRule == (int)FlowRuleEnum.Person)
            {
                string[] array = model.Execution.Split(',');//获得领导，可能有多个领导
                foreach (string str in array)
                {
                    userModelList.Add(str);
                }
            }
            else if (model.FlowRule == (int)FlowRuleEnum.Customer)
            {
                string users  = formContentBLL.GetById(formContentId).CustomMember;
                string[] array = users.Split(',');//获得领导，可能有多个领导
                foreach (string str in array)
                {
                    userModelList.Add(str);
                }
            }
            return userModelList;
        }
        //对比
        private bool JudgeVal(string attrId, string rVal, string cVal, string lVal)
        {
            string attrType = formAttrBLL.GetById(attrId).AttrType;
            return new FlowHelper().Judge(attrType, rVal, cVal, lVal);
        }


        [SupportFilter]
        public ActionResult Create(string id)
        {
            
            Flow_FormModel formModel = formBLL.GetById(id);
            //是否已经设置布局
            if (!string.IsNullOrEmpty(formModel.HtmlForm))
            {
                ViewBag.Html = formModel.HtmlForm;
            }
            else
            {
                ViewBag.Html = ExceHtmlJs(id);
            }
            Flow_FormContentModel model = new Flow_FormContentModel();
            model.FormId = id;
            //创建成功取出步骤
            List<Flow_StepModel> stepModelList = stepBLL.GetList(ref setNoPagerAscById, model.FormId);
            Flow_StepModel stepModel = stepBLL.GetById(stepModelList[0].Id);
            if (stepModel.FlowRule == (int)FlowRuleEnum.Customer)
            {
                ViewBag.Checker = null;
            }
            else
            {
                List<string> users = GetStepCheckMemberList(stepModel.Id,id);
                ViewBag.Checker = users;
            }
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
            //获得对象的类型，model
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
                    sbHtml.Append(JuageExc(o.ToString(), str, ref sbJS));
                }
            }
            #endregion
            sbJS.Append("return true}</script>");
            return sbHtml.ToString()+sbJS.ToString();
        }

        private string JuageExc(string attr, string no,ref StringBuilder sbJS)
        {

            if (!string.IsNullOrEmpty(attr))
            {
                return GetHtml(attr, no, ref sbJS);

            }
            return "";
        }

      


        //获取指定名称的HTML表单
        private string GetHtml(string id, string no, ref StringBuilder sbJS)
        {
            StringBuilder sb = new StringBuilder();
            Flow_FormAttrModel attrModel = formAttrBLL.GetById(id);
            sb.AppendFormat("<tr><th>{0} :</th>", attrModel.Title);
            //获取指定类型的HTML表单
            sb.AppendFormat("<td>{0}</td></tr>", new FlowHelper().GetInput(attrModel.AttrType, attrModel.Name, no));
            sbJS.Append(attrModel.CheckJS);
            return sb.ToString();
        }
    }
}
