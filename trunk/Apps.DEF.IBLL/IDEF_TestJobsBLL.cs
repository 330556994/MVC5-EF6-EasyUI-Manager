using System;
using Apps.Common;
using Apps.Models.DEF;
using System.Collections.Generic;
using Apps.Models;
namespace Apps.DEF.IBLL
{
    public partial interface IDEF_TestJobsBLL
    {
        bool SetCheckFlag(ref ValidationErrors errors, string vercode, bool checkflag);
        bool entityIsExist(string vercode);
        List<DEF_TestJobsModel> GetList(ref GridPager pager, int selShow, string queryStr);
        DEF_TestJobsModel GetModelById(string vercode);
        bool SetCloseTestJobsState(ref ValidationErrors errors, string vercode, bool closeState, string userId);
        bool CreateTestJobs(ref ValidationErrors errors, string vercode);
        bool SetTestJobsDefault(ref ValidationErrors errors, string vercode);
        DEF_TestJobsModel GetDefaultTestJobs(ref ValidationErrors error);
        bool CopyTestJobs(ref ValidationErrors errors, string vercode, string newvercode);
    }
}
