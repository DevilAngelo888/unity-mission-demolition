using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set In Inspector")]
    public int NumClouds = 40;
    public GameObject CloudPrefab;
    public Vector3 CloudPositionMin = new Vector3(-50, -5, 10);
    public Vector3 CloudPositionMax = new Vector3(150, 100, 10);
    public float CloudScaleMin = 1;
    public float CloudScaleMax = 3;
    public float CloudSpeedMult = 0.5f;

    private GameObject[] CloudInstances;

    private void Awake()
    {
        CloudInstances = new GameObject[NumClouds];
        
        var anchor = GameObject.Find("CloudAnchor");
        GameObject cloud;

        for (int i = 0; i < NumClouds; i++)
        {
            cloud = Instantiate(CloudPrefab);
            var cPos = Vector3.zero;
            cPos.x = Random.Range(CloudPositionMin.x, CloudPositionMax.x);
            cPos.y = Random.Range(CloudPositionMin.y, CloudPositionMax.y);

            var scaleLerpKoeff = Random.value;
            var scaleVal = Mathf.Lerp(CloudScaleMin, CloudScaleMax, scaleLerpKoeff);

            cPos.y = Mathf.Lerp(CloudPositionMin.y, cPos.y, scaleLerpKoeff);
            cPos.z = 100 - 90 * scaleLerpKoeff;

            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            cloud.transform.SetParent(anchor.transform);

            CloudInstances[i] = cloud;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var cloud in CloudInstances)
        {
            var scaleVal = cloud.transform.localScale.x;
            var cPos = cloud.transform.position;

            cPos.x -= scaleVal * Time.deltaTime * CloudSpeedMult;

            if (cPos.x <= CloudPositionMin.x)
            {
                cPos.x = CloudPositionMax.x;
            }

            cloud.transform.position = cPos;
        }
    }
}
