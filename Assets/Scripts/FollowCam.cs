using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;

    [Header("Set In Inspector")]
    public float Easing = 0.05f;
    public Vector2 MinXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float CamZ;

    private void Awake()
    {
        CamZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        Vector3 destination;

        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position;

            if (POI.tag == "Projectile" && POI.GetComponent<Rigidbody>().IsSleeping())
            {
                POI = null; 
                return;
            }
        }

        destination.x = Mathf.Max(destination.x, MinXY.x);
        destination.y = Mathf.Max(destination.y, MinXY.y);

        destination = Vector3.Lerp(this.transform.position, destination, Easing);

        destination.z = CamZ;

        transform.position = destination;

        Camera.main.orthographicSize = destination.y + 10;
    }
}
