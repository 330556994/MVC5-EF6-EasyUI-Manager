using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Common;
using Microsoft.Practices.Unity;
using Apps.DEF.IBLL;
using Apps.Models.DEF;
using Apps.DEF.IDAL;
using Apps.BLL.Core;
using Apps.Models;
using Apps.IDAL;
using Apps.MIS.IDAL;
namespace Apps.DEF.BLL
{
    public partial class DEF_DefectBLL
    {
        private const int DELETE_MAX_NUM = 100;

        [Dependency]
        public IDEF_TestJobsRepository testJobsRep { get; set; }

        [Dependency]
        public IDEF_TestCaseRepository testCaseRep { get; set; }

        [Dependency]
        public ISysUserRepository userRep { get; set; }
        [Dependency]
        public IMIS_WebIM_MessageRepository msgRep { get; set; }
        [Dependency]
        public IDEF_TestJobsDetailStepsRepository testJobsDetailStepRep { get; set; }
        //检查对象是否存在
        public bool entityIsExist(string itemid,string vercode ,string code)
        {
            int count = m_Rep.GetList(a => a.ItemID == itemid && a.VerCode==vercode && a.Code==code).Count();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 已审核
        /// </summary>
        /// <param name="error"></param>
        /// <param name="vercode"></param>
        /// <returns></returns>
        //private bool JobsIsChecked(string itemid, string vercode,string code)
        //{
        //    var model = GetModelById(itemid, vercode, code);
        //    if (model==null)
        //    {
        //        return false;
        //    }
        //    if (model.CloseState == true)
        //    {
        //        return true;
        //    }
        //    if (model.CloseState == false)
        //    {
        //        return false;
        //    }
        //    return false;
        //}
        /// <summary>
        /// 任务关闭
        /// </summary>
        /// <param name="error"></param>
        /// <param name="vercode"></param>
        /// <returns></returns>
        private bool CheckJobsIsClosed(ref ValidationErrors errors, string vercode)
        {
            var testJobs = testJobsRep.GetById(vercode);
            if (testJobs == null)
            {
                errors.Add("测试任务不存在");
                return false;
            }
            if (testJobs.CloseState == true)
            {
                errors.Add("测试任务已关闭，不能修改!");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 生成缺陷报告
        /// </summary>
        /// <param name="error"></param>
        /// <param name="vercode"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public bool CreateDefectReport(ref ValidationErrors errors, string vercode, string creator)
        {
            try
            {
                if (!CheckJobsIsClosed(ref errors,vercode))
                {
                    return false;
                }
                if (m_Rep.CreateDefectReport(vercode, creator) != 1)
                {
                    errors.Add("生成缺陷报告错误!");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("删除异常");
                return false;
            }
        }
        //新增
    
        //修改
        public override bool Edit(ref ValidationErrors errors, DEF_DefectModel model)
        {
            try
            {
                //修改前检查关键字
                if (!CheckJobsIsClosed(ref errors, model.VerCode))
                {
                    return false;
                }
                //修改
                if (m_Rep.Edit(model) != 1)
                {
                    errors.Add("修改错误!");
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("修改异常!");
                return false;
            }
        }
        /// <summary>
        /// 更新消息ID
        /// </summary>
        /// <param name="error"></param>
        /// <param name="ids">记录ID</param>
        /// <param name="messageId">已生成的消息ID</param>
        /// <returns></returns>
        public bool SetMessageId(ref ValidationErrors errors, string ids, string messageId, string receiverTitle)
        {
            try
            {

                if (string.IsNullOrEmpty(ids))
                {
                    errors.Add("记录不能为空!");
                    return false;
                }
                if (receiverTitle.Length > 500)
                {
                    receiverTitle = receiverTitle.Substring(0, 499);
                }

                string[] arrIds = ids.Split(',');

                for (int i = 0; i < arrIds.Length; i++)
                {

                    DEF_DefectModel model = GetModelByComplexId(arrIds[i]);
                    if (model == null)
                    {
                        continue;
                    }

                    if (!CheckJobsIsClosed(ref errors, model.VerCode))
                    {
                        return false;
                    }
                    
                    model.MessageId = messageId;
                    model.Receiver = receiverTitle;
                    model.SendDt = DateTime.Now;
                    if (m_Rep.Edit(model) != 1)
                    {
                        errors.Add("设置关闭状态出错!");
                        return false;
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("设置关闭状态异常");
                return false;
            }

        }
        /// <summary>
        /// 缺陷关闭状态
        /// </summary>
        /// <param name="error"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SetDefectCloseState(ref ValidationErrors errors, bool closeState, string ids, string closer)
        {
            try
            {

                if (string.IsNullOrEmpty(ids))
                {
                    errors.Add("记录不能为空!");
                    return false;
                }


                string[] arrIds = ids.Split(',');

                for (int i = 0; i < arrIds.Length; i++)
                {

                    DEF_DefectModel model = GetModelByComplexId(arrIds[i]);
                    if (model == null)
                    {
                        continue;
                    }
                    if (!CheckJobsIsClosed(ref errors, model.VerCode))
                    {
                        return false;
                    }

                    if (closeState)
                    {
                        model.CloseState = closeState;
                        model.Closer = closer;
                        model.CloseDt = DateTime.Now;
                    }
                    else
                    {
                        model.CloseState = closeState;
                        model.Closer = null;
                        model.CloseDt = null;

                    }

                    if (m_Rep.Edit(model) != 1)
                    {
                        errors.Add("设置关闭状态出错!");
                        return false;
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("设置关闭状态异常");
                return false;
            }

        }
        /// <summary>
        /// 设置处理状态
        /// </summary>
        /// <param name="error"></param>
        /// <param name="state"></param>
        /// <param name="ids"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool SetProcessState(ref ValidationErrors errors, bool state, string ids, string userid)
        {
            try
            {

                if (string.IsNullOrEmpty(ids))
                {
                    errors.Add("记录不能为空!");
                    return false;
                }


                string[] arrIds = ids.Split(',');

                for (int i = 0; i < arrIds.Length; i++)
                {

                    DEF_DefectModel model = GetModelByComplexId(arrIds[i]);
                    if (model == null)
                    {
                        continue;
                    }
                    if (!CheckJobsIsClosed(ref errors, model.VerCode))
                    {
                        return false;
                    }

                    if (state)
                    {
                        model.ProcessState = state;
                        model.Processor = userid;
                        model.ProcessDt = DateTime.Now;
                        model.CloseState = true;//默认已审核
                        model.CloseDt = DateTime.Now;
                        model.Closer = userid;
                    }
                    else
                    {
                        model.ProcessState = state;
                        model.Processor = null;
                        model.ProcessDt = null;

                    }

                    if (m_Rep.Edit(model) != 1)
                    {
                        errors.Add("设置处理状态出错!");
                        return false;
                    }
                    //处理成功，相应运行测试模块变更为测试请求为true
                    var entity = testJobsDetailStepRep.GetByComplexId(arrIds[i]);
                    if (entity != null)
                    {
                        DEF_TestJobsDetailSteps tjdsModel = testJobsDetailStepRep.GetById(entity.ItemID, entity.VerCode, entity.Code);
                        tjdsModel.TestRequestFlag = true;

                        testJobsDetailStepRep.Edit(tjdsModel);
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("设置处理状态异常");
                return false;
            }

        }
        //根据主键获取模型
        public DEF_TestJobsDetailStepsModel GetTestJobsDetailStepsModelById(string itemid, string vercode, string code)
        {
            var entity = testJobsDetailStepRep.GetById(itemid, vercode, code);
            if (entity == null)
            {
                return null;
            }
            DEF_TestJobsDetailStepsModel model = new DEF_TestJobsDetailStepsModel();

            //实现对象到模型转换
            model.ItemID = entity.ItemID;
            model.VerCode = entity.VerCode;
            model.Code = entity.Code;
            model.Title = entity.Title;
            model.TestContent = entity.TestContent;
            model.Result = entity.Result;
            model.Sort = entity.Sort;
            model.ResultContent = entity.ResultContent;
            model.ExSort = entity.ExSort;
            model.StepType = entity.StepType;
            model.Tester = entity.Tester;
            model.TestDt = entity.TestDt;

            model.Developer = entity.Developer;
            model.PlanStartDt = entity.PlanStartDt;
            model.PlanEndDt = entity.PlanEndDt;
            model.FinDt = entity.FinDt;
            model.DevFinFlag = entity.DevFinFlag;
            model.TestRequestFlag = entity.TestRequestFlag;
            return model;
        }
        /// <summary>
        /// 设置缺陷错误级别
        /// </summary>
        /// <param name="error"></param>
        /// <param name="errorlevel"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool SetErrorLevel(ref ValidationErrors errors, int errorlevel, string ids)
        {
            try
            {

                if (string.IsNullOrEmpty(ids))
                {
                    errors.Add("记录不能为空!");
                    return false;
                }

                if (errorlevel < 0 || errorlevel > 30)
                {
                    errors.Add("级别要求1~30级");
                    return false;
                
                }

                string[] arrIds = ids.Split(',');

                for (int i = 0; i < arrIds.Length; i++)
                {

                    DEF_DefectModel model = GetModelByComplexId(arrIds[i]);
                    if (model == null)
                    {
                        continue;
                    }
                    if (!CheckJobsIsClosed(ref errors, model.VerCode))
                    {
                        return false;
                    }
                    model.ErrorLevel = errorlevel;

                    if (m_Rep.Edit(model) != 1)
                    {
                        errors.Add("设置级别出错!");
                        return false;
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("设置级别异常");
                return false;
            }

        }
        /// <summary>
        /// 更新备注
        /// </summary>
        /// <param name="error"></param>
        /// <param name="id"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool UpdateRemark(ref ValidationErrors errors, string id, string remark)
        {
            try
            {

                DEF_DefectModel model = GetModelByComplexId(id);
                if (model == null)
                {
                    errors.Add("记录不存在");
                    return false;
                }
                if (!CheckJobsIsClosed(ref errors, model.VerCode))
                {
                    return false;
                }
                model.Remark = remark;


                if (m_Rep.Edit(model) != 1)
                {
                    errors.Add("设置关闭状态出错!");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("设置关闭状态异常");
                return false;
            }

        }

        private int[] ArrayStrToInt(ref ValidationErrors errors, string ids)
        {
            string[] arrIds = ids.Split(',');
            int[] intIds = new Int32[DELETE_MAX_NUM];

            if (arrIds.Length > DELETE_MAX_NUM)
            {
                errors.Add("记录数超过" + DELETE_MAX_NUM + ",请重新选择!");
                return null;
            }
            for (int i = 0; i < arrIds.Length; i++)
            {

                intIds[i] = Int32.Parse(arrIds[i]);
            }
            return intIds;
        }

        public bool DeleteCollection(ref ValidationErrors errors, string ids)
        {
            try
            {

                if (string.IsNullOrEmpty(ids))
                {
                    errors.Add("记录不能为空!");
                    return false;
                }

                string[] arrIds = ids.Split(',');
                if (m_Rep.Delete(arrIds) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                errors.Add("删除失败！");
                ExceptionHander.WriteException(ex);
                return false;
            }

        }
        public bool Delete(ref ValidationErrors errors, string itemid,string vercode ,string code)
        {
            try
            {
                if (!CheckJobsIsClosed(ref errors, vercode))
                {
                    return false;
                }
                //
                if (!entityIsExist(itemid,vercode ,code ))
                {
                    errors.Add("记录不存在");
                    return false;
                }
                if (m_Rep.Delete(itemid,vercode ,code) != 1)
                {
                    errors.Add("删除出错!");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("删除异常");
                return false;
            }

        }

        //根据主键获取对象
        public DEF_Defect GetById(string itemid, string vercode, string code)
        {
            return m_Rep.GetById(itemid, vercode, code);
        }
        //根据主键获取模型
        public DEF_DefectModel GetModelByComplexId(string id)
        {
            var entity = m_Rep.GetByComplexId(id);
            if (entity == null)
            {
                return null;
            }
            return GetModelById(entity.ItemID, entity.VerCode, entity.Code);
        }
        //根据主键获取模型
        public DEF_DefectModel GetModelById(string itemid,string vercode ,string code)
        {
            var entity = m_Rep.GetById(itemid,vercode ,code );
            if (entity == null)
            {
                return null;
            }
            DEF_DefectModel model = new DEF_DefectModel();

            //实现对象到模型转换
            model.ItemID = entity.ItemID;
            model.VerCode = entity.VerCode;
            model.Code = entity.Code;
            model.CaseName = entity.CaseName;
            model.Title = entity.Title;
            model.TestContent = entity.TestContent;
            model.ResultContent = entity.ResultContent;
            model.Creator = entity.Creator;
            model.CrtDt = entity.CrtDt;
            model.Remark = entity.Remark;
            model.Receiver = entity.Receiver;
            model.SendDt = entity.SendDt;
            model.CloseState = entity.CloseState;
            model.Closer = entity.Closer;
            model.CloseDt = entity.CloseDt;
            model.Sort = entity.Sort;
            model.ProcessState = entity.ProcessState;
            model.Processor = entity.Processor;
            model.ProcessDt = entity.ProcessDt;
            model.ErrorLevel = entity.ErrorLevel;
            model.Complex = entity.Complex;
            model.PStartDt = entity.PStartDt;
            model.PEndDt = entity.PEndDt;
            model.Executor = entity.Executor;
            return model;
        }
        private List<DEF_DefectModel> CreateModelList(ref GridPager pager, ref IQueryable<DEF_Defect> queryData)
        {
           
            pager.totalRows = queryData.Count();
            if (pager.totalRows > 0)
            {
                if (pager.page <= 1)
                {
                    queryData = queryData.Take(pager.rows);
                }
                else
                {
                    queryData = queryData.Skip((pager.page - 1) * pager.rows).Take(pager.rows);
                }
            }
            List<DEF_DefectModel> modelList = (from r in queryData
                                               select new DEF_DefectModel
                                               {
                                                   ItemID = r.ItemID,
                                                   VerCode = r.VerCode,
                                                   Code = r.Code,
                                                   CaseName = r.CaseName,
                                                   Title = r.Title,
                                                   TestContent = r.TestContent,
                                                   ResultContent = r.ResultContent,
                                                   Creator = r.Creator,
                                                   CrtDt = r.CrtDt,
                                                   Remark = r.Remark,
                                                   Receiver = r.Receiver,
                                                   SendDt = r.SendDt,
                                                   CloseState = r.CloseState,
                                                   Closer = r.Closer,
                                                   CloseDt = r.CloseDt,
                                                   MessageId = r.MessageId,
                                                   Sort = r.Sort,
                                                   ProcessState = r.ProcessState,
                                                   Processor = r.Processor,
                                                   ProcessDt = r.ProcessDt,
                                                   ErrorLevel = r.ErrorLevel,
                                                   Complex=r.Complex,
                                                   PStartDt=r.PStartDt,
                                                   PEndDt=r.PEndDt,
                                                   Executor=r.Executor,
                                               }).ToList();
            foreach (var m in modelList)
            {
                m.CloserTitle = userRep.GetNameById(m.Closer);
                m.ProcessorTitle = userRep.GetNameById(m.Processor);
                m.ExecutorTitle = userRep.GetNameById(m.Executor);
                m.CreatorTitle = userRep.GetNameById(m.Creator);
                var msg = msgRep.GetMessageBySender(m.MessageId);
                if (msg != null)
                {
                    m.ReceiverTitle = msg.receiverTitle;
                }
            }
            return modelList;
        }
        private List<DEF_DefectModel> CreateModelList(ref GridPager pager, ref List<V_DEF_Defect> queryData)
        {
            List<DEF_DefectModel> modelList = (from r in queryData
                                               select new DEF_DefectModel
                                               {
                                                   ItemID = r.ItemID,
                                                   VerCode = r.VerCode,
                                                   Code = r.Code,
                                                   CaseName = r.CaseName,
                                                   Title = r.Title,
                                                   TestContent = r.TestContent,
                                                   ResultContent = r.ResultContent,
                                                   Creator = r.Creator,
                                                   CrtDt = r.CrtDt,
                                                   Remark = r.Remark,
                                                   Receiver = r.Receiver,
                                                   SendDt = r.SendDt,
                                                   CloseState = r.CloseState,
                                                   Closer = r.Closer,
                                                   CloseDt = r.CloseDt,
                                                   MessageId = r.MessageId,
                                                   Sort = r.Sort,
                                                   ProcessState = r.ProcessState,
                                                   Processor = r.Processor,
                                                   ProcessDt = r.ProcessDt,
                                                   ErrorLevel = r.ErrorLevel,
                                                   Complex = r.Complex,
                                                   PStartDt = r.PStartDt,
                                                   PEndDt = r.PEndDt,
                                                   Executor = r.Executor,
                                               }).ToList();
            foreach (var m in modelList)
            {
                m.CloserTitle = userRep.GetNameById(m.Closer);
                m.ProcessorTitle = userRep.GetNameById(m.Processor);
                m.ExecutorTitle = userRep.GetNameById(m.Executor);
                m.CreatorTitle = userRep.GetNameById(m.Creator);
                var msg = msgRep.GetMessageBySender(m.MessageId);
                if (msg != null)
                {
                    m.ReceiverTitle = msg.receiverTitle;
                }
            }
            return modelList;
        }
        public List<DEF_DefectModel> Query(ref GridPager pager,string vercode, string querystr )
        {
            int rowscount = 0;
            string order = pager.order + " " + pager.sort;

            List<V_DEF_Defect> queryData = m_Rep.Query(querystr, pager.page, pager.rows, order, ref rowscount).ToList();
            pager.totalRows = rowscount;
            //返回列表
            return CreateModelList(ref pager, ref queryData);
        }
        //返回查询模型列表
        public List<DEF_DefectModel> GetList(ref GridPager pager, string querystr, string vercode, bool ok, bool no)
        {
            IQueryable<DEF_Defect> queryData = null;
            if (!string.IsNullOrEmpty(querystr))
            {
                queryData = m_Rep.GetList(a =>
                    (a.Title.Contains(querystr) || a.CaseName.Contains(querystr) 
                    || a.Code.Contains(querystr) ||a.Executor==querystr) 
                    && a.VerCode == vercode);

            }
            else
            {
                queryData = m_Rep.GetList(a => a.VerCode == vercode);
            }
            if (ok == no)
            {
            }
            else if (ok)
            {
                queryData = queryData.Where(a => a.ProcessState == true);
            }
            else
            {
                queryData = queryData.Where(a => (a.ProcessState == null ? false : a.ProcessState) == false);
            }
            //排序
            SortModelList(ref pager, ref queryData);
            //返回列表
            return CreateModelList(ref pager, ref queryData);
        }
        public List<DEF_DefectModel> GetListByVerCode(ref GridPager pager, string vercode, bool ok, bool no)
        {
            IQueryable<DEF_Defect> queryData = queryData = m_Rep.GetList(a => a.VerCode == vercode);

            if (ok == no)
            {
            }
            else if (ok)
            {
                queryData = m_Rep.GetList(a => a.ProcessState == true);
            }
            else
            {
                queryData = m_Rep.GetList(a => (a.ProcessState == null ? false : a.ProcessState) == false);
            }
            //排序
            SortModelList(ref pager, ref queryData);
            //返回列表
            return CreateModelList(ref pager, ref queryData);
        }

        public List<DEF_DefectModel> GetListByVerCode(ref GridPager pager, string vercode)
        {
            IQueryable<DEF_Defect> queryData = queryData = m_Rep.GetList(a => a.VerCode == vercode);
            //排序
            SortModelList(ref pager, ref queryData);
            //返回列表
            return CreateModelList(ref pager, ref queryData);
        }
        /// <summary>
        /// 列表排序
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="queryData"></param>
        private void SortModelList(ref GridPager pager, ref IQueryable<DEF_Defect> queryData)
        {
            //排序
            if (pager.sort == "asc")
            {
                switch (pager.order)
                {
                    case "Title":
                        queryData = queryData.OrderBy(a => a.Title);
                        break;
                    case "ErrorLevel":
                        queryData = queryData.OrderBy(a => a.ErrorLevel);
                        break;
                    case "ProcessState":
                        queryData = queryData.OrderBy(a => a.ProcessState);
                        break;
                    case "PStartDt":
                        queryData = queryData.OrderBy(a => a.PStartDt);
                        break;
                    case "PEndDt":
                        queryData = queryData.OrderBy(a => a.PEndDt);
                        break;
                    case "Executor":
                        queryData = queryData.OrderBy(a => a.Executor);
                        break;
                    case "Complex":
                        queryData = queryData.OrderBy(a => a.Complex);
                        break;
                    case "CloseDt":
                         queryData = queryData.OrderBy(a => a.CloseDt);
                        break;
                    case "Closer":
                        queryData = queryData.OrderBy(a => a.CloseState);
                        break;
                    case "CrtDt":
                        queryData = queryData.OrderBy(a => a.CrtDt);
                        break;
                    case "ProcessDt":
                        queryData = queryData.OrderBy(a => a.ProcessDt);
                        break;
                    case "Processor":
                        queryData = queryData.OrderBy(a => a.Processor);
                        break;
                    default:
                        queryData = queryData.OrderBy(a => a.Sort);
                        break;
                }
            }
            else
            {

                switch (pager.order)
                {
                    case "Title":
                        queryData = queryData.OrderByDescending(a => a.Title);
                        break;
                    case "ErrorLevel":
                        queryData = queryData.OrderByDescending(a => a.ErrorLevel);
                        break;
                    case "ProcessState":
                        queryData = queryData.OrderByDescending(a => a.ProcessState);
                        break;
                    case "PStartDt":
                        queryData = queryData.OrderByDescending(a => a.PStartDt);
                        break;
                    case "PEndDt":
                        queryData = queryData.OrderByDescending(a => a.PEndDt);
                        break;
                    case "Executor":
                        queryData = queryData.OrderByDescending(a => a.Executor);
                        break;
                    case "Complex":
                        queryData = queryData.OrderByDescending(a => a.Complex);
                        break;
                    case "CloseDt":
                        queryData = queryData.OrderByDescending(a => a.CloseDt);
                        break;
                    case "Closer":
                        queryData = queryData.OrderByDescending(a => a.CloseState);
                        break;
                    case "CrtDt":
                        queryData = queryData.OrderByDescending(a => a.CrtDt);
                        break;
                    case "ProcessDt":
                        queryData = queryData.OrderByDescending(a => a.ProcessDt);
                        break;
                    case "Processor":
                        queryData = queryData.OrderByDescending(a => a.Processor);
                        break;
                    default:
                        queryData = queryData.OrderBy(a => a.Sort);
                        break;
                }
            }
        }


        // 设置所有缺陷记录为已审核
        /// </summary>
        /// <param name="error"></param>
        /// <param name="vercode"></param>
        /// <returns></returns>
        public bool CheckAll(ref ValidationErrors errors, string vercode, string userid)
        {
            try
            {
                var jobs = testJobsRep.GetById(vercode);
                if (jobs == null)
                {
                    errors.Add("测试任务不存在");
                    return false;
                }

                var defs = m_Rep.GetList(a => a.VerCode == vercode);
                if (defs == null)
                {
                    errors.Add("没有缺陷记录");
                    return false;
                }

                foreach (var d in defs)
                {
                    d.CloseState = true;
                    d.CloseDt = DateTime.Now;
                    d.Closer = userid;
                }
                
                if (m_Rep.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    errors.Add("更新缺陷记录审核状态错误!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("设置处理状态异常");
                return false;
            }
        }

        //返回查询模型列表
        public List<DEF_DefectModel> GetList2(ref GridPager pager, string querystr, string vercode, bool all, bool ok, bool no)
        {
            IQueryable<DEF_Defect> queryData = null;
            if (!string.IsNullOrEmpty(querystr))
            {

                if (all)
                {

                    if (ok == no)
                    {
                        queryData = m_Rep.GetList(a =>
                    (a.Title.Contains(querystr) || a.CaseName.Contains(querystr)
                    || a.Code.Contains(querystr) || a.Executor == querystr));
                    }
                    else if (ok)
                    {
                        queryData = m_Rep.GetList(a =>
                    (a.Title.Contains(querystr) || a.CaseName.Contains(querystr)
                    || a.Code.Contains(querystr) || a.Executor == querystr) && a.ProcessState == true);
                    }
                    else
                    {
                        queryData = m_Rep.GetList(a =>
                        (a.Title.Contains(querystr) || a.CaseName.Contains(querystr)
                     || a.Code.Contains(querystr) || a.Executor == querystr) && a.ProcessState == false);
                    }

                
                }
                else
                {
                    if (ok == no)
                    {
                        queryData = m_Rep.GetList(a => a.VerCode == vercode);
                    }
                    else if (ok)
                    {
                        queryData = m_Rep.GetList(a => a.ProcessState == true && a.VerCode == vercode);
                    }
                    else
                    {
                        queryData = m_Rep.GetList(a => a.ProcessState == false && a.VerCode == vercode);
                    }
                }
            }
            else
            {
                if (all)
                {

                    if (ok == no)
                    {
                        queryData = m_Rep.GetList();
                    }
                    else if (ok)
                    {
                        queryData = m_Rep.GetList(a => a.ProcessState == true);
                    }
                    else
                    {
                        queryData = m_Rep.GetList(a => a.ProcessState == false);
                    }
                }
                else
                {
                    if (ok == no)
                    {
                        queryData = m_Rep.GetList(a => a.VerCode == vercode);
                    }
                    else if (ok)
                    {
                        queryData = m_Rep.GetList(a => a.ProcessState == true || a.VerCode == vercode);
                    }
                    else
                    {
                        queryData = m_Rep.GetList(a => a.ProcessState == false || a.VerCode == vercode);
                    }
                }
            }

            //排序
            SortModelList(ref pager, ref queryData);
            //返回列表
            return CreateModelList(ref pager, ref queryData);
        }
        public List<DEF_DefectModel> GetListByVerCode2(ref GridPager pager, string vercode, bool all, bool ok, bool no)
        {
            IQueryable<DEF_Defect> queryData;
            if (all)
            {

                if (ok == no)
                {
                    queryData = m_Rep.GetList();
                }
                else if (ok)
                {
                    queryData = m_Rep.GetList(a => a.ProcessState == true);
                }
                else
                {
                    queryData = m_Rep.GetList(a => a.ProcessState == false);
                }
            }
            else
            {
                if (ok == no)
                {
                    queryData = m_Rep.GetList(a => a.VerCode == vercode);
                }
                else if (ok)
                {
                    queryData = m_Rep.GetList(a => a.ProcessState == true && a.VerCode == vercode);
                }
                else
                {
                    queryData = m_Rep.GetList(a => a.ProcessState == false && a.VerCode == vercode);
                }
            }
            //排序
            SortModelList(ref pager, ref queryData);
            //返回列表
            return CreateModelList(ref pager, ref queryData);
        }
        //批量设置
        public bool AllSet(ref ValidationErrors errors, string begintime, string endtime, string member, string ids)
        {
            try
            {
                if (string.IsNullOrEmpty(ids))
                {
                    errors.Add("记录不能为空!");
                    return false;
                }
                string[] arrIds = ids.Split(',');

                for (int i = 0; i < arrIds.Length; i++)
                {

                    DEF_DefectModel model = GetModelByComplexId(arrIds[i]);
                    if (model == null)
                    {
                        continue;
                    }
                    if (!CheckJobsIsClosed(ref errors, model.VerCode))
                    {
                        return false;
                    }

                    if (member != "")
                    {
                        model.Executor = member;
                    }
                    else
                    {
                        model.Executor = null;
                    }
                    if (begintime != "")
                    {
                        try
                        {
                            model.PStartDt = Convert.ToDateTime(begintime);
                        }
                        catch
                        {
                            errors.Add("日期不是正确的!格式为：2012-01-02");
                            return false;
                        }
                    }
                    else
                    {
                        model.PStartDt = null;
                    }
                    if (endtime != "")
                    {
                        try
                        {
                            model.PEndDt = Convert.ToDateTime(endtime);
                        }
                        catch
                        {
                            errors.Add("日期不是正确的!格式为：2012-01-02");
                            return false;
                        }
                    }
                    else
                    {
                        model.PEndDt = null;
                    }
                    if (m_Rep.Edit(model) != 1)
                    {
                        errors.Add("分配执行人员出错!");
                        return false;
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("分配执行人员异常");
                return false;
            }

        }
    }
}
