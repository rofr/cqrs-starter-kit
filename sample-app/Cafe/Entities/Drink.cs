using System;

namespace Cafe
{
    [Serializable]
    public class Drink : TabItem
    {
        public override bool IsReadyToServe
        {
            get { return State == TabItemState.Ordered; }
        }

        public override bool InPreparation
        {
            get { return false; }
        }
    }
}