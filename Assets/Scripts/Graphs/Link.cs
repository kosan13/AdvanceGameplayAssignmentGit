namespace Graphs
{
    public class Link : ILink
    {
        #region Properties
        public INode Source { get; }
        public INode Target { get; }
        public LinkDirection Direction { get; private set; }

        #endregion

        public Link(INode source, INode target, LinkDirection direction)
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

        public static LinkDirection GetLinkDirection(Link link , IPositionNode source, IPositionNode target) => link.Direction = GetLinkDirection(source, target);
        public static LinkDirection GetLinkDirection(IPositionNode source, IPositionNode target)
        {
            if (source.GetWorldPosition.x < target.GetWorldPosition.x) { return LinkDirection.DirectionForward; }
            if (source.GetWorldPosition.x > target.GetWorldPosition.x) { return LinkDirection.DirectionBack; }
            if (source.GetWorldPosition.z < target.GetWorldPosition.z) { return LinkDirection.DirectionRight; }
            if (source.GetWorldPosition.z > target.GetWorldPosition.z) { return LinkDirection.DirectionLeft; }
            return LinkDirection.Null;
        }
    }
}