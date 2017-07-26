﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using System.Transactions;
using Apps.Models.Sys;
using Apps.IBLL;
using Apps.IDAL;
using Apps.BLL.Core;
using Apps.Locale;

namespace Apps.BLL
{
    public partial class SysModuleOperateBLL
    {
        public override List<SysModuleOperateModel> GetList(ref GridPager pager, string mid)
        {

            IQueryable<SysModuleOperate> queryData = null;
            if (!string.IsNullOrEmpty(mid))
            {
                queryData = m_Rep.GetList(a => a.ModuleId==mid);
            }
            else
            {
                queryData = m_Rep.GetList(a => a.ModuleId == "xxxnull");
            }
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
    }
}
