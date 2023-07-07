using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Repositories;
using CoffeeShop.Models;

namespace CoffeeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeController : ControllerBase
    {
        private readonly ICoffeeRepository _coffeeRepository;

        public CoffeeController(ICoffeeRepository coffeeRepository)
        {
            _coffeeRepository = coffeeRepository;
        }

        // https://localhost:5001/api/coffee/
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_coffeeRepository.GetAll());
        }

        // https://localhost:5001/api/coffee/5
        [HttpGet("{id}")] // {id} - route parameter
        public IActionResult Get(int id)
        {
            var coffeeBev = _coffeeRepository.Get(id);
            if (coffeeBev == null)
            {
                return NotFound();
            }
            return Ok(coffeeBev);
        }

        // http://localhost:5001/api/coffee/
        [HttpPost]
        public IActionResult Post(Coffee coffeeBev)
        {
            _coffeeRepository.Add(coffeeBev);
            return CreatedAtAction("Get", new { id = coffeeBev.Id }, coffeeBev);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Coffee coffeeBev)
        {
            if (id != coffeeBev.Id)
            {
                return BadRequest();
            }
            _coffeeRepository.Update(coffeeBev);
            return NoContent();
        }

        // https://localhost:5001/api/
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _coffeeRepository.Delete(id);
            return NoContent();
        }

    }
}
