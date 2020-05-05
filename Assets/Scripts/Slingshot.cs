using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot Instance;

    [Header("Set in Inspector")]
    public GameObject ProjectilePrefab;
    public float VelocityMult = 8f;

    [Header("Set Dynamically")]
    public GameObject LaunchPoint;
    public Vector3 LaunchPos;
    public GameObject Projectile;
    public bool IsAimingMode;

    private Rigidbody _projectileRigidBody;

    private void Awake()
    {
        Instance = this;
        LaunchPoint = GameObject.Find("LaunchPoint");
        LaunchPoint.SetActive(false);

        LaunchPos = LaunchPoint.transform.position;
    }

    private void OnMouseEnter()
    {
        LaunchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        LaunchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        IsAimingMode = true;
        Projectile = Instantiate(ProjectilePrefab);
        Projectile.transform.position = LaunchPos;
        _projectileRigidBody = Projectile.GetComponent<Rigidbody>();
        _projectileRigidBody.isKinematic = true;
    }

    private void Update()
    {
        if (!IsAimingMode)
        {
            return;
        }

        var mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        var mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);


        var mouseDelta = mousePos3D - LaunchPos;

        var maxMagnitude = this.GetComponent<SphereCollider>().radius;

        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        var projectilePosition = LaunchPos + mouseDelta;

        Projectile.transform.position = projectilePosition;

        if (Input.GetMouseButtonUp(0))
        {
            IsAimingMode = false;
            _projectileRigidBody.isKinematic = false;
            _projectileRigidBody.velocity = -mouseDelta * VelocityMult;
            FollowCam.POI = Projectile;
            Projectile = null;

            MissionDemolition.ShotFired();
            ProjectileLine.Instance.POI = Projectile;
        }
    }

    static public Vector3 LAUNCH_POS => Instance == null ? Vector3.zero : Instance.LaunchPos;
}
