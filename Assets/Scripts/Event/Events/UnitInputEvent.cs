using Librarys.EventSystem;
using Librarys.Graphs.Enum;
using UnityEngine;
using Input = UnityEngine.Input;
using Unit = Game.UnitClasses.Unit;

namespace Event.Events
{
    public class UnitInputEvent : UnitEvent
    {
        private bool _done;

        public UnitInputEvent(Unit unit) : base(unit) { }
        
        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.W)) EventHandler.Main.PushEvent(new UnitMoveEvent(Unit, Direction.Forward));
            if (Input.GetKeyDown(KeyCode.A)) EventHandler.Main.PushEvent(new UnitMoveEvent(Unit, Direction.Left));
            if (Input.GetKeyDown(KeyCode.S)) EventHandler.Main.PushEvent(new UnitMoveEvent(Unit, Direction.Back));
            if (Input.GetKeyDown(KeyCode.D)) EventHandler.Main.PushEvent(new UnitMoveEvent(Unit, Direction.Right));
        }
        
        public override bool IsDone() => _done;
    }
}
