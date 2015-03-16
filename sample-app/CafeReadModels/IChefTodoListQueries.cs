using System;
using System.Collections.Generic;
using Cafe;

namespace CafeReadModels
{
    public interface IChefTodoListQueries
    {
        List<TodoListGroup> GetTodoList();
    }
}
