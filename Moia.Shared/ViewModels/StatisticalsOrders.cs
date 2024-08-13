using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moia.Shared.ViewModels
{
    public class StatisticalsOrders
    {
        public string CommitteeTitle { get; set; }
        public int OrdersNum { get; set; }
        public int ConfirmedOrdersNum { get; set; }
        public int NotConfirmedOrdersNum { get; set; }

    }
}
