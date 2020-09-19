using System.Collections.Generic;

namespace Roshambo.Twilio.Models
{
    public class RequestData
    {
        public string Url { get; set; }
        public string Signature { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
