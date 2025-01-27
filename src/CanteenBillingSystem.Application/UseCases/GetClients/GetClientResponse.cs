using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanteenBillingSystem.Application.UseCases.GetClients
{
    public record GetClientResponse
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; }
        public string ClientType { get; set; }
        public decimal Balance { get; set; }
    }
}