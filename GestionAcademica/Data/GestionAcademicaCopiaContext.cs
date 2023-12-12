using System;
using System.Collections.Generic;
using GestionAcademica.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Data;

public partial class GestionAcademicaCopiaContext : DbContext
{
    public GestionAcademicaCopiaContext()
    {
    }

    public GestionAcademicaCopiaContext(DbContextOptions<GestionAcademicaCopiaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrera> Carreras { get; set; }
    public virtual DbSet<CarreraMateria> CarrerasMaterias { get; set; }

    public virtual DbSet<Correlativa> Correlativas { get; set; }

    public virtual DbSet<Cuota> Cuotas { get; set; }

    public virtual DbSet<Cursada> Cursadas { get; set; }

    public virtual DbSet<Domicilio> Domicilios { get; set; }

    public virtual DbSet<Materia> Materias { get; set; }

    public virtual DbSet<Material> Materiales { get; set; }

    public virtual DbSet<Nota> Notas { get; set; }

    public virtual DbSet<Novedad> Novedades { get; set; }

    public virtual DbSet<Pais> Paises { get; set; }

    public virtual DbSet<Temario> Temarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Postulacion> Postulaciones { get; set; }

    public virtual DbSet<UsuarioCursada> UsuarioCursada { get; set; }
    public virtual DbSet<Asistencia> Asistencias { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;initial catalog=GestionAcademicaCopia;integrated security=True;TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrera>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Carreras__3213E83F6795CA3A");

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Facultad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("facultad");

        });

        modelBuilder.Entity<Correlativa>(entity =>
        {
            entity.HasKey(e => e.IdTablaCorrelativas).HasName("PK_Correlativas");

            entity.Property(e => e.IdTablaCorrelativas).HasColumnName("id_tabla_correlativas");
            entity.Property(e => e.IdMateria).HasColumnName("id_materia");
            entity.Property(e => e.IdMateriaCorrelativa).HasColumnName("id_materia_correlativa");

        });

        modelBuilder.Entity<Postulacion>(entity =>
        {
            entity.HasKey(e => e.IdPostulacion).HasName("PK_Postulaciones");

            entity.Property(e => e.IdPostulacion)
                .HasColumnName("id_postulacion");

            entity.Property(e => e.IdCursada).HasColumnName("id_cursada");
            entity.Property(e => e.LegajoProfesor).HasColumnName("legajo_profesor");
            entity.Property(e => e.Estado).HasColumnName("estado");

        });


        modelBuilder.Entity<Cuota>(entity =>
        {
            entity.HasKey(e => e.IdCuota).HasName("PK_Cuota");

            entity.Property(e => e.IdCuota)
                .ValueGeneratedNever()
                .HasColumnName("id_cuota");
            entity.Property(e => e.Anio).HasColumnName("anio");
            entity.Property(e => e.LegajoAlumno).HasColumnName("legajo_alumno");
            entity.Property(e => e.Mes).HasColumnName("mes");
            entity.Property(e => e.Monto).HasColumnName("monto");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.FechaPago).HasColumnName("fecha_pago");

        });

        modelBuilder.Entity<Cursada>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Anio).HasColumnName("anio");
            entity.Property(e => e.Dia).HasColumnName("dia");
            entity.Property(e => e.Turno).HasColumnName("turno");
            entity.Property(e => e.Cuatrimestre).HasColumnName("cuatrimestre");
            entity.Property(e => e.IdMateria).HasColumnName("id_materia");
            entity.Property(e => e.IdProfesor).HasColumnName("id_profesor");
            entity.Property(e => e.MaxAlumnos).HasColumnName("max_alumnos");
        });

        modelBuilder.Entity<CarreraMateria>(entity =>
        {
            entity
                .ToTable("Carrera_materia")
                .HasKey(e => e.IdCarreraMateria).HasName("PK_Carrera_materia_1");


            entity.Property(e => e.IdCarreraMateria).HasColumnName("id_carrera_materia");
            entity.Property(e => e.IdCarrera).HasColumnName("id_carrera");
            entity.Property(e => e.IdMateria).HasColumnName("id_materia");
            entity.Property(e => e.Cuatrimestre).HasColumnName("cuatrimestre");

        });


        modelBuilder.Entity<Domicilio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Domicili__3213E83F33C789BA");

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Altura).HasColumnName("altura");
            entity.Property(e => e.Calle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("calle");
            entity.Property(e => e.CodigoPostal).HasColumnName("codigo_postal");
            entity.Property(e => e.Departamento)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("departamento");
            entity.Property(e => e.IdPais).HasColumnName("id_pais");
            entity.Property(e => e.Localidad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("localidad");
            entity.Property(e => e.Piso).HasColumnName("piso");
            entity.Property(e => e.Provincia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("provincia");

        });

        modelBuilder.Entity<Materia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Materias__3213E83F0480E68C");

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.CargaHoraria).HasColumnName("carga_horaria");

        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.IdMaterial).HasName("PK__Material__81E99B8399A7E295");

            entity.Property(e => e.IdMaterial)
                .HasColumnName("id_material");
            entity.Property(e => e.FechaPublicacion)
                .HasColumnType("smalldatetime")
                .HasColumnName("fecha_publicacion");
            entity.Property(e => e.IdCursada).HasColumnName("id_cursada");
            entity.Property(e => e.Tipo).HasColumnName("tipo");
            entity.Property(e => e.Titulo).HasColumnName("titulo");
            entity.Property(e => e.Texto).HasColumnName("texto");
            entity.Property(e => e.FilePath).HasColumnName("filepath");
        });

        modelBuilder.Entity<Nota>(entity =>
        {
            //entity.HasNoKey();

            entity.HasKey(e => e.Id_nota).HasName("PK_Notas");

            entity.Property(e => e.Fecha)
                .HasColumnType("smalldatetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdCursada).HasColumnName("id_cursada");
            entity.Property(e => e.LegajoAlumno).HasColumnName("legajo_alumno");
            entity.Property(e => e.NotaNumerica).HasColumnName("nota_numerica");
            entity.Property(e => e.TipoNota).HasColumnName("tipo_nota");
        });

        modelBuilder.Entity<Novedad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Novedades");

            entity.Property(e => e.FechaPublicacion)
                .HasColumnType("smalldatetime")
                .HasColumnName("fecha_publicacion");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SnMostrar).HasColumnName("sn_mostrar")
                .HasColumnName("sn_mostrar");
            entity.Property(e => e.Copete).HasColumnName("copete")
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("copete");
            entity.Property(e => e.Texto)
                .IsUnicode(false)
                .HasColumnName("texto");
            entity.Property(e => e.Imagen)
                .IsUnicode(false)
                .HasColumnName("imagen");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("titulo");
        });

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paises__3213E83F929DF571");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Temario>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.IdMateria).HasColumnName("id_materia");
            entity.Property(e => e.Tema)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tema");

        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Legajo).HasName("PK__Alumnos__818C9F8612650E45");

            entity.Property(e => e.Legajo)
                .HasColumnName("legajo");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Clave)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("clave");
            entity.Property(e => e.Correo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Dni)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("dni");
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("smalldatetime")
                .HasColumnName("fecha_nacimiento");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("smalldatetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.IdCarrera).HasColumnName("id_carrera");
            entity.Property(e => e.IdDomicilio).HasColumnName("id_domicilio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Sexo).HasColumnName("sexo");
            entity.Property(e => e.TipoUsuario).HasColumnName("tipo_usuario");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.ModificoClave).HasColumnName("modifico_clave");

        });

        modelBuilder.Entity<UsuarioCursada>(entity =>
        {
            entity
                .ToTable("Usuario_cursada")
                .HasKey(t => new { t.IdCursada, t.LegajoAlumno }).HasName("PK_Usuario_cursada");
            //                .HasNoKey()

            //entity.HasIndex(e => e.IdCursada, "IX_Usuario_cursada").IsUnique();

            entity.Property(e => e.IdCursada).HasColumnName("id_cursada");
            entity.Property(e => e.LegajoAlumno).HasColumnName("legajo_alumno");
            entity.Property(e => e.Activa).HasColumnName("activa");

        });

        modelBuilder.Entity<Asistencia>(entity =>
        {
            entity.HasKey(e => e.Id_Asistencia).HasName("PK_Asistencias_1");

            entity.Property(e => e.Id_cursada).HasColumnName("id_cursada");
            entity.Property(e => e.Legajo_alumno).HasColumnName("legajo_alumno");
            entity.Property(e => e.Concurrio).HasColumnName("concurrio");
            entity.Property(e => e.Fecha).HasColumnName("fecha");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
