using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace pacman
{
    /// <summary>
    /// Ghost controller that controls the behaviour of a ghost
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class GhostController : Controller
    {
        //Frightened loop count value the ghosts are at
        public static int frightenedLoopCount;

        //Default target object ghost moves towards
        [AssertNotNull]public GameObject defaultTargetObject;
        //Number of loops the frightened SFX has left to play
        [AssertNotNull]public Ghost ghost;
        //Current state of the ghost
        [ReadOnlyAttribute]public GhostState currentGhostState;
        //MeshRenderer of the ghost
        [NonSerialized]public MeshRenderer meshRenderer;

        //Where the ghost respawns
        Vector3 ghostRespawnPosition;
        //Reference to pacman controller
        PacmanController controller;
        //List of valid directions
        List<Vector3> directions;

        void Awake()
        {
            directions = new List<Vector3>{ -transform.right, transform.right, -transform.forward, transform.forward };
            meshRenderer = GetComponent<MeshRenderer>();
            controller = FindObjectOfType<PacmanController>();
        }

        IEnumerator Start()
        {
            ghostRespawnPosition = transform.localPosition;
            currentDir = Vector3.zero;
            currentTargetObject = defaultTargetObject;
            controller.pacmanDiesEvent += RespawnGhost;
            SetState(ghost.initialState);
            yield return new WaitUntil(() => PacmanController.pacmanControlState);
            yield return Spawn(ghostRespawnPosition);
        }

        void Update()
        {
            //Execute the ghost's current state
            if (currentGhostState != null)
                currentGhostState.Execute(this);
        }

        /// <summary>
        /// Sets direction to move
        /// </summary>
        public override void SetDirection(Vector3 dir)
        {
            if (CheckDirectionValidity(dir))
                currentDir = dir;
        }

        /// <summary>
        /// Finds a new directions for the ghost
        /// </summary>
        public void FindNewDirection()
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

        /// <summary>
        /// Sets the state of the ghosts
        /// </summary>
        /// <param name="state">State to switch ghosts to</param>
        public void SetState(GhostState state)
        {
            if (currentGhostState == state)
                return;
            currentGhostState = state;
            state.Init(this);
        }

        /// <summary>
        /// Raises the trigger enter event.
        /// </summary>
        /// <param name="col">Collider ghost collided with</param>
        void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<PacmanController>() != null)
            {
                //If ghost is in frightened state, ghost is eaten, otherwise pacman dies
                if (currentGhostState.GetType() == typeof(GhostFrightenedState))
                    controller.StartCoroutine(GhostEaten(ghost.eatGhostSFX));
                else
                    controller.StartCoroutine(controller.PacmanDies());
            }
        }

        /// <summary>
        /// Respawns the ghost
        /// </summary>
        void RespawnGhost()
        {
            StartCoroutine(Spawn(ghostRespawnPosition));
        }

        /// <summary>
        /// Coroutine that spawns the ghost
        /// </summary>
        /// <param name="respawnPos">Respawns ghost at position.</param>
        IEnumerator Spawn(Vector3 respawnPos)
        {
            SetState(ghost.initialState);
            transform.localPosition = respawnPos;
            currentDir = Vector3.zero;
            currentTargetObject = defaultTargetObject;
            meshRenderer.enabled = true;
            yield return new WaitUntil(() => controller.Score >= ghost.releaseAfterScore && PacmanController.pacmanControlState);
            while (transform.localPosition != constants.respawnCompletePosition)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, constants.respawnCompletePosition, Time.deltaTime * ghost.chaseSpeed);
                if (transform.localPosition == constants.respawnCompletePosition)
                    break;
                yield return null;
            }
            SetState(ghost.chaseState);
            FindNewDirection();
        }

        /// <summary>
        /// Pacman consumes a ghost
        /// </summary>
        /// <param name="audioClip">Audioclip to play on consumption</param>
        IEnumerator GhostEaten(AudioClip audioClip)
        {
            controller.Score += constants.ghostEatenValue;

            transform.localPosition = ghostRespawnPosition;
            currentDir = Vector3.zero;
            currentTargetObject = null;
            meshRenderer.enabled = false;
            PacmanController.pacmanControlState = false;

            AudioManager.musicSource.Pause();
            AudioManager.PlaySFX(audioClip);
            yield return new WaitForSeconds(audioClip.length);
            AudioManager.musicSource.UnPause();

            PacmanController.pacmanControlState = true;
            yield return Spawn(ghostRespawnPosition);
        }
    }
}