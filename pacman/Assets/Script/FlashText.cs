using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace pacman
{
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

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("GameScene");
            }
        }

        /// <summary>
        /// Flash text coroutine
        /// </summary>
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
}