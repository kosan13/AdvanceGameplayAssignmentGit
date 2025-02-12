namespace Events
{
    public interface IEvent
    {
        void OnBegin(bool bFirstTime);
        void OnUpdate();
        void OnEnd();
        bool IsDone();
    }
}