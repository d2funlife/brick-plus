namespace Tetris.Models.Suspend
{
    public class SuspendingSession
    {
        public SuspendingSession(){}

        public SuspendingSession(SuspendingGameEngine game)
        {
            Game = game;
        }

        public SuspendingGameEngine Game { get; set; }
        public bool ContinueAvailable { get; set; }
        public string Time { get; set; }
        public int SecondsCount { get; set; }        
    }
}