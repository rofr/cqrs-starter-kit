using System;
using System.Linq;
using OrigoDB.Core;

namespace Cafe
{

    [Serializable]
    public class CloseTab : Command<CafeModel>
    {
        public Guid Id;
        public decimal AmountPaid;

        public override void Execute(CafeModel model)
        {
            if (!model.Tabs.ContainsKey(Id)) Abort("No such tab: " + Id);
            var tab = model.Tabs[Id];
            if (tab.IsClosed) throw new TabNotOpen();
            if (tab.Items.Any(item => item.State != TabItemState.Served)) throw new TabHasUnservedItems();
            var totalDue = tab.Items.Sum(item => item.Price);
            if (AmountPaid < totalDue) throw new MustPayEnough();
            tab.IsClosed = true;
        }
    }
}
