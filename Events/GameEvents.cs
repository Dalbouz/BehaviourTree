namespace KrampStudio.Events
{
    public class WorkingHourEvent : EventRoot
    {
        public bool IsMuseumOpened { get; }
        public WorkingHourEvent(bool isMuseumOpened)
        {
            IsMuseumOpened = isMuseumOpened;
        }
    }
}
