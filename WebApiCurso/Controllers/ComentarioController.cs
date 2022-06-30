using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCurso.DTOs;
using WebApiCurso.Entidades;

namespace WebApiCurso.Controllers
{

    [ApiController]
    // url comentario totalmente dependiente de libro, si no hay libro no hay comentarios
    [Route("api/libros/{libroId:int}/comentarios")]


    public class ComentarioController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ComentarioController(ApplicationDbContext context,IMapper mapper,UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)
        {

            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            var comentarios = await context.Comentarios
                                .Where(x => x.LibroId == libroId).ToListAsync();

            return mapper.Map<List<ComentarioDTO>>(comentarios);

        }


        [HttpGet("{id:int}", Name = "obtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> GetPorId(int id)
        {
            var comentario = await context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);

            if(comentario==null)
            {
                return NotFound();
            }

            return mapper.Map<ComentarioDTO>(comentario);

        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // si o si para ver los claims
        //[AllowAnonymous]// permite entrar al endpoint sin autenticarse
        public async Task<ActionResult>Post(int libroId, ComentarioInsertarDTO comentarioInsertarDTO)
        {

            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;


            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);

            if(!existeLibro)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioInsertarDTO);

            comentario.LibroId = libroId;
            comentario.UsuarioId = usuarioId;
            context.Add(comentario);

            await context.SaveChangesAsync();

            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);

            return CreatedAtRoute("obtenerComentario", new { id = comentario.Id , libroId = libroId }, comentarioDTO);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put (int libroId, int id , ComentarioInsertarDTO comentarioInsertarDTO )
        {
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            var existeComentario = await context.Comentarios.AnyAsync(x => x.Id == id);

            if(!existeComentario)
            {
                return NotFound();
            }
            var comentario = mapper.Map<Comentario>(comentarioInsertarDTO);

            context.Update(comentario);
            comentario.Id = id;
            comentario.LibroId = libroId;
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
