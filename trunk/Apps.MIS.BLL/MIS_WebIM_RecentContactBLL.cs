using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Common;
using Microsoft.Practices.Unity;
using Apps.MIS.BLL;
using Apps.Models;
using Apps.BLL.Core;
using Apps.Models.MIS;
using Apps.MIS.IDAL;

namespace Apps.MIS.BLL
{
    public partial class MIS_WebIM_RecentContactBLL
    {
        // 数据库访问对象
        [Dependency]
        public IMIS_WebIM_RecentContactRepository repository { get; set; }

       /// <summary>
       /// 返回用户的最近联系人信息
       /// </summary>
       /// <param name="pager"></param>
       /// <param name="userId">用户ID</param>
       /// <returns></returns>
        public override List<MIS_WebIM_RecentContactModel> GetList(ref GridPager pager, string userId)
        {
            IQueryable<MIS_WebIM_RecentContact> queryData = null;
            queryData = repository.GetList(a =>a.UserId==userId).OrderByDescending(a => a.Id);

            pager.totalRows = queryData.Count();
            if (pager.totalRows > 0)
            {
                if (pager.page <= 1)
                {
                    queryData = queryData.Take(pager.rows);
                }
                else
                {
                    queryData = queryData.Skip((pager.page - 1) * pager.rows).Take(pager.rows);
                }
            }
            List<MIS_WebIM_RecentContactModel> modelList = (from r in queryData
                                                      select new MIS_WebIM_RecentContactModel
                                                      {
                                                          Id = r.Id,
                                                          ContactPersons = r.ContactPersons,
                                                          UserId = r.UserId,
                                                          InfoTime = r.InfoTime,
                                                          ContactPersonsTitle=r.ContactPersonsTitle,
                                                      }).ToList();

            return modelList;
        }

    }
}
