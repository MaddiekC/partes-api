using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[Table("falta_empleado")]
[Index("IdEmpleado", Name = "idx_fempleado")]
[Index("Fecha", Name = "idx_ffecha")]
public partial class FaltaEmpleado
{
    [Key]
    [Column("idFalta")]
    public int IdFalta { get; set; }

    [Column("idEmpleado")]
    public int? IdEmpleado { get; set; }

    [Column("idJustificacion")]
    public int? IdJustificacion { get; set; }

    [Column("fecha")]
    public DateOnly? Fecha { get; set; }

    [Column("observacion")]
    [StringLength(250)]
    public string? Observacion { get; set; }

    [Column("estado")]
    [StringLength(1)]
    public string? Estado { get; set; }

    [Column("fechacre", TypeName = "datetime")]
    public DateTime? Fechacre { get; set; }

    [Column("usuarioCre")]
    public int? UsuarioCre { get; set; }

    [Column("prm_tipo_falta")]
    public int? PrmTipoFalta { get; set; }

    [Column("usuarioAprob")]
    public int? UsuarioAprob { get; set; }

    [Column("fechaAprob", TypeName = "datetime")]
    public DateTime? FechaAprob { get; set; }

    [Column("tip_rol")]
    public int? TipRol { get; set; }

    [Column("cod_empresa")]
    public int? CodEmpresa { get; set; }

    [Column("sueldo")]
    [StringLength(1)]
    public string? Sueldo { get; set; }

    [Column("solo_corte")]
    [StringLength(1)]
    public string? SoloCorte { get; set; }
}
