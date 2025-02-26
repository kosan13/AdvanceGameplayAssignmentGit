using MeshHandlers;

namespace Events
{
    public abstract class GameEventMeshHandlerBehaviour : MeshHandler, IEvent
    {
        public virtual void OnBegin(bool bFirstTime) { }
        public virtual void OnUpdate() { }
        public virtual void OnEnd() { }
        public virtual bool IsDone() { return true; }
    }
}