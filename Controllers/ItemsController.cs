using System;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<ItemDto> GetItems()
        {

            //AsDto() is an extension method for Item class
            var items = _repository.GetItems()
            .Select(item => item.AsDto());

            return items;
        }
        
        //GET /Items/{id}
        // [HttpGet]
        // [Route("id")]
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
           // var _id = new Guid(id);

            var item = _repository.GetItem(id);

            if (item == null)
            {
                return NotFound();    
            }

            return item.AsDto();
        }

        #region CREATE NEW ITEM
        //POST /items
        [HttpPost]
        public ActionResult<ItemDto> CreateItem([FromBody] CreateItemDto itemDto)
        {
            Item newItem = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            _repository.CreateItem(newItem);

            return CreatedAtAction(nameof(GetItem), new { Id = newItem.Id }, newItem.AsDto());
        }

        #endregion

        //PUT /items/{id}
        [HttpPut("{id}")]
        public ActionResult<ItemDto> UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = _repository.GetItem(id);

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

            _repository.UpdateItem(updatedItem);

            return NoContent();
        }


        //DELETE /items/{id}
        [HttpDelete("{id}")]
        public ActionResult<ItemDto> DeleteItem(Guid id)
        {
            var existingItem = _repository.GetItem(id);

            if (existingItem is null)
            {
                return NotFound();
            }
            _repository.DeleteItem(id);

            return NoContent();
        }

    }
}