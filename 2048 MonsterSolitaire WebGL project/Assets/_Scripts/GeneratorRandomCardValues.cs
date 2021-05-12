using UnityEngine;

public class GeneratorRandomCardValues
{
    public static int Generator()
    {
        float RandomValue = Random.Range(0, 100f);
        if (RandomValue < 33.5f)
        {
            return 2;
        }
        else if (RandomValue < 55.8f)
        {
            return 4;
        }
        else if (RandomValue < 70.8f)
        {
            return 8;
        }
        else if (RandomValue < 80.8f)
        {
            return 16;
        }
        else if (RandomValue < 87.8f)
        {
            return 32;
        }
        else if (RandomValue < 92.33f)
        {
            return 64;
        }
        else if (RandomValue < 98f)
        {
            return 128;
        }
        else if (RandomValue < 99f)
        {
            return 256;
        }
        return 512;
    }
}
