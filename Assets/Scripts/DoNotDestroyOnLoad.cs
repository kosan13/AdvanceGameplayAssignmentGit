using UnityEngine;
public class DoNotDestroyOnLoad : MonoBehaviour
{
    private void OnEnable() => DontDestroyOnLoad(this);
    private void Start() => DontDestroyOnLoad(this);
}