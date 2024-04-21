using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using WebApplication1.Contracts.Payment;

namespace WebApplication1.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly StripeSettings _stripeSettings;

        public PaymentService(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
        }

        public CreditCardInfo ExtractCreditCardInfoFromToken(string confirmationToken)
        {
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

            var confirmationService = new ConfirmationTokenService();
            var confirmation = confirmationService.Get(confirmationToken);

            string last4 = confirmation.PaymentMethodPreview.Card.Last4;

            long expiryMonth = confirmation.PaymentMethodPreview.Card.ExpMonth;
            long expiryYear = confirmation.PaymentMethodPreview.Card.ExpYear;

            DateTime expiryDate = new DateTime((int)expiryYear, (int)expiryMonth, 1);

            return new CreditCardInfo(last4, expiryDate);
        }

        public PaymentResponse MakePayment(string confirmationToken)
        {
            var currency = "aud";
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            PaymentResponse paymentResponse = new PaymentResponse();

            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    PaymentMethodTypes = new List<string>() {"card"},
                    Confirm = true,
                    Amount = 1000,
                    Currency = currency,
                    ConfirmationToken = confirmationToken
                };
                var service = new PaymentIntentService();
                PaymentIntent paymentIntent = service.Create(options);

                paymentResponse.Status = paymentIntent.Status;

                return paymentResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
