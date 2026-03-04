using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[Table("aud_registros")]
[MySqlCollation("utf8mb3_general_ci")]
public partial class AudRegistro
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("fechacre", TypeName = "datetime")]
    public DateTime? Fechacre { get; set; }

    [Column("usuariocre")]
    public int? Usuariocre { get; set; }

    [Column("maquina", TypeName = "text")]
    public string? Maquina { get; set; }

    [Column("script", TypeName = "text")]
    public string? Script { get; set; }
}
