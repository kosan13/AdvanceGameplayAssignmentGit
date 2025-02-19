using Game;
using UnityEditor;
using static Editor.Graphs.EditorGraphUtils;

namespace Editor.Game
{
    [CustomEditor(typeof(BlobDivisionMaze))]
    public class BlobDivisionMazeEditor : UnityEditor.Editor { private void OnSceneGUI() { DrawGraph(target as BlobDivisionMaze); } }
}