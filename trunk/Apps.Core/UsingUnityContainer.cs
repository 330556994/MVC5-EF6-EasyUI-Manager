using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core
{
    public static class UsingUnityContainer
    {
        public static void Init()
        {
            if (_container == null)
                _container = new UnityContainer(); 

        }

        public static UnityContainer _container;
    }
}
