using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Flashes a text on screen. Requires the GameObject to have a Text component
/// </summary>
public class FlashText : MonoBehaviour
{
    public float waitTime;
    public Text text;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        while (true)
        {
            text.gameObject.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            text.gameObject.SetActive(false);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
