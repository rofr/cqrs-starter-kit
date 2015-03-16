using System;

namespace Cafe
{
    [Serializable]
    public class FoodItem: TabItem {
        public override bool IsReadyToServe
        {
            get { return State == TabItemState.Prepared; }
        }

        public override bool InPreparation
        {
            get { return State == TabItemState.Ordered; }
        }
    }
}