namespace APBD7.Models;

public record OrderDTO(int idProduct, int idWarehouse, int amount, DateTime requestDateTime);