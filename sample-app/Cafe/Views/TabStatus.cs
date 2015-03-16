using System;
using System.Collections.Generic;

namespace Cafe
{
    [Serializable]
    public class TabStatus
    {
        public Guid TabId;
        public int TableNumber;
        public string Waiter;
        public List<TabItem> ToServe;
        public List<TabItem> InPreparation;
        public List<TabItem> Served;
    }
}