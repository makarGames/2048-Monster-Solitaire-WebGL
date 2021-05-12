using UnityEngine;
using UnityEngine.UI;

public class ChangingButtonImage : MonoBehaviour
{
    [SerializeField] private Sprite pictureOn;
    [SerializeField] private Sprite pictureOff;

    private Button thisButton;
    private Image thisImage;

    private void Awake()
    {


        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(ChangeButtonPicture);

        thisImage = GetComponent<Image>();

        string name = gameObject.name;
        switch (name)
        {
            case "MuteButton":
                if (PlayerPrefs.GetInt("mute", 1) == 0)
                    thisImage.sprite = pictureOff;
                else
                    thisImage.sprite = pictureOn;
                return;
            case "MuteEffectsButton":
                if (PlayerPrefs.GetInt("soundEffect", 1) == 0)
                    thisImage.sprite = pictureOff;
                else
                    thisImage.sprite = pictureOn;
                return;
            case "Language":
                if (PlayerPrefs.GetString("Language", "rus") == "eng")
                    thisImage.sprite = pictureOff;
                else
                    thisImage.sprite = pictureOn;
                return;
        }
    }

    private void ChangeButtonPicture()
    {
        if (thisImage.sprite == pictureOn)
            thisImage.sprite = pictureOff;
        else
            thisImage.sprite = pictureOn;
    }
}
