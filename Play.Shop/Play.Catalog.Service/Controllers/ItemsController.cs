using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Mappers;
using Play.Common.Contracts;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(Roles = AdminRole)]
    public class ItemsController : ControllerBase
    {
        /*
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow),
        };
        */

        private readonly IRepository<Item> itemsRepository ;

        public ItemsController(IRepository<Item> itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }



        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync())
                        .Select(item => item.AsDto());

            //Console.WriteLine($"Request {requestCounter} 200 (OK).");
            return items;
        }

        [HttpGet("{id}")]
        public async  Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }


        [HttpPost]
        //[Authorize(Policies.Write)]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {
            var item = new Item 
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetByIdAsync), new { Id = item.Id }, item);
        }

        [HttpPut("{id}")]
        //[Authorize(Policies.Write)]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await itemsRepository.UpdateAsync(existingItem);

            return NoContent();

        }

        [HttpDelete("{id}")]
        //[Authorize(Policies.Write)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }
            
            await itemsRepository.DeleteAsync(existingItem.Id);

            return NoContent();
        }


    }
}