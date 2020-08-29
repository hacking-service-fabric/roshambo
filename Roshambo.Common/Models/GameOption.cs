using System.Runtime.Serialization;

namespace Roshambo.Common.Models
{
    [DataContract]
    public enum GameOption
    {
        [EnumMember] Rock,
        [EnumMember] Paper,
        [EnumMember] Scissor
    }
}
