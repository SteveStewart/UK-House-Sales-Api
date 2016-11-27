using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseSales.Domain
{
    public class PropertySummaryWithTransactions
    {
        public PropertySummaryWithTransactions(PropertySummary summary, IEnumerable<PropertyTransaction> transactions)
        {
            if (summary == null)
                throw new ArgumentNullException("summary");
            if (transactions == null)
                throw new ArgumentNullException("transactions");

            Summary = summary;
            Transactions = transactions;
        }

        public PropertySummary Summary { get; private set; }

        public IEnumerable<PropertyTransaction> Transactions { get; private set; }
    }
}
