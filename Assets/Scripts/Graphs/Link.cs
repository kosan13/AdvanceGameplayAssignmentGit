using Enums;

namespace Graphs
{
    public class Link : ILink
    {
        #region Properties
        public INode Source { get; }
        public INode Target { get; }
        public Direction Direction { get; private set; }

        #endregion

        public Link(INode source, INode target, Direction direction)
        {
            Source = source;
            Target = target;
            Direction = direction;
        }
        public Link(INode source, INode target)
        {
            Source = source;
            Target = target;
        }

        public static Direction GetLinkDirection(Link link , IPositionNode source, IPositionNode target) => link.Direction = GetLinkDirection(source, target);
        public static Direction GetLinkDirection(IPositionNode source, IPositionNode target)
        {
            if (source.GetWorldPosition.x < target.GetWorldPosition.x) { return Direction.DirectionForward; }
            if (source.GetWorldPosition.x > target.GetWorldPosition.x) { return Direction.DirectionBack; }
            if (source.GetWorldPosition.z < target.GetWorldPosition.z) { return Direction.DirectionRight; }
            if (source.GetWorldPosition.z > target.GetWorldPosition.z) { return Direction.DirectionLeft; }
            return Direction.Null;
        }
    }
}