using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apps.IDAL;
using Apps.Models;
using System.Data;

namespace Apps.DAL
{
    public partial class SysUserRepository
    {
        public int GetUserCountByDepId(string depId)
        {
            return Context.P_Sys_GetUserCountByDepId(depId).Cast<int>().First();
        }
      
        public IQueryable<SysRole> GetRefSysRole(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return from m in Context.SysUser
                       from f in m.SysRole
                       where m.Id == id
                       select f;
            }
            return null;
        }

        public IQueryable<P_Sys_GetRoleByUserId_Result> GetRoleByUserId(string userId)
        {
            return Context.P_Sys_GetRoleByUserId(userId).AsQueryable();
        }
        public IQueryable<SysUser> GetListByPosId(string posId)
        {
            return Context.SysUser.Where(a => a.PosId == posId);
        }
        public IQueryable<P_Sys_GetUserByDepId_Result> GetUserByDepId(string depId)
        {
            return Context.P_Sys_GetUserByDepId(depId).AsQueryable();
        }

        public void UpdateSysRoleSysUser(string userId, string[] roleIds)
        {
            Context.P_Sys_DeleteSysRoleSysUserByUserId(userId);
            foreach (string roleid in roleIds)
            {
                if (!string.IsNullOrWhiteSpace(roleid))
                {
                        Context.P_Sys_UpdateSysRoleSysUser(roleid,userId);
                 }
            }
            this.SaveChanges();
        }

       public IQueryable<P_Sys_GetAllUsers_Result> GetAllUsers()
       {
           return Context.P_Sys_GetAllUsers().AsQueryable();
       }



       /// <summary>
        /// 根据ID获取一个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetNameById(string id)
        {
           SysUser user = this.GetById(id);
           if (user != null)
           {
               return user.TrueName;
            }
            return "";
        }

    }
}
