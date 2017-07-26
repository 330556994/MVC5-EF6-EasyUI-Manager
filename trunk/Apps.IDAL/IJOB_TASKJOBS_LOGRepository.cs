using System;
using Apps.Models;
namespace Apps.IDAL
{
    public partial interface IJOB_TASKJOBS_LOGRepository
    {

        int DeleteBySno(string sno);
       
        string GetNameById(int itemId);
    }
}
