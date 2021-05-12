using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

public enum GameMode
{
    checking,
    moving,
    idle
}

[RequireComponent(typeof(AudioSource))]
public class Column : MonoBehaviour
{
    public List<Card> cards = new List<Card>();
    public bool columnFulled = false;

    [SerializeField] private AudioClip[] cardMarging = new AudioClip[9];
    [SerializeField] private AudioClip delCardSound;
    private int margeCounter = 0;

    private AudioSource audioS;

    private GameMode gM = GameMode.idle;
    private Card tempCardScript;

    private int startValue, endValue;

    private bool addition = false;
    private float delay = 0;

    private int rows = 10;

    private Vector3 mousePos;

    private void Awake()
    {
        audioS = GetComponent<AudioSource>();
    }

    #region //methods for saving game
    private void Start()
    {
        SaveController.S.columns.Add(this);
    }

    private void OnDestroy()
    {
        SaveController.S.columns.Remove(this);
    }

    public XElement GetElement()
    {
        XAttribute x = new XAttribute("x", transform.position.x);
        XAttribute y = new XAttribute("y", transform.position.y);
        XAttribute z = new XAttribute("z", transform.position.z);

        XAttribute cFulled = new XAttribute("cFulled", columnFulled);

        XElement element = new XElement("ColumnInstance", "Column", x, y, z, cFulled);

        foreach (Card card in cards)
        {
            element.Add(card.GetElement());
        }
        return element;
    }
    #endregion

    private void FixedUpdate()
    {
        if (addition)
        {
            float tempValue = Mathf.Lerp(startValue, endValue, delay);

            tempCardScript.value = Mathf.RoundToInt(tempValue);
            tempCardScript.ColorChang(delay);

            delay += 0.05f;

            if (tempCardScript.value == endValue)
            {
                addition = false;
                delay = 0;

                FloatingScore.S.AddScore(endValue);


                ChackingMatches();
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !Table.S.gamePaused)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (-2 < mousePos.y && mousePos.y < 3)
                if (transform.position.x - 0.5f < mousePos.x && mousePos.x < transform.position.x + 0.5f)
                    TryingToAddNewCard();
        }
    }


    public void TryingToAddNewCard()
    {
        if (columnFulled)
        {
            print("Max rows");
            return;
        }

        if (gM != GameMode.idle) return;

        tempCardScript = CardContainer.S.frontCard; ;

        tempCardScript.DisableCardCollider();
        tempCardScript.thisTransform.GetChild(5).gameObject.SetActive(false);

        gM = GameMode.moving;
        cards.Add(tempCardScript);

        tempCardScript.thisTransform.parent = gameObject.transform;

        tempCardScript.startPos = tempCardScript.thisTransform.position + new Vector3(0, 0, -1);
        tempCardScript.endPos = transform.position + new Vector3(0, 2.363f - cards.Count * 0.45f, -0.05f * cards.Count);
        tempCardScript.startMoving = true;
        tempCardScript.gameObject.tag = "StackCard";



        CardContainer.S.frontCard = null;
        CardContainer.S.backCard.ChangingCardStatus();

        margeCounter = 0;

        Invoke("ChackingMatches", 0.5f);
    }

    private bool FindMatches()
    {
        if (cards.Count >= 2 && (tempCardScript.value == cards[cards.Count - 2].value || tempCardScript.boostCard))
            return true;

        Transform tempCardTransform = tempCardScript.thisTransform;
        if (tempCardScript.delCard)
        {
            tempCardTransform.GetChild(1).gameObject.SetActive(true);
            tempCardTransform.GetChild(3).gameObject.SetActive(false);
            tempCardScript.delCard = false;
        }

        if (tempCardScript.boostCard)
        {
            tempCardTransform.GetChild(2).gameObject.SetActive(false);
            tempCardScript.boostCard = false;
        }
        return false;
    }

    private void ChackingMatches()
    {
        if (!FindMatches() && !addition)
        {
            gM = GameMode.idle;

            if (cards.Count == rows)
            {
                if (!columnFulled)
                {
                    columnFulled = true;
                    Table.S.ColumnFulls();
                }
                print("Max rows");
            }
            SaveController.S.Save();
            return;
        }

        gM = GameMode.checking;

        if (tempCardScript.delCard)
        {
            for (int i = cards.Count - 1; i >= 0; i--)
                cards[i].Destroing();

            audioS.PlayOneShot(delCardSound);
            cards.Clear();
            SaveController.S.Save();
            gM = GameMode.idle;
            return;
        }

        startValue = tempCardScript.value;
        endValue = cards[cards.Count - 2].value * 2;

        tempCardScript.Destroing();
        cards.RemoveAt(cards.Count - 1);

        tempCardScript = cards[cards.Count - 1];
        tempCardScript.thisTransform.GetChild(0).GetComponent<MonsterChenger>().ChangePic(endValue);
        addition = true;

        audioS.PlayOneShot(cardMarging[margeCounter]);
        margeCounter++;
    }
}
