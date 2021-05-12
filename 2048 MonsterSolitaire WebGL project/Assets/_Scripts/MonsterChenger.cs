using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterChenger : MonoBehaviour
{
    public Sprite[] monsters;
    private Dictionary<int, Sprite> monsterVal;

    private int maxCardValue;

    private void Awake()
    {
        monsterVal = new Dictionary<int, Sprite>()
        {
            {2,      monsters[0]},
            {4,      monsters[1]},
            {8,      monsters[2]},
            {16,     monsters[3]},
            {32,     monsters[4]},
            {64,     monsters[5]},
            {128,    monsters[6]},
            {256,    monsters[7]},
            {512,    monsters[8]},
            {1024,   monsters[9]},
            {2048,   monsters[10]},
            {4096,   monsters[11]},
            {8192,   monsters[12]},
            {16384,  monsters[13]},
            {32768,  monsters[14]},
            {65536,  monsters[15]},
            {131072, monsters[16]},
            {262144, monsters[17]},
            {524288, monsters[18]},
        };

        maxCardValue = PlayerPrefs.GetInt("maxCardValue", 2);
    }

    public void ChangePic(int val)
    {
        if (monsterVal.ContainsKey(val))
        {
            GetComponent<SpriteRenderer>().sprite = monsterVal[val];
        }
        else if (val > 524288)
        {
            GetComponent<SpriteRenderer>().sprite = monsterVal[GeneratorRandomCardValues.Generator()];
        }
    }

    public void SetImage(int val)
    {
        if (monsterVal.ContainsKey(val))
        {
            GetComponent<Image>().sprite = monsterVal[val];
            GetComponentInChildren<Text>().text = val.ToString();
        }
        if (val > maxCardValue)
        {
            GetComponent<Image>().color = new Color(0, 0, 0);
        }
    }
}
