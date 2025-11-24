using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[Table("labor")]
[Index("Id", Name = "idxLabor")]
[MySqlCharSet("latin1")]
[MySqlCollation("latin1_swedish_ci")]
public partial class Labor
{
    [Key]
    [Column("id")]
    public uint Id { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(200)]
    public string? Descripcion { get; set; }

    [Column("id_centro_costo")]
    public uint IdCentroCosto { get; set; }

    [Column("narrativa")]
    [StringLength(200)]
    public string? Narrativa { get; set; }

    [Column("avance_minimo")]
    [Precision(10, 6)]
    public decimal AvanceMinimo { get; set; }

    [Column("avance_promedio")]
    [Precision(10, 6)]
    public decimal AvancePromedio { get; set; }

    [Column("avance_maximo")]
    [Precision(10, 6)]
    public decimal AvanceMaximo { get; set; }

    [Column("dias_medicion")]
    public uint? DiasMedicion { get; set; }

    [Column("estado")]
    [StringLength(1)]
    public string Estado { get; set; } = null!;

    [Column("usuarioCre")]
    public uint UsuarioCre { get; set; }

    [Column("fechaCre", TypeName = "datetime")]
    public DateTime FechaCre { get; set; }

    [Column("usuarioMod")]
    public uint? UsuarioMod { get; set; }

    [Column("fechaMod", TypeName = "datetime")]
    public DateTime? FechaMod { get; set; }

    [Column("id_unidad_medida")]
    public int? IdUnidadMedida { get; set; }

    [Column("respeta_minimo")]
    [StringLength(1)]
    public string? RespetaMinimo { get; set; }

    [Column("idGrupo")]
    public int? IdGrupo { get; set; }

    [Column("idFuerzaLab")]
    public int? IdFuerzaLab { get; set; }

    [Column("calidad")]
    public int? Calidad { get; set; }

    [Column("idmacro")]
    public int? Idmacro { get; set; }

    [Column("lotero")]
    public int? Lotero { get; set; }

    [Column("idBansis")]
    public int? IdBansis { get; set; }
}
