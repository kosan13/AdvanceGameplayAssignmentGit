using System.Collections.Generic;
using Librarys.Graphs.Scripts;
using TileSystem;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Librarys.Graphs.Interfaces
{
    public interface INode { IEnumerable<ILink> GetLinks { get; } }
    public interface IPositionNode : INode { Vector3 GetWorldPosition { get; } }
    public interface IGraph { IEnumerable<INode> GetNodes { get; } }
    public interface ISearchableGraph : IGraph { float Heuristic(INode start, INode goal); }
    public interface ILink { INode Source { get; } INode Target { get; }  }
    // public interface INativePositionNode : INativeNode { float3 GetWorldPosition { get; } }
    // public interface INativeNode { NativeList<NativeLink> GetLinks { get; } }
    // public interface INativeLink { public NativeTile Source { get; } public NativeTile Target { get; }}
}