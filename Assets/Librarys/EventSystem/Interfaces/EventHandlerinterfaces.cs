namespace Librarys.EventSystem.Interfaces
{
    public interface IEvent
    {
        void OnBegin(bool bFirstTime);
        void OnUpdate();
        void OnEnd();
        bool IsDone();
    }
}