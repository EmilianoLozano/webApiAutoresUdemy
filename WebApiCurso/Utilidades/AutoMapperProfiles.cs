using AutoMapper;
using WebApiCurso.DTOs;
using WebApiCurso.Entidades;

namespace WebApiCurso.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Desde el dto (fuente) hacia la entidad (destino)
            CreateMap<AutorInsertarDTO, Autor>();

            CreateMap<Autor, AutorDTO>();

            // al autor de la base lo quiero convertir en un autordto
            CreateMap<Autor, AutorDTOconLibros>()
                .ForMember(autorDTO => autorDTO.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros));



            CreateMap<LibroInsertarDTO, Libro>()
                .ForMember(libro=>libro.AutoresLibros,opciones=>opciones.MapFrom(MapAutoresLibros));


            CreateMap<Libro, LibroDTO>();

            CreateMap<Libro, LibroDTOconAutores>()
                .ForMember(libroDTO => libroDTO.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));

            // de un lado y del otro se mapea
            //CreateMap<LibroPatchDTO, Libro>().ReverseMap();


            CreateMap<ComentarioInsertarDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();


        }
        private List<LibroDTO> MapAutorDTOLibros(Autor autor , AutorDTO autorDTO)
        {
            var resultado = new List<LibroDTO>();

            if (autor.AutoresLibros == null)
            {
                return resultado;
            }

            foreach (var autorLibro in autor.AutoresLibros)
            {
                resultado.Add(new LibroDTO()
                {
                    Id = autorLibro.LibroId,
                    Titulo = autorLibro.Libro.Titulo
                });
            }

            return resultado;
        }
        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO)
        {
            var resultado= new List<AutorDTO>();

            if(libro.AutoresLibros==null)
            {
                return resultado;
            }

            foreach (var autorLibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorDTO()
                {
                    Id = autorLibro.AutorId,
                    Nombre = autorLibro.Autor.Nombre
                });
            }

            return resultado;
        }


        private List<AutorLibro> MapAutoresLibros(LibroInsertarDTO libroInsertarDTO,Libro libro)
        {
            var resultado = new List<AutorLibro>();

            if(libroInsertarDTO.AutoresIds==null)
            {
                return resultado;
            }
            foreach(var autorId in libroInsertarDTO.AutoresIds)
            {
                resultado.Add(new AutorLibro() { AutorId = autorId });
            }

            return resultado;
        }



    }
}
