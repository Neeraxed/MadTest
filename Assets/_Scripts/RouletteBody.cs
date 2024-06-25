using System;
using UnityEngine;

public class RouletteBody : MonoBehaviour
{
    public static Action RouletteStopped;

    [SerializeField] private float rotatePower;
    [SerializeField] private float stopPower;

    private Rigidbody2D rb;
    private int inRotate;
    private float t;

    public void Rotate()
    {
        if (GameManager.Instance.Mana > 0 && rb.angularVelocity == 0 && inRotate == 0)
        {
            rb.AddTorque(rotatePower);
            inRotate = 1;
            GameManager.Instance.Mana = -1;
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

            if (rb.angularVelocity == 0 && inRotate == 1)
            {
                t += Time.fixedDeltaTime;
                if (t >= 0.5f)
                {
                    RouletteStopped?.Invoke();
                    inRotate = 0;
                    t = 0;
                }
            }
        }
    }
}
