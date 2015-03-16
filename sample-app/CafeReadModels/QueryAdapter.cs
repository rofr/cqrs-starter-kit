using System;
using System.Collections.Generic;
using Cafe;
using OrigoDB.Core;

namespace CafeReadModels
{
    /// <summary>
    /// Translate calls from web frontend to origodb queries
    /// </summary>
    public class QueryAdapter : IOpenTabQueries, IChefTodoListQueries
    {

        private readonly IEngine<CafeModel> _engine;

        public QueryAdapter(IEngine<CafeModel> engine)
        {
            _engine = engine;
        }


        public class TabInvoice
        {
            public Guid TabId;
            public int TableNumber;
            public List<TabItem> Items;
            public decimal Total;
            public bool HasUnservedItems;
        }

        public List<int> ActiveTableNumbers()
        {
            var query = new ActiveTables();
            return _engine.Execute(query);
        }

        public QueryAdapter.TabInvoice InvoiceForTable(int table)
        {
            throw new NotImplementedException();
        }

        public Guid TabIdForTable(int table)
        {
            var query = new TabIdForTable(table);
            return _engine.Execute(query);
        }

        public TabStatus TabForTable(int table)
        {
            var query = new TabForTable(table);
            return _engine.Execute(query);
        }

        public Dictionary<int, List<TabItem>> TodoListForWaiter(string waiter)
        {
            var query = new WaiterTodoList(waiter);
            return _engine.Execute(query);
        }

        public List<TodoListGroup> GetTodoList()
        {
            var query = new ChefTodoList();
            return _engine.Execute(query);
        }
    }
}
