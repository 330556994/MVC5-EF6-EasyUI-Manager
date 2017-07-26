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
namespace Apps.DEF.BLL
{
    public partial class DEF_TestJobsDetailItemBLL 
    {
        // 数据库访问对象
        [Dependency]
        public IDEF_TestJobsDetailItemRepository repository { get; set; }

        [Dependency]
        public IDEF_TestCaseRepository testCaseRep { get; set; }
        [Dependency]
        public IDEF_TestJobsRepository testJobsRep { get; set; }
        [Dependency]
        public IDEF_TestJobsDetailStepsRepository stepsRep { get; set; }
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
     
        //修改
        public override bool Edit(ref ValidationErrors errors, DEF_TestJobsDetailItemModel model)
        {
            try
            {
                //修改前检查关键字
                if (!entityIsExist(model.VerCode, model.Code))
                {
                    errors.Add("测试项目不存在");
                    return false;
                }
                //修改
                if (repository.Edit(model) != 1)
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
        public bool DevUpdateState(ref ValidationErrors errors, string vercode, string code)
        {
            try
            {
                //修改前检查关键字
                var model = GetModelById(vercode, code);

                if (model == null)
                {
                    errors.Add("用例不存在");
                    return false;
                }
                repository.DevUpdateState(vercode, code);
                //更新项目状态及完成时间
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("更新开发状态异常!");
                return false;
            }
        }
        public bool Delete(ref ValidationErrors errors, string vercode, string code)
        {
            try
            {
                var model = GetModelById(vercode, code);
                if (model == null)
                {
                    errors.Add("项目不存在");
                    return false;
                }
                if (model.Result == false || model.Result == true)
                {
                    errors.Add("项目已进行测试不能删除");
                    return false;
                }
                if (repository.Delete(vercode, code) != 1)
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
                    return repository.DeleteItems(deleteCollection)>0;
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
        public DEF_TestJobsDetailItem GetById(string vercode, string code)
        {
            return repository.GetById(vercode, code);
        }

        //根据主键获取模型
        public DEF_TestJobsDetailItemModel GetModelById(string vercode, string code)
        {
            var entity = repository.GetById(vercode, code);
            if (entity == null)
            {
                return null;
            }
            DEF_TestJobsDetailItemModel model = new DEF_TestJobsDetailItemModel();

            //实现对象到模型转换
            model.VerCode = entity.VerCode;
            model.Code = entity.Code;
            model.Name = entity.Name;
            model.Description = entity.Description;
            model.Result = entity.Result;
            model.Sort = entity.Sort;
            model.ExSort = entity.ExSort;
            model.Lock = entity.Lock;
            model.Developer = entity.Developer;
            model.Tester = entity.Tester;
            model.DevFinFlag = entity.DevFinFlag;
            model.TestRequestFlag = entity.TestRequestFlag;
            model.FinDt = entity.FinDt;
            return model;
        }
        public bool AddTestCase(ref ValidationErrors errors,string vercode,string code)
        {
            try
            {
                if (entityIsExist(vercode, code))
                {
                    errors.Add("项目已存在不能再添加");
                    return false;
                }

                var testcase = testCaseRep.GetById(code);
                if (testcase == null)
                {
                    errors.Add("测试用例不存在");
                    return false;
                }

                var testjobs = testJobsRep.GetById(vercode);
                if (testjobs == null)
                {
                    errors.Add("版本号" + vercode + "测试任务不存在");
                    return false;
                }

                var model = new DEF_TestJobsDetailItemModel() {
                    VerCode = vercode,
                    Code=testcase.Code,
                    Name=testcase.Name,
                    Description=testcase.Description,
                    Sort=999,//显示排序号
                };
                return Create(ref errors, model);
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("添加项目异常");
                return false;
            }
        }
        //返回查询模型列表
        public List<DEF_TestJobsDetailItemModel> GetList(ref GridPager pager, string querystr, string vercode)
        {
            IQueryable<DEF_TestJobsDetailItem> queryData = null;
            if (!string.IsNullOrEmpty(querystr))
            {
                queryData = repository.GetList(a => a.VerCode == vercode && (a.Name.Contains(querystr) || a.Code.Contains(querystr))).OrderBy(a => a.Sort);

            }
            else
            {
                queryData = repository.GetList(a => a.VerCode == vercode).OrderBy(a => a.Sort);
            }
            return CreateModelList(ref pager, ref queryData);
        }
        public List<DEF_TestJobsDetailItemModel> GetListByVerCode(ref GridPager pager, string vercode)
        {
            IQueryable<DEF_TestJobsDetailItem> queryData = null;

            queryData = repository.GetList(a => a.VerCode == vercode).OrderBy(a => a.Sort);
            return CreateModelList(ref pager, ref queryData);
        }
        public List<DEF_TestJobsDetailItemModel> GetListByCode(ref GridPager pager, string vercode, string code)
        {
            IQueryable<DEF_TestJobsDetailItem> queryData = null;

            queryData = repository.GetListByCode(vercode,code);
            int count =queryData.Count();
            if (count < 1)
            {
                queryData=repository.GetList(a => a.Code == code && a.VerCode == vercode).OrderBy(a=>a.Sort);
            }
            return CreateModelList(ref pager, ref queryData);
        }
        private List<DEF_TestJobsDetailItemModel> CreateModelList(ref GridPager pager, ref IQueryable<DEF_TestJobsDetailItem> queryData)
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
            List<DEF_TestJobsDetailItemModel> modelList = (from r in queryData
                                                           select new DEF_TestJobsDetailItemModel
                                                           {
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
                                                               DevFinFlag = r.DevFinFlag,
                                                               TestRequestFlag = r.TestRequestFlag,
                                                               FinDt = r.FinDt,
                                                           }).ToList();

            return modelList;
        }
        public bool SetMember(ref ValidationErrors errors, string member, bool isDev, string ids)
        {
            try
            {

                string[] arrIds = ids.Split(',');

                for (int i = 0; i < arrIds.Length; i++)
                {

                    DEF_TestJobsDetailItemModel model = GetModelByComplexId(arrIds[i]);
                    if (model == null)
                    {
                        continue;
                    }
                    if (isDev)
                    {
                        if (member != "")
                        {
                            model.Developer = member;
                        }
                        else
                        {
                            model.Developer = null;
                        }
                    }
                    else
                    {
                        if (member != "")
                        {
                            model.Tester = member;
                        }
                        else
                        {
                            model.Tester = null;
                        }
                    }


                    if (repository.Edit(model) != 1)
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
        public DEF_TestJobsDetailItemModel GetModelByComplexId(string id)
        {
            var entity = repository.GetByComplexId(id);
            if (entity == null)
            {
                return null;
            }
            return GetModelById(entity.VerCode, entity.Code);
        }
    }
}
