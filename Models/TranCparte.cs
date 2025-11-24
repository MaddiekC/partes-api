using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[Table("tran_cparte")]
[Index("Codigo", Name = "idx_codigo")]
[Index("FechaParte", Name = "idx_fecparte_cab")]
[Index("CodHacienda", Name = "idx_hacienda")]
[Index("SecParte", Name = "idx_parte")]
[MySqlCollation("utf8mb3_general_ci")]
public partial class TranCparte
{
    [Key]
    [Column("sec_parte")]
    public int SecParte { get; set; }

    [Column("codigo")]
    public int? Codigo { get; set; }

    [Column("cod_hacienda")]
    public int? CodHacienda { get; set; }

    [Column("fecha_parte")]
    public DateOnly? FechaParte { get; set; }

    [Column("estado")]
    [StringLength(1)]
    public string? Estado { get; set; }

    [Column("usuario_cre_id")]
    public int? UsuarioCreId { get; set; }

    [Column("fecha_cre", TypeName = "datetime")]
    public DateTime? FechaCre { get; set; }

    [Column("usuario_mod_id")]
    public int? UsuarioModId { get; set; }

    [Column("fecha_mod", TypeName = "datetime")]
    public DateTime? FechaMod { get; set; }

    [Column("equipo_cre")]
    [StringLength(150)]
    public string? EquipoCre { get; set; }

    [Column("equipo_mod")]
    [StringLength(150)]
    public string? EquipoMod { get; set; }

    [Column("usuarioaprob")]
    public int? Usuarioaprob { get; set; }

    [Column("fechaaprob", TypeName = "datetime")]
    public DateTime? Fechaaprob { get; set; }

    [Column("observacion", TypeName = "text")]
    public string? Observacion { get; set; }

    [Column("fechaobs", TypeName = "datetime")]
    public DateTime? Fechaobs { get; set; }

    [Column("cajas_emp")]
    [Precision(10, 0)]
    public decimal? CajasEmp { get; set; }

    [Column("idorden")]
    public int? Idorden { get; set; }

    [Column("usuarioanul")]
    public int? Usuarioanul { get; set; }

    [Column("fechaAnul", TypeName = "datetime")]
    public DateTime? FechaAnul { get; set; }
}
