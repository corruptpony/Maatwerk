using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
    public class AssetSnapping : MonoBehaviour
    {
        public float snappingDistance = 0.05f;

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
                if (!mark.isActiveAndEnabled || !mark.GetComponentInChildren<Renderer>().isVisible)
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
                    if (!currentMark.isActiveAndEnabled || !currentMark.GetComponentInChildren<Renderer>().isVisible || currentMark == closestMark)
                        continue;

                    foreach (VuMarkBehaviour mark in marks)
                    {
                        if(mark != currentMark && mark.isActiveAndEnabled && currentMark.GetComponentInChildren<Renderer>().isVisible)
                        {
                            if(Vector3.Distance(currentMark.transform.position, mark.transform.position) < snappingDistance)
                            {

                            }
                        }
                    }
                }
            }


        }
    }
}

