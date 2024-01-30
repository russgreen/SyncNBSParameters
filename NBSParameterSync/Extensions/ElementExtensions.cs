using Autodesk.Revit.DB;
using Nice3point.Revit.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace SyncNBSParameters.Extensions;
internal static class ElementExtensions
{
    public static bool HasNewChorusParameters(this Element element, List<string> parameterNames)
    {
        var parameters = element.Parameters.Cast<Parameter>().ToList();

        foreach (var parameter in parameterNames)
        {
            if (parameters.Any(p => p.Definition.Name == parameter && p.AsValueString().IsNullOrEmpty() == false))
            {
                return true;
            }
        }

        return false; 
    }
}
