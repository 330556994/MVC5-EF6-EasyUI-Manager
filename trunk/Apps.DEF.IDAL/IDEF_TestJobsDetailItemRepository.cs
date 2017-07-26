using System;
using System.Linq;

using Apps.Models.DEF;
using Apps.Models;
namespace Apps.DEF.IDAL
{
    public partial interface IDEF_TestJobsDetailItemRepository
    {
        int DeleteItems(string[] deleteCollection);
        int Delete(string vercode, string code);
        DEF_TestJobsDetailItem GetByComplexId(string id);
        int Edit(DEF_TestJobsDetailItemModel model);
        DEF_TestJobsDetailItem GetById(string vercode, string code);
        string GetNameById(string vercode, string code);
        IQueryable<DEF_TestJobsDetailItem> GetListByCode(string vercode, string code);
        int DevUpdateState(string vercode, string code);
    }
}
