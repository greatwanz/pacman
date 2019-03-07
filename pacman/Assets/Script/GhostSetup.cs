using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pacman
{
    public class GhostSetup : MonoBehaviour
    {
        public Ghost[] ghosts;

        void Awake()
        {
            foreach (Ghost g in ghosts)
            {
                GhostConsumable c = Instantiate(g.ghostPrefab, transform);
                c.ghost = g;
                c.name = g.name;
                c.transform.localPosition = g.initPosition;
                c.meshRenderer = c.GetComponent<MeshRenderer>();
                c.meshRenderer.material.color = g.initialColour;
            }
        }

    }
}