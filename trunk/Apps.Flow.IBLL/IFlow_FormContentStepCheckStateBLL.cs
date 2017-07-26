﻿using System.Collections.Generic;
using Apps.Common;
using Apps.Models.Flow;
namespace Apps.Flow.IBLL
{
    public partial interface IFlow_FormContentStepCheckStateBLL
    {
        List<Flow_FormContentStepCheckStateModel> GetListByStepCheckId(ref GridPager pager, string queryStr);
        Flow_FormContentStepCheckStateModel GetByStepCheckId(string id);
        
    }
}
