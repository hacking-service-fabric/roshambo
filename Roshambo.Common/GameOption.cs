using System.Runtime.Serialization;

namespace Roshambo.Common
{
    [DataContract]
    public enum GameOption
    {
        [EnumMember] Rock,
        [EnumMember] Paper,
        [EnumMember] Scissor
    }
}
