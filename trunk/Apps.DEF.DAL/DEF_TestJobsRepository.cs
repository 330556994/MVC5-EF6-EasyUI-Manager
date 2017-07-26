
using System;
using System.Linq;


using Apps.Models.DEF;
using Apps.Models;
using Apps.DEF.IDAL;
namespace Apps.DEF.DAL
{
    public partial class DEF_TestJobsRepository
    {
      
        //删除
        public int Delete(string vercode)
        {
           
                int result = Context.P_DEF_DeleteTestJobs(vercode);
                if (result > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
        }
        /// <summary>
        /// 设置一个测试任务为缺省任务
        /// </summary>
        /// <param name="vercode"></param>
        /// <returns></returns>
        public int SetTestJobsDefault(string vercode)
        {

            int result = Context.P_DEF_SetTestJobsDefault(vercode);
                if (result > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
        }
        /// <summary>
        /// 复制测试任务
        /// </summary>
        /// <param name="vercode"></param>
        /// <param name="newvercode"></param>
        /// <returns></returns>
        public int CopyTestJobs(string vercode,string newvercode)
        {
            using (DBContainer db = new DBContainer())
            {

                int result = Context.P_DEF_CopyTestJobs(vercode, newvercode);
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
        /// <summary>
        /// 生成测试任务项目及测试步骤
        /// </summary>
        /// <param name="vercode"></param>
        /// <returns></returns>
        public int CreateTestJobs(string vercode)
        {
                int result = Convert.ToInt32(Context.P_DEF_CreateTestJobs(vercode));
                if (result > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
        }
        //修改
        public int Edit(DEF_TestJobsModel model)
        {
            var entity = (from a in Context.DEF_TestJobs
                              where a.VerCode == model.VerCode
                              select a).FirstOrDefault();
                if (entity == null)
                {
                    return 0;
                }
                //给对象赋值
                entity.VerCode = model.VerCode;
                entity.Name = model.Name;
                entity.Result = model.Result;
                entity.Description = model.Description;
                entity.Creator = model.Creator;
                entity.CrtDt = model.CrtDt;
                entity.CloseState = model.CloseState;
                entity.Closer = model.Closer;
                entity.CloseDt = model.CloseDt;
                entity.CheckFlag = model.CheckFlag;
                return this.SaveChanges();
        }
        //取对象名称
        public string GetNameById(string vercode)
        {
            var entity = Context.DEF_TestJobs.SingleOrDefault(a => a.VerCode == vercode);
                return entity == null ? "" : entity.Name;
        }
    }
}
