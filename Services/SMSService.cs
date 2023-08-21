using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace tcc_mypet_back.Services
{
    public class SMSService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;

        public SMSService(string accountSid, string authToken, string fromPhoneNumber)
        {
            _accountSid = accountSid;
            _authToken = authToken;
            _fromPhoneNumber = fromPhoneNumber;
        }

        public async Task SendSMSAsync(string toPhoneNumber, string message)
        {
            if (!toPhoneNumber.StartsWith("+"))
            {
                toPhoneNumber = "+55" + toPhoneNumber;
            }

            TwilioClient.Init(_accountSid, _authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber(toPhoneNumber)
            )
            {
                From = new PhoneNumber(_fromPhoneNumber),
                Body = message
            };

            var messageResource = await MessageResource.CreateAsync(messageOptions);
            if (messageResource.ErrorCode != null)
            {
                throw new Exception($"Failed to send SMS: {messageResource.ErrorMessage}");
            }
        }

    }
}