using System.Collections.Generic;

namespace Graphs
{
    public static class GraphAlgorithms
    {
        public static HashSet<T> FloodFill<T>(T start) where T : class, INode
        {
            // setup
            Queue<T> open = new();
            HashSet<T> closed = new();
            open.Enqueue(start);

            // search / iteration
            while (open.Count > 0) 
            {
                T node = open.Dequeue();
                closed.Add(node);

                // search the neighbors
                foreach (ILink link in node.GetLinks)
                {
                    if (link.Target is not T neighbor || open.Contains(neighbor) || closed.Contains(neighbor)) continue;
                    open.Enqueue(neighbor);
                }
            }
            return closed;
        }
        public static HashSet<T> GetNodesInRange<T>(T start, int iRange) where T : class, INode
        {
            // setup
            Queue<T> open = new();
            HashSet<T> closed = new();
            open.Enqueue(start);

            for (int i = 0; i <= iRange; ++i)
            {
                Queue<T> nextRipple = new();
                while (open.Count > 0)
                {
                    T node = open.Dequeue();
                    closed.Add(node);

                    // search the neighbors
                    foreach (ILink link in node.GetLinks)
                    {
                        if (link.Target is not T neighbor || open.Contains(neighbor) || closed.Contains(neighbor)) continue;
                        nextRipple.Enqueue(neighbor);
                    }
                }
                open = nextRipple;
            }

            // goodies in here
            return closed;
        }
    }
}