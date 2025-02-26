using Dies;
using UnityEngine;
using static Dies.DiesEnum;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "UnitScriptableObject", menuName = "Scriptable Objects/UnitScriptableObject")]
    public class UnitScriptableObject : UnityEngine.ScriptableObject
    {
        public static int MaxHealth = 10;
        public static  int MaxMovement = 5;
        public static  int MaxActionsPoints = 2;
        public static  DiesEnum InitiativeDies = D20;
    }
}
