using Backend.Data.Contexts;
using Backend.Data.Interfaces;
using Backend.Data.Repositories;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DamageController : ControllerBase
    {
        private static IDamageRepository _damageRepository;

        public DamageController(IDamageRepository damageRepository)
        {
            _damageRepository = damageRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Damage>> GetAllDamages() {
            try {
                var damages = _damageRepository.GetAllDamages();
                return Ok(damages);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Damage> GetDamageById(int id) {
            try {
                var damage = _damageRepository.GetDamageById(id);
                if (damage == null) {
                    return NotFound($"Damage with ID {id} not found.");
                }
                return Ok(damage);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<Damage> Post([FromBody] Damage damage) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                var createdDamage = _damageRepository.CreateDamage(damage);
                return CreatedAtAction(nameof(GetDamageById), new { id = createdDamage.ID }, createdDamage);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            try {
                var success = _damageRepository.DeleteDamageById(id);
                if (!success) {
                    return NotFound($"Damage with ID {id} not found.");
                }
                return NoContent();
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Damage damage)
        {
            if (damage == null || id != damage.ID)
            {
                return BadRequest("Damage data is invalid.");
            }

            try
            {
                var existingDamage = _damageRepository.GetDamageById(id);
                if (existingDamage == null)
                {
                    return NotFound($"Damage with ID {id} not found.");
                }

                var updatedDamage = _damageRepository.UpdateDamage(damage);
                return Ok(updatedDamage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

