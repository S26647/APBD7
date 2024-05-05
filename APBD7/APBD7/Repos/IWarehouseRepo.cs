using APBD7.Models;

namespace APBD7.Repos;

public interface IWarehouseRepo
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<IEnumerable<Warehouse>> GetWarehousesAsync();

    Task<Warehouse> GetWarehouseAsync(int id);
    Task<Product>? GetProductAsync(int id);
    Task<Order> GetOrderAsync(int id, int amount);
    Task<bool> CheckIfProductExists(int id);
    Task<bool> CheckIfWarehouseExists(int id);

    Task<bool> CheckIfOrderOfProductExists(int id, int amount);
    Task<bool> CheckIfProductWarehouseExists(int id);

    Task<int> FulfillOrderAsync(int idWarehouse, int idProduct, int idOrder, int amount, double price);
}