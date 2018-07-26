using System;
using System.Data.Entity;
using System.Linq;
using DatabaseFirst;

namespace LazyLoading
{
    class Program
    {
        static void Main(string[] args)
        {
            ////Lazy Loading
            //QueryWithLazyLoadingEnabled();
            //Console.WriteLine();

            //LazyLoading disabled
            //QueryWithLazyLoadingDisabled();
            //Console.WriteLine();

            //Lazy loading disabled with includes
            //QueryWithLazyLoadingDisabledWithIncludes();
            //Console.WriteLine();

            ////SELECT inner entity
            SelectProperty();
        }


        //SELECT inner entity
        private static void SelectProperty()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("==================\n");

            using (var context = new BluewindsEntities())
            {
                context.Configuration.LazyLoadingEnabled = false;

                var orders1 = context
                   .Orders
                   .Where(x => x.Customer.FirstName == "Ana" && x.Customer.LastName == "Trujillo")
                   .ToList();

                var orders = context
                    .Customers
                    .Where(x => x.FirstName == "Ana" && x.LastName == "Trujillo")
                    .SelectMany(x => x.Orders);

                foreach (var order in orders)
                {
                    Console.WriteLine($"Date: {order.OrderDate} + Amount: {order.TotalAmount}");
                    foreach (var item in order.OrderItems)
                    {
                        Console.WriteLine($"Item: {item.Product.ProductName} + Amount: {item.Quantity}");
                    }
                }
            }
        }

        // Lazy loading disabled with includes
        private static void QueryWithLazyLoadingDisabledWithIncludes()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("==================\n");

            using (var context = new BluewindsEntities())
            {
                context.Configuration.LazyLoadingEnabled = false;
                var customer = context
                    .Customers
                    .Include(c => c.Orders)  // info de todas las ordenes
                    .Include(c => c.Orders.Select(o => o.OrderItems.Select(oi => oi.Product)))  // todos los productos se cargan en memoria
                    .FirstOrDefault(x => x.FirstName == "Ana" && x.LastName == "Trujillo");  // select de ana

                foreach (var order in customer.Orders)
                {
                    Console.WriteLine($"Date: {order.OrderDate} + Amount: {order.TotalAmount}");
                    foreach (var item in order.OrderItems) // aqui la orden de items no está cargada en memoria sin el 2o include, irá a la base de datos
                    {
                        Console.WriteLine($"Item: {item.Product.ProductName} + Amount: {item.Quantity}");
                    }
                }
            }
        }

        

        private static void QueryWithLazyLoadingDisabled()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("==================\n");

            using (var context = new BluewindsEntities())
            {
                context.Configuration.LazyLoadingEnabled = false;
                var customer = context
                    .Customers
                    .FirstOrDefault(x => x.FirstName == "Ana" && x.LastName == "Trujillo");

                Console.WriteLine($"Customer: {customer.Id}");

                var orders = context.Orders.Where(x => x.CustomerId == customer.Id).ToList();

                foreach (var order in customer.Orders)
                {
                    Console.WriteLine($"Date: {order.OrderDate} + Amount: {order.TotalAmount}");
                    foreach (var item in order.OrderItems)
                    {
                        Console.WriteLine($"Item: {item.Product.ProductName} + Amount: {item.Quantity}");
                    }
                }
            }
        }

        //LazyLoading disabled
        private static void QueryWithLazyLoadingEnabled()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("==================\n");

            using (var context = new BluewindsEntities())
            {
                var customer = context
                    .Customers
                    .FirstOrDefault(x => x.FirstName == "Ana" && x.LastName == "Trujillo");

                foreach (var order in customer.Orders)
                {
                    Console.WriteLine($"Date: {order.OrderDate} + Amount: {order.TotalAmount}");
                    foreach (var item in order.OrderItems)
                    {
                        Console.WriteLine($"Item: {item.Product.ProductName} + Amount: {item.Quantity}");
                    }
                }
            }
        }
    }
}
