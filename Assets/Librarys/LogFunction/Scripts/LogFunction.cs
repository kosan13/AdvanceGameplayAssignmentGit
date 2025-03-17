using UnityEngine;

namespace Librarys.LogFunction.Scripts
{
    public sealed class LogFunction
    {
        public static void DebugLog<T>(T value, string prefix = "", string suffix = "") => Debug.Log($"{prefix} {value} {suffix}");
    }
}