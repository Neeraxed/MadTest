using System;
using UnityEngine;

public class RouletteCell : MonoBehaviour
{
    [SerializeField] private RouletteCellTypes type;
    [SerializeField] private int amount;
    [SerializeField] private int chance;

    public RouletteCellTypes Type { get => type; private set { type = value; } }
    public int Amount { get => amount; private set { amount = value; } }
    public int Chance { get => chance; private set { chance = value; } }

    public RouletteCell(int value, int chance)
    {
        Amount = value;
        Chance = chance;
    }
}

public enum RouletteCellTypes
{
    Money,
    Shield,
    Attack,
    Spin
}