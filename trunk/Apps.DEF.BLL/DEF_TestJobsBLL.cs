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
namespace Apps.DEF.BLL
{
    public partial class DEF_TestJobsBLL
    {
        // 数据库访问对象
        [Dependency]
        public IDEF_TestJobsRepository repository { get; set; }

        [Dependency]
        public ISysUserRepository userRep{ get; set; }

         
        public bool SetTestJobsDefault(ref ValidationErrors errors, string vercode)
        {
            try
            {
                DEF_TestJobsModel model = GetModelById(vercode);
                if (model == null)
                {
                    errors.Add("测试任务不存在");
                    return false;

                }
               
                if (repository.SetTestJobsDefault(vercode) == 1)
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
        public bool CopyTestJobs(ref ValidationErrors errors, string vercode, string newvercode)
        {
            try
            {
                DEF_TestJobsModel model = GetModelById(vercode);
                if (model == null)
                {
                    errors.Add("测试任务不存在");
                    return false;

                }
                var newJobs =GetModelById(newvercode);
                if (newJobs != null)
                {
                    errors.Add("版本号已存在");
                    return false;
                    
                }

                if (repository.CopyTestJobs(vercode,newvercode) == 1)
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
        /// <summary>
        /// 生成测试任务项目及测试步骤
        /// </summary>
        /// <param name="error"></param>
        /// <param name="vercode"></param>
        /// <returns></returns>
        public bool CreateTestJobs(ref ValidationErrors errors, string vercode)
        {
            try
            {
                DEF_TestJobsModel model = GetModelById(vercode);
                if (model==null)
                {
                    errors.Add("测试任务不存在");
                    return false;

                }
                if (model.CloseState==true)
                {
                    errors.Add("测试任务已关闭，不能再生成测试项目");
                    return false;
                }
                if (repository.CreateTestJobs(vercode) == 1)
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
        //检查对象是否存在
        public bool entityIsExist(string vercode)
        {
            int count = repository.GetList(a => a.VerCode == vercode).Count();
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
        public override bool Create(ref ValidationErrors errors, DEF_TestJobsModel model)
        {
            try
            {
                //测试关键数值是否有效
                if (entityIsExist(model.VerCode))
                {
                    errors.Add("测试任务已存在");
                    return false;

                }
                //新建对象
                DEF_TestJobs entity = new DEF_TestJobs();

                //实现从模型到对象设置值

                entity.VerCode = model.VerCode;
                entity.Name = model.Name;
                entity.Result = model.Result;
                entity.Description = model.Description;
                entity.Creator = model.Creator;
                entity.CrtDt = model.CrtDt;
                entity.CloseState = false;
                entity.Closer = null;
                entity.CloseDt = null;
                entity.CheckFlag = true;

                if (repository.Create(entity))
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
        public override bool Edit(ref ValidationErrors errors, DEF_TestJobsModel model)
        {
            try
            {
                //修改前检查关键字
                DEF_TestJobsModel jobs = GetModelById(model.VerCode);
                if(jobs==null)
                {
                    errors.Add("测试任务不存在");
                    return false;
                }
                if (jobs.CloseState == true)
                {
                    errors.Add("测试任务已关闭不能修改");
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
        public bool SetCheckFlag(ref ValidationErrors errors, string vercode, bool checkflag)
        {
            try
            {
                //修改前检查关键字
                DEF_TestJobsModel model = GetModelById(vercode);
                if (model == null)
                {
                    errors.Add("测试任务不存在");
                    return false;
                }
                model.CheckFlag = checkflag;
                
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
        public bool SetCloseTestJobsState(ref ValidationErrors errors, string vercode,bool closeState,string userId)
        {
            try
            {
                //修改前检查关键字
                DEF_TestJobsModel model = GetModelById(vercode);
                if (model == null)
                {
                    errors.Add("测试任务不存在");
                    return false;
                }
                if (closeState)
                {
                    model.CloseState = closeState;
                    model.Closer = userId;
                    model.CloseDt = DateTime.Now;
                }
                else {
                    model.CloseState = closeState;
                    model.Closer = null;
                    model.CloseDt = null;
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
        public override bool Delete(ref ValidationErrors errors, string vercode)
        {
            try
            {
                DEF_TestJobsModel  model= GetModelById(vercode);
                if (model == null)
                {
                    errors.Add("测试任务不存在");
                    return false;
                }

                if (model.CloseState==true)
                {
                    errors.Add("测试任务已关闭不能删除");
                    return false;
                }
                if (model.CheckFlag==true)
                {
                    errors.Add("测试任务已锁定不能删除");
                    return false;
                }
                if (repository.Delete(vercode) != 1)
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
     

        /// <summary>
        /// 取默认任务
        /// </summary>
        /// <returns></returns>
        public DEF_TestJobsModel GetDefaultTestJobs(ref ValidationErrors errors)
        {
            try
            {
                
                var entity = repository.GetList(a => a.Def == true).SingleOrDefault();
                if (entity == null)
                {
                    return null;
                }
                DEF_TestJobsModel model = new DEF_TestJobsModel();

                //实现对象到模型转换
                model.VerCode = entity.VerCode;
                model.Name = entity.Name;
                model.Result = entity.Result;
                model.Description = entity.Description;
                model.Creator = entity.Creator;
                model.CrtDt = entity.CrtDt;
                model.CloseState = entity.CloseState;
                model.Closer = entity.Closer;
                model.CloseDt = entity.CloseDt;
                model.Def = entity.Def;
                model.CheckFlag = entity.CheckFlag;

                return model;
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("读取默认版本异常!");
                return null;
            }
        }
        //根据主键获取模型
        public DEF_TestJobsModel GetModelById(string vercode)
        {
            var entity = repository.GetById(vercode);
            if (entity == null)
            {
                return null;
            }
            DEF_TestJobsModel model = new DEF_TestJobsModel();

            //实现对象到模型转换
            model.VerCode = entity.VerCode;
            model.Name = entity.Name;
            model.Result = entity.Result;
            model.Description = entity.Description;
            model.Creator = entity.Creator;
            model.CrtDt = entity.CrtDt;
            model.CloseState = entity.CloseState;
            model.Closer = entity.Closer;
            model.CloseDt = entity.CloseDt;
            model.Def = entity.Def;
            model.CheckFlag = entity.CheckFlag;
            return model;
        }

        //返回查询模型列表
        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="selShow">1 为关闭 2 完成 3 未完成</param>
        /// <param name="queryStr"></param>
        /// <returns></returns>
        public List<DEF_TestJobsModel> GetList(ref GridPager pager, int selShow, string queryStr)
        {
            IQueryable<DEF_TestJobs> queryData = null;
            if (!string.IsNullOrEmpty(queryStr))
            {
                queryData = repository.GetList(a => a.VerCode.Contains(queryStr));

            }
            else
            {
                queryData = repository.GetList();
            }
           
            switch (selShow)
            {
                case 1: queryData = queryData.Where(a => a.CloseState == true); break;
                case 2: queryData = queryData.Where(a => a.Result == true); break;
                case 3: queryData = queryData.Where(a => a.Result == false); break;
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
            List<DEF_TestJobsModel> modelList = (from r in queryData
                                                 select new DEF_TestJobsModel
                                                 {
                                                     VerCode = r.VerCode,
                                                     Name = r.Name,
                                                     Result = r.Result,
                                                     Description = r.Description,
                                                     Creator = r.Creator,
                                                     CrtDt = r.CrtDt,
                                                     CloseState = r.CloseState,
                                                     Closer = r.Closer,
                                                     CloseDt = r.CloseDt,
                                                     Def = r.Def,
                                                     CheckFlag = r.CheckFlag,
                                                 }).ToList();

            foreach (var m in modelList)
            {
                m.CreatorTitle = userRep.GetNameById(m.Creator);
                m.CloserTitle = userRep.GetNameById(m.Closer);
            }
            return modelList;
        }
    }
}
