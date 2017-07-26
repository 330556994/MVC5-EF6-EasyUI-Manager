
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
    public partial class DEF_TestCaseStepsRepository
    {
        //修改
        public int Edit(DEF_TestCaseStepsModel model)
        {
            using (DBContainer db = new DBContainer())
            {
                var entity = (from a in db.DEF_TestCaseSteps
                              where a.ItemID == model.ItemID
                              select a).FirstOrDefault();
                if (entity == null)
                {
                    return 0;
                }
                //给对象赋值
                entity.Title = model.Title;
                entity.TestContent = model.TestContent;
                entity.sort = model.sort;
                return db.SaveChanges();
            }
        }
 
        //取对象名称
        public string GetNameById(string id)
        {
            var entity = Context.DEF_TestCaseSteps.SingleOrDefault(a => a.ItemID == id);
                return entity == null ? "" : entity.Title;
        }

    }
}
