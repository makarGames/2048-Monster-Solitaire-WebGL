using System.Collections.Generic;
using UnityEngine;

public class CardColor
{
    static public Dictionary<int, Color> cardColor = new Dictionary<int, Color>()
    {
        {2,      new Color32(41, 96, 121, 255)},
        {4,      new Color32(213, 137, 55, 255)},
        {8,      new Color32(231, 129, 92, 255)},
        {16,     new Color32(229, 155, 128, 255)},
        {32,     new Color32(229, 96, 107, 255)},
        {64,     new Color32(221, 64, 77, 255)},
        {128,    new Color32(23, 113, 178, 255)},
        {256,    new Color32(225, 107, 64, 255)},
        {512,    new Color32(208, 179, 93, 255)},
        {1024,   new Color32(238, 128, 127, 255)},
        {2048,   new Color32(175, 102, 142, 255)},
        {4096,   new Color32(100, 153, 116, 255)},
        {8192,   new Color32(140, 65, 106, 255)},
        {16384,  new Color32(88, 179, 175, 255)},
        {32768,  new Color32(235, 192, 63, 255)},
        {65536,  new Color32(129, 204, 168, 255)},
        {131072, new Color32(78, 197, 158, 255)},
        {262144, new Color32(195, 55, 54, 255)},
        {524288, new Color32(255, 189, 127, 255)},
    };
}