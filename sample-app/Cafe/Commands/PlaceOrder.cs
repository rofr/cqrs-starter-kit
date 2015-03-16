using System;
using System.Collections.Generic;
using OrigoDB.Core;

namespace Cafe
{

    [Serializable]
    public class PlaceOrder : Command<CafeModel>
    {
        public Guid Id;
        public List<TabItem> Items;

        public override void Execute(CafeModel model)
        {
            if (!model.Tabs.ContainsKey(Id)) Abort("No such tab: " + Id);
            var tab = model.Tabs[Id];
            if (tab.IsClosed) throw new TabNotOpen();
            tab.Items.AddRange(Items);
        }
    }
}
