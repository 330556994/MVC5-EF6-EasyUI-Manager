using Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.IBLL
{
    public interface IBaseBLL<T> : IDisposable
    {
        List<T> GetList(ref GridPager pager, string queryStr);
        bool Create(ref ValidationErrors errors, T model);
        bool Delete(ref ValidationErrors errors, string id);
        bool Delete(ref ValidationErrors errors, string[] deleteCollection);
        bool Edit(ref ValidationErrors errors, T model);
        T GetById(string id);
        bool IsExists(string id);
    }
}
