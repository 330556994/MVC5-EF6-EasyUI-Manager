﻿using Apps.Models;
using System.Linq;
namespace Apps.Flow.IDAL
{
    public partial interface IFlow_FormContentStepCheckStateRepository
    {
        Flow_FormContentStepCheckState GetByStepCheckId(string id);
    }
}