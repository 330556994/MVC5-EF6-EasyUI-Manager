using System;
using Apps.Common;
using Apps.Models.DEF;
using Apps.Models;
using System.Collections.Generic;
namespace Apps.DEF.IBLL
{
    public partial interface IDEF_TestJobsDetailItemBLL
    {
        bool SetMember(ref ValidationErrors validationErrors, string member, bool isDev, string ids);
        bool DevUpdateState(ref ValidationErrors errors, string vercode, string code);
        bool AddTestCase(ref ValidationErrors errors, string vercode, string code);
        bool Delete(ref ValidationErrors errors, string vercode, string code);
        bool DeleteCollection(ref ValidationErrors errors, string[] deleteCollection);
        bool entityIsExist(string vercode, string code);
        DEF_TestJobsDetailItem GetById(string vercode, string code);
        List<DEF_TestJobsDetailItemModel> GetList(ref GridPager pager, string querystr, string vercode);
        List<DEF_TestJobsDetailItemModel> GetListByCode(ref GridPager pager, string vercode, string code);
        List<DEF_TestJobsDetailItemModel> GetListByVerCode(ref GridPager pager, string vercode);
        DEF_TestJobsDetailItemModel GetModelById(string vercode, string code);
    }
}
