using System;
using System.Linq;
using Apps.Flow.IDAL;
using Apps.Models;
using System.Data;

namespace Apps.Flow.DAL
{
    public partial class Flow_FormContentStepCheckStateRepository
    {
        public Flow_FormContentStepCheckState GetByStepCheckId(string id)
        {
            using (DBContainer db = new DBContainer())
            {
                return db.Flow_FormContentStepCheckState.SingleOrDefault(a => a.StepCheckId == id);
            }
        }
    }
}
