using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteBody : MonoBehaviour
{
    public static Action<RouletteCell> RouletteStopped;

    public List<RouletteCell> CellsWithChances = new();

    [SerializeField] private int rotations = 3;
    [SerializeField] private float spinDuration = 1;

    private bool isRotating;
    private readonly List<int> weightedList = new List<int>(100);
    private float anglesPerCell;
    private RouletteCell chosenCell;

    public void Rotate()
    {
        if (GameManager.Instance.Spins > 0 && !isRotating)
        {
            StartCoroutine(SpinTheWheel());
            isRotating = true;
            GameManager.Instance.Spins = -1;
        }
    }
    private void Start()
    {
        isRotating = false;
        anglesPerCell = 360f / CellsWithChances.Count;
        UpdateList();
    }

    private void UpdateList()
    {
        weightedList.Clear();

        foreach (var item in CellsWithChances)
        {
            for (var i = 0; i < item.Chance; i++)
            {
                weightedList.Add(item.Amount);
            }
        }
    }

    private IEnumerator SpinTheWheel(Action<int> onResult = null)
    {
        var randomInfo = new RandomInfo(weightedList, rotations);

        foreach (var item in CellsWithChances)
        {
            if (randomInfo.Value == item.Amount)
                chosenCell = item;
        }

        var currentAngle = transform.eulerAngles.z;

        while (currentAngle >= 360)
        {
            currentAngle -= 360;
        }
        while (currentAngle < -360)
        {
            currentAngle += 360;
        }

        var targetAngle = (CellsWithChances.IndexOf(chosenCell) * anglesPerCell + 360f * rotations - anglesPerCell / 2);

        yield return SpinTheWheel(currentAngle, targetAngle, rotations * spinDuration, chosenCell, onResult);
    }

    private IEnumerator SpinTheWheel(float fromAngle, float toAngle, float duration, RouletteCell chosenCell, Action<int> onResult = null)
    {
        isRotating = true;

        var passedTime = 0f;

        while (passedTime < duration)
        {
            var coef = Mathf.SmoothStep(0, 1, (Mathf.SmoothStep(0, 1, passedTime / duration)));

            transform.localEulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(fromAngle, toAngle, coef));
            passedTime += Time.deltaTime;

            yield return null;
        }

        transform.eulerAngles = new Vector3(0.0f, 0.0f, toAngle);
        isRotating = false;

        RouletteStopped?.Invoke(chosenCell);
    }
}
