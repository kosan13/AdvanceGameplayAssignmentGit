using Librarys.DiesSystem.Scripts.Enum;
using UnityEngine;
using static Librarys.DiesSystem.Scripts.Enum.DiesTypes;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "UnitScriptableObject", menuName = "Scriptable Objects/UnitScriptableObject")]
    public class UnitScriptableObject : UnityEngine.ScriptableObject
    {
        public static int MaxHealth = 10;
        public static int MaxMovement = 5;
        public static int MaxActionsPoints = 2;
        public const DiesTypes InitiativeDies = D20;
    }
}
