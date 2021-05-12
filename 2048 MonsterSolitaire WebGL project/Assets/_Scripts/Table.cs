using UnityEngine;
using System.Collections;

public class Table : MonoBehaviour
{
    static public Table S;

    [Header("Set in Inspector")]
    public GameObject card;
    [SerializeField] private Vector3 backCardPosition = new Vector3(-0.333f, -3.557f, 0f);

    public bool gamePaused;     //я знаю что тут написана полная хуйня, пока я не знаю что написать лучше
    public void GamePaused()
    {
        gamePaused = false;
    }
    public void GameStarted()
    {
        Invoke("GamePaused", 0.1f);
    }

    private int _fulledColumns;
    private int fulledColumns
    {
        get
        {
            return _fulledColumns;
        }
        set
        {
            _fulledColumns = value;
            PlayerPrefs.SetInt("fulledColumns", _fulledColumns);
        }
    }

    private void Awake()
    {
        if (S == null)
            S = this;
    }

    private void Start()
    {
        fulledColumns = PlayerPrefs.GetInt("fulledColumns", 0);

        if (!PlayerPrefs.HasKey("ItsFirstRun"))
        {
            ButtonController.S.ShowInformation();
            PlayerPrefs.SetString("ItsFirstRun", "false");
        }
    }

    public void ColumnFulls()
    {
        fulledColumns++;
        print(fulledColumns);
        if (fulledColumns >= 4)
        {
            QuastionPanel.S.EndGame();
            fulledColumns = 0;
        }
    }


    public void GenerateBackCard()
    {
        StartCoroutine(GenerateCardDelayed());
    }

    private IEnumerator GenerateCardDelayed()
    {

        yield return new WaitForFixedUpdate();
        GameObject backCard = Instantiate(card, backCardPosition, Quaternion.identity);

        backCard.tag = "BackCard";
        Card backCardScript = backCard.GetComponent<Card>();

        CardContainer.S.backCard = backCardScript;

        int value = GeneratorRandomCardValues.Generator();

        while (value > 128)
        {
            if (value == 256)
            {
                backCardScript.delCard = true;
                value = GeneratorRandomCardValues.Generator();
            }
            if (value == 512)
            {
                backCardScript.delCard = false;
                backCardScript.boostCard = true;
                value = 2;
            }
        }
        backCardScript.value = value;
    }
}
