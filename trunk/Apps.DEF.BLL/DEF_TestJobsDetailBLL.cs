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
namespace Apps.DEF.BLL
{
    public partial class DEF_TestJobsDetailBLL
    {
        // 数据库访问对象
        [Dependency]
        public IDEF_TestJobsDetailRepository repository { get; set; }
        [Dependency]
        public IDEF_TestJobsRepository testJobsRep { get; set; }
        [Dependency]
        public IDEF_TestCaseRepository testCaseRep { get; set; }
        [Dependency]
        public IDEF_TestJobsDetailRelationRepository testRelationRep { get; set; }
        //检查对象是否存在
        public bool entityIsExist(string vercode, string code)
        {
            int count = repository.GetList(a => a.VerCode == vercode && a.Code == code).Count();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
      
        public bool Create(ref ValidationErrors errors, string vercode, string codes)
        {
            try
            {
                //测试关键数值是否有效
                var testJobs = testJobsRep.GetById(vercode);
                if (testJobs == null)
                {
                    errors.Add("测试任务不存在");
                    return false;
                }
                if (testJobs.Result != null)
                {
                    errors.Add("任务已进行测试，不能新增测试用例");
                    return false;
                }
                string[] arrCode = codes.Split(',');

                //新建对象

                foreach (var code in arrCode)
                {
                    DEF_TestJobsDetail entity = new DEF_TestJobsDetail();
                    DEF_TestCase testCase = testCaseRep.GetById(code);
                    if (testCase == null)
                    {
                        errors.Add("测试用例不存在");
                        return false;
                    }
                    DEF_TestJobsDetail testJobsCase = repository.GetById(vercode, code);
                    if (testJobsCase != null)
                    {
                        //已添加转到下一个
                        continue;
                    }
                    //添加测试明细
                    entity.VerCode = vercode;
                    entity.Code = testCase.Code;
                    entity.Name = testCase.Name;
                    entity.Description = testCase.Description;
                    entity.Result = null;
                    entity.Sort = 0;
                    if (!repository.Create(entity))
                    {
                        errors.Add("添加测试用例失败");
                        return false;
                    }
                    testRelationRep.CreateTestJobsDetailRelationByCode(vercode, code);
                }
                testJobsRep.CreateTestJobs(vercode);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
            }
            return false;

        }
        
       
        public bool CreateTestJobs(ref ValidationErrors errors, string vercode)
        {
            try
            {

               
                if (testJobsRep.CreateTestJobs(vercode) != 1)
                {
                    errors.Add("生成测试项目出错!");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("生成测试项目异常");
                return false;
            }

        }

        public bool Delete(ref ValidationErrors errors, string vercode, string code)
        {
            try
            {
                //
                var jobs = testJobsRep.GetById(vercode);
                if (jobs == null)
                {
                    errors.Add("任务不存在");
                    return false;
                }

                if (jobs.CloseState == true)
                {
                    errors.Add("任务已关闭不存删除");
                    return false;
                }
                if (jobs.Result !=null)
                {
                    errors.Add("任务已进行测试，不能再生成执行项目");
                    return false;
                }
                var model = GetModelById(vercode, code);

                if (model == null)
                {
                    errors.Add("任务明细项不存");
                    return false;
                }
                if (model.Result == false || model.Result == true)
                {
                    errors.Add("任务明细项已测试，不能删除!");
                    return false;
                }
                if (repository.Delete(vercode, code) != 1)
                {
                    errors.Add("删除出错!");
                    return false;
                }
                if (testJobsRep.CreateTestJobs(vercode) != 1)
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
        public DEF_TestJobsDetail GetById(string vercode, string code)
        {
            return repository.GetById(vercode, code);
        }

        //根据主键获取模型
        public DEF_TestJobsDetailModel GetModelById(string vercode, string code)
        {
            var entity = repository.GetById(vercode, code);
            if (entity == null)
            {
                return null;
            }
            DEF_TestJobsDetailModel model = new DEF_TestJobsDetailModel();

            //实现对象到模型转换
            model.VerCode = entity.VerCode;
            model.Code = entity.Code;
            model.Name = entity.Name;
            model.Description = entity.Description;
            model.Result = entity.Result;
            model.Sort = entity.Sort;

            return model;
        }

        //返回查询模型列表
        public List<DEF_TestJobsDetailModel> GetList(ref GridPager pager, string querystr, string vercode)
        {
            IQueryable<DEF_TestJobsDetail> queryData = null;
            if (!string.IsNullOrEmpty(querystr))
            {
                queryData = repository.GetList(a => a.VerCode == vercode && a.Name.Contains(querystr)).OrderBy(a => a.Sort);

            }
            else
            {
                queryData = repository.GetList(a => a.VerCode == vercode).OrderBy(a => a.Sort);
            }
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
            List<DEF_TestJobsDetailModel> modelList = (from r in queryData
                                                       select new DEF_TestJobsDetailModel
                                                       {
                                                           VerCode = r.VerCode,
                                                           Code = r.Code,
                                                           Name = r.Name,
                                                           Description = r.Description,
                                                           Result = r.Result,
                                                           Sort = r.Sort,
                                                       }).ToList();

            return modelList;
        }

    }
}
