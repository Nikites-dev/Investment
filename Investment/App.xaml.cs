using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Investment.ADOApp;

namespace Investment
{
    public partial class App : Application
    {
        public static InvestmentEntities1 Connection = new InvestmentEntities1();
    }
}
