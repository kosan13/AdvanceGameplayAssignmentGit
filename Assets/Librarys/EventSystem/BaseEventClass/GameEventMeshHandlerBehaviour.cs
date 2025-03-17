using Librarys.EventSystem.Interfaces;
using Librarys.MeshHandlers.Scripts;

namespace Librarys.EventSystem.BaseEventClass
{
    public abstract class GameEventMeshHandlerBehaviour : MeshHandler, IEvent
    {
        public virtual void OnBegin(bool bFirstTime) { }
        public virtual void OnUpdate() { }
        public virtual void OnEnd() { }
        public virtual bool IsDone() => true;
    }
}