using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Table("Sensor_Categories")]
public partial class SensorCategory
{
    [Key]
    public int Id { get; set; }

    [Column("Sensor_Model")]
    [StringLength(255)]
    public string SensorModel { get; set; } = null!;

    [Column("Measurement_Unit")]
    [StringLength(255)]
    public string MeasurementUnit { get; set; } = null!;

    [InverseProperty("SensorModel")]
    public virtual ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
}
