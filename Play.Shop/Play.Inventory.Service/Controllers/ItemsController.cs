using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Contracts;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers
{

    [ApiController]
    [Route("items")]
    //[Authorize]
    public class ItemsController : ControllerBase
    {
        //private const string AdminRole = "Admin";
        private readonly IRepository<InventoryItem> _inventoryItemsRepository;
        private readonly CatalogClient _catalogClient;
        //private readonly IRepository<CatalogItem> catalogItemsRepository;

        public ItemsController(IRepository<InventoryItem> inventoryItemsRepository, CatalogClient catalogClient)
        {
            _inventoryItemsRepository = inventoryItemsRepository;
            _catalogClient = catalogClient;
            //this.catalogItemsRepository = catalogItemsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var catalogItems = await _catalogClient.GetCatalogItemsAsync();
            var inventoryItemEntities = await _inventoryItemsRepository.GetAllAsync(item => item.UserId == userId);
   

            var inventoryItemDtos = inventoryItemEntities.Select(inventoryItem => 
            {
                var catalogItem = catalogItems.Single(item => item.Id == inventoryItem.CatalogItemId);
                return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
            }); 

            return  Ok(inventoryItemDtos);
        }

        [HttpPost]
        //[Authorize(Roles = AdminRole)]
        public async Task<ActionResult> PostAsync(GrantItemDto grantItemDto)
        {
            var inventoryItem = await _inventoryItemsRepository.GetAsync(item => item.UserId == grantItemDto.UserId && item.CatalogItemId == grantItemDto.CatalogItemId);

            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem {
                    CatalogItemId = grantItemDto.CatalogItemId,
                    UserId = grantItemDto.UserId,
                    Quantity = grantItemDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await _inventoryItemsRepository.CreateAsync(inventoryItem);
            }
            else 
            {
                inventoryItem.Quantity += grantItemDto.Quantity;
                await _inventoryItemsRepository.UpdateAsync(inventoryItem);
            }

            return Ok();
        }
    }

}