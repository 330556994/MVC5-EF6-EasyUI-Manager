
using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Common;
using System.Data;
using Apps.Models.DEF;
using Apps.Models;
using Apps.DEF.IDAL;

namespace Apps.DEF.DAL
{
    public partial class DEF_TestJobsDetailItemRepository
    {
        //添加
        public override bool Create(DEF_TestJobsDetailItem entity)
        {
            Context.DEF_TestJobsDetailItem.Add(entity);

                //增加测试步骤
            var caseSteps = Context.DEF_TestCaseSteps.Where(a => a.Code == entity.Code);
                foreach (var caseStep in caseSteps)
                {

                    var step = new DEF_TestJobsDetailSteps();
                    step.ItemID = caseStep.ItemID;
                    step.VerCode = entity.VerCode;
                    step.Code = caseStep.Code;
                    step.Title = caseStep.Title;
                    step.TestContent = caseStep.TestContent;

                    step.Sort = caseStep.sort;
                    step.StepType = 0;
                    step.TestRequestFlag = false;

                    Context.DEF_TestJobsDetailSteps.Add(step);
                }
                return this.SaveChanges()>0;
        }
        //删除
        public int Delete(string vercode, string code)
        {
            using (DBContainer db = new DBContainer())
            {
                int result = db.P_DEF_DeleteTestJobsItem(vercode, code);
                if (result > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 更新开发状态
        /// </summary>
        /// <param name="vercode"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public int DevUpdateState(string vercode, string code)
        {
            using (DBContainer db = new DBContainer())
            {
                return db.P_DEF_DEV_UpdateState(vercode, code);
            }
        }
        // 删除对象集合
        public int DeleteItems(string[] deleteCollection)
        {
            IQueryable<DEF_TestJobsDetailItem> collection = from r in Context.DEF_TestJobsDetailItem
                                                            where deleteCollection.Contains(r.VerCode + "_" + r.Code)
                                                            select r;
            foreach (var deleteItem in collection)
            {
                Context.DEF_TestJobsDetailItem.Remove(deleteItem);

            }
            return Context.SaveChanges();
        }
        //修改
        public int Edit(DEF_TestJobsDetailItemModel model)
        {
            using (DBContainer db = new DBContainer())
            {
                var entity = (from a in db.DEF_TestJobsDetailItem
                              where a.VerCode == model.VerCode
                              where a.Code==model.Code
                              select a).FirstOrDefault();
                if (entity == null)
                {
                    return 0;
                }
                //给对象赋值
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Result = model.Result;
                entity.Sort = model.Sort;
                entity.Lock = model.Lock;
                entity.Developer = model.Developer;
                entity.Tester = model.Tester;
                entity.DevFinFlag = model.DevFinFlag;
                entity.TestRequestFlag = model.TestRequestFlag;
                entity.FinDt = model.FinDt;

                return db.SaveChanges();
            }
        }
        //取一个对象
        public DEF_TestJobsDetailItem GetById(string vercode, string code)
        {
            using (DBContainer db = new DBContainer())
            {
                return db.DEF_TestJobsDetailItem.SingleOrDefault(a => a.VerCode == vercode && a.Code == code);
            }
        }
        //取对象名称
        public string GetNameById(string vercode, string code)
        {
            using (DBContainer db = new DBContainer())
            {
                var entity = db.DEF_TestJobsDetailItem.SingleOrDefault(a => a.VerCode == vercode && a.Code == code);
                return entity == null ? "" : entity.Name;
            }
        }
        //对象列表集
        //取一个对象
        public DEF_TestJobsDetailItem GetByComplexId(string id)
        {
            using (DBContainer db = new DBContainer())
            {
                return db.DEF_TestJobsDetailItem.SingleOrDefault(a => a.VerCode + "_" + a.Code == id);
            }
        }
        //对象列表集
        public IQueryable<DEF_TestJobsDetailItem> GetListByCode(string vercode, string code)
        {
            IQueryable<DEF_TestJobsDetailItem> list =
                            from i in Context.DEF_TestJobsDetailItem
                            join r in Context.DEF_TestCaseRelation on i.Code equals r.CCode
                            where r.PCode == code
                            where i.VerCode==vercode
                            select i;

            return list;
        }

    }
}
