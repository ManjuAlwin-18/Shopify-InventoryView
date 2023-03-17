using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example_FrontEnd.Backend.Models;
using Example_FrontEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Example_FrontEnd.Controllers
{
    [Route("api")]
    [ApiController]
    public class InventoryController : ControllerBase
    {

        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            this._inventoryService = inventoryService;
        }

        [HttpGet]
        public ActionResult<List<InventoryArticle>> GetInventory()
        {
            return this._inventoryService.GetInventory();
        }

    }


}
