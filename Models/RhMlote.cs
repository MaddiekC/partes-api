using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[Table("rh_mlotes")]
[MySqlCollation("utf8mb3_general_ci")]
public partial class RhMlote
{
    [Key]
    [Column("LOTE_ID")]
    public int LoteId { get; set; }

    [Column("AREA_ID")]
    public int? AreaId { get; set; }

    [Column("NOM_LOTE")]
    [StringLength(10)]
    public string? NomLote { get; set; }

    [Column("DESCRIPCIOn")]
    [StringLength(50)]
    public string? Descripcion { get; set; }

    [Column("plantilla")]
    public int? Plantilla { get; set; }

    [Column("UNIDAD")]
    [StringLength(1)]
    public string? Unidad { get; set; }

    [Column("DIMENSION")]
    public double? Dimension { get; set; }

    [Column("ESTADO")]
    [StringLength(1)]
    public string? Estado { get; set; }

    [Column("USUARIO_CRE")]
    public int? UsuarioCre { get; set; }

    [Column("FECHA_CRE")]
    public DateOnly? FechaCre { get; set; }

    [Column("USUARIO_MOD")]
    public int? UsuarioMod { get; set; }

    [Column("FECHA_MOD")]
    public DateOnly? FechaMod { get; set; }
}
