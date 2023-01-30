using Store.Domain.Entitites;

namespace Store.Domain.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        void Save(Order order);
    }
}
