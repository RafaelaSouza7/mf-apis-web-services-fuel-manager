using mf_apis_web_services_fuel_manager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace mf_apis_web_services_fuel_manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConsumosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Consumo
                .Include(t => t.Veiculo)
                .ToListAsync();

            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Consumo
                .Include(t => t.Veiculo)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (model == null) return NotFound(new {message = " O id informado não existe"});

            GerarLinks(model);

            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Consumo model)
        {
            if(model.VeiculoId == 0) 
            {
                return BadRequest(new { message = "O id do veículo informado é inválido" });
            } 

            _context.Consumo.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Consumo model)
        {
            if(id != model.Id) return BadRequest();

            var modeloDb  = await _context.Consumo.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if(modeloDb == null) return NotFound(new {message = "O id informado não existe"});

            _context.Consumo.Update(model);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Consumo.FindAsync(id);

            if (model == null) return NotFound(new { message = "O id informado não existe" });

            _context.Consumo.Remove(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private void GerarLinks(Consumo model)
        {
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "self", metodo: "GET"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "update", metodo: "PUT"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "delete", metodo: "DELETE"));
        }
    }
}
