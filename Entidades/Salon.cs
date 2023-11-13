using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entidades;

[Table("salones")]
public partial class Salon
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("id_division")]
    public long? IdDivision { get; set; }

    [Required]
    [Column("nmr_salon")]
    public long? NmrSalon { get; set; }

    [ForeignKey("IdDivision")]
    [InverseProperty("Salones")]
    public virtual Division? IdDivisionNavigation { get; set; }

    [InverseProperty("IdSalonNavigation")]
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
