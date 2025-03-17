using System.Linq;
using Librarys.DiesSystem.Scripts.Enum;
using UnityEngine;

namespace Librarys.DiesSystem.Scripts
{
    public static class DiesFunction
    {
        /// <summary>
        ///   <para>Get a random number between 1 and the value of the die</para>
        ///   <param name="die">The Max possible value of the random number</param>
        /// </summary>
        public static int RollDie(DiesTypes die) => Random.Range(0, (int)die) + 1;
        /// <summary>
        ///   <para>Get a random number between 1 and the value of the die forech die in the array</para>
        ///   <param name="dies">All dies to add up</param>
        /// </summary>
        public static int RollDies(DiesTypes[] dies) => dies.Sum(RollDie);
    }
}