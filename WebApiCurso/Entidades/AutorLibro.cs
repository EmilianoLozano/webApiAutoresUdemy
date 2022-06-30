



namespace WebApiCurso.Entidades
{
    public class AutorLibro
    {


        public int LibroId { get; set; }
        public int AutorId { get; set; }

        public int Orden { get; set; }  // orden de los autores de un libro

        public Libro Libro { get; set; }
        public Autor Autor { get; set; }


    }
}
