using System;
using Apps.Models.DEF;
using System.Collections.Generic;
using Apps.Common;
using Apps.Models;
namespace Apps.DEF.IBLL
{
    public partial interface IDEF_TestJobsDetailRelationBLL
    {
        bool CreateRelation(ref ValidationErrors errors, string vercode, string pcode, string ccode);
        bool CreateTestJobsDetailRelationByCode(ref ValidationErrors errors, string vercode, string code);
        bool DeleteByVPCcode(ref ValidationErrors errors, string vercode, string pcode, string ccode);
        bool entityIsExist(string vercode, string pcode, string ccode);
        DEF_TestJobsDetailRelation GetById(string vercode, string pcode, string ccode);
        DEF_TestJobsDetailRelationModel GetModelById(string vercode, string pcode, string ccode);
        List<DEF_TestJobsDetailRelationModel> GetListByCode(ref GridPager pager, string vercode, string code);
    }
}
