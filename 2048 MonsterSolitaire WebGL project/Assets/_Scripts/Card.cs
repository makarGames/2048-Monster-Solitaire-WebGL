using UnityEngine;
using System.Xml.Linq;

[RequireComponent(typeof(SpriteRenderer), typeof(Transform), typeof(BoxCollider2D))]
public class Card : MonoBehaviour
{
    [Header("Set in Inspector")]
    //3d text for card values
    [SerializeField] private TextMesh valueTextMesh;
    [SerializeField] private AudioClip cardMoving;

    [Header("Type of Card")]
    public bool delCard = false;        //"Deleting" type of card
    public bool boostCard = false;      //"Doubling" type of card

    [HideInInspector] public Transform thisTransform;

    //position vectors for smooth moving
    [HideInInspector] public Vector3 endPos;              //end position
    [HideInInspector] public Vector3 midPos;              //middle position
    [HideInInspector] public Vector3 startPos;            //start position
    [HideInInspector] public bool startMoving = false;

    private SpriteRenderer thisRenderer;
    private AudioSource audioS;
    private Animation thisAnimation;

    //time interval for moving and value chanching
    private float timeDuration = 0.3f;

    private Vector3 mousePos;

    private Color startColor;
    private Color endColor;

    //values for slow value changing
    private int startempValuealue, endValue;

    private bool moving = false;
    private float timeStart;
    private float counterBeforeDestroing = 0.1f;


    //private int maxCardValue;

    [Space(5)]
    [SerializeField] private int _value;

    public int value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;

            if (_value < 10)
                valueTextMesh.fontSize = 287;
            else if (_value < 100)
                valueTextMesh.fontSize = 250;
            else if (_value < 100000)
                valueTextMesh.fontSize = 160;
            else
                valueTextMesh.fontSize = 125;

            string v = "";
            int tempValue = _value;

            if (_value > 999 && _value < 9999)
            {
                v += tempValue / 100;
                v += "\n";
                v += tempValue % 100;
            }
            else if (_value > 9999 && _value < 99999)
            {
                v += tempValue / 1000;
                v += "\n";
                v += tempValue % 1000;
            }
            else if (_value > 99999)
            {
                v += tempValue / 10000;
                v += "\n";
                tempValue %= 10000;
                v += tempValue / 100;
                v += "\n";
                v += tempValue % 100;
            }
            else
                v += tempValue;

            valueTextMesh.text = v;

            if (CardColor.cardColor.ContainsKey(_value))
                thisRenderer.color = CardColor.cardColor[_value];

            /* maxCardValue = PlayerPrefs.GetInt("maxCardValue");
            if (value > maxCardValue)
            {
                PlayerPrefs.SetInt("maxCardValue", value);
            } */
        }
    }

    private void Awake()
    {
        thisRenderer = GetComponent<SpriteRenderer>();
        thisTransform = GetComponent<Transform>();
        thisAnimation = GetComponent<Animation>();
        audioS = GetComponent<AudioSource>();
    }

    private void Start()
    {
        thisTransform.localScale = new Vector3(0.09f, 0.09f, 1f);
        thisTransform.GetChild(0).GetComponent<MonsterChenger>().ChangePic(value);
        if (delCard)
        {
            thisTransform.GetChild(1).gameObject.SetActive(false);
            thisTransform.GetChild(3).gameObject.SetActive(true);
        }
        if (boostCard)
        {
            thisTransform.GetChild(2).gameObject.SetActive(true);
        }
        startPos = thisTransform.position;
    }

    private void FixedUpdate()
    {
        if (startMoving)
        {
            audioS.PlayOneShot(cardMoving);

            startMoving = false;
            moving = true;
            timeStart = Time.time;

            if (gameObject.tag == "StackCard")
            {
                thisAnimation.Play();
            }

        }

        if (moving)
        {
            float u = (Time.time - timeStart) / timeDuration;

            if (u >= 1)
            {
                u = 1;
                moving = false;
                startPos = thisTransform.position;
                timeDuration = 0.3f;

            }

            u = Easing.Ease(u, "InOut");

            midPos = (1 - u) * startPos + u * endPos;
            thisTransform.position = midPos;
        }
    }

    private void OnMouseDrag()
    {
        if (gameObject.tag == "FrontCard" && !Table.S.gamePaused)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            thisTransform.position = new Vector3(mousePos.x, mousePos.y, thisTransform.position.z);
        }
    }

    private void OnMouseUp()
    {
        if (gameObject.tag != "FrontCard") return;

        endPos = new Vector3(0.33f, -3.557f, -1.15f);
        startPos = thisTransform.position;
        startMoving = true;
    }

    public void DisableCardCollider()
    {
        BoxCollider2D cardColl = GetComponent<BoxCollider2D>();
        cardColl.enabled = false;
    }

    public void ChangingCardStatus()
    {
        gameObject.tag = "FrontCard";
        CardContainer.S.frontCard = this;
        thisTransform.GetChild(5).gameObject.SetActive(true);   //turn on highlight

        startPos = thisTransform.position + new Vector3(0, 0, -1f);
        endPos = thisTransform.position + new Vector3(0.66f, 0, -1f);
        timeDuration = 0.2f;
        startMoving = true;
        Table.S.GenerateBackCard();
    }

    public void ColorChang(float del)
    {
        if (CardColor.cardColor.ContainsKey(value * 2))
        {
            startempValuealue = value;
            endValue = value * 2;

            startColor = CardColor.cardColor[startempValuealue];
            endColor = CardColor.cardColor[endValue];

            thisRenderer.color = CardColor.cardColor[startempValuealue];
        }
        else if (value > 524288 && endColor == Color.clear)
        {
            endColor = CardColor.cardColor[GeneratorRandomCardValues.Generator() * GeneratorRandomCardValues.Generator()];
        }
        else
        {
            Color tempColor = Color.Lerp(startColor, endColor, del);
            thisRenderer.color = tempColor;
        }
        if (del == 1) endColor = Color.clear;
    }



    public void Destroing()
    {
        if (gameObject.tag != "PrepareToDestroy")
        {
            gameObject.tag = "PrepareToDestroy";

            thisAnimation.Play("DestroingAnimation");
        }

        Color color;

        for (int i = 0; i < 5; i++)
        {
            color = thisTransform.GetChild(i).gameObject.GetComponent<Renderer>().material.color;
            color.a = Mathf.Lerp(1f, 0, counterBeforeDestroing);
            thisTransform.GetChild(i).gameObject.GetComponent<Renderer>().material.color = color;
        }

        color = thisRenderer.color;
        color.a = Mathf.Lerp(1f, 0, counterBeforeDestroing);
        thisRenderer.material.color = color;

        if (counterBeforeDestroing < 1f)
        {
            counterBeforeDestroing += 0.2f;
            Invoke("Destroing", 0.05f);

        }
        else
            Destroy(gameObject);
    }

    public XElement GetElement()
    {
        XAttribute x = new XAttribute("x", thisTransform.position.x);
        XAttribute y = new XAttribute("y", thisTransform.position.y);
        XAttribute z = new XAttribute("z", thisTransform.position.z);

        XAttribute recordedValue = new XAttribute("value", value);

        XAttribute delStatus = new XAttribute("delCard", delCard);
        XAttribute boostStatus = new XAttribute("boostCard", boostCard);

        XElement element = new XElement("CardInstance", "Card", x, y, z, recordedValue, delStatus, boostStatus);

        if (gameObject.tag == "BackCard")
            element = new XElement("BackCardInstance", "BackCard", x, y, z, recordedValue, delStatus, boostStatus);
        else if (gameObject.tag == "FrontCard")
            element = new XElement("FrontCardInstance", "FrontCard", x, y, z, recordedValue, delStatus, boostStatus);
        return element;
    }
}
