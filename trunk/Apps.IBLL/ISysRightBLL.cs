using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apps.Models;
using Apps.Common;
using Apps.Models.Sys;

namespace Apps.IBLL
{
   public partial interface ISysRightBLL
    {
        int UpdateRight(SysRightOperateModel model);

        List<P_Sys_GetRightByRoleAndModule_Result> GetRightByRoleAndModule(string roleId, string moduleId);
    }
}
