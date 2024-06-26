using System.Collections.Generic;
using UnityEngine;

public class RandomInfo
{
    public readonly int Index;
    public readonly int Value;
    public readonly List<int> CellsWithChances;
    public readonly int AmountOfFullRotations;

    public RandomInfo(List<int> cellsWithChances, int minRotations, int maxRotations)
    {
        CellsWithChances = cellsWithChances;
        Index = Random.Range(0, CellsWithChances.Count);
        Value = CellsWithChances[Index];
        AmountOfFullRotations = Random.Range(minRotations, maxRotations);
    }
}