using System;
using Enums;
using Game;
using Game.UnitClasses;
using Graphs;
using TileSystem.Tile_Class;
using UnityEngine;

namespace Events.Action
{
    public class UnitMoveEvent : UnitAction
    {
        private readonly Direction _direction;

        public UnitMoveEvent(Unit unit, Direction direction) : base(unit) => _direction = direction;

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            switch (_direction)
            {
                case Direction.Null: break;
                case Direction.DirectionUp: break;
                case Direction.DirectionDown: break;
                case Direction.DirectionForward: Move(Unit, Unit.WorldPosition + Vector3.forward); break;
                case Direction.DirectionBack: Move(Unit, Unit.WorldPosition + Vector3.back); break;
                case Direction.DirectionLeft: Move(Unit, Unit.WorldPosition + Vector3.left); break;
                case Direction.DirectionRight: Move(Unit, Unit.WorldPosition + Vector3.right); break;
                default: throw new ArgumentOutOfRangeException();
            }

            // // if (_path == null || _path.Count <= 0) return;
            // if (_path is not { Count: > 0 }) return;
            // Tile next = _path[0];
            // Unit.transform.position = Vector3.MoveTowards(Unit.transform.position, next.GetWorldPosition, Time.deltaTime * SPEED);
            //
            // Vector3 vToNext = next.GetWorldPosition - Unit.transform.position;
            // if (vToNext.magnitude < 0.01f)
            // {
            //     Unit.SetOccupiedTile(next);
            // }
            // else Unit.transform.rotation = Quaternion.Slerp(Unit.transform.rotation, Quaternion.LookRotation(vToNext), Time.deltaTime * 4.0f);
        }

        public override bool IsDone() => true;

        private void Move(Unit unit, Vector3 newTile)
        {
            foreach (ILink link in unit.OccupiedTile.GetLinks)
            {
                if (((Tile)link.Target).GetWorldPosition == newTile)
                    unit.OccupiedTile = BlobDivisionMaze.Instance.Tilemap.GetTile(((Tile)link.Target).GetIndexPositions);
            }
        }
    }
}
// using System.Collections.Generic;
// using Game.UnitClasses;
// using TileSystem.Tile_Class;
// using UnityEngine;
//
// namespace Events.Action
// {
//     public class UnitMoveEvent : UnitAction
//     {
//         private readonly List<Tile> _path;
//
//         public UnitMoveEvent(Unit unit, List<Tile> path) : base(unit) => _path = path;
//
//         public override void OnUpdate()
//         {
//             const float SPEED = 2.0f;
//
//             base.OnUpdate();
//
//             // if (_path == null || _path.Count <= 0) return;
//             if (_path is not { Count: > 0 }) return;
//             Tile next = _path[0];
//             Unit.transform.position = Vector3.MoveTowards(Unit.transform.position, next.GetWorldPosition, Time.deltaTime * SPEED);
//
//             Vector3 vToNext = next.GetWorldPosition - Unit.transform.position;
//             if (vToNext.magnitude < 0.01f)
//             {
//                 Unit.SetOccupiedTile(next);
//                 _path.RemoveAt(0);
//             }
//             else Unit.transform.rotation = Quaternion.Slerp(Unit.transform.rotation, Quaternion.LookRotation(vToNext), Time.deltaTime * 4.0f);
//         }
//
//         public override bool IsDone() => _path == null || _path.Count == 0;
//     }
// }