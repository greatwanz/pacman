using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Flashes a text on screen. Requires the GameObject to have a Text component
/// </summary>
[RequireComponent(typeof(Text))]
public class FlashText : MonoBehaviour
{
    public float waitTime;

    Text text;
    string initString;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        initString = text.text;
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        while (true)
        {
            text.text = initString;
            yield return new WaitForSeconds(waitTime);
            text.text = string.Empty;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
