using System.Collections.Generic;
using Apps.Common;
using Apps.Models.Flow;
namespace Apps.Flow.IBLL
{
    public partial interface IFlow_FormBLL
    {
        List<Flow_FormModel> GetListByTypeId(string typeId);
    }
}
