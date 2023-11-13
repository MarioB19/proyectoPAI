using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entidades;

[Table("prestamos")]
[Index("Id", IsUnique = true)]
public partial class Prestamo
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("id_tpo_prestamo")]
    public long IdTpoPrestamo { get; set; }

    [Required]
    [Column("id_est_prestamo")]
    public long IdEstPrestamo { get; set; }

    [Column("id_salon")]
    public long IdSalon { get; set; }

    [Column("id_profesor")]
    public long IdProfesor { get; set; }

    [Column("id_usuario")]
    public long IdUsuario { get; set; }

    [Column("fch_inicio", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchInicio { get; set; }

    [Column("fch_fin", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchFin { get; set; }

    [Column("fch_creacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchCreacion { get; set; }

    [Column("fch_modificacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchModificacion { get; set; }

    [Column("fch_eliminacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchEliminacion { get; set; }

    [ForeignKey("IdEstPrestamo")]
    [InverseProperty("Prestamos")]
    public virtual EstPrestamo IdEstPrestamoNavigation { get; set; } = null!;

    [ForeignKey("IdProfesor")]
    [InverseProperty("Prestamos")]
    public virtual Profesor IdProfesorNavigation { get; set; } = null!;

    [ForeignKey("IdSalon")]
    [InverseProperty("Prestamos")]
    public virtual Salon IdSalonNavigation { get; set; } = null!;

    [ForeignKey("IdTpoPrestamo")]
    [InverseProperty("Prestamos")]
    public virtual TpsPrestamo IdTpoPrestamoNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("Prestamos")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    [InverseProperty("IdPrestamoNavigation")]
    public virtual ICollection<PrmEquipo> PrmEquipos { get; set; } = new List<PrmEquipo>();
}
