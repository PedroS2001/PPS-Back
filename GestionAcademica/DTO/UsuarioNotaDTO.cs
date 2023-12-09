namespace GestionAcademica.DTO
{
    public class UsuarioNotaDTO
    {
        public int Legajo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public int PrimerParcial { get; set; }
        public int SegundoParcial { get; set; }
        public int PrimerRecuperatorio { get; set; }
        public int SegundoRecuperatorio { get; set; }
        public int Final { get; set; }
    }
}
