using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerFA.Services.Interfaces
{
    public interface IProducerService
    {
        /// <summary>
        /// Sends the message to the Service Bus.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        Task SendMessageAsync(string message);
    }
}
