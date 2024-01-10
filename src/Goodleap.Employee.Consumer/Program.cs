using Goodleap.Employee.Consumer.Service.Consumers;
using Microsoft.Extensions.Configuration;

namespace Goodleap.Employee.Consumer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var consumerService = new ConsumerService();

            while (true)
            {
                consumerService.Consumer();
            }
        }
    }
}