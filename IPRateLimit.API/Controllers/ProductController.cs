﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace IPRateLimit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProduct()
        {
            return Ok(new { Id = 1, Name = "Kalem", Price = 20 });
        }
    }
}