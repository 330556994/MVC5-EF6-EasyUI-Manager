using System;

using System.Collections.Generic;
using Apps.Models.DEF;
using Apps.Models;
namespace Apps.DEF.IDAL
{
   public partial interface IDEF_DefectRepository
    {
        int CreateDefectReport(string vercode, string creator);
        int Delete(string itemid, string vercode, string code);
        int Edit(DEF_DefectModel model);
        DEF_Defect GetById(string itemid, string vercode, string code);
        string GetNameById(string id);
        DEF_Defect GetByComplexId(string id);
        List<V_DEF_Defect> Query(string queryType, int pageno, int rows, string order, ref int rowscount);
    }
}
