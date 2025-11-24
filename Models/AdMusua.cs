using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[Keyless]
[Table("ad_musua")]
[Index("UsCodigo", Name = "idx_uscodigo")]
[MySqlCollation("utf8mb3_general_ci")]
public partial class AdMusua
{
    [Column("US_CODIGO")]
    public int? UsCodigo { get; set; }

    [Column("US_NOMBRE")]
    [StringLength(50)]
    public string? UsNombre { get; set; }

    [Column("US_LOGIN")]
    [StringLength(20)]
    public string? UsLogin { get; set; }

    [Column("US_PASSW")]
    [StringLength(50)]
    public string? UsPassw { get; set; }

    [Column("US_FECING")]
    public DateOnly? UsFecing { get; set; }

    [Column("US_TIPUSU")]
    [StringLength(1)]
    public string? UsTipusu { get; set; }

    [Column("US_EMAIL")]
    [StringLength(100)]
    public string? UsEmail { get; set; }

    [Column("US_ESTADO")]
    [StringLength(1)]
    public string? UsEstado { get; set; }

    [Column("us_mobil")]
    public int? UsMobil { get; set; }
}
