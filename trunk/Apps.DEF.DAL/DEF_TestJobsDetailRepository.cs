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
    public partial class DEF_TestJobsDetailRepository
    {
        public int CreateTestJobsItem(string vercode, string code)
        {
            return Context.P_DEF_CreateTestJobsDetailRelationByCode(vercode, code);
        }
        //删除
        public int DeleteByVerCode(string vercode, string code)
        {
            int result = Context.P_DEF_DeleteTestJobsDetail(vercode, code);
                if (result > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
        }
        // 删除对象集合
        public void DeleteByVerCodes(string[] deleteCollection)
        {
            IQueryable<DEF_TestJobsDetail> collection = from r in Context.DEF_TestJobsDetail
                                                        where deleteCollection.Contains(r.VerCode +"_"+r.Code)
                                                        select r;
            foreach (var deleteItem in collection)
            {
                Context.DEF_TestJobsDetail.Remove(deleteItem);

            }
        }
        //修改
        public int Edit(DEF_TestJobsDetailModel model)
        {

            var entity = (from a in Context.DEF_TestJobsDetail
                              where a.VerCode == model.VerCode
                              where a.Code==model.Code
                              select a).SingleOrDefault();
                if (entity == null)
                {
                    return 0;
                }
                //给对象赋值
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Result = model.Result;
                entity.Sort = model.Sort;
                return this.SaveChanges();

        }
        //取一个对象
        public DEF_TestJobsDetail GetById(string vercode,string code)
        {
            return Context.DEF_TestJobsDetail.SingleOrDefault(a => a.VerCode == vercode && a.Code == code);
  
        }
        //取对象名称
        public string GetNameById(string vercode, string code)
        {

            var entity = Context.DEF_TestJobsDetail.SingleOrDefault(a => a.VerCode == vercode && a.Code == code);
                return entity == null ? "" : entity.Name;
        }
        //对象列表集
    }
}
