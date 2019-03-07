using UnityEngine;
using System;
using System.Collections;
using UnityEngine.AI;
using System.Linq;

namespace pacman
{
    /// <summary>
    /// A ghost
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class GhostConsumable : Consumable
    {
        //Current state of the ghost
        [ReadOnlyAttribute]public GhostState currentGhostState;
        [ReadOnlyAttribute]public int targetIndex;
        //Coroutine to time unfrightening of ghosts
        public static Coroutine unfrightenCoroutine;
        //Definitions of the ghost
        [NonSerialized]public Ghost ghost;

        //MeshRenderer of the ghost
        [NonSerialized]public MeshRenderer meshRenderer;
        [NonSerialized]public Pathfinding pathfinding;
        [NonSerialized]public Transform target;
        [NonSerialized]public Vector3[] path;

        IEnumerator Start()
        {
            target = FindObjectOfType<PacmanController>().transform;
            pathfinding = GetComponent<Pathfinding>();
            SetState(ghost.initialState);
            yield return new WaitUntil(() => variables.pacmanControlState);
            SetState(ghost.chaseState);
        }

        void Update()
        {
            //Execute the ghost's current state
            if (currentGhostState != null)
                currentGhostState.Execute(this);
        }

        protected override void OnTriggerEnter(Collider col)
        {
            PacmanController p = col.GetComponent<PacmanController>();

            if (p != null)
            {
                //If ghost is in frightened state, ghost is eaten, otherwise pacman dies
                if (currentGhostState.GetType() == typeof(GhostFrightenedState))
                    p.StartCoroutine(p.ConsumeGhost(this, ghost.eatGhostSFX));
                else
                    p.StartCoroutine(p.PacmanDies());
            }
        }

        /// <summary>
        /// Sets the state of the ghosts
        /// </summary>
        /// <param name="state">State to switch ghosts to</param>
        public static void SetState(GhostState state)
        {
            GhostConsumable[] consumable = FindObjectsOfType<GhostConsumable>();

            foreach (GhostConsumable g in consumable)
            {
                state.Init(g);
                g.currentGhostState = state;
            }
        }

        public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                path = newPath;
                targetIndex = 0;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        IEnumerator FollowPath()
        {
            if (path.Length > 0)
            {
                Vector3 currentWaypoint = path[0];
                while (true)
                {
                    if (transform.position == currentWaypoint)
                    {
                        targetIndex++;
                        if (targetIndex >= path.Length)
                        {
                            yield break;
                        }
                        currentWaypoint = path[targetIndex];
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, ghost.chaseSpeed * Time.deltaTime);
                    }
                    yield return null;
                }
            }
        }

        public void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }

    }
}