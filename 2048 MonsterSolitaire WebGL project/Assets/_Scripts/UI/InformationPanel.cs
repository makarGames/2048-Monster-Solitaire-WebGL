using System.Collections;
using UnityEngine;

public class InformationPanel : MonoBehaviour
{
    static public InformationPanel S;

    [SerializeField] private GameObject panel;
    //[SerializeField] private GameObject quastionPanel;


    private void Awake()
    {
        if (S == null)
            S = this;
    }

    public void ShoClose(bool state)
    {
        StartCoroutine(ActivatingPanel(state));
    }

    private IEnumerator ActivatingPanel(bool state)
    {
        Animation thisAnimation;
        thisAnimation = panel.GetComponent<Animation>();

        if (state)
        {
            panel.SetActive(state);
            thisAnimation.Play();
        }
        else
        {
            thisAnimation.Play("FadeOut");
            yield return new WaitForSeconds(0.5f);
            panel.SetActive(state);
        }
    }
}
