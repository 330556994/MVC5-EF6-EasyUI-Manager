using System;
using System.Linq;
using System.Data;
using Apps.Models;
using Apps.IDAL;
using System.Collections.Generic;
using System.Data.Entity;

namespace Apps.DAL
{
    public class WebpartRepository : IDisposable, IWebpartRepository
    {
        DBContainer db;
        public WebpartRepository(DBContainer context)
        {
            this.db = context;
        }

        public DBContainer Context
        {
            get { return db; }
        }
        /// <summary>
        /// 获取最新信息
        /// </summary>
        /// <param name="top"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<P_Sys_WebPart_Result> GetPartData3(int top, string userId)
        {
            
             return Context.P_WebPart_GetInfo(top, userId).AsQueryable().ToList();
        }
        /// <summary>
        /// 获取最新共享
        /// </summary>
        /// <param name="top"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public List<P_Mis_FileGetMyReadFile_Result> GetPartData8(int top, string userId)
        //{
        //    using (SysEntities db = SysDb.getDB())
        //    {
        //        return db.P_WebPart_GetShareFileByUserId(userId, top).AsQueryable().ToList();
        //    }
        //}
        /// <summary>
        /// 新建或修改HTML
        /// </summary>
        /// <param name="html"></param>
        public int SaveHtml(string userId,string html)
        {
            SysUserConfig ss = GetByIdAndUserId("webpart", userId);
            if ( ss== null)
            {
                ss = new SysUserConfig();
                ss.Id = "webpart";
                ss.UserId = userId;
                ss.Value = html;
                ss.Type = "webpart";
                ss.State = true;
                ss.Name = "自由桌面";
                Context.SysUserConfig.Add(ss);
            }
            else
            {
                ss.Value = html;
                new SysUserConfigRepository(Context).Edit(ss);
            }
            return Context.SaveChanges();
            
        }

        public SysUserConfig GetByIdAndUserId(string id, string userId)
        {

            return Context.SysUserConfig.SingleOrDefault(a => a.Id == id && a.UserId == userId);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {

            if (disposing)
            {
                Context.Dispose();
            }
        }
    }
}
