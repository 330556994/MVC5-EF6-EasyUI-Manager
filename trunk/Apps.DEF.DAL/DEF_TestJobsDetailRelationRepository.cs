
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
    public partial class DEF_TestJobsDetailRelationRepository 
    {
       
        //添加
        public int CreateTestJobsDetailRelationByCode(string vercode,string code)
        {
           
               int result= Context.P_DEF_CreateTestJobsDetailRelationByCode(vercode, code);
               if (result > 0)
               {
                   return 1;
               }
               else
               {
                   return 0;
               }
        }
        //删除
        public int Delete(string vercode, string pcode, string ccode)
        {
                DEF_TestJobsDetailRelation deleteItem = Context.DEF_TestJobsDetailRelation.SingleOrDefault(a => a.VerCode == vercode && a.PCode == pcode && a.CCode == ccode);
                if (deleteItem != null)
                {
                    Context.DEF_TestJobsDetailRelation.Remove(deleteItem);
                    return Context.SaveChanges();
                }
                return 0;
        }
        // 删除对象集合
        public int DeleteItems(string[] deleteCollection)
        {
            IQueryable<DEF_TestJobsDetailRelation> collection = from r in Context.DEF_TestJobsDetailRelation
                                                                where deleteCollection.Contains(r.VerCode+"_"+r.PCode+"_"+r.CCode)
                                                                select r;
            foreach (var deleteItem in collection)
            {
                Context.DEF_TestJobsDetailRelation.Remove(deleteItem);

            }
            return Context.SaveChanges();
        }
        //修改
        public int Edit(DEF_TestJobsDetailRelationModel model)
        {
            using (DBContainer db = new DBContainer())
            {
                var entity = (from a in db.DEF_TestJobsDetailRelation
                              where a.VerCode == model.VerCode
                              where a.PCode==model.PCode
                              where a.CCode==model.CCode
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
                entity.ExSort = model.ExSort;
                return db.SaveChanges();
            }
        }
        //取一个对象
        public DEF_TestJobsDetailRelation GetById(string vercode,string pcode,string ccode)
        {
            using (DBContainer db = new DBContainer())
            {
                return db.DEF_TestJobsDetailRelation.SingleOrDefault(a => a.VerCode==vercode && a.PCode==pcode && a.CCode==ccode);
            }
        }
        //取对象名称
        public string GetNameById(string vercode, string pcode, string ccode)
        {
            using (DBContainer db = new DBContainer())
            {
                var entity = db.DEF_TestJobsDetailRelation.SingleOrDefault(a => a.VerCode == vercode && a.PCode == pcode && a.CCode == ccode);
                return entity == null ? "" : entity.Name;
            }
        }
    }
}
