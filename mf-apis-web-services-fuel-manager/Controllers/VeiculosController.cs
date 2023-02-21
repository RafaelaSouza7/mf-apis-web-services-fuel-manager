﻿using mf_apis_web_services_fuel_manager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace mf_apis_web_services_fuel_manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Veiculos
                .Include(t => t.Consumos)
                .ToListAsync();
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Veiculos
                .Include(t => t.Consumos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (model == null) return NotFound(new { message = "O Id informado não existe" });

            GerarLinks(model);

            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Veiculo model)
        {
            if(model.AnoFabricacao <= 0 || model.AnoModelo <= 0)
            {
                return BadRequest(new {message = "Ano de fabricação e ano do modelo devem ser maior que zero"});
            }
             _context.Veiculos.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new {id = model.Id}, model);
            
        }

        [HttpPut("{id}")]

        public async Task<ActionResult> Update (int id, Veiculo model)
        {
            if (id != model.Id) return BadRequest();
            
            var modeloDb = await _context.Veiculos.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if (modeloDb == null) return NotFound(new { message = "O Id informado não existe" });

            _context.Veiculos.Update(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
            
            var model = await _context.Veiculos.FindAsync(id);

            if (model == null) return NotFound(new { message = "O Id informado não existe" });

            _context.Veiculos.Remove(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private void GerarLinks(Veiculo model)
        {
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "self", metodo: "GET"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "update", metodo: "PUT"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "delete", metodo: "DELETE"));
        }
    }
}
