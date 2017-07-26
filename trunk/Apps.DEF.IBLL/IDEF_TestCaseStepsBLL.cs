using System;
using Apps.Common;
using Apps.Models.DEF;
using Apps.Models;
using System.Collections.Generic;
namespace Apps.DEF.IBLL
{
    public partial interface IDEF_TestCaseStepsBLL
    {
        DEF_TestCaseStepsModel GetModelById(string id);
    }
}
