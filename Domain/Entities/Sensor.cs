using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public partial class Sensor
{
    [Key]
    public int Id { get; set; }

    [Column("Sensor_Model_Id")]
    public int SensorModelId { get; set; }

    [StringLength(255)]
    public string Location { get; set; } = null!;

    public string? Metadata { get; set; }

    [InverseProperty("Sensor")]
    public virtual ICollection<ScheduledCalc> ScheduledCalcs { get; set; } = new List<ScheduledCalc>();

    [ForeignKey("SensorModelId")]
    [InverseProperty("Sensors")]
    public virtual SensorCategory SensorModel { get; set; } = null!;

    [InverseProperty("Sensor")]
    public virtual ICollection<SensorReading> SensorReadings { get; set; } = new List<SensorReading>();

    [ForeignKey("SensorParentId")]
    [InverseProperty("SensorParents")]
    public virtual ICollection<Sensor> SensorChildren { get; set; } = new List<Sensor>();

    [ForeignKey("SensorChildId")]
    [InverseProperty("SensorChildren")]
    public virtual ICollection<Sensor> SensorParents { get; set; } = new List<Sensor>();
}
