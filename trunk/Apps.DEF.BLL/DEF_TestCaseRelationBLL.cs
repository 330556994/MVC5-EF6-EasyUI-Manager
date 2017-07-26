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
namespace Apps.DEF.BLL
{
    public partial class DEF_TestCaseRelationBLL
    {
        [Dependency]
        public IDEF_TestCaseRepository caseRepository { get; set; }
        //检查对象是否存在
        public bool entityIsExist(string pcode, string ccode)
        {
            int count = m_Rep.GetList(a => a.PCode == pcode && a.CCode == ccode).Count();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //新增
        public bool Create(ref ValidationErrors errors, string code, string codeIds)
        {
            try
            {
                //测试关键数值是否有效

                if (caseRepository.GetById(code) == null)
                {
                    errors.Add("不存在的用例！");
                    return false;
                }

                string[] arr = codeIds.Split(',');
                foreach (string str in arr)
                {
                    //数据库没有，存在这个用例，不是空值时成立,是否存在交叉引用
                    if (!entityIsExist(code, str) && !entityIsExist(str,code ) && code != str && caseRepository.GetById(str) != null)
                    {
                        DEF_TestCaseRelation entity = new DEF_TestCaseRelation();
                        entity.PCode = code;
                        entity.CCode = str;
                        entity.ReMark = "";
                        entity.Sort = 1;
                        m_Rep.Create(entity);
                    }
                    else
                    {
                        errors.Add(str+"存在交叉引用！请检查"+str+"。");
                    }
                }
                if ( m_Rep.GetTestCaseRelationByCode(code)> 1)
                {
                    if (entityIsExist(code, code))
                    {
                        if (!Delete(ref errors, code, code))
                        {
                            errors.Add("删除默认行出错！请手动删除" + code);
                            return false;
                        }
                    }
                }
                if (errors.Error != "")
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
            }
            return false;

        }
        //新增
        public bool CreateRepeatRow(ref ValidationErrors errors, string code)
        {
            try
            {
                //测试关键数值是否有效

                if (caseRepository.GetById(code) == null)
                {
                    errors.Add("不存在的用例！");
                    return false;
                }
                    //不是空值时成立
                     if (caseRepository.GetById(code) != null)
                    {
                        DEF_TestCaseRelation entity = new DEF_TestCaseRelation();
                        entity.PCode = code;
                        entity.CCode = code;
                        entity.Sort = 1;
                        entity.ReMark = "";
                        m_Rep.Create(entity);
                    }

                return true;

            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
            }
            return false;

        }
        public bool Edit(ref ValidationErrors errors, string id, int sort)
        {
            try
            {
                string[] ids = id.Split('_');
                if (ids.Length != 2)
                {
                    return false;
                }
                else
                {
                    DEF_TestCaseRelation entity = m_Rep.GetById(ids[0],ids[1]);
                    if (entity == null)
                    {
                        errors.Add("不存在的用例关系！");
                        return false;
                    }
                    entity.Sort = sort;
                    return m_Rep.Edit(entity);

                }
            }
            catch(Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public bool Delete(ref ValidationErrors errors, string pcode, string ccode)
        {
            try
            {
                //至少保留一行
                if (m_Rep.GetTestCaseRelationByCode(pcode) == 1)
                {
                    errors.Add("至少保留一个记录，后台才能正确运算！");
                    return false;
                }
                if (m_Rep.DeleteByPcodeCcode(pcode, ccode) != 1)
                {
                    errors.Add("删除出错!");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                errors.Add("删除异常");
                return false;
            }

        }
     
        //根据主键获取对象
        public DEF_TestCaseRelation GetById(string pcode, string ccode)
        {
            return m_Rep.GetById(pcode, ccode);
        }

        //根据主键获取模型
        public DEF_TestCaseRelationModel GetModelById(string pcode, string ccode)
        {
            var entity = m_Rep.GetById(pcode, ccode);
            if (entity == null)
            {
                return null;
            }
            DEF_TestCaseRelationModel model = new DEF_TestCaseRelationModel();

            //实现对象到模型转换
            model.PCode = entity.PCode;
            model.CCode = entity.CCode;
            model.ReMark = entity.ReMark;

            return model;
        }

        //返回查询模型列表
        public override List<DEF_TestCaseRelationModel> GetList(ref GridPager pager, string code)
        {
            IQueryable<DEF_TestCaseRelation> queryData = null;
                queryData = m_Rep.GetList(a=>a.PCode == code).OrderBy(a => a.Sort);

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
            List<DEF_TestCaseRelationModel> modelList = (from r in queryData
                                                         select new DEF_TestCaseRelationModel
                                                         {
                                                             PCode = r.PCode,
                                                             CCode = r.CCode,
                                                             Sort = r.Sort,
                                                             ReMark = r.ReMark,
                                                         }).ToList();

            return modelList;
        }

    }
}
