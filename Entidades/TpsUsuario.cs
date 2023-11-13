using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entidades;

[Table("tps_usuarios")]
[Index("Id", IsUnique = true)]
public partial class TpsUsuario
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("descripcion")]
    [StringLength(100)]
    public string? Descripcion { get; set; }

    [InverseProperty("IdTpoUsuarioNavigation")]
    public virtual ICollection<AccInterfaz> AccInterfaces { get; set; } = new List<AccInterfaz>();

    [InverseProperty("IdTpoUsuarioNavigation")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
