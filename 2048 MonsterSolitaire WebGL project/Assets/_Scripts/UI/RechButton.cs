using UnityEngine;
using UnityEngine.UI;

public class RechButton : MonoBehaviour
{
    public static RechButton S;
    [Header("Set in Inspector")]
    [SerializeField] private Text textNumber;

    private int _numberOfUses;
    public int numberOfUses
    {
        get
        {
            return _numberOfUses;
        }

        set
        {
            _numberOfUses = value;
            textNumber.text = _numberOfUses.ToString();
            PlayerPrefs.SetInt("RechButtonNumOfUses", _numberOfUses);
        }
    }

    private void Awake()
    {
        if (S == null)
            S = this;
    }

    private void Start()
    {
        numberOfUses = PlayerPrefs.GetInt("RechButtonNumOfUses", 3);
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        if (numberOfUses <= 0) return;

        RechargeCards(CardContainer.S.frontCard, 2);
        RechargeCards(CardContainer.S.backCard, 4);

        numberOfUses--;

        SaveController.S.Save();
    }

    private void RechargeCards(Card card, int value)
    {
        Card cardScript = card;
        Transform cardTransform = cardScript.thisTransform;
        cardTransform.GetChild(1).gameObject.SetActive(true);
        cardTransform.GetChild(2).gameObject.SetActive(false);
        cardTransform.GetChild(3).gameObject.SetActive(false);

        cardScript.value = value;

        cardScript.thisTransform.GetChild(0).GetComponent<MonsterChenger>().ChangePic(value);
        cardScript.ColorChang(1);

        cardScript.boostCard = false;
        cardScript.delCard = false;
    }
}
