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
    public int Spins
    {
        get => spinsAmount;
        set
        {
            UpdateRecources(RouletteCellTypes.Spin, value);
            CheckIsRestorationNeeded();
        }
    }

    private void Awake()
    {
        Instance = this;
        UpdateRecources(RouletteCellTypes.Money, PlayerPrefs.GetInt("Money", 0));
        if (PlayerPrefs.GetFloat("LastOnline") == 0 || PlayerPrefs.GetFloat("LastOnline", Int64.MaxValue) - (DateTime.Now.Hour + (DateTime.Now.Minute * 0.01f)) > 9999)
            UpdateRecources(RouletteCellTypes.Spin, maximumSpins);
        else
            UpdateRecources(RouletteCellTypes.Spin, PlayerPrefs.GetInt("Spins", 0));

        CheckIsRestorationNeeded();
    }

    private void OnEnable()
    {
        RouletteBody.RouletteStopped += UpdateRecources;
    }

    private void OnDisable()
    {
        RouletteBody.RouletteStopped -= UpdateRecources;
        PlayerPrefs.SetFloat("LastOnline", DateTime.Now.Hour + (DateTime.Now.Minute * 0.01f));
    }
    private void CheckIsRestorationNeeded()
    {
        if (spinsAmount < maximumSpins)
        {
            if (coroutine == null)
                coroutine = StartCoroutine(RestoreSpins());
        }
    }
    private IEnumerator RestoreSpins()
    {
        yield return new WaitForSecondsRealtime(timeToRestoreMana);
        Spins = 1;
        coroutine = null;
    }

    private void UpdateRecources(RouletteCellTypes type, int value)
    {
        if (type == RouletteCellTypes.Money && moneyAmount + value > 0)
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
            if (spinsText != null)
            {
                spinsText.text = spinsAmount.ToString();
                PlayerPrefs.SetInt("Spins", spinsAmount);
            }
        }
    }

    private void UpdateRecources(RouletteCell cell)
    {
        if (cell.Type == RouletteCellTypes.Money && moneyAmount + cell.Amount > 0)
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
            if (spinsText != null)
            {
                spinsText.text = spinsAmount.ToString();
                PlayerPrefs.SetInt("Spins", spinsAmount);
            }
        }
    }
}
