using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingScore : MonoBehaviour
{
    static public FloatingScore S;

    private RectTransform rectTrans;
    private Text thisText;

    private Vector2 endPosition;

    private string scoreString;
    private int _score;
    public int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            if (_score == 0)
            {
                scoreString = "";
                thisText.text = scoreString;
            }
            else
            {
                scoreString = _score.ToString();
                thisText.text = '+' + scoreString;
            }
        }
    }

    private void Awake()
    {
        thisText = GetComponent<Text>();
        rectTrans = GetComponent<RectTransform>();

        if (S == null)
        {
            S = this;
        }

        score = 0;
        endPosition = ScoreManager.S.gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    public void AddScore(int newScore)
    {
        score = newScore;
        StartCoroutine(Move());
        StartCoroutine(ChangingTransporency());
        ScoreManager.S.Addition(newScore);
    }

    private IEnumerator Move()
    {
        float parameter = 0;
        Vector2 startPosition = rectTrans.anchoredPosition;
        while (!endPosition.Equals(rectTrans.anchoredPosition))
        {
            rectTrans.anchoredPosition = Vector2.Lerp(startPosition, endPosition, parameter);
            parameter += 0.075f;
            yield return new WaitForFixedUpdate();
        }
        rectTrans.anchoredPosition = startPosition;
    }

    private IEnumerator ChangingTransporency()
    {
        Color c = thisText.color;
        c.a = 0;
        thisText.color = c;

        float parameter = 0;
        float transporency = 0;
        while (!thisText.color.a.Equals(1))
        {
            c = thisText.color;
            c.a = Mathf.Lerp(transporency, 1, parameter);
            thisText.color = c;

            parameter += 0.075f;
            yield return new WaitForFixedUpdate();
        }
        c.a = 0;
        thisText.color = c;
    }
}
