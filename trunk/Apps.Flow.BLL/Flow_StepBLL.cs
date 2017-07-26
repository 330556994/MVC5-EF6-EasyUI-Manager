using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using System.Transactions;
using Apps.Models.Flow;
using Apps.Flow.IBLL;
using Apps.Flow.IDAL;
using Apps.BLL.Core;
using Apps.Locale;

namespace Apps.Flow.BLL
{
    public partial class Flow_StepBLL
    {
        public override List<Flow_StepModel> GetList(ref GridPager pager, string formId)
        {

            IQueryable<Flow_Step> queryData = null;

            queryData = m_Rep.GetList(a => a.FormId == formId);
        
            pager.totalRows = queryData.Count();
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
      
    }
}
