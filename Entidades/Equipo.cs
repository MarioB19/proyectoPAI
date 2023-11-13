using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entidades;

[Table("equipos")]
[Index("Id", IsUnique = true)]
public partial class Equipo
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("cnt_disponible")]
    public long? CntDisponible { get; set; }

    [Required]
    [StringLength(60)]
    [Column("nombre")]
    public string? Nombre { get; set; }

    [Required]
    [StringLength(80)]
    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("fch_creacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchCreacion { get; set; }

    [Column("fch_modificacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchModificacion { get; set; }

    [Column("fch_eliminacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchEliminacion { get; set; }

    [Required]
    [Column("num_inventario")]
    public long? NumInventario { get; set; }

    [Required]
    [Column("anio_material")]
    public long? AnioMaterial { get; set; }

    [InverseProperty("IdEquipoNavigation")]
    public virtual ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();

    [InverseProperty("IdEquipoNavigation")]
    public virtual ICollection<PrmEquipo> PrmEquipos { get; set; } = new List<PrmEquipo>();
}
