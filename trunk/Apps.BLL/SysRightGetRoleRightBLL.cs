using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apps.IBLL;
using Microsoft.Practices.Unity;
using Apps.IDAL;
using Apps.Models;
using Apps.Common;
using Apps.BLL.Core;
using Apps.Models.Sys;
using System.Transactions;
namespace Apps.BLL
{
    public class SysRightGetRoleRightBLL :  ISysRightGetRoleRightBLL
    {
        [Dependency]
        public ISysRightGetRoleRightRepository sysRightGetRoleRightRepository { get; set; }

        public List<P_Sys_GetRightByRole_Result> GetList(string userId)
        {
            return sysRightGetRoleRightRepository.GetList(userId);
        }
        
    }
}
