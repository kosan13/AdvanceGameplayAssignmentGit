using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(BlobDivisionMaze))]
    public class BlobDivisionMazeEditor : Editor { private void OnSceneGUI() { Graphs.EditorGraphUtils.DrawGraph(target as BlobDivisionMaze); } }
}