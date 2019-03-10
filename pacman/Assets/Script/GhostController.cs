using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace pacman
{
    /// <summary>
    /// A ghost
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class GhostController : Controller
    {
        //Current state of the ghost
        [ReadOnlyAttribute]public GhostState currentGhostState;
        [ReadOnlyAttribute]public int frightenedLoopCount;
        [AssertNotNull]public Ghost ghost;
        [AssertNotNull]public GameObject initialTargetObject;
        //MeshRenderer of the ghost
        [NonSerialized]public MeshRenderer meshRenderer;
        //Number of loops the frightened SFX has left to play

        PacmanController controller;
        List<Vector3> directions;

        void Awake()
        {
            directions = new List<Vector3>{ -transform.right, transform.right, -transform.forward, transform.forward };
            meshRenderer = GetComponent<MeshRenderer>();
        }

        IEnumerator Start()
        {
            controller = FindObjectOfType<PacmanController>();
            SetState(ghost.initialState);
            yield return new WaitUntil(() => PacmanController.pacmanControlState);
            yield return Spawn();
        }

        void Update()
        {
            //Execute the ghost's current state
            if (currentGhostState != null)
                currentGhostState.Execute(this);

            if (currentTargetObject != null && transform.position == currentTargetObject.transform.position)
            {
                FindNewDirection();
            }
            
            MoveToTarget(ghost.chaseSpeed);
        }

        void FindNewDirection()
        {
            while (true)
            {
                int rand = UnityEngine.Random.Range(0, directions.Count);

                if (-currentDir != directions[rand] && CheckDirectionValidity(directions[rand], true))
                {
                    currentDir = directions[rand];
                    break;
                }
            }
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<PacmanController>() != null)
            {
                //If ghost is in frightened state, ghost is eaten, otherwise pacman dies
                if (currentGhostState.GetType() == typeof(GhostFrightenedState))
                    controller.StartCoroutine(ConsumeGhost(ghost.eatGhostSFX));
                else
                    controller.StartCoroutine(controller.PacmanDies());
            }
        }

        public IEnumerator Spawn()
        {
            SetState(ghost.initialState);
            meshRenderer.enabled = true;
            yield return new WaitUntil(() => controller.Score >= ghost.releaseAfterScore);
            while (transform.localPosition != constants.respawnCompletePosition)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, constants.respawnCompletePosition, Time.deltaTime * ghost.chaseSpeed);
                if (transform.localPosition == constants.respawnCompletePosition)
                    break;
                yield return null;
            }
            SetState(ghost.chaseState);
            currentDir = -transform.right;
            currentTargetObject = initialTargetObject;
            frightenedLoopCount = 0;
        }

        /// <summary>
        /// Sets the state of the ghosts
        /// </summary>
        /// <param name="state">State to switch ghosts to</param>
        public static void SetState(GhostState state)
        {
            GhostController[] consumable = FindObjectsOfType<GhostController>();

            foreach (GhostController g in consumable)
            {
                state.Init(g);
                g.currentGhostState = state;
            }
        }

        /// <summary>
        /// Pacman consumes a ghost
        /// </summary>
        /// <param name="audioClip">Audioclip to play on consumption</param>
        public IEnumerator ConsumeGhost(AudioClip audioClip)
        {
            transform.localPosition = constants.ghostRespawnPosition;
            currentDir = Vector3.zero;
            currentTargetObject = null;
            meshRenderer.enabled = false;
            PacmanController.pacmanControlState = false;
            AudioManager.musicSource.Pause();
            AudioManager.PlaySFX(audioClip);
            yield return new WaitForSeconds(audioClip.length);
            AudioManager.musicSource.UnPause();
            PacmanController.pacmanControlState = true;
            yield return Spawn();
        }

    }
}