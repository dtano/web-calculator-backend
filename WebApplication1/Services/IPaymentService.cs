using WebApplication1.Contracts.Payment;

namespace WebApplication1.Services
{
    public interface IPaymentService
    {
        PaymentResponse MakePayment(string confirmationToken);
        CreditCardInfo ExtractCreditCardInfoFromToken(string confirmationToken);
    }
}
