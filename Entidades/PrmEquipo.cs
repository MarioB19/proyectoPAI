using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entidades;

[Table("prm_equipos")]
[Index("Id", IsUnique = true)]
public partial class PrmEquipo
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("id_equipo")]
    public long IdEquipo { get; set; }

    [Required]
    [Column("id_prestamo")]
    public long IdPrestamo { get; set; }

    [ForeignKey("IdEquipo")]
    [InverseProperty("PrmEquipos")]
    public virtual Equipo IdEquipoNavigation { get; set; } = null!;

    [ForeignKey("IdPrestamo")]
    [InverseProperty("PrmEquipos")]
    public virtual Prestamo IdPrestamoNavigation { get; set; } = null!;
}
