using System;
using System.Collections.Generic;
using Cafe;

namespace CafeReadModels
{
    public interface IOpenTabQueries
    {
        List<int> ActiveTableNumbers();
        QueryAdapter.TabInvoice InvoiceForTable(int table);
        Guid TabIdForTable(int table);
        TabStatus TabForTable(int table);
        Dictionary<int, List<TabItem>> TodoListForWaiter(string waiter);
    }
}
