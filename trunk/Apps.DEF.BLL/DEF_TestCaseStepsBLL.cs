using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Common;
using Microsoft.Practices.Unity;
using Apps.DEF.IBLL;
using Apps.Models.DEF;
using Apps.DEF.IDAL;
using Apps.BLL.Core;
using Apps.Models;
namespace Apps.DEF.BLL
{
    public partial class DEF_TestCaseStepsBLL 
    {
        // 数据库访问对象
        [Dependency]
        public IDEF_TestCaseStepsRepository repository { get; set; }

        //检查对象是否存在
        public bool entityIsExist(string id)
        {
            int count = repository.GetList(a => a.ItemID == id).Count();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        

        //根据主键获取模型
        public DEF_TestCaseStepsModel GetModelById(string id)
        {
            var entity = repository.GetById(id);
            if (entity == null)
            {
                return null;
            }
            DEF_TestCaseStepsModel model = new DEF_TestCaseStepsModel();

            //实现对象到模型转换
            model.ItemID = entity.ItemID;
            model.Code = entity.Code;
            model.Title = entity.Title;
            model.TestContent = entity.TestContent;
            model.state = entity.state;
            model.sort = entity.sort;

            return model;
        }

        //返回查询模型列表
        public override List<DEF_TestCaseStepsModel> GetList(ref GridPager pager, string code)
        {
            IQueryable<DEF_TestCaseSteps> queryData = null;

            queryData = repository.GetList(a => a.Code == code).OrderBy(a=>a.sort);

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
            List<DEF_TestCaseStepsModel> modelList = (from r in queryData
                                                      select new DEF_TestCaseStepsModel
                                                      {
                                                          ItemID = r.ItemID,
                                                          Code = r.Code,
                                                          Title = r.Title,
                                                          TestContent = r.TestContent,
                                                          state = r.state,
                                                          sort = r.sort,
                                                      }).ToList();

            return modelList;
        }

    }
}
