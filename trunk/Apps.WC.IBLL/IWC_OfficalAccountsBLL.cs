﻿using Apps.Models.WC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.WC.IBLL
{
    public partial interface IWC_OfficalAccountsBLL
    {
        WC_OfficalAccountsModel GetCurrentAccount();
        bool SetDefault(string id);
    }
}
