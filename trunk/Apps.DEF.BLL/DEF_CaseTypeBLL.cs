using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using System.Transactions;
using Apps.Models.DEF;
using Apps.DEF.IBLL;
using Apps.DEF.IDAL;
using Apps.BLL.Core;
using Apps.Locale;

namespace Apps.DEF.BLL
{
    public partial class DEF_CaseTypeBLL
    {

        public List<DEF_CaseTypeModel> GetList(string parentId, string allFlag)
        {
            IQueryable<DEF_CaseType> queryData;
            if (string.IsNullOrEmpty(parentId))
            {
                queryData = m_Rep.GetList(a => a.ParentId == "_root");
            }
            else
            {
                queryData = m_Rep.GetList(a => a.ParentId == parentId);
            }
            //获取不包含全部用例的项
            if (!string.IsNullOrEmpty(allFlag))
            {
                queryData = queryData.Where(a => a.Id != "_all");
            }
            return CreateModelList(ref queryData);
        }

    }
}
