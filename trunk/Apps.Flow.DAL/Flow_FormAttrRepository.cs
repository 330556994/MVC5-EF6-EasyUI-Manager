using System;
using System.Linq;
using Apps.Flow.IDAL;
using Apps.Models;
using System.Data;

namespace Apps.Flow.DAL
{
    public partial class Flow_FormAttrRepository
    {

        public int GetAttrCountByName(string name) {
            return Context.Flow_FormAttr.Where(a => a.Name == name).Count();
        }
        
    }
}
