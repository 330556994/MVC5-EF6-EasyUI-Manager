using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apps.Models;
using Microsoft.Practices.Unity;
using Apps.IDAL;
using Apps.IBLL;
using Apps.Common;
using Apps.BLL.Core;
using Apps.Models.Sys;
using Apps.Locale;
namespace Apps.BLL
{
    public partial class SysRightBLL
    {
        
        [Dependency]
        public ISysModuleRepository SysModuleRepository { get; set; }

        public int UpdateRight(SysRightOperateModel model)
        {
            return m_Rep.UpdateRight(model);
        }
       
        public List<P_Sys_GetRightByRoleAndModule_Result> GetRightByRoleAndModule(string roleId, string moduleId)
        {
            return m_Rep.GetRightByRoleAndModule(roleId, moduleId);
        }

   
    }
}
