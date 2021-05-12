using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ButtonController : MonoBehaviour
{
    static public ButtonController S;

    [Header("Set in inspector")]
    [SerializeField] private GameObject informationPanel;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioMixerGroup mixerGroup;
    [SerializeField] private GameObject gear;

    private bool soundEffect;
    private AudioSource audioS;
    private string path;
    private bool gearVisibility = false;

    private void Awake()
    {
        if (S == null)
            S = this;

        audioS = GetComponent<AudioSource>();
        path = Application.persistentDataPath + "/testSave.xml";
        soundEffect = (PlayerPrefs.GetInt("soundEffect", 1) == 1);
    }

    private void Start()
    {
        mixerGroup.audioMixer.SetFloat("EffectSound", soundEffect ? 0 : -80);
    }

    public void PlayingClick()
    {
        audioS.PlayOneShot(buttonClick);
    }

    public void GoToGooglePlay()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.makar.dva");
    }

    public void MusicSound()
    {
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicRepeat>().SetMute();
    }

    public void GearInOut()
    {
        Animation gearAnimation = gear.GetComponent<Animation>();
        Transform gearTransform = gear.GetComponent<Transform>();
        if (!gearVisibility)
        {
            gearAnimation.Play("GearSettings");
            gearTransform.GetChild(0).gameObject.SetActive(true);

        }
        else
        {
            gearAnimation.Play("GearSettingsOut");
        }
        gearVisibility = !gearVisibility;
    }

    public void EffectSound()
    {

        soundEffect = !soundEffect;

        mixerGroup.audioMixer.SetFloat("EffectSound", soundEffect ? 0 : -80);
        PlayerPrefs.SetInt("soundEffect", soundEffect ? 1 : 0);
    }

    public void GoToMenu()
    {
        SaveController.S.Save();
        StartCoroutine(LoadingSceneDelayed("Menu"));
    }

    private IEnumerator LoadingSceneDelayed(string sceneName)
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneName);
    }

    public void ToMonsterList()
    {
        SceneManager.LoadScene("MonsterList");
    }

    public void Restart()
    {
        QuastionPanel.S.Restart();
    }

    public void NewGame()
    {
        File.Delete(path);
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("fulledColumns", 0);
        PlayerPrefs.SetInt("DelButtonNumOfUses", 3);
        PlayerPrefs.SetInt("RechButtonNumOfUses", 3);
        StartCoroutine(LoadingSceneDelayed("Table"));
    }

    public void LoadGame()
    {
        StartCoroutine(LoadingSceneDelayed("Table"));
    }

    public void ShowInformation()
    {
        Table.S.gamePaused = true;
        InformationPanel.S.ShoClose(true);
    }

    public void CloseInformation()
    {
        InformationPanel.S.ShoClose(false);
        Table.S.GameStarted();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
