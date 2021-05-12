using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    static public ScoreManager S;

    [SerializeField] private Text bestScoreString;

    private Animation scoreScaling;
    private Text scoreString;
    private bool addition;

    private int startValue, endValue;
    private int _score;

    private float delay = 0;

    public int score
    {
        get
        {
            return _score;
        }

        set
        {
            _score = value;
            scoreString.text = _score.ToString();
            PlayerPrefs.SetInt("Score", _score);
        }
    }

    private int _bestScore;
    private int bestScore
    {
        get
        {
            return _bestScore;
        }

        set
        {
            _bestScore = value;
            bestScoreString.text = _bestScore.ToString();
            PlayerPrefs.SetInt("BestScore", _bestScore);
        }
    }

    private void Awake()
    {
        scoreString = GetComponent<Text>();
        scoreScaling = GetComponent<Animation>();

        score = PlayerPrefs.GetInt("Score", 0);
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (S == null)
        {
            S = this;
        }
    }

    private void FixedUpdate()
    {
        if (addition)
        {
            float tempValue = Mathf.Lerp(startValue, endValue, delay);
            delay += 0.1f;
            score = Mathf.RoundToInt(tempValue);

            if (bestScore < score)
            {
                bestScore = score;
            }

            if (score == endValue)
            {
                //controller.Save();
                addition = false;
                delay = 0;
            }
        }
    }

    public void Addition(int additionScore)
    {
        scoreScaling.Play();
        startValue = score;
        endValue = score + additionScore;
        addition = true;
    }
}
