using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entidades;

[Table("mantenimientos")]
[Index("Id", IsUnique = true)]
public partial class Mantenimiento
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("id_tpo_mantenimiento")]
    public long IdTpoMantenimiento { get; set; }

    [Required]
    [Column("id_equipo")]
    public long IdEquipo { get; set; }

    [Required]
    [StringLength(100)]
    [Column("descripciones")]
    public string? Descripciones { get; set; }

    [Required]
    [StringLength(80)]
    [Column("refaccion")]
    public string? Refaccion { get; set; }

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

    [ForeignKey("IdEquipo")]
    [InverseProperty("Mantenimientos")]
    public virtual Equipo IdEquipoNavigation { get; set; } = null!;

    [ForeignKey("IdTpoMantenimiento")]
    [InverseProperty("Mantenimientos")]
    public virtual TpsMantenimiento IdTpoMantenimientoNavigation { get; set; } = null!;
}
