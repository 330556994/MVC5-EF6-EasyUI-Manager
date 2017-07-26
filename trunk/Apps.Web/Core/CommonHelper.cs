
using Apps.IBLL;
using Apps.Models;
using Apps.Models.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Apps.Common;
using Apps.DAL;
namespace Apps.Web.Core
{
    public class CommonHelper
    {

        #region 获取多选不带人员的组织架构
        public string GetStructMulTree()
        {
            StringBuilder sb = new StringBuilder();
            using (SysStructRepository structRep = new SysStructRepository(new DBContainer()))
            {
                IQueryable<SysStruct> queryData = structRep.GetList();
                IQueryable<SysStruct> query = queryData.Where(a => a.ParentId == "0").OrderBy(a => a.Sort);
                sb.Append("<ul id=\"StructMulTree\" class=\"easyui-tree\"  data-options=\"checkbox:true\">");
                foreach (var l in query)
                {
                    sb.Append("<li data-options=\"attributes:{'id':'" + l.Id + "'}\">");
                    sb.AppendFormat("<span>{0}</span>", l.Name);
                    sb.Append(GetStructLayout(queryData, l.Id,false));
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
            }
            return sb.ToString();
        }
        #endregion


        #region 组织架构
        public string GetStructTree(bool isCount)
       {
           StringBuilder sb = new StringBuilder();
           using (SysStructRepository structRep = new SysStructRepository(new DBContainer()))
           {
               IQueryable<SysStruct> queryData = structRep.GetList();
               IQueryable<SysStruct> query = queryData.Where(a => a.ParentId == "0").OrderBy(a => a.Sort);
               sb.Append("<ul id=\"StructTree\" class=\"easyui-tree\"  data-options=\"onClick:function(node){ getSelected();}\">");
               foreach (var l in query)
               {
                   sb.Append("<li data-options=\"attributes:{'id':'"+l.Id+"'}\">");
                   if (isCount)
                   {
                       sb.AppendFormat("<span>{0} ({1})</span>", l.Name, GetMemberCount(l.Id));
                   }
                   else
                   {
                       sb.AppendFormat("<span>{0}</span>", l.Name);
                   }
                   sb.Append(GetStructLayout(queryData, l.Id, isCount));
                   sb.Append("</li>");
               }
               sb.Append("</ul>");
           }
           return sb.ToString();
       }
      
        #endregion

        public int GetMemberCount(string depId)
        {
            using (SysUserRepository m_Rep = new SysUserRepository(new DBContainer()))
            {

                return m_Rep.GetUserCountByDepId(depId);
            }
        }
        public string GetStructLayout(IQueryable<SysStruct> queryData, string parentId, bool isCount)
        {
            IQueryable<SysStruct> query = queryData.Where(a => a.ParentId == parentId).OrderBy(a => a.Sort);
            StringBuilder sb = new StringBuilder();
            if (query.Count() > 0)
            {

                sb.Append("<ul>");
                foreach (var r in query)
                {
                    sb.Append("<li data-options=\"attributes:{'id':'" + r.Id + "'}\">");
                    if (isCount)
                    {
                        sb.AppendFormat("<span>{0} ({1})</span>", r.Name, GetMemberCount(r.Id));
                    }
                    else
                    {
                        sb.AppendFormat("<span>{0}</span>", r.Name);
                    }
                    sb.Append(GetStructLayout(queryData, r.Id, isCount));
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
            }
            return sb.ToString();
        }
    }
}