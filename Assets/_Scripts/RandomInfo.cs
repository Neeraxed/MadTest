using System.Collections.Generic;
using UnityEngine;

public class RandomInfo
{
    public readonly int Index;
    public readonly int Value;
    public readonly List<int> CellsWithChances;

    public RandomInfo(List<int> cellsWithChances, int rotations)
    {
        CellsWithChances = cellsWithChances;
        Index = Random.Range(0, CellsWithChances.Count);
        Value = CellsWithChances[Index];
    }
}