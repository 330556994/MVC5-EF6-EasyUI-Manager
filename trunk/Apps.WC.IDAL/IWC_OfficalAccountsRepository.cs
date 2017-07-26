using Apps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.WC.IDAL
{
    public partial interface IWC_OfficalAccountsRepository
    {
        WC_OfficalAccounts GetCurrentAccount();
        bool SetDefault(string id);
    }
}
