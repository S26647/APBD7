using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using APBD7.Models;

namespace APBD7.Repos;

public class WarehouseRepo : IWarehouseRepo
{
    private IConfiguration _configuration;

    public WarehouseRepo(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        using SqlConnection con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Product";

        var productsList = new List<Product>();

        using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (await dr.ReadAsync())
            {
                productsList.Add(new Product
                {
                    IdProduct = Int32.Parse(dr["IdProduct"].ToString()),
                    Name = dr["Name"].ToString(),
                    Description = dr["Description"].ToString(),
                    Price = Double.Parse(dr["Price"].ToString())
                });
            }
        }

        return productsList;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        using SqlConnection con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Order";

        var ordersList = new List<Order>();

        using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (await dr.ReadAsync())
            {
                ordersList.Add(new Order
                {
                    IdOrder = Int32.Parse(dr["IdOrder"].ToString()),
                    IdProduct = Int32.Parse(dr["IdProduct"].ToString()),
                    Amount = Int32.Parse(dr["Amount"].ToString()),
                    CreatedAt = DateTime.Parse(dr["CreatedAt"].ToString()),
                    FulfilledAt = dr["FulfilledAt"].ToString() == "" ? null : DateTime.Parse(dr["FulfilledAt"].ToString())
                });
            }
        }

        return ordersList;
    }

    public async Task<IEnumerable<Warehouse>> GetWarehousesAsync()
    {
        using SqlConnection con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Warehouse";

        var warehousesList = new List<Warehouse>();

        using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (await dr.ReadAsync())
            {
                warehousesList.Add(new Warehouse
                {
                    IdWarehouse = Int32.Parse(dr["IdWarehouse"].ToString()),
                    Name = dr["Name"].ToString(),
                    Address = dr["Address"].ToString()
                });
            }
        }

        return warehousesList;
    }
    
    public async Task<Warehouse?> GetWarehouseAsync(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM WAREHOUSE WHERE IdWarehouse = @IdWarehouse";
        cmd.Parameters.AddWithValue("@IdWarehouse", id);


        using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (await dr.ReadAsync())
            {
                return new Warehouse
                {
                    IdWarehouse = Int32.Parse(dr["IdWarehouse"].ToString()),
                    Name = dr["Name"].ToString(),
                    Address = dr["Address"].ToString()
                };
            }
        }

        return null;
    }

    public async Task<Product>? GetProductAsync(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM PRODUCT WHERE IdProduct = @IdProduct";
        cmd.Parameters.AddWithValue("@IdProduct", id);


        using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (await dr.ReadAsync())
            {
                return new Product
                {
                    IdProduct = Int32.Parse(dr["IdProduct"].ToString()),
                    Name = dr["Name"].ToString(),
                    Description = dr["Description"].ToString(),
                    Price = Double.Parse(dr["Price"].ToString())

                };
            }
        }

        return null;
    }
    public async Task<Order> GetOrderAsync(int id, int amount)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM [ORDER] WHERE IdProduct = @id AND Amount = @amount AND FulfilledAt IS NULL";
        cmd.Parameters.AddWithValue("@IdProduct", id);
        cmd.Parameters.AddWithValue("@Amount", amount);


        using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (await dr.ReadAsync())
            {
                return new Order
                {
                    IdOrder = Int32.Parse(dr["IdOrder"].ToString()),
                    IdProduct = Int32.Parse(dr["IdProduct"].ToString()),
                    Amount = Int32.Parse(dr["Amount"].ToString()),
                    CreatedAt = DateTime.Parse(dr["CreatedAt"].ToString()),
                    FulfilledAt = dr["FulfilledAt"].ToString() == ""
                        ? null
                        : DateTime.Parse(dr["FulfilledAt"].ToString())
                };
            }
        }

        return null;
    }

    public async Task<bool> CheckIfProductExists(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Product WHERE IdProduct = @id";
        cmd.Parameters.AddWithValue("@IdProduct", id);

        int counter = 0;

        using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (await dr.ReadAsync())
            {
                counter++;
            }
        }

        return counter > 0;
    }

    public async Task<bool> CheckIfWarehouseExists(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Warehouse WHERE IdWarehouse = @id";
        cmd.Parameters.AddWithValue("@Warehouse", id);

        int counter = 0;

        using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (await dr.ReadAsync())
            {
                counter++;
            }
        }

        return counter > 0;
    }

    public async Task<bool> CheckIfOrderOfProductExists(int id, int amount)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM [ORDER] WHERE IdProduct = @id AND Amount >= @amount AND Fulfilled IS NULL";
        cmd.Parameters.AddWithValue("@IdProduct", id);
        cmd.Parameters.AddWithValue("@Amount", amount);

        int counter = 0;

        using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (await dr.ReadAsync())
            {
                counter++;
            }
        }

        return counter > 0;
    }

    public async Task<int> FulfillOrderAsync(int idWarehouse, int idProduct, int idOrder, int amount, double price)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        using var cmd = new SqlCommand();
        cmd.Connection = con;

        DbTransaction transaction = await con.BeginTransactionAsync();
        cmd.Transaction = (SqlTransaction)transaction;
        
        cmd.CommandText = "UPDATE [ORDER] SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder;" +
                          "INSERT INTO PRODUCT_WAREHOUSE(IdWarehouse,IdProduct,IdOrder,Amount,Price,CreatedAt) VALUES(@IdWarehouse,@IdProduct,@IdOrder,@Amount,@Price,@CreatedAt)" +
                          "SELECT SCOPE_IDENTITY()";
        
        int insertedRecordId = 0;
        String dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        
        cmd.Parameters.AddWithValue("@IdWarehouse", idWarehouse);
        cmd.Parameters.AddWithValue("@IdProduct", idProduct);
        cmd.Parameters.AddWithValue("@IdOrder", idOrder);
        cmd.Parameters.AddWithValue("@FulfilledAt", DateTime.Now.ToString(dateTimeFormat));
        cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString(dateTimeFormat));
        cmd.Parameters.AddWithValue("@Amount", amount);
        cmd.Parameters.AddWithValue("@Price", amount * price);
        
        try
        {
            insertedRecordId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }
        catch (SqlException ex)
        {

            Console.WriteLine("error in transaction");
            transaction.Rollback();
        }

        transaction.Commit();

        return insertedRecordId;
    }
    
    
    
    public async Task<bool> CheckIfProductWarehouseExists(int idOrder)
    {
        int count = 0;
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Product_Warehouse WHERE IdOrder = @IdOrder";
        cmd.Parameters.AddWithValue("@IdOrder", idOrder);


        using (var dr = await cmd.ExecuteReaderAsync())
        {
            
            while (await dr.ReadAsync())
            {
                count++;
            }
        }

        return count > 0;
    }
}