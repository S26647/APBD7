using APBD7.Ex;
using APBD7.Models;
using APBD7.Repos;
using APBD7.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD7.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> FulfillOrderAsync(OrderDTO orderDTO)
    {
        int productWarehouseId;
        (int idProduct, int idWarehouse, int amount, DateTime requestDateTime) = orderDTO;
        try
        {
            productWarehouseId =
                await _warehouseService.FulfillOrderAsync(idWarehouse, idProduct, amount, requestDateTime);
        }
        catch (NoSuchProductException)
        {
            return StatusCode(404, "No such product exists");
        }
        catch (NoSuchWarehouseException)
        {
            return StatusCode(404, "No such warehouse exists");
        }
        catch (AmountException)
        {
            return StatusCode(400, "amount must be greater than zero");
        }
        catch (NoSuchOrderException)
        {
            return StatusCode(404, "no matching order found");
        }
        catch (OrderAlreadyFulfilledException)
        {
            return StatusCode(400, "order already fulfilled");
        }
        

        return Ok(productWarehouseId);
    }
    
    
}