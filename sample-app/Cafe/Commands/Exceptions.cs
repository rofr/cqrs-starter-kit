using System;
using System.Collections.Generic;
using OrigoDB.Core;

namespace Cafe
{
    public class TabNotOpen : CommandAbortedException
    {
    }

    public class DrinksNotOutstanding : CommandAbortedException
    {
        public DrinksNotOutstanding(UnmatchedMenuNumbers inner)
            :base("Can't serve drinks that have not been ordered", inner)
        {
            
        }
    }

    public class UnmatchedMenuNumbers : CommandAbortedException
    {
        public UnmatchedMenuNumbers(IEnumerable<int> numbers)
            :base(String.Join(",", numbers))
        {
        }
        
    }

    public class FoodNotOutstanding : CommandAbortedException
    {
        public FoodNotOutstanding(UnmatchedMenuNumbers inner)
            :base("Can't prepare food that has not been ordered", inner)
        {
            
        }
    }

    public class FoodNotPrepared : CommandAbortedException
    {
        public FoodNotPrepared(UnmatchedMenuNumbers inner)
            :base("Can't serve food that has not been prepared", inner)
        {
            
        }
    }

    public class MustPayEnough : Exception
    {
    }

    public class TabHasUnservedItems : Exception
    {
    }
}
