using System;
using Apps.Models.DEF;
using System.Collections.Generic;
using Apps.Common;
using Apps.Models;
namespace Apps.DEF.IBLL
{
    public partial interface IDEF_TestCaseBLL
    {
      
        List<DEF_TestCaseModel> GetList(ref GridPager pager, string querystr, string moduleId);
        DEF_TestCaseModel GetModelById(string code);
        List<DEF_TestCaseModel> GetListByModuleId(ref GridPager pager, string moduleId);
    }
}
