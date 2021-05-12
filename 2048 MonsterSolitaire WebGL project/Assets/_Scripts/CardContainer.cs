using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    public static CardContainer S;

    [HideInInspector] public Card frontCard;
    [HideInInspector] public Card backCard;

    private void Awake()
    {
        if (S == null)
        {
            S = this;
        }
    }
}
