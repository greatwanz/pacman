using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

namespace pacman
{
    /// <summary>
    /// Player controller that controls pacman
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PacmanController : MonoBehaviour
    {
        //Which object pacman currently hits
        [ReadOnlyAttribute]public GameObject currentHitObject;
        //Last direction pacman travels
        [ReadOnlyAttribute]public Vector3 lastDir;
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
        [NonSerialized]public CharacterController controller;

        //Move speed of pacman
        public float thrust;
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
                scoreText.text = "High Score\n" + scoreBacking;
            }
            get
            {
                return scoreBacking;
            }
        }

        //backing field of score
        int scoreBacking;
        //distance of object currently hit
        float currentHitDistance;

        IEnumerator Start()
        {
            variables.pacmanControlState = false;
            controller = GetComponent<CharacterController>();
            //Instantiate lives
            lives = constants.startingLives;
            for (int i = 0; i < lives; ++i)
            {
                Instantiate(lifeIndicator, livesTransform);
            }
            AudioManager.PlaySFX(audioResources.introMusic);
            notificationText.gameObject.SetActive(true);
            notificationText.text = "Ready!";
            notificationText.color = Color.yellow;
            yield return new WaitForSeconds(audioResources.introMusic.length);
            //Spaawn pacman without waiting
            yield return Spawn(0);
            AudioManager.PlayMusic(audioResources.sirenMusic);
            //enable controls
            variables.pacmanControlState = true;
        }

        void FixedUpdate()
        {
            //Don't allow controls if pacman isn't controllable
            if (!variables.pacmanControlState)
                return;
        
            SetDirection();
            //Move in the last specified direction if one is specified and it doesn't cause a collision
            if (lastDir != Vector3.zero && !Collision(lastDir, collisionDistance))
            {
                controller.SimpleMove(lastDir * thrust);
            }
        }

        /// <summary>
        /// Sets direction to move
        /// </summary>
        public void SetDirection()
        {
            if (Input.GetKey(KeyCode.LeftArrow) && !Collision(-transform.right, collisionDistance))
                lastDir = -transform.right;
            else if (Input.GetKey(KeyCode.RightArrow) && !Collision(transform.right, collisionDistance))
                lastDir = transform.right;
            else if (Input.GetKey(KeyCode.DownArrow) && !Collision(-transform.forward, collisionDistance))
                lastDir = -transform.forward;
            else if (Input.GetKey(KeyCode.UpArrow) && !Collision(transform.forward, collisionDistance))
                lastDir = transform.forward;
        }

        /// <summary>
        /// Spawn pacman
        /// </summary>
        /// <param name="waitTime">Wait a specified amount of time before spawning</param>
        public IEnumerator Spawn(float waitTime)
        {
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
            //clear last direction
            lastDir = Vector3.zero;
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
        /// <param name="distance">Distance to checl</param>
        bool Collision(Vector3 dir, float distance)
        {
            RaycastHit hit;
            //Perform a sperecast to check for collision
            if (Physics.SphereCast(transform.position, sphereCollider.radius, dir, out hit, distance, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                currentHitObject = hit.transform.gameObject;
                currentHitDistance = hit.distance;
                return true;
            }
            else
            {
                currentHitDistance = distance;
                currentHitObject = null;
                return false;
            }
        }

        /// <summary>
        /// Raises the draw gizmos selected event. Draws outline of spherecast.
        /// </summary>
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Debug.DrawLine(transform.position, transform.position + lastDir * currentHitDistance);
            Gizmos.DrawWireSphere(transform.position + lastDir * currentHitDistance, sphereCollider.radius);
        }
    }
}