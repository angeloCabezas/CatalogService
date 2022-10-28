using CatalogService.DTO;
using CatalogService.Entities;
using CatalogService.Extensions;
using CatalogService.Interfaces;
using CatalogService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemController : ControllerBase
    {
        private readonly IRepository<Item> _itemRepository;

        private static readonly List<ItemDto> items = new List<ItemDto> {
            new ItemDto (Guid.NewGuid(), "Potion", "Restores a small amount of hp", 5, DateTimeOffset.UtcNow),
            new ItemDto (Guid.NewGuid(), "Antidote", "Cures potion", 7, DateTimeOffset.UtcNow),
            new ItemDto (Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow),
        };

        public ItemController(IRepository<Item> itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems()
        {
            var items = await _itemRepository.GetAllAsync();
            var itemsDto = items.Select(x => x.asDto());
            return Ok(itemsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemById(Guid id) 
        {
            var item = await _itemRepository.GetAsync(id);
            if (item == null) return NotFound();
            return item.asDto(); 
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Add(CreateItemDto createItemDto)
        {
            var item = new Item {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Price = createItemDto.Price,
                Description = createItemDto.Description,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _itemRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetItemById), new { id= item.Id },item);
        }

        [HttpPut]
        public async Task<ActionResult<ItemDto>> Update(Guid id,UpdateItemDto updateItemDto)
        {
            var existingItem = await _itemRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await _itemRepository.UpdateAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDto>> Delete(Guid id)
        {
            var item = await _itemRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            await _itemRepository.DeleteAsync(item.Id);

            return NoContent();
        }
    }
}
