using System.Runtime.Serialization;

namespace Roshambo.Common.Models
{
    [DataContract]
    public enum TurnWinner
    {
        [EnumMember] Human,
        [EnumMember] Computer,
        [EnumMember] Tie
    }
}
