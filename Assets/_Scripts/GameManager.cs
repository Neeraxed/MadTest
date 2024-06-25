using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TMPro.TextMeshProUGUI moneyText;
    [SerializeField] private TMPro.TextMeshProUGUI spinsText;
    [SerializeField] private int maximumSpins;

    private int moneyAmount;
    private int spinsAmount;
    private Coroutine cor;

    public int Mana
    {
        get => spinsAmount;
        set
        {
            UpdateRecources(RouletteCellTypes.Spin, value);

            if (spinsAmount < maximumSpins)
            {
                if(cor == null)
                    StartCoroutine(RestoreMana());
            }
        }
    }

    private void Awake()
    {
        Instance = this;
        UpdateRecources(RouletteCellTypes.Money, PlayerPrefs.GetInt("Money", 0));

        if (PlayerPrefs.GetFloat("LastOnline", 0) - Time.time > 9999)
            UpdateRecources(RouletteCellTypes.Spin, maximumSpins);
        else
            UpdateRecources(RouletteCellTypes.Spin, PlayerPrefs.GetInt("Spins", 0));
    }

    private void OnEnable()
    {
        RoulettePointer.GotInfoFromCell += UpdateRecources;
    }

    private void OnDisable()
    {
        RoulettePointer.GotInfoFromCell -= UpdateRecources;
    }

    private IEnumerator RestoreMana()
    {
        yield return new WaitForSecondsRealtime(5);
        Mana = 1;
        cor = null;
    }

    private void UpdateRecources(RouletteCellTypes type, int value)
    {
        if (type == RouletteCellTypes.Money)
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
}