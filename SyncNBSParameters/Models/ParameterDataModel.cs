using Autodesk.Revit.DB;

namespace SyncNBSParameters.Models;
public class ParameterDataModel
{
    public ElementId ID { get; set; } = ElementId.InvalidElementId;
    public string Name { get; set; } = string.Empty;
    public string Guid { get; set; } = string.Empty;
    public ElementBinding Binding { get; set; } = null!;
    public Definition Definition { get; set; } = null!;
}
