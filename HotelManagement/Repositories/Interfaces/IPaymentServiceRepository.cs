using HotelManagement.Models;

namespace HotelManagement.Repositories.Interfaces
{
    public interface IPaymentServiceRepository
    {
        Task<IEnumerable<PaymentService>> GetAllPaymentServicesAsync();
        Task<PaymentService> GetPaymentServiceByIdAsync(int paymentServiceId);
        Task<int> CreatePaymentServiceAsync(PaymentService paymentService);
        Task<bool> UpdatePaymentServiceAsync(PaymentService paymentService);
        Task<bool> DeletePaymentServiceAsync(int paymentServiceId);
    }
}
