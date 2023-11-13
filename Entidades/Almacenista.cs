using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entidades;

[Table("almacenistas")]
[Index("Id", IsUnique = true)]
public partial class Almacenista
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("id_usuario")]
    public long IdUsuario { get; set; }

    [Required]
    [Column("nomina")]
    public long? Nomina { get; set; }

    
    [Column("fch_creacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchCreacion { get; set; }

    [Column("fch_modificacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchModificacion { get; set; }

    [Column("fch_eliminacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchEliminacion { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Almacenista")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
