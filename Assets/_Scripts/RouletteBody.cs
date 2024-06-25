using System;
using UnityEngine;

public class RouletteBody : MonoBehaviour
{
    public static Action RouletteStopped;

    [SerializeField] private float rotatePower;
    [SerializeField] private float stopPower;

    private Rigidbody2D rb;
    private bool isRotating;
    private float t;

    public void Rotate()
    {
        if (GameManager.Instance.Spins > 0 && rb.angularVelocity == 0 && !isRotating)
        {
            rb.AddTorque(rotatePower);
            isRotating = true;
            GameManager.Instance.Spins = -1;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            if (rb.angularVelocity > 0)
            {
                rb.angularVelocity -= stopPower * Time.fixedDeltaTime;
                rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, 0, 1440);
            }

            if (rb.angularVelocity == 0 && isRotating)
            {
                t += Time.fixedDeltaTime;
                if (t >= 0.5f)
                {
                    RouletteStopped?.Invoke();
                    isRotating = false;
                    t = 0;
                }
            }
        }
    }
}
