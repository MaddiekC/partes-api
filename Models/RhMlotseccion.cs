using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[Keyless]
[Table("rh_mlotseccion")]
[MySqlCollation("utf8mb3_general_ci")]
public partial class RhMlotseccion
{
    [Column("LOTE_ID")]
    public int? LoteId { get; set; }

    [Column("NOM_SECCION")]
    [StringLength(10)]
    public string? NomSeccion { get; set; }

    [Column("plantilla")]
    public int? Plantilla { get; set; }

    [Column("DIMENSION")]
    public double? Dimension { get; set; }

    [Column("USUARIO_CRE")]
    public int? UsuarioCre { get; set; }

    [Column("FECHA_CRE")]
    public DateOnly? FechaCre { get; set; }

    [Column("ESTADO")]
    [StringLength(1)]
    public string? Estado { get; set; }

    [Column("Usuario_admin")]
    public int? UsuarioAdmin { get; set; }
}
