using Event;
using Events;

namespace UI
{
    public class OptionsMenu : GameEventBehaviour
    {
        private bool _done;
        private void OnEnable() => EventHandler.Main.PushEvent(this);
        private void OnDisable() => EventHandler.Main.RemoveEvent(this);
        public override bool IsDone() { return _done; }
        public void OnBack() => _done = true;
    }
}