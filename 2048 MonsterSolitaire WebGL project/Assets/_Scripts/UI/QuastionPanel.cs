using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using System.Collections;

public class QuastionPanel : MonoBehaviour
{
    static public QuastionPanel S;

    [SerializeField] private GameObject QuastionPanelFA;

    private Transform panelTransform;
    private Text inscription;
    private Button noMark;
    private Button yesMark;

    private bool eng;

    private void Awake()
    {
        if (S == null)
            S = this;

        panelTransform = GetComponent<Transform>().GetChild(0);

        noMark = panelTransform.GetChild(2).GetComponent<Button>();
        noMark.onClick.AddListener(ClosePanel);

        yesMark = panelTransform.GetChild(1).GetComponent<Button>();

        eng = (PlayerPrefs.GetString("Language", "eng") == "eng");
    }

    private void Start()
    {
        Advertisement.Initialize("4094945", false);
    }

    public void Activation()
    {
        Table.S.gamePaused = true;
        panelTransform.gameObject.SetActive(true);
        StartCoroutine(ActivatingPanel(true));
        inscription = GetComponentInChildren<Text>();
    }

    private IEnumerator ActivatingPanel(bool state)
    {
        Animation thisAnimation;
        thisAnimation = QuastionPanelFA.GetComponent<Animation>();
        if (state)
        {
            thisAnimation.Play("QuastionPanelAppearance");
        }
        else
        {
            thisAnimation.Play("QuastionPanelFadeOut");
            yield return new WaitForSeconds(0.5f);
            panelTransform.gameObject.SetActive(state);
        }
    }

    public void EndGame()
    {
        Activation();

        if (Advertisement.IsReady())
            Advertisement.Show();

        if (eng)
            inscription.text = "GREAT!\nYour score\n" + ScoreManager.S.score + "\nStart new game?";
        else
            inscription.text = "СУПЕР!\nТвой результат\n" + ScoreManager.S.score + "\nНачать заново?";

        yesMark.interactable = true;
        yesMark.onClick.RemoveAllListeners();
        yesMark.onClick.AddListener(ButtonController.S.NewGame);
    }

    public void Restart()
    {
        Activation();

        if (eng)
            inscription.text = "Restart?";
        else
            inscription.text = "Заново?";

        yesMark.interactable = true;
        yesMark.onClick.RemoveAllListeners();
        yesMark.onClick.AddListener(ButtonController.S.NewGame);
    }

    private void ClosePanel()
    {
        StartCoroutine(ActivatingPanel(false));
        Table.S.GameStarted();
    }
}
