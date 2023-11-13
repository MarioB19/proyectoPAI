using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entidades;

[Table("interfaces")]
[Index("Id", IsUnique = true)]
public partial class Interfaz
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [StringLength(60)]
    [Column("nombre")]
    public string? Nombre { get; set; }

    [InverseProperty("IdInterfazNavigation")]
    public virtual ICollection<AccInterfaz> AccInterfaces { get; set; } = new List<AccInterfaz>();
}
