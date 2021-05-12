using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

public class SaveController : MonoBehaviour
{
    static public SaveController S;
    [Header("Set in Inspector")]
    [SerializeField] private GameObject Column;
    [SerializeField] private GameObject Card;
    [SerializeField] private TextAsset startLevel;

    [Header("Set Dynamically")]
    public List<Column> columns = new List<Column>();

    private string path;

    private void Awake()
    {
        if (S == null)
            S = this;
    }
    private void Start()
    {
        path = Application.persistentDataPath + "/testSave.xml";
        print(path);
        Load();
    }

    public void Save()
    {
        XElement root = new XElement("root");
        Card FrontCard = CardContainer.S.frontCard;
        Card BackCard = CardContainer.S.backCard;

        foreach (Column cols in columns)
        {
            root.Add(cols.GetElement());
        }

        root.Add(FrontCard.GetElement());
        root.Add(BackCard.GetElement());

        //XDocument saveDoc = new XDocument(root);

        File.WriteAllText(path, root.ToString());
    }

    public void Load()
    {
        XElement root = null;
        if (!File.Exists(path))
        {
            print("SaveData not fonded");
            root = XDocument.Parse(startLevel.ToString()).Element("root");
        }
        else
        {
            root = XDocument.Parse(File.ReadAllText(path)).Element("root");
        }

        if (root == null)
        {
            print("Level load failed!");
            return;
        }

        GenerateScene(root);
    }

    private void GenerateScene(XElement root)
    {
        var c = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        c.NumberFormat.NumberDecimalSeparator = "."; // Разделитель. Если у тебя запятая, тогда ставь ","

        GameObject tempColumn;
        GameObject tempCard;

        foreach (XElement ColumnInstance in root.Elements("ColumnInstance"))
        {
            tempColumn = Instantiating(ColumnInstance, Column, c);
            Column tempColumnScript = tempColumn.GetComponent<Column>();
            tempColumnScript.columnFulled = bool.Parse(ColumnInstance.Attribute("cFulled").Value);

            foreach (XElement CardInstance in ColumnInstance.Elements("CardInstance"))
            {
                tempCard = Instantiating(CardInstance, Card, c);

                Card tempCardScript = tempCard.GetComponent<Card>();

                tempCardScript.DisableCardCollider();
                tempCardScript.thisTransform.parent = tempColumn.transform;
                tempCardScript.value = int.Parse(CardInstance.Attribute("value").Value);

                tempColumnScript.cards.Add(tempCardScript);
                tempCard.tag = "StackCard";
            }
        }


        XElement BackCardInstance = root.Element("BackCardInstance");
        CardInstantiate(BackCardInstance, c);

        XElement FrontCardInstance = root.Element("FrontCardInstance");
        CardInstantiate(FrontCardInstance, c);
    }

    private GameObject Instantiating(XElement position, GameObject instGO, System.IFormatProvider c)
    {
        Vector3 vectorPosition;
        vectorPosition.x = float.Parse(position.Attribute("x").Value, c);
        vectorPosition.y = float.Parse(position.Attribute("y").Value, c);
        vectorPosition.z = float.Parse(position.Attribute("z").Value, c);

        return Instantiate(instGO, vectorPosition, Quaternion.identity);
    }

    private void CardInstantiate(XElement card, System.IFormatProvider c)
    {
        XElement CardInstance = card;

        GameObject tempCard = Instantiating(CardInstance, Card, c);

        tempCard.tag = card.Value;
        Card tempCardScript = tempCard.GetComponent<Card>();

        if (tempCard.tag == "FrontCard")
        {
            tempCardScript.thisTransform.GetChild(5).gameObject.SetActive(true);
            CardContainer.S.frontCard = tempCardScript;
        }
        else CardContainer.S.backCard = tempCardScript;

        tempCardScript.value = int.Parse(CardInstance.Attribute("value").Value);
        tempCardScript.delCard = bool.Parse(CardInstance.Attribute("delCard").Value);
        tempCardScript.boostCard = bool.Parse(CardInstance.Attribute("boostCard").Value);
    }
}
