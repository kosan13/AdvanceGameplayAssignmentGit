using System.Collections.Generic;
using Game.UnitClasses;

namespace SortingAlgorithms
{
    public static class SortingAlgorithms
    {
        public static void QuickSortUnit(IList<Unit> array, int startIndex, int endIndex)
        {
            while (true)
            {
                if (endIndex <= startIndex) return;
                int pivot = Partition(array, startIndex, endIndex);
                QuickSortUnit(array, startIndex, pivot - 1);
                startIndex = pivot + 1;
            }
        }
        private static int Partition(IList<Unit> array, int startIndex, int endIndex)
        {
            Unit pivot = array[endIndex];
            int i = startIndex - 1;
            (Unit valueTwo, Unit valueOnes) tempValue;

            for (int index = startIndex; index <= endIndex; index++)
            {
                if (array[index].Initiative >= pivot.Initiative) continue;
                i++;
                tempValue = VariableSwapping(array[i], array[index]);
                array[i] = tempValue.valueTwo;
                array[index] = tempValue.valueOnes;
            }
            i++;
            tempValue = VariableSwapping(array[i], array[endIndex]);
            array[i] = tempValue.valueTwo;
            array[endIndex] = tempValue.valueOnes;
            return i;
        }
        private static (TValue valueTwo, TValue valueOnes) VariableSwapping<TValue>(TValue valueOnes,TValue valueTwo) => (valueTwo, valueOnes);
    }
}