using System;
using System.Linq;
using DatabaseFirst;

namespace SimpleQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            var from = DateTime.Parse("2012-01-01");
            var to = from.AddYears(1);

            // primer query
            //using (var database = new BluewindsEntities())  // cómo crear nuestro objeto
            //{

            //    var query = database  // se hace la consulta
            //        .Orders
            //        //  .Where(x => x.OrderDate >= from && x.OrderDate < to && x.TotalAmount > 100);
            //        .Where(x => x.OrderDate >= from)
            //        .Where(x => x.OrderDate < to)
            //        .Where(x => x.TotalAmount > 100);


            //    // para hacer un AND
            //    query = query.Where(x => x.OrderDate < to);
            //    query = query.Where(x => x.TotalAmount > 100);


            //    var twentyTwelveOrders = query.ToList();   // obtener el resultado

            //    //var any = twentyTwelveOrders.Any(); //materializa una vez los que cumplan

            //   // var quantityOfOrders = twentyTwelveOrders.Count();  // cuantos

            //    var firstOrder = twentyTwelveOrders.First(); // trae top 1 del query que hacemos, da or sentado que al menos 1 cumple condición, sino dará una excepción NULL que no dará error.



            //    foreach (var order in twentyTwelveOrders)
            //    {
            //        Console.WriteLine(order.Id);
            //    }
            //}


            // segundo query groupby. Groupby generará una lista de listas

            //using (var context = new BluewindsEntities())
            //{

            //    var monthlyOrdersGroups = context
            //        .Orders
            //        .Where(x => x.OrderDate >= from && x.OrderDate < to)
            //        .GroupBy(x => x.OrderDate.Month)
            //        .ToList();

            //    foreach (var monthlyOrders in monthlyOrdersGroups)
            //    {
            //        Console.WriteLine($"Mes: {monthlyOrders.Key}: Total; {monthlyOrders.Count()}");
            //        foreach (var order in monthlyOrders)
            //        {
            //            Console.WriteLine(order.Id);
            //        }
            //    }
            //}



            // tercer query de orden por total amount. 

            using (var context = new BluewindsEntities())
            {

                var lastWeekOrders = context  // lista en memoria 
                    .Orders
                    .Where(x => x.CustomerId == 2)
                    .OrderByDescending(x => x.TotalAmount)
                    .ToList();

                var lastweekOrders1 = lastWeekOrders  // lista en memoria....
                    .Where(x => x.CustomerId == 2)
                    .OrderByDescending(x => x.TotalAmount)
                    .ToList();


                foreach (var order in lastWeekOrders)
                {
                    Console.WriteLine($"Date: {order.OrderDate} + Amount: {order.TotalAmount}");
                }
            }
        }
    }
}
