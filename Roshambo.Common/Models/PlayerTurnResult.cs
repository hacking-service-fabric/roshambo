using System;
using System.Collections.Generic;
using System.Text;

namespace Roshambo.Common.Models
{
    public class PlayerTurnResult
    {
        public int CurrentStreak { get; set; }
        public int PreviousStreak { get; set; }
        public bool NextMoveReady { get; set; }
    }
}
