using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Core.Entities;
using Microservicio.Infrastructure.Interfaces;
using Microservicio.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Api.Validators;

namespace Microservicio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocionController : ControllerBase
    {
        private IPromocionCollection db = new PromocionCollection();

        [HttpGet]
        public async Task<IActionResult> getAllPromociones()
        {
            return Ok(await db.getallPromociones());
        }


        [HttpGet("id={id}")]
        public async Task<IActionResult> getPromocionDetails(Guid id)
        {
            var respuest = await db.GetPromocionById(id);
            if(respuest!=null)
                return Ok(respuest);
            else return Ok("No se encontro la promocion");
        }

        [HttpGet("vigentes")]
        public async Task<IActionResult> getPromocionesVigentes()
        {
            return Ok(await db.getPromocionesVigentes());

        }
        [HttpGet("vigentesporfecha/fecha={fecha}")]
        public async Task<IActionResult> getPromocionesVigentesxFecha(DateTime? fecha)
        {
            if (fecha == null)
                return BadRequest("No ingreso fecha correctamente!");

            return Ok(await db.getPromocionesVigentesxFecha(fecha));

        }

        [HttpGet("vigentesporventa")]
        public async Task<IActionResult> getPromocionesVigentesxVenta([FromBody] Promocion promocion)
        {            
            if(!promocion.mediosDePago.Any())
                return BadRequest("Debe ingresar algun medio de pago!");
            
            if (!promocion.bancos.Any())
                return BadRequest("Debe ingresar un banco!");
            if (!promocion.categoriasProductos.Any())
                return BadRequest("Debe ingresar al menos una categoria de producto!");
            
            if(!PromocionValid.validarxValores(promocion))
                return BadRequest("Los valores ingresados no son validos!");

            return Ok(await db.getPromocionesVigentesxVenta(promocion));

        }

        [HttpPost]
        public async Task<IActionResult> CreatePromocion([FromBody] Promocion promocion )
        {
            if(promocion==null)
               return BadRequest("No se puede agregar una promocion vacia");

            if(!PromocionValid.validarAlAgregar(promocion) || !PromocionValid.validarxValores(promocion))
                return BadRequest("La promocion no es valida!");

            if (promocion.categoriasProductos == null)
                ModelState.AddModelError("CategoriasProductos", "Categoria no puede ser vacio!");

            await db.insertPromocion(promocion);
            return Created("Created", promocion.id);
        }
        
        [HttpPut("id={id}")]
        public async Task<IActionResult> ModificarPromocion([FromBody] Promocion promocion,Guid id)
        {
            if (promocion == null)
                return BadRequest("No se puede modificar una promocion vacia");

            if (!PromocionValid.validarAlAgregar(promocion) || !PromocionValid.validarxValores(promocion))
                return BadRequest("La promocion no es valida!");

            promocion.id = id; 
            promocion.activo = true;
            await db.UpdatePromocion(promocion);
            return Ok("Se modifico correctamente");

        }

        [HttpPut("Eliminar/id={id}")]
        public async Task<IActionResult> BorrarPromocion(Guid id)
        {
            if (id == null)
                return BadRequest("ingrese correctamente el Id");
           
            await db.EliminarPromocion(id);
            return Ok("Se borro la promocion correctamente");

        }

        [HttpPut("ModificarVigencia/id={id}")]
        public async Task<IActionResult> ModificarFechaVigencia([FromBody] Promocion promocion, Guid id)
        {
            if (id == null)
                return BadRequest("ingrese correctamente el Id");
            if(!promocion.fechaInicio.HasValue)
                return BadRequest("ingrese correctamente fecha inicio!");
            if (!promocion.fechaFin.HasValue)
                return BadRequest("ingrese correctamente fecha final!");

            if (promocion.fechaInicio.Value.Date> promocion.fechaFin.Value.Date)
                return BadRequest("La fecha inicial no debe ser mayor que fecha final!");
                       
            await db.UpdateVigencia(promocion,id);
            return Ok("Se modifico correctamente la fecha de vigencia!");

        }
    }
}
