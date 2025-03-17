using Game;
using Game.UnitClasses;
using Librarys.Graphs.Enum;
using Librarys.Graphs.Interfaces;
using TileSystem.Tile_Class;
using UI;
using Unity.Mathematics;
using UnityEngine;
using static Librarys.Graphs.Enum.Direction;

namespace Event.Events
{
    public class UnitMoveEvent : UnitEvent
    {
        private readonly Direction _direction;
        private readonly int _moveAmount;

        public UnitMoveEvent(Unit unit, Direction direction, int moveAmount = 1) : base(unit) 
        {
            _direction = direction;
            _moveAmount = moveAmount; 
        }

        public override void OnUpdate()
        {
            if (PauseMenu.Instance.gameObject.activeSelf) return;
            switch (_direction)
            {
                case Forward: Move(Unit, Unit.WorldPosition + Unit.transform.forward, _moveAmount); break;
                case Back: Move(Unit, Unit.WorldPosition - Unit.transform.forward, _moveAmount); break;
                case Left: Move(Unit, Unit.WorldPosition - Unit.transform.right, _moveAmount); break;
                case Right: Move(Unit, Unit.WorldPosition + Unit.transform.right, _moveAmount); break;
                case Null: 
                case Up: 
                case Down: 
                default: Debug.Log("Not a valid Direction"); break;
            }
        }
        public override bool IsDone() => true;
        public static void Move(Unit unit, Vector3 newTile, int moveAmount)
        {
            if (unit.CurrentMovement <= 0) return;
            Vector3Int newTileInt = new(Mathf.RoundToInt(newTile.x), 0, Mathf.RoundToInt(newTile.z));
            foreach (ILink link in unit.OccupiedTile.GetLinks)
            {
                if (((Tile)link.Target).GetWorldPosition != newTileInt) continue;
                unit.OccupiedTile = BlobDivisionMaze.Instance.Tilemap.GetTile(((Tile)link.Target).GetIndexPositions);
                unit.RemoveOneMovement();
                break;
            }
        }
    }
}