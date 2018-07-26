using System;
using System.Linq;
using DatabaseFirst;

namespace NavigationProperties
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new BluewindsEntities())
            {

                //var lastWeekOrders2 = context
                //  .Orders
                //  .Where(order => order.CustomerId == 1) // sobre el customerID
                //  .OrderBy(x => x.TotalAmount)
                //  .ToList();

                //var lastWeekOrders1 = context
                //  .Orders


                //  .Where(order => order.Customer.Id == 1) // sobre el Id del customer
                //  .OrderBy(x => x.TotalAmount)
                //  .ToList();

                var lastWeekOrders = context
                    .Orders
                    .Where(order => order.Customer.FirstName == "Ana" && order.Customer.LastName == "Trujillo")
                    .OrderBy(x => x.TotalAmount)
                    .ToList();

                foreach (var order in lastWeekOrders)
                {
                    Console.WriteLine($"{order.Id} + Date: {order.OrderDate} + Amount: {order.TotalAmount}");
                }
            }
        }
    }
}
