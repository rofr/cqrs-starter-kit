using System;
using System.Linq;
using OrigoDB.Core;

namespace Cafe
{
    [Serializable]
    public class TabIdForTable : Query<CafeModel, Guid>
    {
        public readonly int TableNumber;

        public TabIdForTable(int tableNumber)
        {
            TableNumber = tableNumber;
        }

        public override Guid Execute(CafeModel model)
        {
            return model
                .Tabs
                .Values
                .First(t => !t.IsClosed && t.TableNumber == TableNumber)
                .Id;

        }
    }
}