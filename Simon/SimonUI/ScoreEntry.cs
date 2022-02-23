using System;

namespace SimonUI
{
    public class ScoreEntry
    {
        public string Name { get; set; }
        public double Score { get; set; }
        public DateTime CreationTime { get; set; }

        public ScoreEntry(string playerName, double score, DateTime creationTime)
        {
            Name = playerName;
            Score = score;
            CreationTime = creationTime;
        }
    }
}
