using System.Linq;
using Random = UnityEngine.Random;

namespace Dies
{
    public abstract class DiesFunction
    {
        public static int RollADie(DiesEnum dies) => Random.Range(0, (int)dies) + 1;
        public static int RollDies(DiesEnum[] dies) => dies.Sum(RollADie);
    }
}