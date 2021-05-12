using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SandwichButton : MonoBehaviour
{
    private Animation thisAnimation;
    private Button thisButton;
    private GameObject bar;
    private bool turn;

    private void Awake()
    {
        turn = false;

        bar = transform.GetChild(0).gameObject;

        thisAnimation = GetComponent<Animation>();

        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(Enabling);
    }

    private void Enabling()
    {
        if (turn)
            thisAnimation.Play("TableBarOut");
        else
            thisAnimation.Play("TableBarAppearance");
        //bar.SetActive(!turn);
        turn = !turn;
    }
}
