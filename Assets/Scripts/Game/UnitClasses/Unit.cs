using System.Collections.Generic;
using System.Linq;
using Events;
using Events.Action;
using ScriptableObject;
using TileSystem.Tile_Class;
using UnityEngine;
using static Dies.DiesFunction;
using Random = UnityEngine.Random;

namespace Game.UnitClasses
{
    public class Unit : GameEventMeshHandlerBehaviour
    {
        private Tile _occupiedTile;
        
        public int CurrentHealth { get; protected set; }
        public int CurrentMovement { get; protected set; }
        public int CurrentActionsPoints { get; protected set; }
        protected int Initiative { get; private set; }
        protected Tile HoverTile { get; private set; }
        
        public Tile OccupiedTile
        {
            get => _occupiedTile;
            private set
            {
                if (value == null) { _occupiedTile = null; return; }
                _occupiedTile?.SetIsOccupied(false);
                _occupiedTile = value;
                _occupiedTile.SetIsOccupied(true);
                WorldPosition = value.GetWorldPosition;
            }
        }
        protected Vector3 WorldPosition { get; private set; }
        

        
        
        #region Properties

        protected static HashSet<Tile> GetLevel => BlobDivisionMaze.Instance.Level;
        protected virtual bool IsPlayer { get; set; } = false;
        public static List<Unit> AllUnits = new();
        public static List<Unit> Enemies => AllUnits.FindAll(IsEnemy);
        public List<Unit> EnemiesInRange => Enemies.FindAll(e => Vector3.Distance(transform.position, e.transform.position) <= CurrentMovement);
        public MeshFilter GetMeshFilter => MeshFilter;
        public MeshRenderer GetMeshRenderer => MeshRenderer;
        public MeshCollider GetMeshCollider => MeshCollider;
        
        protected int RollInitiative() => Initiative = RollADie(UnitScriptableObject.InitiativeDies);
        public Tile SetOccupiedTile(Tile tile) => OccupiedTile = tile;

        #endregion

        private void OnEnable() => AllUnits.Add(this);
        private void OnDisable() => AllUnits.Remove(this);
        private void Start()
        {
            // roll for initiative
            RollInitiative();
            // get tile
            Tile tile = GetLevel.ElementAt(Random.Range(0, GetLevel.Count - 1));
            while (tile.GetIsOccupied) tile = GetLevel.ElementAt(Random.Range(0, GetLevel.Count - 1));
            OccupiedTile = tile;
        }

        public override void OnBegin(bool bFirstTime)
        {
            base.OnBegin(bFirstTime);
            CurrentMovement = UnitScriptableObject.MaxMovement;
            CurrentHealth = UnitScriptableObject.MaxHealth;
            CurrentActionsPoints = UnitScriptableObject.MaxActionsPoints;
        
            if (bFirstTime)
            {
                // increase turn time
                // TurnTime += m_fTurnDuration;
        
                if (IsPlayer)
                {
                    EventHandler.Main.PushEvent(new UnitInputEvent(this));
                }
                else
                {
                    //DoAIMove();
                }
            }
        }
        
        public void TakeDamage(int iDmg)
        {
            // m_iUnitCount -= iDmg;
            // if (m_iUnitCount <= 0)
            // {
            //     //EventHandler.Main.PushEvent(new UnitActions.Death(this));
            // }
        }

        public override bool IsDone() => CurrentActionsPoints == 0;

        public static bool IsEnemy(Unit unit) => unit.IsPlayer == false;
    }
}

namespace Game
{
    // public class Dune_Unit : EventHandler.GameEventBehaviour
    // {
    //     public float ShootRange = 8;
    //     public int VisionRange = 10;
    //
    //     public int Damage = 2;
    //     
    //     private float time = 0.5f;
    //
    //
    //     public UnityEvent DamageTaken;
    //     public UnityEvent actionpointDepleted;
    //
    //
    //
    //     public void TakeDamage(int damage)
    //     {
    //         UnitHealth -= damage;
    //         DamageTaken.Invoke();
    //         if (UnitHealth <= 0)
    //         {
    //             Destroy(gameObject);
    //         }
    //     }
    //     public override void OnBegin(bool bFirstTime)
    //     {
    //         base.OnBegin(bFirstTime);
    //         
    //         time = 0.5f;
    //
    //         if (bFirstTime)
    //         {
    //             OverwatchNodes = null;
    //         }
    //
    //         if (ActionsPoints > 0)
    //         {
    //             if (Team.IsPlayerTeam)
    //             {
    //                 EventHandler.Main.PushEvent(new PlayerInput(this));
    //             }
    //             else
    //             {
    //                 DoAIMove();
    //             }
    //         }
    //        
    //     }
    //     private void DoAIMove()
    //     {
    //         if (EnemiesInRange.Count > 0)
    //         {
    //             target = GetClosestEnemy(EnemiesInRange);
    //             EventHandler.Main.PushEvent(new ShootEvent(this,target));
    //         }
    //         else if (Enemies.Count > 0)
    //         {
    //             float BestDistance = float.MaxValue;
    //             target = Enemies[Random.Range(0,Enemies.Count)];
    //             
    //             if (target != null) 
    //             {
    //               EventHandler.Main.PushEvent(new MoveEvent(this, target.node));
    //             }
    //
    //         }
    //     }
    //     private Dune_Unit GetClosestEnemy(List<Dune_Unit> enemys)
    //     {
    //         Dune_Unit closestEnemy = enemys[0];
    //         float closestdistance = float.MaxValue;
    //
    //         for (int i = 0; i < enemys.Count; i++)
    //         {
    //             float tempdistance = Vector3.Distance(transform.position, enemys[i].transform.position);
    //             if (tempdistance < closestdistance)
    //             {
    //                 closestdistance = tempdistance;
    //                 closestEnemy = enemys[i];
    //             }
    //         }
    //
    //         return closestEnemy;
    //     }
    // }
}