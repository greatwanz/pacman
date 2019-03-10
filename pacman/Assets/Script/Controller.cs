using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace pacman
{
    public class Controller : MonoBehaviour
    {
        //Layermask of collision objects
        public LayerMask layerMask;
        //Global constants
        [AssertNotNull]public Constants constants;
        //Global variables
        [AssertNotNull]public Variables variables;
        //Which object pacman currently hits
        [ReadOnlyAttribute][SerializeField]protected GameObject currentTargetObject;
        //Last direction pacman travels
        [ReadOnlyAttribute][SerializeField]protected Vector3 currentDir;
        //Queued direction pacman travels in when possible
        [ReadOnlyAttribute][SerializeField]protected Vector3 queuedDir;

        protected void MoveToTarget(float speed)
        {
            //Don't allow controls if pacman isn't controllable
            if (!PacmanController.pacmanControlState)
                return;

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
                    transform.position = Vector3.MoveTowards(transform.position, currentTargetObject.transform.position, speed * Time.deltaTime);
                }
            }
        }

        /// <summary>
        /// Sets direction to move
        /// </summary>
        protected void SetDirection(Vector3 dir)
        {
            queuedDir = dir;
            if (CheckDirectionValidity(dir))
                currentDir = queuedDir;
        }

        /// <summary>
        /// Check whether pacman collides with a collider in a particular direction
        /// </summary>
        /// <param name="dir">Direction to check</param>
        /// <param name="getFirstTarget">Whether to get first available target. Default: last target.</param>
        protected virtual bool CheckDirectionValidity(Vector3 dir, bool getFirstTarget = false)
        {
            //Perform a raycast in direction dir, order by distance from pacman
            RaycastHit[] hitArray = Physics.RaycastAll(transform.position, dir, Mathf.Infinity, layerMask).OrderBy(h => h.distance).ToArray();

            Transform targetHit = null;

            //Loop finds furthest target before hitting a wall
            for (int i = 0; i < hitArray.Length; ++i)
            {
                if (hitArray[i].transform.gameObject == currentTargetObject)
                    continue;
                
                //If object hit is in validmove layer, set it as the target
                if (hitArray[i].transform.gameObject.layer == LayerMask.NameToLayer("ValidMove"))
                {
                    targetHit = hitArray[i].transform;
                    if (getFirstTarget)
                        break;
                    else
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