using Librarys.EventSystem.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Librarys.EventSystem.Editor
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
                string eventName = "   #" + eventHandler.EventStack.IndexOf(evt) + ": " + evt;
                if (evt is Object obj)
                {
                    if (GUILayout.Button(eventName, evt == eventHandler.CurrentEvent ? EditorStyles.boldLabel : EditorStyles.label)) Selection.activeObject = obj;
                }
                else EditorGUILayout.LabelField(eventName, evt == eventHandler.CurrentEvent ? EditorStyles.boldLabel : EditorStyles.label);
            }
            GUILayout.EndVertical();
        }
    }
}