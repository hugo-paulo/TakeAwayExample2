using System;
using System.Collections.Generic;
using System.Text;

namespace TakeAwayExample2.Utility
{
    public class StripeSettings
    {
        //These need to match the key:value pairs in Stripe object in the appsettings.json file
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
}
