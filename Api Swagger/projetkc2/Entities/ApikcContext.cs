using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace projetkc2.Entities;

public partial class ApikcContext : DbContext
{
    public ApikcContext()
    {
    }

    public ApikcContext(DbContextOptions<ApikcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Information> Information { get; set; }

    public virtual DbSet<Plante> Plantes { get; set; }

    public virtual DbSet<Reserve> Reserves { get; set; }

    public virtual DbSet<Types> Types { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Information>(entity =>
        {
            entity.HasKey(e => e.IdInformation).HasName("PRIMARY");

            entity.ToTable("information");

            entity.HasIndex(e => e.IdPlante, "Id_plante");

            entity.Property(e => e.IdInformation).HasColumnName("Id_information");
            entity.Property(e => e.IdPlante).HasColumnName("Id_plante");
            entity.Property(e => e.Irrigation).HasMaxLength(50);
            entity.Property(e => e.Kc)
                .HasMaxLength(50)
                .HasColumnName("kc");
            entity.Property(e => e.Periode)
                .HasMaxLength(50)
                .HasColumnName("periode");
            entity.Property(e => e.Stades)
                .HasMaxLength(250)
                .HasColumnName("stades");
            entity.Property(e => e.Vergers).HasMaxLength(50);

            entity.HasOne(d => d.IdPlanteNavigation).WithMany(p => p.Information)
                .HasForeignKey(d => d.IdPlante)
                .HasConstraintName("FK_information_apikc.plante");
        });

        modelBuilder.Entity<Plante>(entity =>
        {
            entity.HasKey(e => e.IdPlante).HasName("PRIMARY");

            entity.ToTable("plante");

            entity.HasIndex(e => e.IdType, "FK_plante_apikc.type");

            entity.Property(e => e.IdPlante).HasColumnName("Id_plante");
            entity.Property(e => e.IdType).HasColumnName("Id_type");
            entity.Property(e => e.NomPlante)
                .HasMaxLength(50)
                .HasColumnName("nom_plante");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Plantes)
                .HasForeignKey(d => d.IdType)
                .HasConstraintName("FK_plante_apikc.type");
        });

        modelBuilder.Entity<Reserve>(entity =>
        {
            entity.HasKey(e => e.IdReserve).HasName("PRIMARY");

            entity.ToTable("reserve");

            entity.Property(e => e.IdReserve).HasColumnName("id_reserve");
            entity.Property(e => e.CodePostale)
                .HasDefaultValueSql("'0'")
                .HasColumnName("code_postale");
            entity.Property(e => e.ReserveDeau)
                .HasDefaultValueSql("'0'")
                .HasColumnName("reserve_deau");
        });

        modelBuilder.Entity<Types>(entity =>
        {
            entity.HasKey(e => e.IdType).HasName("PRIMARY");

            entity.ToTable("type");

            entity.Property(e => e.IdType).HasColumnName("Id_type");
            entity.Property(e => e.NomType)
                .HasMaxLength(50)
                .HasColumnName("nom_type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
