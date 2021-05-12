using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LanguageMenu : MonoBehaviour
{
    [SerializeField] private List<Button> menuButtons = new List<Button>();
    [SerializeField] private List<Sprite> engSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> rusSprites = new List<Sprite>();

    private bool _eng;
    private bool eng
    {
        get
        {
            return _eng;
        }
        set
        {
            _eng = value;
            PlayerPrefs.SetString("Language", _eng ? "eng" : "rus");
        }
    }

    private void Awake()
    {
        eng = (PlayerPrefs.GetString("Language", "eng") == "eng");
        SetLanguage(eng);
    }

    public void ChangeLanguage()
    {
        eng = !eng;
        SetLanguage(eng);
    }

    private void SetLanguage(bool language)
    {
        if (language)
        {
            for (int i = 0; i < menuButtons.Count; i++)
            {
                SetButtonSprite(menuButtons[i], engSprites[i]);
            }
        }
        else
        {
            for (int i = 0; i < menuButtons.Count; i++)
            {
                SetButtonSprite(menuButtons[i], rusSprites[i]);
            }
        }
    }

    private void SetButtonSprite(Button changingButton, Sprite settingSprite)
    {
        changingButton.GetComponent<Image>().sprite = settingSprite;
        changingButton.GetComponent<Image>().SetNativeSize();
        changingButton.GetComponent<RectTransform>().sizeDelta /= 2f;
    }
}
