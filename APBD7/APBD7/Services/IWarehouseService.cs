namespace APBD7.Services;

public interface IWarehouseService
{
    Task<int> FulfillOrderAsync(int idWarehouse, int idProduct, int amount, DateTime requestDateTime);
    Task<int> FulfillOrderWithProcedureAsync(int idWarehouse, int idProduct, int amount, DateTime requestDateTime);
}