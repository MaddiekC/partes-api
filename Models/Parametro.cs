using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

/// <summary>
/// Todos los parametros del Sistema
/// </summary>
[Table("parametro")]
[MySqlCharSet("latin1")]
[MySqlCollation("latin1_swedish_ci")]
public partial class Parametro
{
    [Key]
    [Column("id")]
    public uint Id { get; set; }

    [Column("tipo")]
    [StringLength(20)]
    public string Tipo { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(100)]
    public string Descripcion { get; set; } = null!;

    [Column("sectipo")]
    public uint Sectipo { get; set; }

    [Column("string1")]
    [StringLength(100)]
    public string String1 { get; set; } = null!;

    [Column("string2")]
    [StringLength(100)]
    public string String2 { get; set; } = null!;

    [Column("valor1")]
    [Precision(10, 4)]
    public decimal Valor1 { get; set; }

    [Column("valor2")]
    [Precision(10, 4)]
    public decimal Valor2 { get; set; }

    [Column("estado")]
    [StringLength(1)]
    public string Estado { get; set; } = null!;

    [Column("fechaCre", TypeName = "datetime")]
    public DateTime FechaCre { get; set; }

    [Column("usuarioCre")]
    public uint UsuarioCre { get; set; }

    [Column("fechaMod", TypeName = "datetime")]
    public DateTime? FechaMod { get; set; }

    [Column("usuarioMod")]
    public uint? UsuarioMod { get; set; }

    /// <summary>
    /// 2=Maxima 1=Mediana 0=Ninguna
    /// </summary>
    [Column("prioridad")]
    public uint? Prioridad { get; set; }
}
