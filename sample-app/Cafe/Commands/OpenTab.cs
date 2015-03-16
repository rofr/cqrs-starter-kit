using System;
using OrigoDB.Core;

namespace Cafe
{

    [Serializable]
    public class OpenTab : Command<CafeModel>
    {
        public Guid Id;
        public int TableNumber { get; set; }
        public string Waiter { get; set; }
        
        public override void Execute(CafeModel model)
        {
            if (model.Tabs.ContainsKey(Id)) Abort("Tab Id already exists");
            var tab = new Tab(Id)
            {
                TableNumber = TableNumber, 
                Waiter = Waiter
            };

            model.Tabs.Add(Id, tab);
        }
    }
}
