using System.Collections.Generic;
using Apps.Common;
using Apps.Models.Sys;
namespace Apps.IBLL
{
   public partial interface ISysPositionBLL
    {
        List<SysPositionModel> GetPosListByDepId(ref GridPager pager, string depId);
     
    }
}
