using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool GoalMet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Projectile") return;

        Goal.GoalMet = true;
        var material = GetComponent<Renderer>().material;

        var color = material.color;
        color.a = 1;
        material.color = color;
    }
}
