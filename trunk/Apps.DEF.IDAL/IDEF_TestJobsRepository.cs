using System;
using Apps.Models.DEF;
using Apps.Models;
namespace Apps.DEF.IDAL
{
    public partial interface IDEF_TestJobsRepository
    {
        int Delete(string vercode);
        int Edit(DEF_TestJobsModel model);
        string GetNameById(string vercode);
        int CreateTestJobs(string vercode);
        int SetTestJobsDefault(string vercode);
        int CopyTestJobs(string vercode, string newvercode);
    }
}
