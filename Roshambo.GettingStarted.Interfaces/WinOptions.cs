using System.Runtime.Serialization;

namespace Roshambo.GettingStarted.Interfaces
{
    [DataContract]
    public enum WinOptions
    {
        Won,
        Lost,
        Tied
    }
}
