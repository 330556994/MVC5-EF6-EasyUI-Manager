using System.Collections.Generic;
using Apps.Common;
using Apps.Models.Flow;
namespace Apps.Flow.IBLL
{
    public partial interface IFlow_StepRuleBLL
    {
        List<Flow_StepRuleModel> GetList(string stepId);
    }
}
