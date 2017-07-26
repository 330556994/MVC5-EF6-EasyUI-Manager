using System;
using Apps.Models.DEF;
using Apps.Models;
using System.Linq;
namespace Apps.DEF.IDAL
{
    public partial interface IDEF_TestJobsDetailStepsRepository
    {

        int Delete(string itemid, string vercode, string code);
     
        DEF_TestJobsDetailSteps GetByComplexId(string id);
        DEF_TestJobsDetailSteps GetById(string itemid, string vercode, string code);
        string GetNameById(string id);
    }
}
