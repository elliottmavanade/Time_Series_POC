using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Table("Scheduled_Calcs")]
public partial class ScheduledCalc
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("Sensor_Id")]
    public int SensorId { get; set; }

    [ForeignKey("SensorId")]
    [InverseProperty("ScheduledCalcs")]
    public virtual Sensor Sensor { get; set; } = null!;
}
