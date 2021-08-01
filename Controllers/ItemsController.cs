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

        #region CREATE


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

        //PUT /items
        [HttpPost]
        public ActionResult<ItemDto> UpdateItem([FromBody] UpdateItemDto itemDto)
        {
            Item newItem = new()
            {
                Id = itemDto.Id,
                Name = itemDto.Name,
                Price = itemDto.Price,
            };

            _repository.UpdateItem(newItem);

            return CreatedAtAction(nameof(GetItem), new { Id = newItem.Id }, newItem.AsDto());
        }


    }
    
}