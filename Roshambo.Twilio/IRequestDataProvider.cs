using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Roshambo.Twilio.Models;

namespace Roshambo.Twilio
{
    public interface IRequestDataProvider
    {
        Task<RequestData> GetRequestData();
    }
}
