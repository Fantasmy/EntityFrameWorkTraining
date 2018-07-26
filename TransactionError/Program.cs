using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionError
{
    class Program
    {
        static void Main(string[] args)
        {//DemoInsert();

            //FailingInsert();

            //TransactionedInsert();

            TransactionedInsertV2();

        }

        private static void TransactionedInsertV2()
        {
            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BluewindsEntities"].ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var customerId = 0;
                        var orderId = 0;
                        using (var context = new BluewindsEntities(connection, false))
                        {
                            context.Database.UseTransaction(transaction);
                            context.Database.Log = Console.Write;
                            var customer = new Customer()
                            {
                                City = "Barcelona",
                                Country = "Spain",
                                FirstName = "Dario",
                                LastName = "Griffo",
                                Phone = "123456789"
                            };

                            context.Customers.Add(customer);
                            context.SaveChanges();
                        }

                        using (var context = new BluewindsEntities(connection, false))
                        {
                            context.Database.UseTransaction(transaction);
                            context.Database.Log = Console.Write;
                            var order = new Order()
                            {
                                OrderDate = DateTime.Today,
                                CustomerId = customerId,
                                TotalAmount = 100,
                                OrderNumber = "123456"
                            };

                            context.Orders.Add(order);
                            context.SaveChanges();
                        }

                        using (var context = new BluewindsEntities(connection, false))
                        {
                            context.Database.UseTransaction(transaction);
                            context.Database.Log = Console.Write;
                            var item = new OrderItem()
                            {
                                Product = context.Products.First(x => x.Id == 1),
                                OrderId = orderId + 1,
                                Quantity = 10,
                                UnitPrice = 50
                            };

                            context.OrderItems.Add(item);

                            context.SaveChanges();

                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex);
                    }
                }
            }
        }


        private static void FailingInsert()
        {
            using (var context = new BluewindsEntities())
            {
                context.Database.Log = Console.Write;

                var customer = new Customer()
                {
                    City = "Barcelona",
                    Country = "Spain",
                    FirstName = "Dario",
                    LastName = "Lopez",
                    Phone = "123456789"
                };

                context.Customers.Add(customer);
                context.SaveChanges();

                var order = new Order()
                {
                    OrderDate = DateTime.Today,
                    CustomerId = customer.Id,
                    TotalAmount = 100,
                    OrderNumber = "123456"
                };

                context.Orders.Add(order);
                context.SaveChanges();

                var item = new OrderItem()
                {
                    Product = context.Products.First(x => x.Id == 1),
                    OrderId = order.Id + 1,
                    Quantity = 10,
                    UnitPrice = 50
                };

                context.OrderItems.Add(item);

                context.SaveChanges();
            }
        }

        private static void DemoInsert()
        {
            using (var context = new BluewindsEntities())
            {
                context.Database.Log = Console.Write;

                var customer = new Customer()
                {
                    City = "Barcelona",
                    Country = "Spain",
                    FirstName = "Dario",
                    LastName = "Griffo",
                    Phone = "123456789"
                };

                context.Customers.Add(customer);
                context.SaveChanges();

                var order = new Order()
                {
                    OrderDate = DateTime.Today,
                    CustomerId = customer.Id,
                    TotalAmount = 100,
                    OrderNumber = "123456"
                };

                context.Orders.Add(order);
                context.SaveChanges();

                var item = new OrderItem()
                {
                    Product = context.Products.First(x => x.Id == 1),
                    OrderId = order.Id,
                    Quantity = 10,
                    UnitPrice = 50
                };

                context.OrderItems.Add(item);

                context.SaveChanges();
            }
        }
    }
}
