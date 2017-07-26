using System;
using Apps.Models;
using System.Collections.Generic;
using System.Linq;
using Apps.IDAL;
namespace Apps.DAL
{
    public class SysRightGetRoleRightRepository : ISysRightGetRoleRightRepository, IDisposable
    {
         DBContainer db;
         public SysRightGetRoleRightRepository(DBContainer context)
        {
            this.db = context;
        }

        public DBContainer Context
        {
            get { return db; }
        }
        public List<P_Sys_GetRightByRole_Result> GetList(string roleId)
        {
                var roleRight = Context.P_Sys_GetRightByRole(roleId).ToList();
                return roleRight;
        }

        public void Dispose()
        { }
    }

}
