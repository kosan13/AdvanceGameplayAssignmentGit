using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class EventHandler : MonoBehaviour
    {
        private HashSet<IEvent> _startedEvents = new ();
        private static EventHandler _main;

        #region Properties

        public IEvent CurrentEvent { get; private set; }
        public List<IEvent> EventStack { get; } = new ();

        public static EventHandler Main
        {
            get
            {
                if (_main != null || !Application.isPlaying) return _main;
                GameObject go = new ("MainEventHandler");
                DontDestroyOnLoad(go);
                _main = go.AddComponent<EventHandler>();
                return _main;
            }
        }

        #endregion
        
        private void Update() { UpdateEvents(); }
        
        public void PushEvent(IEvent evt)
        {
            if (evt == null) return;
            // already on stack?
            EventStack.RemoveAll(e => e == evt);
            // insert event
            EventStack.Insert(0, evt);
            // reset current event?
            if (CurrentEvent != null && CurrentEvent != evt)
                CurrentEvent = null;
        }
        public void RemoveEvent(IEvent evt)
        {
            if (evt == null || !EventStack.Contains(evt)) return;
            // call on end?
            if (evt == CurrentEvent || _startedEvents.Contains(evt))
            {
                evt.OnEnd();
                CurrentEvent = null;
            }
            // remove the event
            EventStack.Remove(evt);
        }
        
        private void UpdateEvents()
        {
            if (EventStack.Count == 0) { return; }
            
            // pick a new current event?
            if (CurrentEvent == null)
            {
                // set current event
                _startedEvents.RemoveWhere(evt => evt == null);
                CurrentEvent = EventStack[0];
                bool bFirstTime = !_startedEvents.Contains(CurrentEvent);
                _startedEvents.Add(CurrentEvent);
                CurrentEvent.OnBegin(bFirstTime);

                // did something affect the stack in the OnBegin()?
                if (EventStack != null)
                {
                    if (EventStack.Count > 0 && CurrentEvent != EventStack[0])
                    {
                        CurrentEvent = null;
                        UpdateEvents();
                    }
                }
            }
            
            // update current event
            if (CurrentEvent == null) return;
            CurrentEvent.OnUpdate();
            
            // still the same event?
            if (EventStack.Count <= 0 || CurrentEvent != EventStack[0]) return;
            
            // did we finish the event?
            if (!CurrentEvent.IsDone()) return;
            EventStack.RemoveAt(0);
            CurrentEvent.OnEnd();
            _startedEvents.Remove(CurrentEvent);
            CurrentEvent = null;
        }

        private void OnGUI()
        {
            if (this != _main) return;

            #if UNITY_EDITOR
            const float lineHeight = 32.0f;
            GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.7f);
            Rect r = new Rect(0, 0, 250.0f, lineHeight * EventStack.Count);
            GUI.DrawTexture(r, Texture2D.whiteTexture);

            Rect line = new Rect(10, 0, r.width - 20, lineHeight);
            for (int i = 0; i < EventStack.Count; i++)
            {
                GUI.color = EventStack[i] == CurrentEvent ? Color.green : Color.white;
                GUI.Label(line, "#" + i + ": " + EventStack[i].ToString(), i == 0 ? UnityEditor.EditorStyles.boldLabel : UnityEditor.EditorStyles.label);
                line.y += line.height;
            }
            #endif
        }
    }
}