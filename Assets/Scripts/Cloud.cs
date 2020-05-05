using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Set In Inspector")]
    public GameObject CloudSphere;
    public int NumSpheresMin = 6;
    public int NumSpheresMax = 10;
    public Vector3 SphereOffsetScale = new Vector3(5, 2, 1);
    public Vector2 SphereScaleRangeX = new Vector2(4, 8);
    public Vector2 SphereScaleRangeY = new Vector2(3, 4);
    public Vector2 SphereScaleRangeZ = new Vector2(2, 4);
    public float ScaleYMin = 2f;

    private List<GameObject> Spheres;


    // Start is called before the first frame update
    void Start()
    {
        Spheres = new List<GameObject>();

        var num = Random.Range(NumSpheresMin, NumSpheresMax);

        for (var i = 0; i < num; i++)
        {
            var cloudSphere = Instantiate(CloudSphere);
            Spheres.Add(cloudSphere);

            var cloudTransform = cloudSphere.transform;
            cloudTransform.SetParent(this.transform);

            var offset = Random.insideUnitSphere;

            offset.x *= SphereOffsetScale.x;
            offset.y *= SphereOffsetScale.y;
            offset.z *= SphereOffsetScale.z;
            cloudTransform.localPosition = offset;

            var scale = Vector3.one;
            scale.x = Random.Range(SphereScaleRangeX.x, SphereScaleRangeX.y);
            scale.y = Random.Range(SphereScaleRangeY.x, SphereScaleRangeY.y);
            scale.z = Random.Range(SphereScaleRangeZ.x, SphereScaleRangeZ.y);

            scale.y *= 1 - (Mathf.Abs(offset.x) / SphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, ScaleYMin);

            cloudTransform.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
