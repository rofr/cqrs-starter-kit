using System;
using System.Collections.Generic;
using System.Linq;
using OrigoDB.Core;

namespace Cafe
{
    [Serializable]
    public class WaiterTodoList : Query<CafeModel, Dictionary<int, List<TabItem>>>
    {

        public readonly string Waiter;

        public WaiterTodoList(string waiter)
        {
            Waiter = waiter;
        }

        public override Dictionary<int, List<TabItem>> Execute(CafeModel model)
        {
            return model
                .Tabs
                .Values
                .Where(t => t.Waiter == Waiter && !t.IsClosed && t.Items.Any(item => item.IsReadyToServe))
                .GroupBy(t => t.TableNumber)
                .ToDictionary(
                    g => g.Key,
                    g => g.SelectMany(t => t.Items.Where(item => item.IsReadyToServe)).ToList());

        }
    }
}