using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[Table("det_asistencia")]
[Index("IdAsistencia", Name = "fk_cab_asist")]
[Index("CodTrabaj", Name = "idx_empleado")]
[Index("Fecha", Name = "idx_fec_asis")]
public partial class DetAsistencia
{
    [Key]
    [Column("secuencia")]
    public int Secuencia { get; set; }

    [Column("idBiometrico")]
    public int? IdBiometrico { get; set; }

    [Column("idAsistencia")]
    public int? IdAsistencia { get; set; }

    [Column("cod_empresa")]
    public int? CodEmpresa { get; set; }

    [Column("cod_trabaj")]
    public int? CodTrabaj { get; set; }

    [Column("fecha")]
    public DateOnly? Fecha { get; set; }

    [Column("tipo")]
    [StringLength(1)]
    public string? Tipo { get; set; }

    [Column("hora", TypeName = "time")]
    public TimeOnly? Hora { get; set; }

    [Column("usuarioCre")]
    public int? UsuarioCre { get; set; }

    [Column("estado")]
    [StringLength(1)]
    public string? Estado { get; set; }

    [Column("fechacre", TypeName = "datetime")]
    public DateTime? Fechacre { get; set; }

    [Column("maquina")]
    [StringLength(50)]
    public string? Maquina { get; set; }
}
