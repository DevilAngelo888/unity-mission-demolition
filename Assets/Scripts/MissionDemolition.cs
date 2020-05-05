using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    Idle,
    Playing,
    LevelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition Instance;

    [Header("Set In Inspector")]
    public Text UITLevel;
    public Text UITShots;
    public Text UITButton;

    public Vector3 CastlePos;
    public List<GameObject> Castles;

    [Header("Set Dynamically")]
    public int Level;
    public int LevelMax;
    public int ShotsTaken;
    public GameObject Castle;
    public GameMode Mode = GameMode.Idle;
    public string Showing = "Show Slingshot";


    private void Start()
    {
        Instance = this;

        Level = 0;
        LevelMax = Castles.Count;

        StartLevel();
    }

    private void StartLevel()
    {
        if (Castle != null)
        {
            Destroy(Castle);
        }

        var oldProjectiles = GameObject.FindGameObjectsWithTag("Projectile");

        foreach (var projectile in oldProjectiles)
        {
            Destroy(projectile);
        }

        Castle = Instantiate(Castles[Level]);
        Castle.transform.position = CastlePos;

        ShotsTaken = 0;

        SwitchView("Show Both");

        ProjectileLine.Instance.Clear();

        Goal.GoalMet = false;

        UpdateGUI();

        Mode = GameMode.Playing;
    }

    void UpdateGUI()
    {
        UITLevel.text = $"Level: {Level + 1} of {LevelMax}";
        UITShots.text = $"Shots taken: {ShotsTaken}";
    }

    private void Update()
    {
        UpdateGUI();

        if (Goal.GoalMet && Mode == GameMode.Playing)
        {
            Mode = GameMode.LevelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        Level++;

        if (Level == LevelMax)
        {
            Level = 0;
        }

        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if (string.IsNullOrEmpty(eView))
        {
            eView = UITButton.text;
        }

        Showing = eView;

        switch (Showing)
        {
            case "Show Slingshot":
            {
                FollowCam.POI = null;
                UITButton.text = "Show Castle";
                break;
            }
            case "Show Castle":
            {
                FollowCam.POI = Instance.Castle;
                UITButton.text = "Show Both";
                break;
            }
            case "Show Both":
            {
                FollowCam.POI = GameObject.Find("ViewBoth");
                UITButton.text = "Show Slingshot";
                break;
            }
        }
    }

    public static void ShotFired()
    {
        Instance.ShotsTaken++;
    }
}
