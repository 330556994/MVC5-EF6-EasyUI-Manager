using System;
using Apps.Models;
using System.Collections.Generic;
using System.Linq;
namespace Apps.IDAL
{
    public interface ISysRightGetUserRightRepository
    {
        List<P_Sys_GetRightByUser_Result> GetList(string userId);
    }
}
