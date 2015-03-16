using System;
using System.Collections.Generic;
using System.Linq;
using Cafe;
using NUnit.Framework;
using OrigoDB.Core;

namespace CafeTests
{

    [TestFixture]
    public class TabTests
    {
        private Guid testId;
        private int testTable;
        private string testWaiter;
        private TabItem testDrink1;
        private TabItem testDrink2;
        private TabItem testFood1;
        private TabItem testFood2;
        private CafeModel model;

        [SetUp]
        public void Setup()
        {
            model = new CafeModel();

            testId = Guid.NewGuid();
            testTable = 42;
            testWaiter = "Derek";

            testDrink1 = new Drink
            {
                MenuNumber = 4,
                Description = "Sprite",
                Price = 1.50M
            };
            testDrink2 = new Drink
            {
                MenuNumber = 10,
                Description = "Beer",
                Price = 2.50M
            };

            testFood1 = new FoodItem
            {
                MenuNumber = 16,
                Description = "Beef Noodles",
                Price = 7.50M
            };
            testFood2 = new FoodItem
            {
                MenuNumber = 25,
                Description = "Vegetable Curry",
                Price = 6.00M
            };
        }

        [Test]
        public void CanOpenANewTab()
        {
            var cmd = new OpenTab
            {
                Id = testId,
                Waiter = testWaiter,
                TableNumber = testTable
            };
            cmd.Execute(model);
            var tab = model.Tabs[testId];
            Assert.IsTrue(!tab.IsClosed);
        }


        private void Arrange(params Command<CafeModel>[] commands)
        {
            try
            {
                foreach (var cmd in commands) cmd.Execute(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Assert.Fail("Preconditions failed");

            }
        }


        private void Act(Command<CafeModel> command)
        {
            command.Execute(model);
        }

        [Test, ExpectedException(typeof(TabNotOpen))]
        public void CanNotOrderWithClosedTab()
        {
            Arrange(
                new OpenTab() { Id = testId },
                new CloseTab { Id = testId, AmountPaid = 100 }
                );

            Act(new PlaceOrder
            {
                Id = testId,
                Items = new List<TabItem> { testDrink1 }
            });

        }

        [Test]
        public void CanPlaceDrinksOrder()
        {

            Arrange(new OpenTab
            {
                Id = testId,
                TableNumber = testTable,
                Waiter = testWaiter
            });
            Act(new PlaceOrder
            {
                Id = testId,
                Items = new List<TabItem> { testDrink1, testDrink2 }
            });

            var items = model.Tabs[testId].Items;
            Assert.True(items.Contains(testDrink1));
            Assert.True(items.Contains(testDrink2));
            Assert.AreEqual(2, items.Count);
        }

        [Test]
        public void Can_place_food_order()
        {
            Arrange(new OpenTab
            {
                Id = testId,
                TableNumber = testTable,
                Waiter = testWaiter
            });
            Act(new PlaceOrder
            {
                Id = testId,
                Items = new List<TabItem> { testFood1, testFood2 }
            });

            var items = model.Tabs[testId].Items;
            Assert.True(items.Contains(testFood1));
            Assert.True(items.Contains(testFood2));
            Assert.AreEqual(2, items.Count);
        }

        [Test]
        public void CanPlaceFoodAndDrinkOrder()
        {
            Arrange(new OpenTab
            {
                Id = testId,
                TableNumber = testTable,
                Waiter = testWaiter
            });
            Act(new PlaceOrder
            {
                Id = testId,
                Items = new List<TabItem> { testFood1, testDrink2 }
            });

            var items = model.Tabs[testId].Items;
            Assert.True(items.Contains(testFood1));
            Assert.True(items.Contains(testDrink2));
            Assert.AreEqual(2, items.Count);
        }

        [Test]
        public void OrderedDrinksCanBeServed()
        {
            Arrange(
                new OpenTab
                {
                    Id = testId,
                    TableNumber = testTable,
                    Waiter = testWaiter
                },
                new PlaceOrder
                {
                    Id = testId,
                    Items = new List<TabItem> { testDrink1, testDrink2 }
                }
            );
            Act(new MarkDrinksServed
            {
                Id = testId,
                MenuNumbers = new List<int> { testDrink1.MenuNumber, testDrink2.MenuNumber }
            });

            var items = model.Tabs[testId].Items;
            Assert.True(items.Contains(testDrink1));
            Assert.True(items.Contains(testDrink2));
            Assert.AreEqual(2, items.Count(i => i.State == TabItemState.Served));
        }

        [Test, ExpectedException(typeof(DrinksNotOutstanding))]
        public void CanNotServeAnUnorderedDrink()
        {

            Arrange(
                new OpenTab()
                {
                    Id = testId,
                    TableNumber = testTable,
                    Waiter = testWaiter
                },
                new PlaceOrder()
                {
                    Id = testId,
                    Items = new List<TabItem> { testDrink1 }
                });

            Act(
                new MarkDrinksServed
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testDrink2.MenuNumber }
                });


        }

        [Test, ExpectedException(typeof(DrinksNotOutstanding))]
        public void CanNotServeAnOrderedDrinkTwice()
        {

            Arrange(
                new OpenTab
                {
                    Id = testId,
                    TableNumber = testTable,
                    Waiter = testWaiter
                },
                new PlaceOrder
                {
                    Id = testId,
                    Items = new List<TabItem> { testDrink1, testDrink2 }
                },
                new MarkDrinksServed
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testDrink1.MenuNumber, testDrink2.MenuNumber }
                });
            Act(new MarkDrinksServed
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testDrink1.MenuNumber, testDrink2.MenuNumber }
                }
            );
        }

        [Test]
        public void OrderedFoodCanBeMarkedPrepared()
        {
            Arrange(
                new OpenTab()
                {
                    Id = testId,
                    TableNumber = testTable,
                    Waiter = testWaiter
                },
                new PlaceOrder()
                {
                    Id = testId,
                    Items = new List<TabItem> { testFood1, testFood2 }
                });

            Act(new MarkFoodPrepared
            {
                Id = testId,
                MenuNumbers = new List<int> { testFood1.MenuNumber, testFood2.MenuNumber }
            });

            var items = model.Tabs[testId].Items;
            Assert.True(items.Contains(testFood1));
            Assert.True(items.Contains(testFood2));
            Assert.IsTrue(items.All(item => item.State == TabItemState.Prepared));
        }

        [Test, ExpectedException(typeof(FoodNotOutstanding))]
        public void FoodNotOrderedCanNotBeMarkedPrepared()
        {
            Arrange(
                new OpenTab()
                {
                    Id = testId,
                    TableNumber = testTable,
                    Waiter = testWaiter
                },
                new PlaceOrder()
                {
                    Id = testId,
                    Items = new List<TabItem> { testFood1 }
                });

            Act(new MarkFoodPrepared
            {
                Id = testId,
                MenuNumbers = new List<int> { testFood1.MenuNumber, testFood2.MenuNumber }
            });
        }

        [Test, ExpectedException(typeof(FoodNotOutstanding))]
        public void CanNotMarkFoodAsPreparedTwice()
        {
            Arrange(
                new OpenTab()
                {
                    Id = testId,
                    TableNumber = testTable,
                    Waiter = testWaiter
                },
                new PlaceOrder()
                {
                    Id = testId,
                    Items = new List<TabItem> { testFood1 }
                },
                new MarkFoodPrepared
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testFood1.MenuNumber }
                });


            Act(new MarkFoodPrepared
            {
                Id = testId,
                MenuNumbers = new List<int> { testFood1.MenuNumber }
            });
        }

        [Test]
        public void CanServePreparedFood()
        {
            Arrange(
                new OpenTab()
                {
                    Id = testId,
                    TableNumber = testTable,
                    Waiter = testWaiter
                },
                new PlaceOrder()
                {
                    Id = testId,
                    Items = new List<TabItem> { testFood1 }
                },
                new MarkFoodPrepared
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testFood1.MenuNumber }
                });

            Act(
                new MarkFoodServed
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testFood1.MenuNumber }
                });

            var items = model.Tabs[testId].Items;
            Assert.True(items.Contains(testFood1));
            Assert.AreEqual(1, items.Count(i => i.State == TabItemState.Served));
        }


        [Test, ExpectedException(typeof(FoodNotPrepared))]
        public void CanNotServeUnorderedFood()
        {
            Arrange(new OpenTab
            {
                Id = testId,
                TableNumber = testTable,
                Waiter = testWaiter
            });
            Act(new MarkFoodServed
            {
                Id = testId,
                MenuNumbers = new List<int>
            {
                testDrink1.MenuNumber
            }
            });
        }

        [Test, ExpectedException(typeof(FoodNotPrepared))]
        public void CanNotServeOrderedButUnpreparedFood()
        {
            Arrange(
                new OpenTab()
                {
                    Id = testId,
                    TableNumber = testTable,
                    Waiter = testWaiter
                },
                new PlaceOrder()
                {
                    Id = testId,
                    Items = new List<TabItem> { testFood1 }
                });

            Act(
                new MarkFoodServed
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testFood1.MenuNumber }
                });
        }

        [Test]
        public void CanCloseTabByPayingExactAmount()
        {
            Arrange(
                new OpenTab()
                {
                    Id = testId,
                    TableNumber = testTable,
                    Waiter = testWaiter
                },
                new PlaceOrder()
                {
                    Id = testId,
                    Items = new List<TabItem> { testFood1, testDrink2 }
                },
                new MarkFoodPrepared
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testFood1.MenuNumber }
                },
                new MarkFoodServed
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testFood1.MenuNumber }
                },
                new MarkDrinksServed()
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testDrink2.MenuNumber }
                });

            var exactAmount = testFood1.Price + testDrink2.Price;

            Act(new CloseTab
            {
                Id = testId,
                AmountPaid = exactAmount
            });


            var items = model.Tabs[testId].Items;
            Assert.True(items.Contains(testFood1));
            Assert.True(items.Contains(testDrink2));
            Assert.AreEqual(2, items.Count(i => i.State == TabItemState.Served));
            Assert.IsTrue(model.Tabs[testId].IsClosed);

        }

        [Test]
        public void CanCloseTabWithTip()
        {
            Arrange(
                new OpenTab()
                {
                    Id = testId,
                    TableNumber = testTable,
                    Waiter = testWaiter
                },
                new PlaceOrder()
                {
                    Id = testId,
                    Items = new List<TabItem> { testFood1, testDrink2 }
                },
                new MarkFoodPrepared
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testFood1.MenuNumber }
                },
                new MarkFoodServed
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testFood1.MenuNumber }
                },
                new MarkDrinksServed()
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testDrink2.MenuNumber }
                });

            var exactAmount = testFood1.Price + testDrink2.Price;
            var tip = 35;

            Act(new CloseTab
            {
                Id = testId,
                AmountPaid = exactAmount + tip
            });


            var items = model.Tabs[testId].Items;
            Assert.True(items.Contains(testFood1));
            Assert.True(items.Contains(testDrink2));
            Assert.AreEqual(2, items.Count(i => i.State == TabItemState.Served));
            Assert.IsTrue(model.Tabs[testId].IsClosed);
        }

        [Test, ExpectedException(typeof(MustPayEnough))]
        public void MustPayEnoughToCloseTab()
        {
            Arrange(
            new OpenTab()
            {
                Id = testId,
                TableNumber = testTable,
                Waiter = testWaiter
            },
            new PlaceOrder()
            {
                Id = testId,
                Items = new List<TabItem> { testFood1, testDrink2 }
            },
            new MarkFoodPrepared
            {
                Id = testId,
                MenuNumbers = new List<int> { testFood1.MenuNumber }
            },
            new MarkFoodServed
            {
                Id = testId,
                MenuNumbers = new List<int> { testFood1.MenuNumber }
            },
            new MarkDrinksServed()
            {
                Id = testId,
                MenuNumbers = new List<int> { testDrink2.MenuNumber }
            });

            var exactAmount = testFood1.Price + testDrink2.Price;

            Act(new CloseTab
            {
                Id = testId,
                AmountPaid = exactAmount - 10
            });
        }


        [Test, ExpectedException(typeof(TabHasUnservedItems))]
        public void CanNotCloseTabWithUnservedDrinksItems()
        {
            Arrange(
                new OpenTab()
                {
                    Id = testId,
                    TableNumber = testTable,
                    Waiter = testWaiter
                },
                new PlaceOrder()
                {
                    Id = testId,
                    Items = new List<TabItem> { testFood1, testDrink2 }
                },
                new MarkFoodPrepared
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testFood1.MenuNumber }
                },
                new MarkFoodServed
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testFood1.MenuNumber }
                });

            var exactAmount = testFood1.Price + testDrink2.Price;

            Act(new CloseTab
            {
                Id = testId,
                AmountPaid = exactAmount
            });
        }

        [Test, ExpectedException(typeof(TabHasUnservedItems))]
        public void CanNotCloseTabWithUnpreparedFoodItems()
        {
            Arrange(
                new OpenTab
                {
                    Id = testId,
                    TableNumber = testTable,
                    Waiter = testWaiter
                },
                new PlaceOrder
                {
                    Id = testId,
                    Items = new List<TabItem> { testFood1, testDrink2 }
                });

            var exactAmount = testFood1.Price + testDrink2.Price;

            Act(new CloseTab
            {
                Id = testId,
                AmountPaid = exactAmount
            });
        }

        [Test, ExpectedException(typeof(TabHasUnservedItems))]
        public void CanNotCloseTabWithUnservedFoodItems()
        {
            Arrange(
                new OpenTab()
                {
                    Id = testId,
                    TableNumber = testTable,
                    Waiter = testWaiter
                },
                new PlaceOrder()
                {
                    Id = testId,
                    Items = new List<TabItem> { testFood1, testDrink2 }
                },
                new MarkFoodPrepared
                {
                    Id = testId,
                    MenuNumbers = new List<int> { testFood1.MenuNumber }
                });

            var exactAmount = testFood1.Price + testDrink2.Price;

            Act(new CloseTab
            {
                Id = testId,
                AmountPaid = exactAmount
            });
        }
    }
}
