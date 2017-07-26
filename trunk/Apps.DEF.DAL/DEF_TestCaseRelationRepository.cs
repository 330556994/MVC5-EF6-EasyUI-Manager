
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
    public partial class DEF_TestCaseRelationRepository{
        //添加
      
        //取一个对象
        public DEF_TestCaseRelation GetById(string pcode, string ccode)
        {
          
                return Context.DEF_TestCaseRelation.SingleOrDefault(a => a.PCode == pcode && a.CCode==ccode);
        }

        public int GetTestCaseRelationByCode(string pcode)
        {
            return Context.DEF_TestCaseRelation.AsQueryable().Where(a => a.PCode == pcode).Count();
        }

        public int DeleteByPcodeCcode(string pcode, string ccode)
        {
            DEF_TestCaseRelation deleteItem = Context.DEF_TestCaseRelation.SingleOrDefault(a => a.PCode == pcode && a.CCode == ccode);
                if (deleteItem != null)
                {
                    Context.DEF_TestCaseRelation.Remove(deleteItem);
                    return Context.SaveChanges();
                }
                return 0;
        }
    }
}
