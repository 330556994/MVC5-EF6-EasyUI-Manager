
using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Common;
using System.Data;
using Apps.Models.DEF;
using Apps.Models;
using Apps.DEF.IDAL;
using System.Data.Entity.Core.Objects;

namespace Apps.DEF.DAL
{
    public partial class DEF_DefectRepository
    {
        /// <summary>
        /// 生成缺陷报告
        /// </summary>
        /// <param name="vercode"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public int CreateDefectReport(string vercode, string creator)
        {
            using (DBContainer db = new DBContainer())
            {
                int result = db.P_DEF_CreateDefectReport(vercode, creator);
                if (result > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
  
        //删除
        public int Delete(string itemid, string vercode, string code)
        {
                DEF_Defect deleteItem = Context.DEF_Defect.SingleOrDefault(a => a.ItemID == itemid && a.VerCode==vercode && a.Code==code);
                if (deleteItem != null)
                {
                    Context.DEF_Defect.Remove(deleteItem);
                    return this.SaveChanges();
                }
                return 0;
        }
    
        
        //修改
        public int Edit(DEF_DefectModel model)
        {
            var entity = (from a in Context.DEF_Defect
                              where a.ItemID == model.ItemID
                              where a.VerCode==model.VerCode
                              where a.Code==model.Code
                              select a).FirstOrDefault();
                if (entity == null)
                {
                    return 0;
                }
                //给对象赋值

                entity.Title = model.Title;
                entity.TestContent = model.TestContent;
                entity.ResultContent = model.ResultContent;
                entity.Creator = model.Creator;
                entity.CrtDt = model.CrtDt;
                entity.Remark = model.Remark;
                entity.Receiver = model.Receiver;
                entity.SendDt = model.SendDt;
                entity.CloseState = model.CloseState;
                entity.Closer = model.Closer;
                entity.CloseDt = model.CloseDt;
                entity.MessageId = model.MessageId;
                entity.Sort = model.Sort;
                entity.ProcessState = model.ProcessState;
                entity.Processor = model.Processor;
                entity.ProcessDt = model.ProcessDt;
                entity.ErrorLevel = model.ErrorLevel;
                entity.CaseName = model.CaseName;
                entity.Complex = model.Complex;
                entity.PStartDt = model.PStartDt;
                entity.PEndDt = model.PEndDt;
                entity.Executor = model.Executor;
                
                return this.SaveChanges();
            
        }

        //取一个对象
        public DEF_Defect GetById(string itemid, string vercode, string code)
        {
            using (DBContainer db = new DBContainer())
            {
                return Context.DEF_Defect.SingleOrDefault(a => a.ItemID == itemid && a.VerCode == vercode && a.Code == code);
            }
        }
        //取一个对象
        public DEF_Defect GetByComplexId(string id)
        {
            return Context.DEF_Defect.SingleOrDefault(a => a.ItemID + "_" + a.VerCode + "_" + a.Code == id);
        }
        //取对象名称
        public string GetNameById(string id)
        {
            var entity = Context.DEF_Defect.SingleOrDefault(a => a.ItemID == id);
                return entity == null ? "" : entity.Title;
        }

        /// <summary>
        /// 全能查询
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<V_DEF_Defect> Query(string queryType, int pageno, int rows, string order, ref int rowscount)
        {
                ObjectParameter pRowscount = new ObjectParameter("rowscount", typeof(int));

                List<V_DEF_Defect> list = Context.P_DEF_QueryDefect(queryType, pageno, rows, order, pRowscount).ToList();
                rowscount = (int)pRowscount.Value;
                return list;
        }
    }
}
