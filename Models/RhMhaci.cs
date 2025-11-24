using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[Keyless]
[Table("rh_mhaci")]
[Index("CodHacienda", Name = "idxHacienda")]
[MySqlCollation("utf8mb3_general_ci")]
public partial class RhMhaci
{
    [Column("COD_HACIENDA")]
    public int? CodHacienda { get; set; }

    [Column("NOM_HACIENDA")]
    [StringLength(40)]
    public string? NomHacienda { get; set; }

    [Column("ESTADO")]
    [StringLength(1)]
    public string? Estado { get; set; }
}
