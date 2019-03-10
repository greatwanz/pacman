using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

namespace pacman
{
    /// <summary>
    /// Player controller that controls pacman
    /// </summary>
    public class PacmanController : Controller
    {
        //Whether pacman can be controlled
        public static bool pacmanControlState;
        //Lives pacman has remaining
        [ReadOnlyAttribute]public int lives;
        //Global audio resources
        [AssertNotNull]public AudioResources audioResources;
        //Text on screen to notify player
        [AssertNotNull]public Text notificationText;
        //Score indicator
        [AssertNotNull]public Text scoreText;
        //Sound effect to play when ghost kills pacman
        [AssertNotNull]public AudioClip pacmanDiesSFX;
        //'Wa' sfx
        [AssertNotNull]public AudioClip waSFX;
        //'Ka' sfx
        [AssertNotNull]public AudioClip kaSFX;

        //Move speed of pacman
        public float pacmanSpeed;
        //Is 'Ka' the sound effect currently played?
        public bool isKa;
        //Default speed of pacman
        public int defaultSpeed;
        //Localposition pacman respawns at
        public Vector3 respawnPos;

        public event Action spawnEvent;

        public delegate void PacmanDiesEvent();

        public event PacmanDiesEvent pacmanDiesEvent;

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
            pacmanSpeed = defaultSpeed;
            lives = constants.startingLives;
            //Wait until pacman becomes controllable
            yield return new WaitUntil(() => PacmanController.pacmanControlState);
            //Spawn pacman without waiting
            yield return Spawn(0);
        }

        void Update()
        {
            //Set direction according to arrow key pressed
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                SetDirection(-transform.right);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                SetDirection(transform.right);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                SetDirection(-transform.forward);
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                SetDirection(transform.forward);

            MoveToTarget(pacmanSpeed);
        }

        /// <summary>
        /// Sets direction to move
        /// </summary>
        public override void SetDirection(Vector3 dir)
        {
            queuedDir = dir;
            if (CheckDirectionValidity(dir))
                currentDir = queuedDir;
        }

        /// <summary>
        /// Pacmans dies.
        /// </summary>
        public IEnumerator PacmanDies()
        {
            currentDir = Vector3.zero;
            queuedDir = Vector3.zero;
            currentTargetObject = null;

            PacmanController.pacmanControlState = false;
            //clear direction
            AudioManager.musicSource.Stop();

            yield return new WaitForSeconds(constants.shortDelay);
            AudioManager.PlaySFX(pacmanDiesSFX);
            yield return new WaitForSeconds(pacmanDiesSFX.length);
            yield return new WaitForSeconds(constants.shortDelay);

            //if lives is zero, reduce life count, which causes game to end
            if (lives == 0)
            {
                lives--;
                yield break;
            }

            if (pacmanDiesEvent != null)
                pacmanDiesEvent();

            notificationText.gameObject.SetActive(true);
            notificationText.text = "Ready!";
            notificationText.color = Color.yellow;
            yield return Spawn(1);
        }

        /// <summary>
        /// Spawn pacman
        /// </summary>
        /// <param name="waitTime">Wait a specified amount of time before spawning</param>
        IEnumerator Spawn(float waitTime)
        {
            if (spawnEvent != null)
                spawnEvent();
            
            lives--;
            transform.localPosition = respawnPos;
            yield return new WaitForSeconds(waitTime);

            AudioManager.PlayMusic(audioResources.sirenMusic);
            notificationText.gameObject.SetActive(false);
            PacmanController.pacmanControlState = true;
        }
    }
}