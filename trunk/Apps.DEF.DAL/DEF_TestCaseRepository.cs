
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
    public partial class DEF_TestCaseRepository
    {
        //删除
        public int Delete(string code)
        {
            DEF_TestCase deleteItem = Context.DEF_TestCase.SingleOrDefault(a => a.Code == code);
                if (deleteItem != null)
                {

                    if (Context.DEF_TestJobsDetail.Where(a => a.Code == code).Count() > 0)
                    {
                        return 0;
                    }
                    if (Context.DEF_TestJobsDetailItem.Where(a => a.Code == code).Count() > 0)
                    {
                        return 0;
                    }

                    var StepsList = Context.DEF_TestCaseSteps.Where(a => a.Code == code);
                    foreach (var en in StepsList)
                    {
                        Context.DEF_TestCaseSteps.Remove(en);
                    }

                    var RelationList = Context.DEF_TestCaseRelation.Where(a => a.CCode == code || a.PCode == code);
                    foreach (var en in RelationList)
                    {
                        Context.DEF_TestCaseRelation.Remove(en);
                    }

                    Context.DEF_TestCase.Remove(deleteItem);
                    return this.SaveChanges();
                }
                return 0;
            
        }

      
        //取对象名称
        public string GetNameById(string code)
        {
            using (DBContainer db = new DBContainer())
            {
                var entity = db.DEF_TestCase.SingleOrDefault(a => a.Code == code);
                return entity == null ? "" : entity.Name;
            }
        }
    }
}
