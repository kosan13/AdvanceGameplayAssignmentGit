namespace BootStrapScripts
{
    public class DoNotDestroyOnLoad : BootStrapMonoBehaviour
    {
        public override void Init () => DontDestroyOnLoad(this);
    }
}