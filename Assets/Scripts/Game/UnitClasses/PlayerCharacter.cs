using Events;
using UnityEngine;
using EventHandler = Events.EventHandler;

namespace Game.UnitClasses
{
    public class PlayerCharacter : Unit
    {
        #region Properties
        
        public static PlayerCharacter Instance { get; private set; }

        #endregion

        private void Start()
        {
            Instance = this;
            RotationEvent.CreatRotationEvent();
            EventHandler.Main.PushEvent(this);
        }
        private void OnDisable() => Instance = Instance == this ? null : Instance;

        public override void OnBegin(bool bFirstTime)
        {
            base.OnBegin(bFirstTime);
            if (bFirstTime) { }
        }
        
        public static GameObject CreatPlayerCharacter() => (GameObject)Resources.Load("Prefab/PlayerCharacter");
        public static GameObject CreatAndInstantiatePlayerCharacter() => Instantiate(CreatPlayerCharacter());
    }
}