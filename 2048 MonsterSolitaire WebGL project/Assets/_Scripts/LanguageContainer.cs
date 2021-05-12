using UnityEngine;
using UnityEngine.UI;

public class LanguageContainer : MonoBehaviour
{
    [SerializeField] private Text[] gameText = new Text[9];

    private string[] engStrings =
    {
        "BEST",
        "HOW TO PLAY",
        "To play, you need to move the highlighted card in your hand\nto one of the four columns. This can be done directly by dragging it or by taping on the column. The same cards are\nadded together.",
        "HELP BUTTONS",
        "- removes the highlighted card",
        "- replaces the cards\nin your hand with 4 and 2",
        "HELP CARDS",
        " - this card is\nalways adds up with\nthe card in the column",
        " - the card with an \narrow clears the\ncolumn if it is add up"
    };

    private string[] rusStrings =
    {
        "РЕКОРД",
        "КАК ИГРАТЬ",
        "Для игры нужно перемещать подсвеченную карту в руке в одну из четырёх колонок. Это можно делать непосредственно перетаскивая её или тапая на колонку. Одинаковые карты складываются.",
        "КНОПКИ-ПОМОЩНИКИ",
        " - удаляет подсвеченную карту",
        "   - заменяет карты в руке на 4 и 2",
        "КАРТЫ-ПОМОЩНИКИ",
        " - эта карта всегда\n   складывается с картой в\nколонке",
        " - карта со стрелочкой\nочищает колонку,\n если складывается"
    };
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
        //print(eng ? "eng" : "rus");

        SetLanguage(eng);
    }

    private void SetLanguage(bool engLanguage)
    {
        if (engLanguage)
        {
            for (int i = 0; i < gameText.Length; i++)
            {
                gameText[i].text = engStrings[i];
            }
        }
        else
        {
            for (int i = 0; i < gameText.Length; i++)
            {
                gameText[i].text = rusStrings[i];
            }
        }
    }
}
