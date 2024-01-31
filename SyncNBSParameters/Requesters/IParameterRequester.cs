using SyncNBSParameters.Models;

namespace SyncNBSParameters.Requesters;
public interface IParameterRequester
{
    void ParameterComplete(string variableName, ParameterDataModel parameterDataModel);
}
