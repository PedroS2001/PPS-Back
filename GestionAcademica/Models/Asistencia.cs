namespace GestionAcademica.Models
{
    public class Asistencia
    {
        public int Id_Asistencia{ get; set; }
        public int Id_cursada { get; set; }
        public int Legajo_alumno { get; set; }
        public int Concurrio { get; set; }
        public DateTime Fecha { get; set; }
    }
}
