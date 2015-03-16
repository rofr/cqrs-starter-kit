using System;
using System.Collections.Generic;
using OrigoDB.Core;

namespace Cafe
{

    [Serializable]
    public class MarkFoodServed : Command<CafeModel>
    {
        public Guid Id;
        public List<int> MenuNumbers;

        public override void Execute(CafeModel model)
        {
            try
            {
                if (!model.Tabs.ContainsKey(Id)) Abort("No such tab: " + Id);
                model.Tabs[Id].Mark<FoodItem>(TabItemState.Prepared, MenuNumbers, TabItemState.Served);
            }
            catch (UnmatchedMenuNumbers ex)
            {
                throw new FoodNotPrepared(ex);
            }
        }
    }
}
