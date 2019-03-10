using System.Collections;
using UnityEngine;
using System.Reflection;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace pacman
{
    /// <summary>
    /// GameManager class. Manages the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        //Transform containing pacdots
        [AssertNotNull]public Transform pacdotsTransform;
        //Text to show notifications
        [AssertNotNull]public Text notificationText;
        //Global Audio resources
        [AssertNotNull]public AudioResources audioResources;
        //Global constants
        [AssertNotNull]public Constants constants;

        PacmanController pacmanController;

        void Awake()
        {
            if (Application.isEditor)
            {
                AttributeAssert(AssertFieldNotNull);
            }
            PacmanController.pacmanControlState = false;
        }

        void Start()
        {
            pacmanController = FindObjectOfType<PacmanController>();
            StartCoroutine(GameLoop());	
        }

        IEnumerator GameLoop()
        {
            yield return StartGame();
            yield return PlayingGame();
            yield return EndGame();
        }

        IEnumerator StartGame()
        {
            notificationText.gameObject.SetActive(true);
            notificationText.text = "Ready!";
            notificationText.color = Color.yellow;

            //Wait until space is pressed
            AudioManager.PlaySFX(audioResources.introMusic);
            yield return new WaitForSeconds(audioResources.introMusic.length);
            notificationText.gameObject.SetActive(false);
            //enable controls
            PacmanController.pacmanControlState = true;
        }

        IEnumerator PlayingGame()
        {
            yield return new WaitUntil(() => pacdotsTransform.childCount == 0 || pacmanController.lives < 0);
        }

        IEnumerator EndGame()
        {
            PacmanController.pacmanControlState = false;
            Destroy(pacmanController);

            AudioManager.musicSource.Stop();

            if (pacdotsTransform.childCount == 0)
            {
                yield return new WaitForSeconds(constants.shortDelay);
                AudioManager.PlaySFX(audioResources.intermissionMusic);
                yield return new WaitForSeconds(audioResources.intermissionMusic.length);
                notificationText.gameObject.SetActive(true);
            }
            else
            {
                notificationText.text = "Game Over";
                notificationText.gameObject.SetActive(true);
                notificationText.color = Color.red;
                yield return new WaitForSeconds(3);
            }

            notificationText.text = "'R' to restart";
            notificationText.color = Color.white;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));
            SceneManager.LoadScene("GameScene");
        }

        /// <summary>
        /// Checks for the validity of a particular attribute
        /// </summary>
        /// <param name="assertArray">Array of asserts</param>
        void AttributeAssert(params Action<MonoBehaviour,Type,FieldInfo[]>[] assertArray)
        {
            MonoBehaviour[] sceneActive = FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour script in sceneActive)
            {
                var scriptType = script.GetType();
                var fields = scriptType.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (Action<MonoBehaviour,Type,FieldInfo[]> assert in assertArray)
                {
                    assert(script, scriptType, fields);
                }
            }
        }

        /// <summary>
        ///     Checks whether a particular field is unassigned or null
        /// </summary>
        /// <param name="script">Scripts to check</param>
        /// <param name="scriptType">Script type</param>
        /// <param name="fields">Fields of class</param>
        void AssertFieldNotNull(MonoBehaviour script, Type scriptType, FieldInfo[] fields)
        {
            foreach (var field in fields)
            {
                var assertNotNulls = field.GetCustomAttributes(typeof(AssertNotNull), true);
                if (assertNotNulls.Length < 1)
                    continue;

                var value = field.GetValue(script);
                if (value == null || (value is UnityEngine.Object && (UnityEngine.Object)value == null))
                {
                    Debug.LogErrorFormat(script, "Script '{0}', field '{1}' is unassigned.", scriptType.Name, field.Name);
                    break;
                }
            }
        }
    }
}