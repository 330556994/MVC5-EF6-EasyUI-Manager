using System;
using System.Linq;
using Apps.Flow.IDAL;
using Apps.Models;
using System.Data;

namespace Apps.Flow.DAL
{
    public partial class Flow_FormContentRepository
    {

        public IQueryable<Flow_FormContent> GeExamineListByUserId(string userId)
        {
            IQueryable<Flow_FormContent> list = (from a in Context.Flow_FormContent
                                                 join b in Context.Flow_Step
                                                on a.FormId equals b.FormId
                                                 join c in Context.Flow_FormContentStepCheck
                                                on b.Id equals c.StepId
                                                 join d in Context.Flow_FormContentStepCheckState
                                                on c.Id equals d.StepCheckId
                                                where d.UserId == userId && !a.IsDelete
                                                select a).Distinct();
            return list;
        }

        public IQueryable<Flow_FormContent> GeExamineList()
        {
            IQueryable<Flow_FormContent> list = (from a in Context.Flow_FormContent
                                                 join b in Context.Flow_Step
                                                 on a.FormId equals b.FormId
                                                 join c in Context.Flow_FormContentStepCheck
                                                 on b.Id equals c.StepId
                                                 join d in Context.Flow_FormContentStepCheckState
                                                 on c.Id equals d.StepCheckId
                                                 select a).Distinct();
            return list;
        }

     
    }
}
