using System.Collections;
using UnityEngine;

public class MusicRepeat : MonoBehaviour
{
    [SerializeField] private AudioClip embient;
    [SerializeField] [Min(0f)] private float delay = 55f;
    private AudioSource thisAudoiSource;
    private bool mute;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        thisAudoiSource = GetComponent<AudioSource>();

        if (GameObject.FindGameObjectsWithTag("Music").Length > 1) Destroy(gameObject);

        mute = (PlayerPrefs.GetInt("mute", 1) == 0);
        thisAudoiSource.mute = mute;
    }

    private void Start()
    {
        StartCoroutine(DelayPlaing());
    }

    public void SetMute()
    {

        mute = !mute;
        thisAudoiSource.mute = mute;
        PlayerPrefs.SetInt("mute", mute ? 0 : 1);
    }

    IEnumerator DelayPlaing()
    {
        while (true)
        {
            thisAudoiSource.PlayOneShot(embient);
            yield return new WaitForSeconds(delay);
        }
    }
}
