using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

namespace pacman
{
    /// <summary>
    /// Player controller that controls pacman
    /// </summary>
    public class PacmanController : MonoBehaviour
    {
        //Which object pacman currently hits
        [ReadOnlyAttribute]public GameObject currentTargetObject;
        //Last direction pacman travels
        [ReadOnlyAttribute]public Vector3 currentDir;
        //Queued direction pacman travels in when possible
        [ReadOnlyAttribute]public Vector3 queuedDir;
        //Lives pacman has remaining
        [ReadOnlyAttribute]public int lives;

        //Global audio resources
        [AssertNotNull]public AudioResources audioResources;
        //Global constants
        [AssertNotNull]public Constants constants;
        //Global variables
        [AssertNotNull]public Variables variables;
        //Text on screen to notify player
        [AssertNotNull]public Text notificationText;
        //Score indicator
        [AssertNotNull]public Text scoreText;
        //Sphere collider to represent pacman
        [AssertNotNull]public SphereCollider sphereCollider;
        //Life indicator prefab
        [AssertNotNull]public Image lifeIndicator;
        //Transform of life indicators
        [AssertNotNull]public Transform livesTransform;

        //Character controller to control pacman

        //Distance to detect collisions
        public float collisionDistance;
        //Layermask of collision objects
        public LayerMask layerMask;
        //Is 'Ka' the sound effect currently played?
        public bool isKa;

        //Property to update score
        public int Score
        {
            set
            { 
                scoreBacking = value;
                //Update score text
                scoreText.text = "High Score\n" + scoreBacking;
            }
            get
            {
                return scoreBacking;
            }
        }

        //backing field of score
        int scoreBacking;

        IEnumerator Start()
        {
            variables.pacmanControlState = false;
            //Instantiate lives
            lives = constants.startingLives;
            for (int i = 0; i < lives; ++i)
            {
                Instantiate(lifeIndicator, livesTransform);
            }

            //Wait until pacman becomes controllable
            yield return new WaitUntil(() => variables.pacmanControlState);
            //Spawn pacman without waiting
            yield return Spawn(0);
        }

        void Update()
        {
            //Don't allow controls if pacman isn't controllable
            if (!variables.pacmanControlState)
                return;

            //Set direction according to arrow key pressed
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                SetDirection(-transform.right);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                SetDirection(transform.right);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                SetDirection(-transform.forward);
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                SetDirection(transform.forward);

            //If current direction pacman is travelling is not the queued direction, and the queued direction is valid
            //Set current direction to queued direction
            if (currentDir != queuedDir && CheckDirectionValidity(queuedDir))
            {
                currentDir = queuedDir;
            }
            else
            {
                //Move in the current direction towards target
                if (currentTargetObject != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentTargetObject.transform.position, variables.pacmanSpeed * Time.deltaTime);
                }
            }
        }

        /// <summary>
        /// Sets direction to move
        /// </summary>
        public void SetDirection(Vector3 dir)
        {
            queuedDir = dir;
            if (CheckDirectionValidity(dir))
            {
                currentDir = queuedDir;
            }
        }

        /// <summary>
        /// Spawn pacman
        /// </summary>
        /// <param name="waitTime">Wait a specified amount of time before spawning</param>
        public IEnumerator Spawn(float waitTime)
        {
            currentDir = Vector3.zero;
            queuedDir = Vector3.zero;
            currentTargetObject = null;
            Destroy(livesTransform.GetChild(lives - 1).gameObject);
            lives--;
            transform.localPosition = constants.pacmanRespawnPosition;
            yield return new WaitForSeconds(waitTime);
            notificationText.gameObject.SetActive(false);
            variables.pacmanControlState = true;
            AudioManager.PlayMusic(audioResources.sirenMusic);
        }

        /// <summary>
        /// Pacmans dies.
        /// </summary>
        /// <param name="audioClip">Audio clip to play when pacman dies</param>
        public IEnumerator PacmanDies(AudioClip audioClip)
        {
            variables.pacmanControlState = false;
            //clear direction
            AudioManager.musicSource.Stop();
            yield return new WaitForSeconds(constants.shortDelay);
            AudioManager.PlaySFX(audioClip);
            yield return new WaitForSeconds(audioClip.length);
            yield return new WaitForSeconds(constants.shortDelay);

            //if lives is zero, reduce life count, which causes game to end
            if (lives == 0)
            {
                lives--;
                yield break;
            }

            notificationText.gameObject.SetActive(true);
            notificationText.text = "Ready!";
            notificationText.color = Color.yellow;
            yield return Spawn(1);
        }

        /// <summary>
        /// Pacman consumes a ghost
        /// </summary>
        /// <param name="g">Ghost consumed</param>
        /// <param name="audioClip">Audioclip to play on consumption</param>
        public IEnumerator ConsumeGhost(Ghost g, AudioClip audioClip)
        {
            variables.pacmanControlState = false;
            g.transform.localPosition = constants.ghostRespawnPosition;
            AudioManager.musicSource.Pause();
            AudioManager.PlaySFX(audioClip);
            yield return new WaitForSeconds(audioClip.length);
            AudioManager.musicSource.UnPause();
            variables.pacmanControlState = true;
        }

        /// <summary>
        /// Check whether pacman collides with a collider in a particular direction
        /// </summary>
        /// <param name="dir">Direction to checl</param>
        bool CheckDirectionValidity(Vector3 dir)
        {
            //Perform a raycast in direction dir, order by distance from pacman
            RaycastHit[] hitArray = Physics.RaycastAll(transform.position, dir, Mathf.Infinity, layerMask).OrderBy(h => h.distance).ToArray();

            Transform targetHit = null;

            //Loop finds furthest target before hitting a wall
            for (int i = 0; i < hitArray.Length; ++i)
            {
                //If object hit is in validmove layer, set it as the target
                if (hitArray[i].transform.gameObject.layer == LayerMask.NameToLayer("ValidMove"))
                {
                    targetHit = hitArray[i].transform;
                    continue;
                }

                //End loop if raycast hits wall
                if (hitArray[i].transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    break;
            }

            //If a target transform was hit, set it as the target object pacman moves towards
            if (targetHit != null)
            {
                currentTargetObject = targetHit.gameObject;
                return true;
            }

            return false;
        }
    }
}