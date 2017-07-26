using Apps.Common;
using Apps.Models;
using Apps.Models.Spl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Spl.BLL
{
    public partial class Spl_ProductBLL
    {
        public override List<Spl_ProductModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<Spl_Product> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.Id.Contains(queryStr)
								|| a.Name.Contains(queryStr)
								|| a.Code.Contains(queryStr)
								
								|| a.Color.Contains(queryStr)
								
								|| a.CategoryId.Contains(queryStr)
								
								|| a.CreateBy.Contains(queryStr)
								
								);
            }
            else
            {
                queryData = m_Rep.GetList();
            }
        
            //启用通用列头过滤
            if (!string.IsNullOrWhiteSpace(pager.filterRules))
            {
                List<DataFilterModel> dataFilterList = JsonHandler.Deserialize<List<DataFilterModel>>(pager.filterRules).Where(f => !string.IsNullOrWhiteSpace(f.value)).ToList();
                queryData = LinqHelper.DataFilter<Spl_Product>(queryData, dataFilterList);
            }

            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
        public override List<Spl_ProductModel> CreateModelList(ref IQueryable<Spl_Product> queryData)
        {

            List<Spl_ProductModel> modelList = (from r in queryData
                                              select new Spl_ProductModel
                                              {
													Id = r.Id,
													Name = r.Name,
													Code = r.Code,
													Price = r.Price,
													Color = r.Color,
													Number = r.Number,
													CategoryId = r.CategoryId,
													CreateTime = r.CreateTime,
													CreateBy = r.CreateBy,
													CostPrice = r.CostPrice,
                                                    ProductCategory = r.Spl_ProductCategory.Name
                                              }).ToList();

            return modelList;
        }

    }
}
