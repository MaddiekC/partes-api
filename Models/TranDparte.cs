using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[Keyless]
[Table("tran_dparte")]
[Index("SecParte", Name = "idx_detparte")]
[Index("CodLabor", Name = "idx_labor")]
[Index("LoteId", Name = "idx_lote")]
[Index("CodTrabaj", Name = "idx_trabaj")]
[MySqlCollation("utf8mb3_general_ci")]
public partial class TranDparte
{
    [Column("sec_parte")]
    public int? SecParte { get; set; }

    [Column("secuencia")]
    public int? Secuencia { get; set; }

    [Column("cod_trabaj")]
    public int? CodTrabaj { get; set; }

    [Column("cod_labor")]
    public int? CodLabor { get; set; }

    [Column("lote_id")]
    public int? LoteId { get; set; }

    [Column("fecha_inicio")]
    public DateOnly? FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateOnly? FechaFin { get; set; }

    [Column("hora_inicio")]
    [StringLength(10)]
    public string? HoraInicio { get; set; }

    [Column("hora_fin")]
    [StringLength(10)]
    public string? HoraFin { get; set; }

    [Column("cantidad")]
    [Precision(10, 2)]
    public decimal? Cantidad { get; set; }

    [Column("valor_unitario")]
    [Precision(15, 5)]
    public decimal? ValorUnitario { get; set; }

    [Column("valor_total")]
    [Precision(16, 6)]
    public decimal? ValorTotal { get; set; }

    [Column("nom_seccion")]
    [StringLength(100)]
    public string? NomSeccion { get; set; }

    [Column("excepcion")]
    public int? Excepcion { get; set; }

    [Column("calificacion")]
    public int? Calificacion { get; set; }

    [Column("motivo_exc")]
    [StringLength(200)]
    public string? MotivoExc { get; set; }

    [Column("motivo_cal")]
    [StringLength(200)]
    public string? MotivoCal { get; set; }

    [Column("tar_val")]
    [Precision(15, 5)]
    public decimal? TarVal { get; set; }

    [Column("val_tar")]
    [Precision(15, 5)]
    public decimal? ValTar { get; set; }

    [Column("idparcela_lote")]
    public int? IdparcelaLote { get; set; }

    [Column("idparcela_empleado")]
    public int? IdparcelaEmpleado { get; set; }
}
