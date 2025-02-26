using Events;
using UnityEngine;
using EventHandler = Events.EventHandler;

namespace Game.UnitClasses
{
    public class PlayerCharacter : Unit
    {
        private const string Path = "Prefab/PlayerCharacter";
        #region Properties
        
        public static PlayerCharacter Instance { get; private set; }
        protected override bool IsPlayer { get; set; } = true;

        #endregion

        private void Start()
        {
            Instance = this;
            RotationEvent.CreatRotationEvent();
            EventHandler.Main.PushEvent(this);
        }
        private void OnDisable() => Instance = Instance == this ? null : Instance;
        public static GameObject CreatPlayerCharacter() => (GameObject)Resources.Load(Path);
        public static GameObject CreatAndInstantiatePlayerCharacter() => Instantiate(CreatPlayerCharacter());
    }
}