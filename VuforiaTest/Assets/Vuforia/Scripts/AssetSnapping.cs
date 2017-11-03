using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
    public class AssetSnapping : MonoBehaviour
    {
        public float snappingDistance = 0.05f;
        public float rotationStepSize = 90;

        // Update is called once per frame
        void Update()
        {
            // Get all VuMark instances in the scene
            VuMarkBehaviour[] marks = FindObjectsOfType<VuMarkBehaviour>();

            // Get VuMark closest to centre of the scene
            float shortestDistance = Mathf.Infinity;
            VuMarkBehaviour closestMark = null;

            foreach (VuMarkBehaviour mark in marks)
            {
                //Check for active childs
                bool activeChild = false;
                foreach(Renderer rend in mark.GetComponentsInChildren<Renderer>())
                {
                    if(rend.isVisible)
                    {
                        activeChild = true;
                    }
                }

                //Continue to next child if no active marker is found
                if (!mark.isActiveAndEnabled || !activeChild)
                    continue;

                float distanceToCentre = Vector3.Distance(mark.transform.position, new Vector3(0, 0, 0));

                if (distanceToCentre < shortestDistance)
                {
                    shortestDistance = distanceToCentre;
                    closestMark = mark;
                }
            }

            // Only run the rest of the code if an active marker was found
            if (closestMark != null)
            {
                // Move closest marker to centre of the scene
                closestMark.transform.position = new Vector3(0, 0, 0);
                closestMark.transform.eulerAngles = new Vector3(0, 0, 0);
                closestMark.snapped = true;

                // Set child back to original position
                Transform[] transforms = GetComponentsInChildren<Transform>();
                foreach (Transform tf in transforms)
                {
                    if (tf.tag == closestMark.VuMarkTarget.InstanceId.StringValue)
                    {
                        tf.localPosition = new Vector3(0.5f, 0, -0.5f);
                    }
                }

                // For each mark in the scene, look for active markers surrounding it
                foreach (VuMarkBehaviour currentMark in marks)
                {
                    //Check for active childs
                    bool activeChild = false;
                    foreach (Renderer rend in currentMark.GetComponentsInChildren<Renderer>())
                    {
                        if (rend.isVisible)
                        {
                            activeChild = true;
                        }
                    }

                    // Skip middle marker and inactive markers
                    if (!currentMark.isActiveAndEnabled || !activeChild || currentMark.snapped)
                        continue;

                    foreach (VuMarkBehaviour mark in marks)
                    {
                        // Skip comparing with yourself and inactive markers
                        if(mark != currentMark && mark.isActiveAndEnabled && !currentMark.snapped && mark.snapped)
                        {
                            // If another marker is found nearby, snap this marker to it
                            if(Vector3.Distance(currentMark.transform.position, mark.transform.position) < snappingDistance)
                            {
                                // Determine on which side the marker needs to snap
                                Vector3 snappingPos = determineSnappingPosition(currentMark.transform.position, mark.transform.position);

                                // Copy calculated x and z coordinates, for the snapping position, keep y at 0
                                currentMark.transform.position = new Vector3(snappingPos.x, 0, snappingPos.z);

                                // Copy the x and z angles from the marker we are snapping to, take the rotation of the marker itself.
                                currentMark.transform.eulerAngles = new Vector3(mark.transform.eulerAngles.x,
                                                                                determineYRotation(currentMark.transform.eulerAngles.y), 
                                                                                mark.transform.eulerAngles.z);

                                currentMark.snapped = true;
                            }
                        }
                    }
                }

                // Reset snapped boolean
                foreach(VuMarkBehaviour mark in marks)
                {
                    mark.snapped = false;
                }
            }
        }

        /* Calculate the nearest snapping location */
        Vector3 determineSnappingPosition(Vector3 currentMark, Vector3 mark)
        {
            // Calculate distance to each snappingpoint
            float dist1 = Vector3.Distance(currentMark, mark + new Vector3(0.04f, 0, 0));
            float dist2 = Vector3.Distance(currentMark, mark + new Vector3(-0.04f, 0, 0));
            float dist3 = Vector3.Distance(currentMark, mark + new Vector3(0, 0, 0.04f));
            float dist4 = Vector3.Distance(currentMark, mark + new Vector3(0, 0, -0.04f));

            // Determine which is closest and return that location
            if(dist1 < dist2 && dist1 < dist3 && dist1 < dist4)
            {
                return mark + new Vector3(0.04f, 0, 0);
            }
            else if(dist2 < dist1 && dist2 < dist3 && dist2 < dist4)
            {
                return mark + new Vector3(-0.04f, 0, 0);
            }
            else if(dist3 < dist1 && dist3 < dist2 && dist3 < dist4)
            {
                return mark + new Vector3(0, 0, 0.04f);
            }
            else
            {
                return mark + new Vector3(0, 0, -0.04f);
            }
        }

        /* Set the Y rotation to the nearest multiple of rotationStepSize between 0 and 359 */
        float determineYRotation(float rot)
        {
            // Work with positive rotations only
            while (rot < 0f)
                rot += 360;

            // Work with whole numbers only
            rot = Mathf.Round(rot);

            // Get 0ffset
            float offset = rot % rotationStepSize;

            // Round down to nearest multiple of rotationStepSize
            rot = rot - (rot % rotationStepSize);

            // an offset larger than half the rotationStepSize neans it should have been rounded up instead
            if (offset > rotationStepSize / 2)
                rot += rotationStepSize;

            // Make rotation between 0 and 359
            while (rot >= 360)
                rot -= 360;

            return rot;
        }

    }
}

