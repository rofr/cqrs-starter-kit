using System;
using System.Collections.Generic;
using OrigoDB.Core;


namespace Cafe
{

    [Serializable]
    public class MarkDrinksServed : Command<CafeModel>
    {
        public Guid Id;
        public List<int> MenuNumbers;

        public override void Execute(CafeModel model)
        {
            try
            {
                if (!model.Tabs.ContainsKey(Id)) Abort("No such tab: " + Id);
                var tab = model.Tabs[Id];
                tab.Mark<Drink>(TabItemState.Ordered, MenuNumbers, TabItemState.Served);
            }
            catch (UnmatchedMenuNumbers ex)
            {
                throw new DrinksNotOutstanding(ex);
            }
        }
    }
}
