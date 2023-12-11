using GestionAcademica.Models;

namespace GestionAcademica.DTO
{
    public class AltaCarreraDTO
    {
        public string Nombre { get; set; }
        public string Facultad { get; set; }
        public List<CarreraMateria> Materias { get; set; }
    }
}
