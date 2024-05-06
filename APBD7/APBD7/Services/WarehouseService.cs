using APBD7.Ex;
using APBD7.Models;
using APBD7.Repos;

namespace APBD7.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepo _warehouseRepo;

    public WarehouseService(IWarehouseRepo warehouseRepo)
    {
        _warehouseRepo = warehouseRepo;
    }
    
    public async Task<int> FulfillOrderAsync(int idWarehouse, int idProduct, int amount,DateTime requestDateTime)
    {
        Product? product = await _warehouseRepo.GetProductAsync(idProduct);
        if (product == null)
        {
            throw new NoSuchProductException();
        }

        double price = product.Price;
        if (await _warehouseRepo.GetWarehouseAsync(idWarehouse) == null)
        {
            throw new NoSuchWarehouseException();
        }

        if (amount <= 0)
        {
            throw new AmountException();
        }

        Order? orderToFulfill = await _warehouseRepo.GetOrderAsync(idProduct, amount);
        if (orderToFulfill == null)
        {
            throw new NoSuchOrderException();
        }
        
        if (orderToFulfill.CreatedAt.CompareTo(requestDateTime) >= 0)
        {
            throw new NoSuchOrderException();
        }

        if (await _warehouseRepo.CheckIfProductWarehouseExists(orderToFulfill.IdOrder))
        {
            throw new OrderAlreadyFulfilledException();
        }
        
        return await _warehouseRepo.FulfillOrderAsync(idWarehouse, idProduct, orderToFulfill.IdOrder, amount,price);

    }

    public Task<int> FulfillOrderWithProcedureAsync(int idWarehouse, int idProduct, int amount, DateTime requestDateTime)
    {
        throw new NotImplementedException();
    }
}