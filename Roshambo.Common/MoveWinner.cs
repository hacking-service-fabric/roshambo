using System.Runtime.Serialization;

namespace Roshambo.Common
{
    [DataContract]
    public enum MoveWinner
    {
        [EnumMember] Human,
        [EnumMember] Computer,
        [EnumMember] Tie
    }
}
