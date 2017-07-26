using System;
using Apps.Models.DEF;
using Apps.Models;
namespace Apps.DEF.IDAL
{
    public partial interface IDEF_TestJobsDetailRepository
    {
        int CreateTestJobsItem(string vercode, string code);
        void DeleteByVerCodes(string[] deleteCollection);
        int DeleteByVerCode(string vercode, string code);
        int Edit(DEF_TestJobsDetailModel model);
        DEF_TestJobsDetail GetById(string vercode, string code);
        string GetNameById(string vercode, string code);
    }
}
