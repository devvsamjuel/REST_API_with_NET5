using System;
using System.Collections.Generic;
using Catalog.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly InMemItemsRepository _repository;
        public ItemsController()
        {
            _repository = new InMemItemsRepository();
        }

        //GET /Items
        [HttpGet]
        public IEnumerable<Item> GetItems()
        {
            var items = _repository.GetItems();
            return items;
        }
        
        //GET /Items/{id}
        // [HttpGet]
        // [Route("id")]
        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(Guid id)
        {
           // var _id = new Guid(id);

            var item = _repository.GetItem(id);

            if (item == null)
            {
                return NotFound();    
            }

            return item;
        }


    }
    
}