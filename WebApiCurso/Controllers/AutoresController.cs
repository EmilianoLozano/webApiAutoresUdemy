
using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCurso.DTOs;
using WebApiCurso.Entidades;
using WebApiCurso.Servicios;

namespace WebApiCurso.Controllers
{

    [ApiController]
    [Route("api/autores")]          //      api/autores

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy ="esAdmin") ] // se ingresa solo con autorizacion jwt bearer

    public class AutoresController : ControllerBase 
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AutoresController(ApplicationDbContext context, IMapper mapper,IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        //[HttpGet("configuraciones")]
        //public ActionResult<string> ObtenerConfiguracion()
        //{
        //    // OBTENER DATA DE ICONFIGURATION q esta en appsettings.json
        //    return configuration["ConnectionStrings:defaultConnection"];
        //}



        
        [HttpGet]                   //      api/autores
       
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {

            var autores=  await context.Autores.ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);

        }


        [HttpGet("{id}",Name = "obtenerAutor")]             // api/autores/1
        public async Task<ActionResult<AutorDTOconLibros>> Get(int id)
        {
            var autor= await context.Autores
                .Include(autorDB=>autorDB.AutoresLibros)
                .ThenInclude(autorLibroDB=>autorLibroDB.Libro)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            return mapper.Map<AutorDTOconLibros>(autor); 
        }

        [HttpGet("{nombre}")]             // api/autores/1
        public async Task<ActionResult<List<AutorDTO>>> Get([FromRoute] string nombre)
        {
            var autores = await context.Autores.Where(x => x.Nombre == nombre).ToListAsync();
    
            return mapper.Map<List<AutorDTO>>(autores);

        }


        [HttpPost]
        public async Task<ActionResult> Post (AutorInsertarDTO autorInsertarDTO)
        {
            // Validacion en controlador 
            // AnyAsync = Any. Busca por linq en la base si ya existe el mismo nombre del autor nuevo
            var existeAutorConElMismoNombre = await context.Autores.AnyAsync(x => x.Nombre == autorInsertarDTO.Nombre);

            if(existeAutorConElMismoNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {autorInsertarDTO.Nombre}");
            }

            // Map del destino, en este caso autor, y esto lo mapeo con autorinsertardto
            var autor = mapper.Map<Autor>(autorInsertarDTO);

            context.Add(autor);

            await context.SaveChangesAsync();

            var autorDTO = mapper.Map<AutorDTO>(autor);


            //return Ok();
            return CreatedAtRoute("obtenerAutor", new { id = autor.Id }, autorDTO);

        }

        [HttpPut("{id}")]  
        public async Task<ActionResult> Put(AutorInsertarDTO autorInsertarDTO ,int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorInsertarDTO);
            autor.Id = id;
            
            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }
      
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if(!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });

            await context.SaveChangesAsync();

            return NoContent();

        }


    }



}
