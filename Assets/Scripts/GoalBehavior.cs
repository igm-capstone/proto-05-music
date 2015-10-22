using UnityEngine;
using System.Collections;

public class GoalBehavior : MonoBehaviour
{
    public Transform NxtLvlStartPoint;
    //public AnimationCurve MoveCurve;

    Vector2 DashPosition;

    public Material NxtPlyrMat;


    // Use this for initialization
    void Start()
    {
        DashPosition = new Vector2(NxtLvlStartPoint.position.x, NxtLvlStartPoint.position.y);
    }


    public void GoToNextLevel(GameObject PlyrObj)
    {
        // Change Material
        Renderer PlyrRend = PlyrObj.GetComponentInChildren<Renderer>();
        PlyrRend.material = NxtPlyrMat;

        // Go to Next Level.
        PlyrObj.GetComponent<PlayerController>().DashTo(DashPosition, 0.5f, null);
    }
}


