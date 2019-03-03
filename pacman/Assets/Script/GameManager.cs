using System.Collections;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace pacman
{
    /// <summary>
    /// GameManager class. Manages the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [NonSerialized]public static PacmanController[] players;
        [NonSerialized]public static Ghost[] ghosts;

        [AssertNotNull]public Transform playersTransform;
        [AssertNotNull]public Transform ghostsTransform;
        [AssertNotNull]public GameObject gameScreenObjects;
        [AssertNotNull]public Transform pacdotsTransform;
        [AssertNotNull]public Transform powerPelletsTransform;
        [AssertNotNull]public Text notificationText;
        [AssertNotNull]public AudioResources audioResources;
        [AssertNotNull]public Constants constants;
        [AssertNotNull]public Variables variables;


        void Awake()
        {
            if (Application.isEditor)
            {
                AttributeAssert(AssertFieldNotNull);
            }

            players = playersTransform.GetComponentsInChildren<PacmanController>(true);
            ghosts = ghostsTransform.GetComponentsInChildren<Ghost>(true);
        }

        void Start()
        {
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
            AudioManager.PlayMusic(audioResources.sirenMusic);
            //enable controls
            variables.pacmanControlState = true;
        }

        IEnumerator PlayingGame()
        {
            yield return new WaitUntil(() => pacdotsTransform.childCount == 0 || players.Any(p => p.lives < 0));
        }

        IEnumerator EndGame()
        {
            foreach (PacmanController p in players)
            {
                Destroy(p);
            }
            PowerPelletConsumable[] pellets = powerPelletsTransform.GetComponentsInChildren<PowerPelletConsumable>();
            foreach (PowerPelletConsumable p in pellets)
            {
                p.StopAllCoroutines();
            }

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