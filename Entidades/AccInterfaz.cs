using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entidades;

[Table("acc_interfaces")]
[Index("Id", IsUnique = true)]
public partial class AccInterfaz
{
    [Key]
    [Column("id")]
    [Required]
    public long Id { get; set; }

    [Column("id_tpo_usuario")]
    [Required]
    public long IdTpoUsuario { get; set; }

    [Column("id_interfaz")]
    [Required]
    public long IdInterfaz { get; set; }

    [ForeignKey("IdInterfaz")]
    [InverseProperty("AccInterfaces")]
    public virtual Interfaz IdInterfazNavigation { get; set; } = null!;

    [ForeignKey("IdTpoUsuario")]
    [InverseProperty("AccInterfaces")]
    public virtual TpsUsuario IdTpoUsuarioNavigation { get; set; } = null!;
}
