using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.IntegrationEvents.Service1
{
    public record TestMsg : IntegerationEvent
    {
        public string Msg { get; init; }
    }
}
