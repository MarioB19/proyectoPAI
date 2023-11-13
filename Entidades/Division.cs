using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entidades;

[Table("divisiones")]
[Index("Id", IsUnique = true)]
public partial class Division
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("descripcion")]
    [StringLength(90)]
    public string? Descripcion { get; set; }

    [Column("fch_creacion",TypeName = "DATETIME")]
     [DataType(DataType.DateTime)]
    public DateTime? FchCreacion { get; set; }

    [Column("fch_modificacion", TypeName = "DATETIME")]
     [DataType(DataType.DateTime)]
    public DateTime? FchModificacion { get; set; }

    [Column("fch_eliminacion",TypeName = "DATETIME")]
     [DataType(DataType.DateTime)]
    public DateTime? FchEliminacion { get; set; }

    [InverseProperty("IdDivisionNavigation")]
    public virtual ICollection<Salon> Salones { get; set; } = new List<Salon>();
}
