using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Score : MonoBehaviour
{
    Greivin greivinScript;
    public Text scoreText;


    void Start() {
        greivinScript = GameObject.FindObjectOfType(typeof(Greivin)) as Greivin;
    }

    void Update() {
        scoreText.text = greivinScript.scoreText;
    }
}
