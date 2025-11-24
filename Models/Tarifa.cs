using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[PrimaryKey("IdLabor", "Secuencia")]
[Table("tarifa")]
[MySqlCharSet("latin1")]
[MySqlCollation("latin1_swedish_ci")]
public partial class Tarifa
{
    [Key]
    [Column("id_labor")]
    public uint IdLabor { get; set; }

    [Key]
    [Column("secuencia")]
    public uint Secuencia { get; set; }

    [Column("valor")]
    [Precision(10, 6)]
    public decimal Valor { get; set; }

    [Column("fecha_inicio")]
    public DateOnly FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateOnly FechaFin { get; set; }

    [Column("estado")]
    [StringLength(1)]
    public string Estado { get; set; } = null!;

    [Column("fechaCre", TypeName = "datetime")]
    public DateTime FechaCre { get; set; }

    [Column("usuarioCre")]
    public uint UsuarioCre { get; set; }

    [Column("tarifa2")]
    [Precision(10, 6)]
    public decimal? Tarifa2 { get; set; }

    [Column("val_opt")]
    [Precision(10, 6)]
    public decimal? ValOpt { get; set; }

    [Column("val_med")]
    [Precision(10, 6)]
    public decimal? ValMed { get; set; }

    [Column("val_bas")]
    [Precision(10, 6)]
    public decimal? ValBas { get; set; }
}
