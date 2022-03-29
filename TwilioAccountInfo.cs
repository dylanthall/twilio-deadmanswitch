using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace DeadMansSwitch
{
    internal class TwilioAccountInfo
    {
        public static string AccountSID = "<INSERT_SID>";
        public static string AuthToken = "<INSERT_TOKEN>";
        public static string TwilioNumber = "<INSERT_NUMBER>";

        public static void Message(String number, String message)
        {
            TwilioClient.Init(AccountSID, AuthToken);
            var msg_resource = MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber(TwilioNumber),
                to: new Twilio.Types.PhoneNumber(number)
            );
            Console.WriteLine(msg_resource.Sid);
        }
    }
}
