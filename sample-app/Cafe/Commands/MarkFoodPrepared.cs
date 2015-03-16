using System;
using System.Collections.Generic;
using OrigoDB.Core;

namespace Cafe
{
    [Serializable]
    public class MarkFoodPrepared : Command<CafeModel>
    {
        public Guid Id;
        public List<int> MenuNumbers;

        public override void Execute(CafeModel model)
        {
            try
            {
                if (!model.Tabs.ContainsKey(Id)) Abort("No such tab: " + Id);
                model.Tabs[Id].Mark<FoodItem>(TabItemState.Ordered, MenuNumbers, TabItemState.Prepared);
            }
            catch (UnmatchedMenuNumbers ex)
            {
                throw new FoodNotOutstanding(ex);
            }
        }
    }
}
