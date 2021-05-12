using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DelButton : MonoBehaviour
{
    public static DelButton S;
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
            textNumber.text = value.ToString();
            PlayerPrefs.SetInt("DelButtonNumOfUses", _numberOfUses);
        }
    }
    private void Awake()
    {
        if (S == null)
            S = this;
    }

    private void Start()
    {
        numberOfUses = PlayerPrefs.GetInt("DelButtonNumOfUses", 3);
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        if (numberOfUses > 0)
        {
            CardContainer.S.frontCard.Destroing();
            CardContainer.S.backCard.ChangingCardStatus();
            numberOfUses--;
            StartCoroutine(Saving());
        }
    }
    private IEnumerator Saving()
    {
        yield return new WaitForSeconds(1f);
        SaveController.S.Save();
    }
}