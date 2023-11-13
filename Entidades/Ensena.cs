using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entidades;

[Table("ensena")]
[Index("Id", IsUnique = true)]
public partial class Ensena
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("id_profesor")]
    public long IdProfesor { get; set; }

    [Column("id_grupo")]
    public long IdGrupo { get; set; }

    [ForeignKey("IdGrupo")]
    [InverseProperty("Ensenas")]
    public virtual Grupo IdGrupoNavigation { get; set; } = null!;

    [ForeignKey("IdProfesor")]
    [InverseProperty("Ensenas")]
    public virtual Profesor IdProfesorNavigation { get; set; } = null!;
}
