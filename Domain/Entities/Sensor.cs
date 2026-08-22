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

    // Sensor_Calc_Relationships (self-referencing parent/child) is modeled as an explicit
    // SensorCalcRelationship join entity (see Domain.Entities.SensorCalcRelationship) rather than an
    // EF skip-navigation many-to-many, so no SensorChildren/SensorParents navigation collections live here.
}
