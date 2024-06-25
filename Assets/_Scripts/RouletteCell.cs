using UnityEngine;

public class RouletteCell : MonoBehaviour
{
    [SerializeField] private RouletteCellTypes type;
    [SerializeField] private int amount;

    public RouletteCellTypes Type { get => type; }
    public int Amount { get => amount; }
}

public enum RouletteCellTypes
{
    Money,
    Shield,
    Attack, 
    Spin
}