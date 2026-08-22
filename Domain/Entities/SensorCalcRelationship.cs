using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Explicit join entity over the self-referencing <c>Sensor_Calc_Relationships</c> table
/// (parent sensor id -&gt; child sensor id), modeled deliberately as its own entity rather than an
/// EF skip-navigation many-to-many so that querying a parent sensor's children stays a straightforward,
/// explicit query.
/// </summary>
[Table("Sensor_Calc_Relationships")]
public partial class SensorCalcRelationship
{
    [Column("Sensor_Parent_Id")]
    public int ParentSensorId { get; set; }

    [Column("Sensor_Child_Id")]
    public int ChildSensorId { get; set; }
}
