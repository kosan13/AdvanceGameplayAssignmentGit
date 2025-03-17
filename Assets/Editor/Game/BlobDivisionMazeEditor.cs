using Game;
using UnityEditor;
using static Librarys.Graphs.Editor.EditorGraphUtils;

namespace Editor.Game
{
    [CustomEditor(typeof(BlobDivisionMaze))]
    public class BlobDivisionMazeEditor : UnityEditor.Editor { private void OnSceneGUI() { DrawGraph(target as BlobDivisionMaze); } }
}