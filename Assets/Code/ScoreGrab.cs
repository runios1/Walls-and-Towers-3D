using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class ScoreGrab : MonoBehaviour
{
    private ScoringSystem scoringSystem;
    public TextMeshProUGUI text;
    public String gameStatus;
    // Start is called before the first frame update
    void Start()
    {
        scoringSystem = FindObjectOfType<ScoringSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = gameStatus + "\nScore: " + scoringSystem.getScore().ToString("F2"); 
    }
}
