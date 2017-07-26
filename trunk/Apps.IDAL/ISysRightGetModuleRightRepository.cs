using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apps.Models;

namespace Apps.IDAL
{
    public interface ISysRightGetModuleRightRepository
    {
        IQueryable<SysModuleOperate> GetList();
        List<P_Sys_GetModule_RoleRight_Result> GetModuleRoleRight(string moduleId);
        List<P_Sys_GetModule_UserRight_Result> GetModuleUserRight(string moduleId);
    }
}
