using System;
using System.Collections.Generic;
using OrigoDB.Core;

namespace Cafe
{
    [Serializable]
    public class CafeModel : Model
    {
        internal SortedDictionary<Guid, Tab> Tabs;

        public CafeModel()
        {
            Tabs = new SortedDictionary<Guid, Tab>();
        }
    }
}
