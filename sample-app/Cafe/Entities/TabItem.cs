using System;

namespace Cafe
{
    [Serializable]
    public abstract class TabItem
    {
        internal TabItemState State;
        public decimal Price;
        public string Description;
        public int MenuNumber;

        public abstract bool IsReadyToServe { get; }

        public abstract bool InPreparation { get; }
    }
}
