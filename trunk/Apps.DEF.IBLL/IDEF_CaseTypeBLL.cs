using System.Collections.Generic;
using Apps.Common;
using Apps.Models.DEF;
namespace Apps.DEF.IBLL
{
    public partial interface IDEF_CaseTypeBLL
    {
        List<DEF_CaseTypeModel> GetList(string parentId, string allFlag);
        
    }
}
