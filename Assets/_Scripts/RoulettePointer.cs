using System;
using UnityEngine;

public class RoulettePointer : MonoBehaviour
{
    public static Action<RouletteCellTypes,int> GotInfoFromCell;

    [SerializeField] private RectTransform rouletteCenter;
    [SerializeField] private RectTransform startingPoint;

    private void OnEnable()
    {
        RouletteBody.RouletteStopped += CheckResult;
    }

    private void OnDisable()
    {
        RouletteBody.RouletteStopped -= CheckResult;
    }

    private void CheckResult()
    {
        RaycastHit2D hit = Physics2D.Raycast(startingPoint.position, startingPoint.TransformDirection(rouletteCenter.position - startingPoint.position));
        if (hit)
        {
            if (hit.collider.TryGetComponent(out RouletteCell cell))
            {
                GotInfoFromCell?.Invoke(cell.Type, cell.Amount);
            }
        }
    }
}
