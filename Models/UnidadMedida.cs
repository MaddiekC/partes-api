using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartesApi.Models;

[Table("unidad_medida")]
[MySqlCharSet("latin1")]
[MySqlCollation("latin1_swedish_ci")]
public partial class UnidadMedida
{
    [Key]
    [Column("id")]
    public uint Id { get; set; }

    [Column("nombre")]
    [StringLength(25)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(50)]
    public string Descripcion { get; set; } = null!;

    [Column("estado")]
    [StringLength(1)]
    public string Estado { get; set; } = null!;

    [Column("fechaCre", TypeName = "datetime")]
    public DateTime FechaCre { get; set; }

    [Column("usuarioCre")]
    public uint UsuarioCre { get; set; }

    [Column("fechaMod", TypeName = "datetime")]
    public DateTime FechaMod { get; set; }

    [Column("usuarioMod")]
    public uint UsuarioMod { get; set; }
}
