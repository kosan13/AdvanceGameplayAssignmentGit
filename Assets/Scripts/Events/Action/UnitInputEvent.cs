using Game.UnitClasses;
using UnityEngine;
using Input = UnityEngine.Input;

namespace Events.Action
{
    public class UnitInputEvent : UnitAction
    {
        private bool _done;

        public UnitInputEvent(Unit unit) : base(unit) { }

        public override void OnBegin(bool bFirstTime)
        {
            base.OnBegin(bFirstTime);
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();


            if (Input.GetKeyDown(KeyCode.A))
            {
                EventHandler.Main.PushEvent(new UnitMoveEvent(Unit, _path));
            }
            
        }
        
        public override bool IsDone() => _done;
        
        public override void OnEnd()
        {
            base.OnEnd();
            // EventHandler.Main.PushEvent(new UnitMoveEvent(Unit, _path));
        }
    }
}
// using System.Collections.Generic;
// using System.Linq;
// using Game.UnitClasses;
// using Graphs;
// using MeshHandlers;
// using TileSystem.Tile_Class;
// using UnityEngine;
// using static Graphs.GraphAlgorithms;
// using static TileSystem.TileSystemFunctions;
//
// namespace Events.Action
// {
//     public class UnitInputEvent : UnitAction
//     {
//         private HashSet<Tile> _reachableTiles;
//         private Tile _goal;
//         private List<Tile> _path;
//         private bool _done;
//
//         public UnitInputEvent(Unit unit) : base(unit) { }
//
//         public override void OnBegin(bool bFirstTime)
//         {
//             base.OnBegin(bFirstTime);
//             _goal = null;
//             // get tiles in range
//             _reachableTiles = GetNodesInRange(Unit.OccupiedTile, Unit.CurrentMovement);
//         }
//         
//         public override void OnUpdate()
//         {
//             base.OnUpdate();
//         
//             
//             // // generate a mesh
//             // List<Vector3> vertices = new();
//             // List<Vector2> uv = new();
//             // List<Color> colors = new();
//             // List<int> triangles = new();
//             // // draw reachable tiles
//             // foreach (Vector3 position in from tile in _reachableTiles where Unit is null select tile.GetWorldPosition)
//             //     CreatQuad(position,Vector3.up, 1, new Color(0.0f, 0.0f, 1f, 1f), vertices, uv, colors, triangles);
//             //
//             // if (Unit is not null)
//             // {
//             //     foreach (Vector3 position in Unit.EnemiesInRange.Select(enemy => enemy.OccupiedTile.GetWorldPosition))
//             //         CreatQuad(position, Vector3.up, 1, new Color(1.0f, 0.0f, 0.0f, 1f), vertices, uv, colors, triangles);
//             //
//             //     Unit.GetMeshFilter.mesh = MeshHandler.NewMesh(vertices, uv, colors, triangles);
//             // }
//         }
//         
//         public override bool IsDone() => _done;
//         
//         public override void OnEnd()
//         {
//             base.OnEnd();
//             EventHandler.Main.PushEvent(new UnitMoveEvent(Unit, _path));
//         }
//     }
// }
