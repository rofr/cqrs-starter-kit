using System;
using System.Collections.Generic;
using System.Linq;
using OrigoDB.Core;

namespace Cafe
{
    [Serializable]
    public class ChefTodoList : Query<CafeModel, List<TodoListGroup>>
    {
        public override List<TodoListGroup> Execute(CafeModel model)
        {
            return model
                .Tabs
                .Values
                .Where(t => t.Items.OfType<FoodItem>().Any(item => item.State == TabItemState.Ordered))
                .Select(tab => new TodoListGroup
                {
                    Tab = tab.Id,
                    Items =
                        new List<TodoListItem>(
                            tab.Items.OfType<FoodItem>().Where(item => item.State == TabItemState.Ordered).Select(item => new TodoListItem
                            {
                                Description = item.Description,
                                MenuNumber = item.MenuNumber
                            }))
                }).ToList();
        }
    }
}