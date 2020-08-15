using System.Runtime.Serialization;

namespace Roshambo.Common
{
    [DataContract]
    public enum TurnWinner
    {
        [EnumMember] Human,
        [EnumMember] Computer,
        [EnumMember] Tie
    }
}
