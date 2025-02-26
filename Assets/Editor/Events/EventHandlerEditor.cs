using Event;
using UnityEditor;
using UnityEngine;

namespace Editor.Events
{
    [CustomEditor(typeof(EventHandler), true)]
    public class EventHandlerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EventHandler eventHandler = target as EventHandler;
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Event Stack", EditorStyles.boldLabel);
            GUILayout.BeginVertical(EditorStyles.helpBox);
            if (eventHandler is null) return;
            foreach (IEvent evt in eventHandler.EventStack)
            {
                string name = "   #" + eventHandler.EventStack.IndexOf(evt) + ": " + evt;
                if (evt is Object obj)
                {
                    if (GUILayout.Button(name, evt == eventHandler.CurrentEvent ? EditorStyles.boldLabel : EditorStyles.label)) Selection.activeObject = obj;
                }
                else EditorGUILayout.LabelField(name, evt == eventHandler.CurrentEvent ? EditorStyles.boldLabel : EditorStyles.label);
            }
            GUILayout.EndVertical();
        }
    }
}