using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesText : MonoBehaviour
{

    Text textLives;

    // Start is called before the first frame update
    void Start()
    {
        textLives = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (textLives != null)
            textLives.text = "VIDAS x " + GameController.lives;
    }
}
