using System;
using Apps.Models;
using System.Linq;

namespace Apps.DEF.IDAL
{
    public partial interface IDEF_TestCaseRepository
    {

        int Delete(string code);
 
        string GetNameById(string code);
    }
}
