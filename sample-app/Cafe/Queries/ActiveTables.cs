using System;
using System.Collections.Generic;
using System.Linq;
using OrigoDB.Core;

namespace Cafe
{
    [Serializable]
    public class ActiveTables : Query<CafeModel, List<int>>
    {
        public override List<int> Execute(CafeModel model)
        {
            return model
                .Tabs
                .Values
                .Where(t => !t.IsClosed)
                .Select(t => t.TableNumber)
                .Distinct()
                .OrderBy(t => t)
                .ToList();
        }
    }
}
