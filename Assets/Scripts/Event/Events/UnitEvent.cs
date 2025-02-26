using Event.BaseEventClass;
using Game.UnitClasses;

namespace Event.Events
{
    public abstract class UnitEvent : GameEvent
    {
        protected readonly Unit Unit;
        protected UnitEvent(Unit unit) => Unit = unit;
        public override string ToString() => $"Unit {GetType().Name} Input Event: {Unit.name}";
    }
}