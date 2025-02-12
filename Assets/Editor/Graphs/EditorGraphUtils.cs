using UnityEngine;
using UnityEditor;

namespace Graphs
{
    public static class EditorGraphUtils 
    {
        public static void DrawGraph(IGraph graph)
        {
            foreach (INode node in graph.GetNodes)
            {
                if (node is not IPositionNode source) continue;
                // draw node position
                Handles.color = Color.yellow;
                Handles.CubeHandleCap(0, source.GetWorldPosition, Quaternion.identity, 0.1f, EventType.Repaint);
                
                // draw node links
                foreach (ILink link in source.GetLinks)
                {
                    if (link.Target is not IPositionNode target) continue;
                    Handles.color = Color.blue;
                    Handles.DrawLine(source.GetWorldPosition, target.GetWorldPosition);
                }
            }
        }
    }
}