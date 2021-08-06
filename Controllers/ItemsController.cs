using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _repository;
        public ItemsController(IItemsRepository repository)
        {
            this._repository = repository;
        }

        //GET /Items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {

            //AsDto() is an extension method for Item class
            var items = (await _repository.GetItemsAsync())
                            .Select(item => item.AsDto());

            return items;
        }
        
        //GET /Items/{id}
        // [HttpGet]
        // [Route("id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            // var _id = new Guid(id);

            var item = await _repository.GetItemAsync(id);

            if (item == null)
            {
                return NotFound();    
            }

            return item.AsDto();
        }

        #region CREATE NEW ITEM
        //POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync([FromBody] CreateItemDto itemDto)
        {
            Item newItem = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            await _repository.CreateItemAsync(newItem);

            return CreatedAtAction(nameof(GetItemAsync), new { Id = newItem.Id }, newItem.AsDto());
        }

        #endregion

        //PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await _repository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            // Item newItem = new()
            // {
            //     Name = itemDto.Name,
            //     Price = itemDto.Price,
            // };

            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            await _repository.UpdateItemAsync(updatedItem);

            return NoContent();
        }


        //DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDto>> DeleteItemAsync(Guid id)
        {
            var existingItem = await _repository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }
            await _repository.DeleteItemAsync(id);

            return NoContent();
        }

    }
}