using Librarys.EventSystem.Interfaces;
using UnityEngine;

namespace Librarys.EventSystem.BaseEventClass
{
    public abstract class GameEventBehaviour : MonoBehaviour, IEvent
    {
        public virtual void OnBegin(bool bFirstTime) { }
        public virtual void OnUpdate() { }
        public virtual void OnEnd() { }
        public virtual bool IsDone() => true;
    }
}