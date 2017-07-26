using System;
using Apps.Common;
using Apps.Models.DEF;
using Apps.Models;
using System.Collections.Generic;
namespace Apps.DEF.IBLL
{
    public partial interface IDEF_TestCaseRelationBLL
    {
        bool Create(ref ValidationErrors errors, string code, string codeIds);
        bool CreateRepeatRow(ref ValidationErrors errors, string code);
        bool Edit(ref ValidationErrors errors, string id, int sort);
        bool Delete(ref ValidationErrors errors, string pcode, string ccode);
        bool entityIsExist(string pcode, string ccode);
        DEF_TestCaseRelation GetById(string pcode, string ccode);
        DEF_TestCaseRelationModel GetModelById(string pcode, string ccode);
    }
}
