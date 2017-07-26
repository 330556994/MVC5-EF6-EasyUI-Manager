using System;
using Apps.Common;
using Apps.Models.DEF;
using Apps.Models;
using System.Collections.Generic;
namespace Apps.DEF.IBLL
{
    public partial interface IDEF_TestJobsDetailBLL
    {
        bool CreateTestJobs(ref ValidationErrors errors, string vercode);
        bool Delete(ref ValidationErrors errors, string vercode,string code);
        bool DeleteCollection(ref ValidationErrors errors, string[] deleteCollection);
        bool entityIsExist(string vercode, string code);
        DEF_TestJobsDetail GetById(string vercode, string code);
        List<DEF_TestJobsDetailModel> GetList(ref GridPager pager, string querystr, string vercode);
        DEF_TestJobsDetailModel GetModelById(string vercode, string code);
        bool Create(ref ValidationErrors errors, string vercode, string codes);
        
    }
}
