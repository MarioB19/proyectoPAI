using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entidades;

[Table("usuarios")]
[Index("Id", IsUnique = true)]
public partial class Usuario
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("id_tpo_usuario")]
    [Required]
    public long IdTpoUsuario { get; set; }

    [Required]
    [Column("id_est_usuario")]
    public long IdEstUsuario { get; set; }


    [Required]
    [Column("contrasena")]
    public byte[]? Contrasena { get; set; }

    [Column("nombre")]
    [StringLength(60)]
    public string? Nombre { get; set; }

    [Column("apl_materno")]
    [StringLength(70)]
    public string? AplMaterno { get; set; }

    [Column("apl_paterno")]
    [StringLength(70)]
    public string? AplPaterno { get; set; }

    [Column("fch_creacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchCreacion { get; set; }

    [Column("fch_modificacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchModificacion { get; set; }

    [Column("fch_eliminacion", TypeName = "DATETIME")]
    [DataType(DataType.DateTime)]
    public DateTime? FchEliminacion { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Almacenista> Almacenista { get; set; } = new List<Almacenista>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Coordinador> Coordinadores { get; set; } = new List<Coordinador>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();

    [ForeignKey("IdEstUsuario")]
    [InverseProperty("Usuarios")]
    public virtual EstUsuario IdEstUsuarioNavigation { get; set; } = null!;

    [ForeignKey("IdTpoUsuario")]
    [InverseProperty("Usuarios")]
    public virtual TpsUsuario IdTpoUsuarioNavigation { get; set; } = null!;

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Profesor> Profesores { get; set; } = new List<Profesor>();
}
