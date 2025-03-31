using Librarys.Graphs.Enum;
using Librarys.Graphs.Interfaces;
using static Librarys.Graphs.Enum.Direction;

namespace Librarys.Graphs.Scripts
{
    public struct Link : ILink
    {
        #region Properties
        public INode Source { get; }
        public INode Target { get; }
        public Direction Direction { get; private set; }
        #endregion
        
        public Link(INode source, INode target, Direction direction = Null)
        {
            Source = source;
            Target = target;
            Direction = direction;
        }
        
        public static Direction GetLinkDirection(Link link , IPositionNode source, IPositionNode target) => link.Direction = GetLinkDirection(source, target);
        public static Direction GetLinkDirection(IPositionNode source, IPositionNode target)
        {
            Direction direction = Null;
            if (source.GetWorldPosition.x < target.GetWorldPosition.x) { direction = Forward; }
            if (source.GetWorldPosition.x > target.GetWorldPosition.x) { direction = Back; }
            if (source.GetWorldPosition.z < target.GetWorldPosition.z) { direction = Right; }
            if (source.GetWorldPosition.z > target.GetWorldPosition.z) { direction = Left; }
            return direction;
        }
    }
    
    // public struct NativeLink : INativeLink
    // {
    //     #region Properties
    //     public NativeTile Source { get; }
    //     public NativeTile Target { get; }
    //     public Direction Direction { get; private set; }
    //     #endregion
    //
    //     public NativeLink(NativeTile source, NativeTile target, Direction direction = Null)
    //     {
    //         Source = source;
    //         Target = target;
    //         Direction = direction;
    //     }
    //     
    //     public static Direction GetLinkDirection(NativeLink nativeLink , IPositionNode source, IPositionNode target) => nativeLink.Direction = GetLinkDirection(source, target);
    //     public static Direction GetLinkDirection(IPositionNode source, IPositionNode target)
    //     {
    //         Direction direction = Null;
    //         if (source.GetWorldPosition.x < target.GetWorldPosition.x) { direction = Forward; }
    //         if (source.GetWorldPosition.x > target.GetWorldPosition.x) { direction = Back; }
    //         if (source.GetWorldPosition.z < target.GetWorldPosition.z) { direction = Right; }
    //         if (source.GetWorldPosition.z > target.GetWorldPosition.z) { direction = Left; }
    //         return direction;
    //     }
    //     
    //     #region operator
    //     
    //     public static bool operator == (NativeLink linkOne, NativeLink linkTwo)
    //     {
    //         bool2 check = linkOne.Source.GetIndexPositions == linkTwo.Source.GetIndexPositions;
    //         return check is { x: true, y: true };
    //     }
    //     public static bool operator != (NativeLink linkOne, NativeLink linkTwo) => !(linkOne == linkTwo);
    //     
    //     #endregion
    // }
}