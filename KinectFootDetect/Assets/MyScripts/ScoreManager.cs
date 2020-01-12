using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager: MonoBehaviour
{

    public static int P1Score;
    public static int P2Score;
    public static int P3Score;
    public static int P4Score;

    void Start()
    {
        P1Score = 0;
        P2Score = 0;
        P3Score = 0;
        P4Score = 0;
    }


    public static void AddPointsP1(int points)
    {
        P1Score = P1Score + points;
    }

    public static void AddPointsP2(int points)
    {
        P2Score = P2Score + points;
    }

    public static void AddPointsP3(int points)
    {
        P3Score = P3Score + points;
    }

    public static void AddPointsP4(int points)
    {
        P4Score = P4Score + points;
    }

}
