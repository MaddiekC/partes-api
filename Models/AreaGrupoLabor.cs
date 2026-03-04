using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[Keyless]
[Table("area_grupo_labor")]
public partial class AreaGrupoLabor
{
    [Column("idArea")]
    public int? IdArea { get; set; }

    [Column("idGrupo")]
    public int? IdGrupo { get; set; }

    [Column("estado")]
    [StringLength(1)]
    [MySqlCharSet("latin1")]
    [MySqlCollation("latin1_swedish_ci")]
    public string? Estado { get; set; }
}
