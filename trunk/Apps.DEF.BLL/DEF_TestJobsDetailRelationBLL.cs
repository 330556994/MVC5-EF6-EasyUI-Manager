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
    public partial class DEF_TestJobsDetailRelationBLL
    {
        // 数据库访问对象
        [Dependency]
        public IDEF_TestJobsDetailRelationRepository repository { get; set; }
        [Dependency]
        public IDEF_TestJobsDetailRepository testJobsDetailRep { get; set; }
        [Dependency]
        public IDEF_TestJobsRepository testJobsRep { get; set; }
        [Dependency]
        public IDEF_TestCaseRepository testCaseRep { get; set; }
        public bool CreateTestJobsDetailRelationByCode(ref ValidationErrors errors, string vercode, string code)
        {
            try
            {

                if (repository.CreateTestJobsDetailRelationByCode(vercode,code) == 1)
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
                return false;
            }
            
        }
        //检查对象是否存在
        public bool entityIsExist(string vercode, string pcode, string ccode)
        {
            int count = repository.GetList(a => a.VerCode == vercode && a.PCode == pcode && a.CCode == ccode).Count();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //新增
        public override bool Create(ref ValidationErrors errors, DEF_TestJobsDetailRelationModel model)
        {
            try
            {
                //测试关键数值是否有效
                if (entityIsExist(model.VerCode, model.PCode, model.CCode))
                {
                    errors.Add("测试任务已存在");
                    return false;

                }
                //新建对象
                DEF_TestJobsDetailRelation entity = new DEF_TestJobsDetailRelation();

                //实现从模型到对象设置值

                entity.VerCode = model.VerCode;
                entity.PCode = model.PCode;
                entity.CCode = model.CCode;
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Result = model.Result;
                entity.Sort = model.Sort;
                entity.ExSort = model.ExSort;

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
        //修改


        public bool DeleteByVPCcode(ref ValidationErrors errors, string vercode, string pcode, string ccode)
        {
            try
            {
                var model = GetModelById(vercode, pcode, ccode);
                if (model == null)
                {
                    errors.Add("记录不存!");
                    return false;
                }
                if (repository.Delete(vercode, pcode, ccode) != 1)
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
        public bool CreateRelation(ref ValidationErrors errors, string vercode, string pcode,string ccode)
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

                //测试关键数值是否有效
                if (entityIsExist(vercode, pcode, ccode))
                {
                    errors.Add("子用例已存在");
                    return false;

                }
                DEF_TestJobsDetail testJobsCase = testJobsDetailRep.GetById(vercode, pcode);
                if (testJobsCase == null)
                {
                    errors.Add("测试用例不存在");
                    return false;
                }
                //新增关系用例
                DEF_TestCase testCase = testCaseRep.GetById(ccode);
                if (testCase == null)
                {
                    errors.Add("测试用例不存在");
                    return false;
                }
                DEF_TestJobsDetailRelationModel relationModel =new DEF_TestJobsDetailRelationModel();

                relationModel.VerCode = vercode;
                relationModel.PCode = pcode;
                relationModel.CCode=ccode;
                relationModel.Name = testCase.Name;
                relationModel.Description = testCase.Description;
                relationModel.Sort = testCase.Sort;

                Create(ref errors, relationModel);

                //生新生成测试项目
                testJobsRep.CreateTestJobs(vercode);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
            }
            return false;

        }
      
        //根据主键获取对象
        public DEF_TestJobsDetailRelation GetById(string vercode, string pcode, string ccode)
        {
            return repository.GetById(vercode, pcode, ccode);
        }

        //根据主键获取模型
        public DEF_TestJobsDetailRelationModel GetModelById(string vercode, string pcode, string ccode)
        {
            var entity = repository.GetById(vercode, pcode, ccode);
            if (entity == null)
            {
                return null;
            }
            DEF_TestJobsDetailRelationModel model = new DEF_TestJobsDetailRelationModel();

            //实现对象到模型转换
            model.VerCode = entity.VerCode;
            model.PCode = entity.PCode;
            model.CCode = entity.CCode;
            model.Name = entity.Name;
            model.Description = entity.Description;
            model.Result = entity.Result;
            model.Sort = entity.Sort;
            model.ExSort = entity.ExSort;

            return model;
        }

       
        public List<DEF_TestJobsDetailRelationModel> GetListByCode(ref GridPager pager, string vercode, string code)
        {
            IQueryable<DEF_TestJobsDetailRelation> queryData = null;
            queryData = repository.GetList(a => a.VerCode == vercode && a.PCode == code).OrderBy(a => a.Sort);


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
            List<DEF_TestJobsDetailRelationModel> modelList = (from r in queryData
                                                               select new DEF_TestJobsDetailRelationModel
                                                               {
                                                                   VerCode = r.VerCode,
                                                                   PCode = r.PCode,
                                                                   CCode = r.CCode,
                                                                   Name = r.Name,
                                                                   Description = r.Description,
                                                                   Result = r.Result,
                                                                   Sort = r.Sort,
                                                                   ExSort = r.ExSort,
                                                               }).ToList();

            return modelList;
        }
    }
}
