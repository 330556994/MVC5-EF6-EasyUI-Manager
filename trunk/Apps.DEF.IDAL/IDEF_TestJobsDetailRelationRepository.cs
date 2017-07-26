using System;
using Apps.Models.DEF;
using Apps.Models;
namespace Apps.DEF.IDAL
{
    public partial interface IDEF_TestJobsDetailRelationRepository
    {
        int CreateTestJobsDetailRelationByCode(string vercode, string code);
        int DeleteItems( string[] deleteCollection);
        int Delete(string vercode, string pcode, string ccode);
        int Edit(DEF_TestJobsDetailRelationModel model);
        DEF_TestJobsDetailRelation GetById(string vercode, string pcode, string ccode);
        string GetNameById(string vercode, string pcode, string ccode);
    }
}
