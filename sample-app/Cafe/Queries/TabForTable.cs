using System;
using OrigoDB.Core;

namespace Cafe
{
    [Serializable]
    public class TabForTable : Query<CafeModel, TabStatus>
    {
        public readonly int TableNumber;

        public TabForTable(int tableNumber)
        {
            TableNumber = tableNumber;
        }

        public override TabStatus Execute(CafeModel model)
        {
            var id = new TabIdForTable(TableNumber).Execute(model);
            var tab = model.Tabs[id];
            return tab.ToStatus();
        }
    }
}