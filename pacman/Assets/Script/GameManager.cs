using System.Collections;
using UnityEngine;
using System.Reflection;
using System;

public class GameManager : MonoBehaviour
{
    public bool showMainMenu;
    [AssertNotNull]public GameObject titleScreenUI;
    [AssertNotNull]public GameObject gameScreenUI;
    [AssertNotNull]public GameObject gameScreenObjects;

    void Awake()
    {
        if (Application.isEditor)
        {
            AttributeAssert(AssertFieldNotNull);
        }

        titleScreenUI.SetActive(showMainMenu);
        gameScreenUI.SetActive(!showMainMenu);
        gameScreenObjects.SetActive(!showMainMenu);
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
        //Wait until space is pressed
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        titleScreenUI.SetActive(false);
    }

    IEnumerator PlayingGame()
    {
        gameScreenUI.SetActive(true);
        gameScreenObjects.SetActive(true);
        yield return null;
    }

    IEnumerator EndGame()
    {
        yield return null;
    }
        
    // Checks for the validity of a particular attribute
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

    //Checks whether a particular field is unassigned or null
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
