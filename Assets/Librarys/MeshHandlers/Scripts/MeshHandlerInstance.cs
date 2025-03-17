namespace Librarys.MeshHandlers.Scripts
{
    public class MeshHandlerInstance : MeshHandler
    {
        public static MeshHandlerInstance Instance { get; private set; }
        protected override void OnEnable()
        {
            base.OnEnable();
            Instance = this;
        }

        protected override void OnDisable()
        {
            base.OnEnable();
            Instance = Instance == this ? null : Instance;
        }
    }
}