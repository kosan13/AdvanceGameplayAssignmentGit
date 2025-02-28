using Event.Events;
using Newtonsoft.Json;
using UnityEngine;
using EventHandler = Event.EventHandler;

namespace Game.UnitClasses
{
    public class PlayerCharacter : Unit
    {
        private const string Path = "Prefab/PlayerCharacter";
        #region Properties

        [JsonIgnore] public static PlayerCharacter Instance { get; private set; }
        // protected override bool IsPlayer { get; set; } = true;

        #endregion

        private void Start()
        {
            Instance = this;
            IsPlayer = true;
            RotationEvent.CreatRotationEvent();
            EventHandler.Main.PushEvent(this);
        }
        private void OnDisable() => Instance = Instance == this ? null : Instance;
        public static GameObject CreatPlayerCharacter() => (GameObject)Resources.Load(Path);
        public static GameObject CreatAndInstantiatePlayerCharacter() => Instantiate(CreatPlayerCharacter());

        public static PlayerCharacter SetPlayerCharacterInstance(PlayerCharacter newPlayerCharacter) => Instance = newPlayerCharacter;
        public static PlayerCharacter SetAndInstantiatePlayerCharacterInstance(PlayerCharacter newPlayerCharacter) => Instantiate(SetPlayerCharacterInstance(newPlayerCharacter));
    }
}