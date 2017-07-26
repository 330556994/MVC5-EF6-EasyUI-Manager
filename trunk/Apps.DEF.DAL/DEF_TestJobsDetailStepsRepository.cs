using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Common;
using System.Data;
using Apps.Models;
using Apps.DEF.IDAL;

namespace Apps.DEF.DAL
{
    public partial class DEF_TestJobsDetailStepsRepository
    {
        //删除
        public int Delete(string itemid, string vercode, string code)
        {
            using (DBContainer db = new DBContainer())
            {
                DEF_TestJobsDetailSteps deleteItem = db.DEF_TestJobsDetailSteps.SingleOrDefault(a => a.ItemID == itemid && a.VerCode == vercode && a.Code == code);
                if (deleteItem != null)
                {
                    db.DEF_TestJobsDetailSteps.Remove(deleteItem);
                    return db.SaveChanges();
                }
                return 0;
            }
        }
        //取一个对象
        public DEF_TestJobsDetailSteps GetByComplexId(string id)
        {
            using (DBContainer db = new DBContainer())
            {
                return db.DEF_TestJobsDetailSteps.SingleOrDefault(a => a.ItemID + "_" + a.VerCode + "_" + a.Code == id);
            }
        }

        //取一个对象
        public DEF_TestJobsDetailSteps GetById(string itemid,string vercode ,string code)
        {
            using (DBContainer db = new DBContainer())
            {
                return db.DEF_TestJobsDetailSteps.SingleOrDefault(a => a.ItemID == itemid && a.VerCode==vercode && a.Code==code);
            }
        }
    

        //取对象名称
        public string GetNameById(string id)
        {
            using (DBContainer db = new DBContainer())
            {
                var entity = db.DEF_TestJobsDetailSteps.SingleOrDefault(a => a.ItemID == id);
                return entity == null ? "" : entity.Title;
            }
        }

    }
}
