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
    [Route("[controller]")]
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


    }
    
}