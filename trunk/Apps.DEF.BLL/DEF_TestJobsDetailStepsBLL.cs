using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Common;
using Microsoft.Practices.Unity;


using Apps.DEF.IBLL;
using Apps.Models.DEF;
using Apps.DEF.IDAL;
using Apps.Models;
using Apps.BLL.Core;
using Apps.Locale;
namespace Apps.DEF.BLL
{
    public partial class DEF_TestJobsDetailStepsBLL
    {
        // 数据库访问对象
        [Dependency]
        public IDEF_TestJobsDetailStepsRepository repository { get; set; }
        [Dependency]
        public IDEF_TestJobsDetailItemRepository testItemRep { get; set; }
        [Dependency]
        public IDEF_DefectRepository defectRep { get; set; }

        //检查对象是否存在
        public bool entityIsExist(string id)
        {
            int count = repository.GetList(a => a.ItemID == id).Count();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CreateDefect(ref ValidationErrors errors, DEF_TestJobsDetailStepsModel model,string creator)
        {
            try
            {
                //设置为测试未通过
                model.Result = false;
                if (Create(ref errors, model))
                {
                    defectRep.CreateDefectReport(model.VerCode, creator);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
            }
            return false;

        }
        //新增
        public override bool Create(ref ValidationErrors errors, DEF_TestJobsDetailStepsModel model)
        {
            try
            {
                //测试关键数值是否有效
                DEF_TestJobsDetailItem testItem= testItemRep.GetById(model.VerCode, model.Code);
                if (testItem == null)
                {
                    errors.Add("测试项目不存在");
                    return false;
                }
                //新建对象
                DEF_TestJobsDetailSteps entity = new DEF_TestJobsDetailSteps();

                //实现从模型到对象设置值
                entity.ItemID = model.ItemID;
                entity.VerCode = model.VerCode;
                entity.Code = model.Code;
                entity.Title = model.Title;
                entity.TestContent = model.TestContent;
                entity.Result = model.Result;
                entity.Sort = model.Sort;
                entity.ResultContent = model.ResultContent;
                entity.StepType = model.StepType;
                entity.TestDt = model.TestDt;
                entity.Tester = model.Tester;

                entity.Developer = model.Developer;
                entity.PlanStartDt = model.PlanStartDt;
                entity.PlanEndDt = model.PlanEndDt;
                entity.FinDt = model.FinDt;
                entity.DevFinFlag = model.DevFinFlag;
                entity.TestRequestFlag = model.TestRequestFlag;

                if (repository.Create(entity) )
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
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
            }
            return false;

        }
        /// <summary>
        /// 执行测试
        /// </summary>
        /// <param name="error"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool RunTest(ref ValidationErrors errors, DEF_TestJobsDetailStepsModel model)
        {
            try
            {
                //修改前检查关键字
                
                DEF_TestJobsDetailSteps stepsModel = GetById(model.ItemID,model.VerCode,model.Code);
                if (stepsModel == null)
                {
                    errors.Add("测试步骤不存在");
                    return false;
                }
                stepsModel.ResultContent = model.ResultContent;
                stepsModel.Result = model.Result;

                if (stepsModel.Result == null)
                {
                    stepsModel.TestDt = null;
                    stepsModel.Tester = null;
                }
                else
                {
                    stepsModel.TestDt = model.TestDt;
                    stepsModel.Tester = model.Tester;
                    //更新测试请求状态
                    stepsModel.TestRequestFlag = false;
                }
                
                //修改
                if (!repository.Edit(stepsModel))
                {
                    errors.Add("修改错误!");
                }
                //更新开发状态
                testItemRep.DevUpdateState(model.VerCode, model.Code);
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
        /// 执行开发
        /// </summary>
        /// <param name="error"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool RunDev(ref ValidationErrors errors, DEF_TestJobsDetailStepsModel model)
        {

            DEF_TestJobsDetailSteps entity = repository.GetById(model.ItemID);
            if (entity == null)
            {
                errors.Add(Resource.Disable);
                return false;
            }
            entity.ResultContent = model.ResultContent;
            entity.Result = null;//默认未测试通过
            entity.TestDt = null;
            entity.Tester = null;
            entity.Developer = model.Developer;
            entity.DevFinFlag = model.Result;//完成标识:未完成=false,完成=true
            entity.FinDt = DateTime.Now;
            entity.TestRequestFlag = true;//测试请求标识:不要求测试=false,要求测试=true

            if (repository.Edit(entity))
            {
                return true;
            }
            else
            {
                errors.Add(Resource.NoDataChange);
                return false;
            }

          
        }
              public bool DeleteDefect(ref ValidationErrors errors, string itemid,string vercode ,string code,string creator )
        {
            try
            {
                var model = GetModelById(itemid, vercode, code);
                if (model == null)
                {
                    errors.Add("测试步骤不存在");
                    return false;
                }
                if (model.StepType ==0)
                {
                    errors.Add("系统步骤不能删除，请选择步骤类型为自定义的记录");
                    return false;
                }


                if (repository.Delete(itemid, vercode, code) != 1)
                {
                    errors.Add("删除出错!");
                    return false;
                }else
                {
                    defectRep.CreateDefectReport(model.VerCode, creator);
                    return true;
                }

            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("删除异常");
                return false;
            }

        }

        public bool Delete(ref ValidationErrors errors, string itemid, string vercode, string code)
        {
            try
            {
                var model = GetModelById(itemid, vercode, code);
                if (model == null)
                {
                    errors.Add("测试步骤不存在");
                    return false;
                }

                if (model.Result == false || model.Result == true)
                {
                    errors.Add("测试步骤已执行测试，不能删除");
                    return false;
                }
                if (repository.Delete(itemid, vercode, code) != 1)
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
        //删除
        public bool DeleteCollection(ref ValidationErrors errors, string[] deleteCollection)
        {
            if (deleteCollection != null)
            {
                repository.Delete(deleteCollection);
                try
                {

                    return repository.Delete(deleteCollection)>0;
                }
                catch (Exception ex)
                {
                    errors.Add("删除失败！");
                    ExceptionHander.WriteException(ex);
                    return false;
                }
            }
            return false;
        }
        //根据主键获取对象
        public DEF_TestJobsDetailSteps GetById(string itemid, string vercode, string code)
        {
            return repository.GetById(itemid,vercode,code);
        }

        //根据主键获取模型
        public DEF_TestJobsDetailStepsModel GetModelById(string itemid,string vercode,string code)
        {
            var entity = repository.GetById(itemid, vercode, code);
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

        //返回查询模型列表
        public List<DEF_TestJobsDetailStepsModel> GetList(ref GridPager pager, string querystr, string vercode)
        {
            IQueryable<DEF_TestJobsDetailSteps> queryData = null;
            if (!string.IsNullOrEmpty(querystr))
            {
                queryData = repository.GetList(a => a.Title.Contains(querystr) && a.VerCode == vercode).OrderBy(a => a.Sort);

            }
            else
            {
                queryData = repository.GetList().OrderBy(a => a.Sort);
            }
            return CreateModelList(ref pager, ref queryData);
        }
        public List<DEF_TestJobsDetailStepsModel> GetListByCode(ref GridPager pager, string vercode,string code,string querystr)
        {
            IQueryable<DEF_TestJobsDetailSteps> queryData = null;
            if (!string.IsNullOrWhiteSpace(querystr))
            {
                queryData = repository.GetList(a => a.VerCode == vercode && a.Code == code && a.Developer == querystr).OrderBy(a => a.Sort);
            }
            else
            {
                queryData = repository.GetList(a => a.VerCode == vercode && a.Code == code).OrderBy(a => a.Sort);
            }
            //排序
            if (pager.sort == "asc")
            {
                switch (pager.order)
                {
                    case "TestRequestFlag":
                        queryData = queryData.OrderBy(a => a.TestRequestFlag);
                        break;
                    case "Result":
                        queryData = queryData.OrderBy(a => a.Result);
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
                    case "TestRequestFlag":
                        queryData = queryData.OrderByDescending(a => a.TestRequestFlag);
                        break;
                    case "Result":
                        queryData = queryData.OrderByDescending(a => a.Result);
                        break;
                    default:
                        queryData = queryData.OrderBy(a => a.Sort);
                        break;
                }
            }
            return CreateModelList(ref pager, ref queryData);
        }
        private List<DEF_TestJobsDetailStepsModel> CreateModelList(ref GridPager pager, ref IQueryable<DEF_TestJobsDetailSteps> queryData)
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
            List<DEF_TestJobsDetailStepsModel> modelList = (from r in queryData
                                                            select new DEF_TestJobsDetailStepsModel
                                                            {
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
                                                                TestDt = r.TestDt,
                                                                Tester = r.Tester,
                                                                Developer = r.Developer,
                                                                PlanStartDt = r.PlanStartDt,
                                                                PlanEndDt = r.PlanEndDt,
                                                                FinDt = r.FinDt,
                                                                DevFinFlag = r.DevFinFlag,
                                                                TestRequestFlag = r.TestRequestFlag,
                                                            }).ToList();

            return modelList;
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

                    DEF_TestJobsDetailSteps model = repository.GetById(arrIds[i]);
                    if (model == null)
                    {
                        continue;
                    }
                    if (member != "")
                    {
                        model.Developer = member;
                    }
                    else
                    {
                        model.Developer = null;
                    }
                    if (begintime != "")
                    {
                        try
                        {
                            model.PlanStartDt = Convert.ToDateTime(begintime);
                        }
                        catch
                        {
                            errors.Add("日期不是正确的!格式为：2012-01-02");
                            return false;
                        }
                    }
                    else
                    {
                        model.PlanStartDt = null;
                    }
                    if (endtime != "")
                    {
                        try
                        {
                            model.PlanEndDt = Convert.ToDateTime(endtime);
                        }
                        catch
                        {
                            errors.Add("日期不是正确的!格式为：2012-01-02");
                            return false;
                        }
                    }
                    else
                    {
                        model.PlanEndDt = null;
                    }
                    if (!repository.Edit(model))
                    {
                        errors.Add("批量设置出错!");
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
        //根据主键获取模型
        public DEF_TestJobsDetailStepsModel GetModelByComplexId(string id)
        {
            var entity = repository.GetByComplexId(id);
            if (entity == null)
            {
                return null;
            }
            return GetModelById(entity.ItemID, entity.VerCode, entity.Code);
        }
    }
}
