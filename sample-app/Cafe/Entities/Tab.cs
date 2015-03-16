using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafe
{
    [Serializable]
    internal class Tab
    {
        public readonly Guid Id;
        public string Waiter { get; set; }
        public int TableNumber { get; set; }

        internal bool IsClosed;

        public List<TabItem> Items { get; private set; }

        public Tab(Guid id)
        {
            Id = id;
            Items = new List<TabItem>();
        }


        
        internal void Mark<T>(TabItemState fromState, List<int> menuNumbers, TabItemState toState) where T: TabItem
        {
            //take a copy so we can modify without side effects
            var numbers = new List<int>(menuNumbers);

            var itemsToMark = new List<T>();
            foreach (var tabItem in Items.OfType<T>().Where(d => d.State == fromState))
            {
                int idx = numbers.IndexOf(tabItem.MenuNumber);
                if (idx >= 0)
                {
                    itemsToMark.Add(tabItem);
                    numbers[idx] = -1;
                }
            }

            if (numbers.Any(n => n >= 0)) throw new UnmatchedMenuNumbers(numbers);

            itemsToMark.ForEach(d => d.State = toState);
        }


        internal TabStatus ToStatus()
        {
            return new TabStatus
            {
                TabId = Id,
                TableNumber = TableNumber,
                Waiter = Waiter,
                ToServe = new List<TabItem>(Items.Where(i => i.IsReadyToServe)),
                InPreparation = new List<TabItem>(Items.Where(i => i.InPreparation)),
                Served = new List<TabItem>(Items.Where(i => i.State == TabItemState.Served))
            };
        }
    }
}
