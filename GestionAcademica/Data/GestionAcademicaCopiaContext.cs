﻿using System;
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

    public virtual DbSet<UsuarioCursada> UsuarioCursada { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;initial catalog=GestionAcademicaCopia;integrated security=True;TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrera>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Carreras__3213E83F6795CA3A");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Correlativa>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.IdMateria).HasColumnName("id_materia");
            entity.Property(e => e.IdMateriaCorrelativa).HasColumnName("id_materia_correlativa");

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

        });

        modelBuilder.Entity<Cursada>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activa).HasColumnName("activa");
            entity.Property(e => e.Cuatrimestre).HasColumnName("cuatrimestre");
            entity.Property(e => e.IdMateria).HasColumnName("id_materia");
            entity.Property(e => e.IdProfesor).HasColumnName("id_profesor");
        });

        modelBuilder.Entity<CarreraMateria>(entity =>
        {
            entity.HasNoKey()
                .ToTable("carrera_materia");


            entity.Property(e => e.IdCarrera).HasColumnName("id_carrera");
            entity.Property(e => e.IdMateria).HasColumnName("id_materia");

        });


        modelBuilder.Entity<Domicilio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Domicili__3213E83F33C789BA");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
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
            entity.Property(e => e.LegajoAlumno).HasColumnName("legajo_alumno");
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
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.IdMaterial).HasName("PK__Material__81E99B8399A7E295");

            entity.Property(e => e.IdMaterial)
                .ValueGeneratedNever()
                .HasColumnName("id_material");
            entity.Property(e => e.FechaPublicacion)
                .HasColumnType("smalldatetime")
                .HasColumnName("fecha_publicacion");
            entity.Property(e => e.IdCursada).HasColumnName("id_cursada");
            entity.Property(e => e.Tipo).HasColumnName("tipo");
        });

        modelBuilder.Entity<Nota>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Fecha)
                .HasColumnType("smalldatetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdCursada).HasColumnName("id_cursada");
            entity.Property(e => e.LegajoAlumno).HasColumnName("legajo_alumno");
            entity.Property(e => e.NotaNumerica).HasColumnName("nota_numerica");
            entity.Property(e => e.TipoNota)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo_nota");
        });

        modelBuilder.Entity<Novedad>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.FechaPublicacion)
                .HasColumnType("smalldatetime")
                .HasColumnName("fecha_publicacion");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Texto)
                .IsUnicode(false)
                .HasColumnName("texto");
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
                .ValueGeneratedNever()
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
            entity.Property(e => e.IdCarrera).HasColumnName("id_carrera");
            entity.Property(e => e.IdDomicilio).HasColumnName("id_domicilio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Sexo).HasColumnName("sexo");
            entity.Property(e => e.TipoUsuario).HasColumnName("tipo_usuario");

        });

        modelBuilder.Entity<UsuarioCursada>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Usuario_cursada");

            entity.HasIndex(e => e.IdCursada, "IX_Usuario_cursada").IsUnique();

            entity.Property(e => e.IdCursada).HasColumnName("id_cursada");
            entity.Property(e => e.LegajoAlumno).HasColumnName("legajo_alumno");


        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
