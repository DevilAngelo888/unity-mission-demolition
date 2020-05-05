using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine Instance;

    [Header("Set In Inspector")]
    public float MinDist = 0.1f;

    private LineRenderer _line;
    private GameObject _poi;
    private List<Vector3> _points;

    private void Awake()
    {
        Instance = this;
        _line = GetComponent<LineRenderer>();
        _line.enabled = false;
        _points = new List<Vector3>();
    }

    public Vector3 LastPoint => _points == null ? Vector3.zero : _points.Last();

    public GameObject POI
    {
        get { return _poi; }
        set
        {
            _poi = value;

            if (_poi != null)
            {
                _line.enabled = false;
                _points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear()
    {
        _poi = null;
        _line.enabled = false;
        _points = new List<Vector3>();
    }

    public void AddPoint()
    {
        var point = _poi.transform.position;

        if (_points.Count > 0 && (point - LastPoint).magnitude < MinDist)
        {
            return;
        }

        if ( _points.Count == 0)
        {
            var launchPosDiff = point - Slingshot.LAUNCH_POS;
            _points.Add(point + launchPosDiff);
            _points.Add(point);
            _line.positionCount = 2;

            _line.SetPosition(0, _points[0]);
            _line.SetPosition(1, _points[1]);

            _line.enabled = true;
        }
        else
        {
            _points.Add(point);
            _line.positionCount = _points.Count;
            _line.SetPosition(_points.Count - 1, LastPoint);

            _line.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (POI == null)
        {
            if (FollowCam.POI != null && FollowCam.POI.tag == "Projectile")
            {
                POI = FollowCam.POI;
            }
            else
            {
                return;
            }
        }

        AddPoint();

        if (FollowCam.POI == null)
        {
            POI = null;
        }
    }
}
