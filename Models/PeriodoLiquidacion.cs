using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[Keyless]
[Table("periodo_liquidacion")]
[MySqlCollation("utf8mb3_general_ci")]
public partial class PeriodoLiquidacion
{
    [Column("tip_rol")]
    public int? TipRol { get; set; }

    [Column("dias")]
    public int? Dias { get; set; }

    [Column("periodo")]
    public int? Periodo { get; set; }

    [Column("ano")]
    public int? Ano { get; set; }

    [Column("mes")]
    public int? Mes { get; set; }

    [Column("fechainicio")]
    public DateOnly? Fechainicio { get; set; }

    [Column("fechafin")]
    public DateOnly? Fechafin { get; set; }

    [Column("ind_cerrado")]
    public sbyte? IndCerrado { get; set; }

    [Column("estado")]
    [StringLength(1)]
    public string? Estado { get; set; }

    [Column("usuario")]
    public int? Usuario { get; set; }

    [Column("fec_sistema", TypeName = "datetime")]
    public DateTime? FecSistema { get; set; }

    [Column("liquidado")]
    [StringLength(1)]
    public string? Liquidado { get; set; }

    [Column("fechaliq", TypeName = "datetime")]
    public DateTime? Fechaliq { get; set; }
}
