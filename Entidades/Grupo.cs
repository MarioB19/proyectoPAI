using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entidades;

[Table("grupos")]
[Index("Id", IsUnique = true)]
public partial class Grupo
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [StringLength(60)]
    [Column("nombre")]
    public string? Nombre { get; set; }

    [Column("fch_creacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchCreacion { get; set; }

    [Column("fch_modificacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchModificacion { get; set; }

    [Column("fch_eliminacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchEliminacion { get; set; }

    [InverseProperty("IdGrupoNavigation")]
    public virtual ICollection<Ensena> Ensenas { get; set; } = new List<Ensena>();

    [InverseProperty("IdGrupoNavigation")]
    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
}
