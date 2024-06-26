using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TMPro.TextMeshProUGUI moneyText;
    [SerializeField] private TMPro.TextMeshProUGUI spinsText;
    [SerializeField] private int maximumSpins;
    [SerializeField] private float timeToRestoreMana;

    private int moneyAmount;
    private int spinsAmount;
    private Coroutine coroutine;

    private void OnEnable()
    {
        RouletteBody.RouletteStopped += UpdateRecources;
    }
    private void OnDisable()
    {
        RouletteBody.RouletteStopped -= UpdateRecources;

    }

    public int Spins
    {
        get => spinsAmount;
        set
        {
            UpdateRecources(RouletteCellTypes.Spin, value);

            if (spinsAmount < maximumSpins)
            {
                if (coroutine == null)
                    StartCoroutine(RestoreMana());
            }
        }
    }

    private void Awake()
    {
        Instance = this;
        UpdateRecources(RouletteCellTypes.Money, PlayerPrefs.GetInt("Money", 0));

        if (PlayerPrefs.GetFloat("LastOnline") == 0 || PlayerPrefs.GetFloat("LastOnline", Int64.MaxValue) - Time.time > 9999)
            UpdateRecources(RouletteCellTypes.Spin, maximumSpins);
        else
            UpdateRecources(RouletteCellTypes.Spin, PlayerPrefs.GetInt("Spins", 0));
    }

    private IEnumerator RestoreMana()
    {
        yield return new WaitForSecondsRealtime(timeToRestoreMana);
        Spins = 1;
        coroutine = null;
    }

    private void UpdateRecources(RouletteCellTypes type, int value)
    {
        if (type == RouletteCellTypes.Money || moneyAmount + value > 0)
        {
            moneyAmount += value;
            if (moneyText != null)
            {
                moneyText.text = moneyAmount.ToString();
                PlayerPrefs.SetInt("Money", moneyAmount);
            }
        }
        else if (type == RouletteCellTypes.Spin)
        {
            spinsAmount += value;
            PlayerPrefs.SetFloat("LastOnline", Time.time);
            if (spinsText != null)
            {
                spinsText.text = spinsAmount.ToString();
                PlayerPrefs.SetInt("Spins", spinsAmount);
            }
        }
    }

    private void UpdateRecources(RouletteCell cell)
    {
        if (cell.Type == RouletteCellTypes.Money)
        {
            moneyAmount += cell.Amount;
            if (moneyText != null)
            {
                moneyText.text = moneyAmount.ToString();
                PlayerPrefs.SetInt("Money", moneyAmount);
            }
        }
        else if (cell.Type == RouletteCellTypes.Spin)
        {
            spinsAmount += cell.Amount;
            PlayerPrefs.SetFloat("LastOnline", Time.time);
            if (spinsText != null)
            {
                spinsText.text = spinsAmount.ToString();
                PlayerPrefs.SetInt("Spins", spinsAmount);
            }
        }
    }
}
