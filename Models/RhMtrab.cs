using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[Keyless]
[Table("rh_mtrab")]
[Index("NombreCorto", "Empresa", Name = "idxemptrab")]
[Index("CodTrabaj", Name = "idxtrabaj")]
[Index("CodTrabaj", "CodEmpresa", Name = "idxtrabajemp")]
[MySqlCollation("utf8mb3_general_ci")]
public partial class RhMtrab
{
    [Column("COD_TRABAJ")]
    public int? CodTrabaj { get; set; }

    [Column("NUM_CEDULA")]
    [StringLength(13)]
    public string? NumCedula { get; set; }

    [Column("APELLIDO_1")]
    [StringLength(20)]
    public string? Apellido1 { get; set; }

    [Column("APELLIDO_2")]
    [StringLength(20)]
    public string? Apellido2 { get; set; }

    [Column("NOMBRE_1")]
    [StringLength(20)]
    public string? Nombre1 { get; set; }

    [Column("NOMBRE_2")]
    [StringLength(20)]
    public string? Nombre2 { get; set; }

    [Column("NOMBRE_CORTO")]
    [StringLength(54)]
    public string? NombreCorto { get; set; }

    [Column("FEC_SALIDA")]
    public DateOnly? FecSalida { get; set; }

    [Column("COD_LABOR")]
    public int? CodLabor { get; set; }

    [Column("COD_HACIENDA")]
    public int? CodHacienda { get; set; }

    [Column("COD_CENTRO")]
    public int? CodCentro { get; set; }

    [Column("COD_TIPO")]
    public int? CodTipo { get; set; }

    [Column("HECTAREAS")]
    public float? Hectareas { get; set; }

    [Column("ESTADO")]
    [StringLength(1)]
    public string? Estado { get; set; }

    [Column("COD_EMPRESA")]
    public int? CodEmpresa { get; set; }

    [Column("EMPRESA")]
    [StringLength(100)]
    public string? Empresa { get; set; }

    [Column("SUELDO")]
    public sbyte? Sueldo { get; set; }

    [Column("TIP_ROL")]
    public int? TipRol { get; set; }

    [Column("TIP_CONTRATO")]
    public int? TipContrato { get; set; }

    [Column("SOLO_CORTE")]
    public sbyte? SoloCorte { get; set; }

    [Column("FEC_INGRESO")]
    public DateOnly? FecIngreso { get; set; }

    [Column("fechasub", TypeName = "datetime")]
    public DateTime? Fechasub { get; set; }

    [Column("usuariosub")]
    public int? Usuariosub { get; set; }

    [Column("codcargo")]
    public int? Codcargo { get; set; }

    [Column("nomcargo")]
    [StringLength(100)]
    public string? Nomcargo { get; set; }

    [Column("edad")]
    public int? Edad { get; set; }

    [Column("fec_nacimiento")]
    public DateOnly? FecNacimiento { get; set; }

    [Column("es_lotero")]
    public int? EsLotero { get; set; }

    [Column("fl_cargo")]
    [StringLength(100)]
    public string? FlCargo { get; set; }
}
