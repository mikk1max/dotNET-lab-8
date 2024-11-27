using dotNET_lab_8.Data;
using dotNET_lab_8.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotNET_lab_8.Controllers
{
    [Route("api/fox")]
    [ApiController]
    public class FoxController(IFoxesRepository foxesRepository) : ControllerBase
    {
        readonly IFoxesRepository foxesRepository = foxesRepository;

        [HttpGet]
        public IEnumerable<Fox> Get()
        {
            return foxesRepository.GetAll()
            .OrderByDescending(fox => fox.Loves)
            .ThenBy(fox => fox.Hates);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var fox = foxesRepository.Get(id);
            if (fox == null)
            {
                return NotFound();
            }
            return Ok(fox);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Fox fox)
        {
            foxesRepository.Add(fox);
            return CreatedAtAction(nameof(Get), new { id = fox.Id }, fox);
        }

        [HttpPut("love/{id}")]
        public IActionResult Love(int id)
        {
            var fox = foxesRepository.Get(id);
            if (fox == null)
                return NotFound();
            fox.Loves++;
            foxesRepository.Update(id, fox);
            return Ok(fox);
        }

        [HttpPut("hate/{id}")]
        public IActionResult Hate(int id)
        {
            var fox = foxesRepository.Get(id);
            if (fox == null)
                return NotFound();
            fox.Hates++;
            foxesRepository.Update(id, fox);
            return Ok(fox);
        }
    }
}

