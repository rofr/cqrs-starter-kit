using System;
using System.Collections.Generic;

namespace Cafe
{
    [Serializable]
    public class TodoListGroup
    {
        public Guid Tab;
        public List<TodoListItem> Items;
    }
}