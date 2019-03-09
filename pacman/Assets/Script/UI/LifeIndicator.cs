﻿using UnityEngine;
using UnityEngine.UI;

namespace pacman
{
    public class LifeIndicator : MonoBehaviour
    {
        //Life indicator prefab
        [AssertNotNull]public Image lifeIndicator;
        [AssertNotNull]public Constants constants;

        PacmanController controller;

        // Use this for initialization
        void Start()
        {
            controller = FindObjectOfType<PacmanController>();
            controller.spawnEvent += DecreaseLife;

            for (int i = 0; i < constants.startingLives; ++i)
            {
                Instantiate(lifeIndicator, transform);
            }
        }

        void DecreaseLife()
        {
            Destroy(transform.GetChild(controller.lives - 1).gameObject);
        }
    }
}