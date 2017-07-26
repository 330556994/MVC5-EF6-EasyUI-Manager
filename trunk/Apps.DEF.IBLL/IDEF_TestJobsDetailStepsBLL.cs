using System;
using Apps.Common;
using Apps.Models.DEF;
using Apps.DEF.IDAL;
using Apps.Models;
using System.Collections.Generic;
namespace Apps.DEF.IBLL
{
    public partial interface IDEF_TestJobsDetailStepsBLL
    {
        bool AllSet(ref ValidationErrors errors, string begintime, string endtime, string member, string ids);
        bool CreateDefect(ref ValidationErrors errors, DEF_TestJobsDetailStepsModel model, string creator);
       //IDEF_DefectRepository defectRep { get; set; }
        bool Delete(ref ValidationErrors errors, string itemid, string vercode, string code);
        bool DeleteCollection(ref ValidationErrors errors, string[] deleteCollection);
        bool DeleteDefect(ref ValidationErrors errors, string itemid, string vercode, string code,string creator);
        bool entityIsExist(string id);
        DEF_TestJobsDetailSteps GetById(string itemid, string vercode, string code);
        List<DEF_TestJobsDetailStepsModel> GetList(ref GridPager pager, string querystr, string vercode);
        List<DEF_TestJobsDetailStepsModel> GetListByCode(ref GridPager pager, string vercode, string code, string querystr);
        DEF_TestJobsDetailStepsModel GetModelById(string itemid, string vercode, string code);
       //IDEF_TestJobsDetailStepsRepository repository { get; set; }
        bool RunTest(ref ValidationErrors errors, DEF_TestJobsDetailStepsModel model);
       //IDEF_TestJobsDetailItemRepository testItemRep { get; set; }
       bool RunDev(ref ValidationErrors errors, DEF_TestJobsDetailStepsModel model);
    }
}
