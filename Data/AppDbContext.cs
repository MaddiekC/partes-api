using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PartesApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace PartesApi.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdMusua> AdMusuas { get; set; }

    public virtual DbSet<Labor> Labors { get; set; }

    public virtual DbSet<RhMhaci> RhMhacis { get; set; }

    public virtual DbSet<RhMlote> RhMlotes { get; set; }

    public virtual DbSet<RhMtrab> RhMtrabs { get; set; }

    public virtual DbSet<Tarifa> Tarifas { get; set; }

    public virtual DbSet<TranCparte> TranCpartes { get; set; }

    public virtual DbSet<TranDparte> TranDpartes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_bin")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Labor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Estado).IsFixedLength();
            entity.Property(e => e.RespetaMinimo)
                .HasDefaultValueSql("'N'")
                .IsFixedLength();
        });

        modelBuilder.Entity<RhMlote>(entity =>
        {
            entity.HasKey(e => e.LoteId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Tarifa>(entity =>
        {
            entity.HasKey(e => new { e.IdLabor, e.Secuencia })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.Property(e => e.IdLabor).ValueGeneratedOnAdd();
            entity.Property(e => e.Estado).IsFixedLength();
        });

        modelBuilder.Entity<TranCparte>(entity =>
        {
            entity.HasKey(e => e.SecParte).HasName("PRIMARY");

            entity.Property(e => e.SecParte).ValueGeneratedNever();
            entity.Property(e => e.Estado).IsFixedLength();
        });

        modelBuilder.Entity<TranDparte>(entity =>
        {
            entity.Property(e => e.HoraFin).IsFixedLength();
            entity.Property(e => e.HoraInicio).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
