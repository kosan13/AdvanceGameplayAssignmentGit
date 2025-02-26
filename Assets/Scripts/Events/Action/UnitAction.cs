using Game.UnitClasses;

namespace Events.Action
{
    public abstract class UnitAction : GameEvent
    {
        protected readonly Unit Unit;
        protected UnitAction(Unit unit) => Unit = unit;
        public override string ToString() => $"Unit {GetType().Name} Input Event: {Unit.name}";
    }
}