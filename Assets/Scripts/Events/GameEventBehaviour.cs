using UnityEngine;

namespace Events
{
    public abstract class GameEventBehaviour : MonoBehaviour, IEvent
    {
        public virtual void OnBegin(bool bFirstTime) { }
        public virtual void OnUpdate() { }
        public virtual void OnEnd() { }
        public virtual bool IsDone() { return true; }
    }
}