using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloRabbitMq.Watermark.Services
{
    public class productImageCreatedEvent
    {
        public string ImageName { get; set; }
    }
}
