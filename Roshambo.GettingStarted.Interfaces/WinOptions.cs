using System.Runtime.Serialization;

namespace Roshambo.GettingStarted.Interfaces
{
    [DataContract]
    public enum WinOptions
    {
        [EnumMember] Won,
        [EnumMember] Lost,
        [EnumMember] Tied
    }
}
