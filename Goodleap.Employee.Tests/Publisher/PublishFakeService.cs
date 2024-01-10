using Goodleap.Employee.Api.Business.DomainEvents.Publishers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goodleap.Employee.Tests.Publisher
{
    public class PublishFakeService : IPublishService
    {
        public Task Publish<T>(string topic, T Object)
        {
            return Task.CompletedTask;
        }
    }
}
