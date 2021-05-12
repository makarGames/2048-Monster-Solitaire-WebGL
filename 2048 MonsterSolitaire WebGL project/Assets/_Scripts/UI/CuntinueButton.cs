using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CuntinueButton : MonoBehaviour
{
    private string path;

    private void Awake()
    {
        path = Application.persistentDataPath + "/testSave.xml";
        if (!File.Exists(path))
            GetComponent<Button>().interactable = false;
    }
}
