using System.Collections.Generic;
using System.Linq;
using Event.BaseEventClass;
using Event.Events;
using Newtonsoft.Json;
using ScriptableObject;
using TileSystem.Tile_Class;
using UnityEngine;
using static Dies.DiesFunction;
using static SortingAlgorithms.SortingAlgorithms;
using EventHandler = Event.EventHandler;
using Random = UnityEngine.Random;

namespace Game.UnitClasses
{
    public class Unit : GameEventMeshHandlerBehaviour
    {
        private Tile _occupiedTile;
        private const float MoveSpeed = 10;

        #region Properties

        public int CurrentHealth { get; protected set; }
        public int CurrentMovement { get; protected set; }
        public int CurrentActionsPoints { get; protected set; }
        public Vector3 WorldPosition { get; private set; }
        public int Initiative { get; protected set; }
        public Tile OccupiedTile
        {
            get => _occupiedTile;
            set
            {
                if (value == null) { _occupiedTile = null; return; }
                _occupiedTile?.SetIsOccupied(false);
                _occupiedTile = value;
                _occupiedTile.SetIsOccupied(true);
                WorldPosition = value.GetWorldPosition;
            }
        }
        [JsonIgnore] protected static HashSet<Tile> GetLevel => BlobDivisionMaze.Instance.TileLevel;
        public virtual bool IsPlayer { get; protected set; } = false;
        public static List<Unit> AllUnits = new();
        [JsonIgnore] public static List<Unit> Enemies => AllUnits.FindAll(IsEnemy);
        [JsonIgnore] public List<Unit> EnemiesInRange => Enemies.FindAll(e => Vector3.Distance(transform.position, e.transform.position) <= CurrentMovement);
        [JsonIgnore] public MeshFilter GetMeshFilter => MeshFilter;
        [JsonIgnore] public MeshRenderer GetMeshRenderer => MeshRenderer;
        [JsonIgnore] public MeshCollider GetMeshCollider => MeshCollider;
        
        protected int RollInitiative() => Initiative = RollADie(UnitScriptableObject.InitiativeDies);
        public Tile SetOccupiedTile(Tile tile) => OccupiedTile = tile;

        public int RemoveMovement(int value) => CurrentMovement - value;
        public int RemoveOneMovement() => RemoveMovement(1);

        #endregion

        private void OnEnable() => AllUnits.Add(this);
        private void OnDisable() => AllUnits.Remove(this);

        public override void OnBegin(bool bFirstTime)
        {
            CurrentMovement = UnitScriptableObject.MaxMovement;
            CurrentActionsPoints = UnitScriptableObject.MaxActionsPoints;
        
            if (bFirstTime)
            {
                CurrentHealth = UnitScriptableObject.MaxHealth;
                // roll for initiative
                RollInitiative();
                // get tile
                Tile tile = GetLevel.ElementAt(Random.Range(0, GetLevel.Count - 1));
                while (tile.GetIsOccupied) tile = GetLevel.ElementAt(Random.Range(0, GetLevel.Count - 1));
                OccupiedTile = tile;
                
                // increase turn
                if (IsPlayer) EventHandler.Main.PushEvent(new UnitInputEvent(this));
                else {/*DoAIMove();*/}
            }
        }
        private void Update() => transform.position = Vector3.Lerp(transform.position, WorldPosition, Time.deltaTime * MoveSpeed);
        public void TakeDamage(int iDmg) { }
        public override bool IsDone() => CurrentActionsPoints == 0;
        public static bool IsEnemy(Unit unit) => unit.IsPlayer == false;

        public static void Sort(List<Unit> units, bool largestToSmallest = false) { QuickSortUnit(units, 0, units.Count - 1); if (largestToSmallest) units.Reverse(); }
    }
}