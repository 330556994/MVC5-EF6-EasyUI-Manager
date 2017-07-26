using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Common;
using Microsoft.Practices.Unity;


using Apps.DEF.IBLL;
using Apps.Models.DEF;
using Apps.DEF.IDAL;
using Apps.Models;
using Apps.BLL.Core;
using Apps.IDAL;
using Apps.Locale;
namespace Apps.DEF.BLL
{
    public partial class DEF_TestCaseBLL
    {
        // 数据库访问对象
        [Dependency]
        public IDEF_CaseTypeRepository caseTypeRep { get; set; }
        //检查对象是否存在
     
        //根据主键获取模型
        public DEF_TestCaseModel GetModelById(string code)
        {
            var entity = m_Rep.GetById(code);
            if (entity == null)
            {
                return null;
            }
            DEF_TestCaseModel model = new DEF_TestCaseModel();

            //读取ModuleId_title
            string moduleIdTitle = null;
            string moduleId = null;
            var module = caseTypeRep.GetById(model.ModuleId);
            if (module == null)
            {
                moduleId = DEF_TestCaseModel.DEFAULT_MODULEID;
                moduleIdTitle = DEF_TestCaseModel.DEFAULT_MODULEID_TITLE;
            }
            else
            {
                moduleId = model.ModuleId;
                moduleIdTitle = module.Name;
            }
            //实现对象到模型转换
            model.Code = entity.Code;
            model.Name = entity.Name;
            model.Description = entity.Description;
            model.Sort = entity.Sort;
            model.ModuleId = moduleId;
            model.ModuleIdTitle = moduleIdTitle;

            return model;
        }

    

        //返回查询模型列表
        public List<DEF_TestCaseModel> GetList(ref GridPager pager, string querystr, string moduleId)
        {
            IQueryable<DEF_TestCase> queryData = null;
            if (!string.IsNullOrEmpty(querystr))
            {
                queryData = m_Rep.GetList(a => a.Code.Contains(querystr) || a.Name.Contains(querystr)).OrderBy(a => a.Sort);

            }
            else
            {
                queryData = m_Rep.GetList().OrderBy(a => a.Sort);
            }
            if (!(moduleId == null || moduleId == "_all"))
            {
                queryData = queryData.Where(a => a.ModuleId == moduleId);
            }
            pager.totalRows = queryData.Count();
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
      
        //返回查询模型列表
        public List<DEF_TestCaseModel> GetListByModuleId(ref GridPager pager, string moduleId)
        {
            IQueryable<DEF_TestCase> queryData = null;

            if (moduleId == "_all")
            {
                queryData = m_Rep.GetList().OrderBy(a => a.Sort);
            }
            else
            {
                queryData = m_Rep.GetList(a => a.ModuleId == moduleId).OrderBy(a => a.Sort);
            }
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
            List<DEF_TestCaseModel> modelList = (from r in queryData
                                                 select new DEF_TestCaseModel
                                                 {
                                                     Code = r.Code,
                                                     Name = r.Name,
                                                     Description = r.Description,
                                                     Sort = r.Sort,
                                                     ModuleId = r.ModuleId,
                                                 }).ToList();

            return modelList;
        }
    }
}
