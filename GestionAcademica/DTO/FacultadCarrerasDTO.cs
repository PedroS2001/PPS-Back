namespace GestionAcademica.DTO
{
    public class FacultadCarrerasDTO
    {
        public string NombreFacultad { get; set; }
        public List<Models.Carrera> Carreras { get; set; } = new List<Models.Carrera>();
    }
}
