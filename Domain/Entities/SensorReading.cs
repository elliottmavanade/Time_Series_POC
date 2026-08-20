using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Table("Sensor_Readings")]
[Index("SensorId", "DateCreated", Name = "IDX_Sensor_Readings_SensorDate")]
public partial class SensorReading
{
    [Key]
    public int Id { get; set; }

    [Column("Sensor_Id")]
    public int SensorId { get; set; }

    [Column("Sensor_Value")]
    public int SensorValue { get; set; }

    [Column("Date_Created", TypeName = "datetime")]
    public DateTime DateCreated { get; set; }

    [ForeignKey("SensorId")]
    [InverseProperty("SensorReadings")]
    public virtual Sensor Sensor { get; set; } = null!;
}
