namespace KrampStudio.Events
{

    public class NewEvent : EventRoot
    {
        public int Score { get; }
        public string CallerName { get; }
        public NewEvent(int score, string callerName)
        {
            Score = score;
            CallerName = callerName;
        }
    }

    public class NewEvent2 : EventRoot
    {
        public NewEvent2()
        {

        }
    }
}