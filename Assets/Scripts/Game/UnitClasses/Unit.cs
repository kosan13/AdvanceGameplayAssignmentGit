using System;
using System.Collections.Generic;
using System.Linq;
using Event.BaseEventClass;
using Event.Events;
using ScriptableObject;
using TileSystem.Tile_Class;
using UnityEngine;
using static Dies.DiesFunction;
using EventHandler = Event.EventHandler;
using Random = UnityEngine.Random;

namespace Game.UnitClasses
{
    public class Unit : GameEventMeshHandlerBehaviour
    {
        private Tile _occupiedTile;
        private Vector3 _worldPosition;
        private const float MoveSpeed = 10;

        #region Properties

        public int CurrentHealth { get; protected set; }
        public int CurrentMovement { get; protected set; }
        public int CurrentActionsPoints { get; protected set; }
        
        public Vector3 WorldPosition
        {
            get => _worldPosition;
            private set
            {
                _worldPosition = value;
            } 
        }
        public int Initiative { get; private set; }
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

        public int RemoveMovement(int value) => CurrentMovement - value;
        public int RemoveOneMovement() => RemoveMovement(1);

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
        private void Update() => transform.position = Vector3.Lerp(transform.position, _worldPosition, Time.deltaTime * MoveSpeed);

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

        public static void Sort(List<Unit> units)
        {
            QuickSort(units, 0, units.Count);
        }
        
        public static void QuickSort(IList<Unit> array, int start, int end)
        {
            while (true)
            {
                if (end <= start) return;
                int pivot = Partition(array, start, end);
                QuickSort(array, start, pivot - 1);
                start = pivot + 1;
            }
        }
        private static int Partition(IList<Unit> array, int start, int end)
        {
            int pivot = array[end].Initiative;
            int i = start - 1;

            (Unit valueTwo, Unit valueOnes) temp;

            for (int j = start; j <= end; j++)
            {
                if (array[j].Initiative >= pivot) continue;
                i++;
                temp = VariableSwapping(array[i], array[j]);
                array[i] = temp.valueTwo;
                array[j] = temp.valueOnes;
            }

            i++;
            temp = VariableSwapping(array[i], array[end]);
            array[i] = temp.valueTwo;
            array[end] = temp.valueOnes;
            return i;
        }
        public static (TValue valueTwo, TValue valueOnes) VariableSwapping<TValue>(TValue valueOnes,TValue valueTwo)
        {
            return (valueTwo, valueOnes);
        }
    }
}