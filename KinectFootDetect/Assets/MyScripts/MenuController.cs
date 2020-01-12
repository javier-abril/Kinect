using Dweiss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour,SpeechRecognitionInterface
{
    private string lastLevel;
    public bool autoLoadLevel=true;
    public float autoLoadTime=20.0f;
    private bool stopTimer=false;
    public Text textTimer;

    // Start is called before the first frame update
    void Start()
    {
        Settings.Instance.LoadToScript();
        if (Settings.Instance.lastLevel != "")
            this.lastLevel = Settings.Instance.lastLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if(autoLoadLevel && !stopTimer)
        {
            textTimer.text = Mathf.Round((autoLoadTime - Time.timeSinceLevelLoad)).ToString();
        }

        if(Time.timeSinceLevelLoad > autoLoadTime && autoLoadLevel && !stopTimer)
        {
            switch (lastLevel)
            {
                case "Goombas":
                    LoadLuigis();
                    break;
                case "Luigis":
                    LoadGoombaRun();
                    break;
                case "GoombasRun":
                    LoadGoomba();
                    break;
                case "FloorCalibration":
                    LoadRandom();
                    break;
                case "Menu":
                    LoadRandom();
                    break;
                default:
                    LoadRandom();
                    break;
            }
        }
    }

    public void LoadCalibration()
    {
        SceneManager.LoadScene("FloorCalibration", LoadSceneMode.Single);
    }

    public void LoadLuigis()
    {
        SceneManager.LoadScene("Luigis", LoadSceneMode.Single);
    }

    public void LoadGoomba()
    {
        SceneManager.LoadScene("Goombas", LoadSceneMode.Single);
    }

    public void LoadGoombaRun()
    {
        SceneManager.LoadScene("GoombasRun", LoadSceneMode.Single);
    }

    public void LoadRandom()
    {
        int rand = Random.Range(2, 100);

        if (rand % 2 == 0)
            LoadGoomba();
        else
        {
            rand = Random.Range(2, 100);
            if (rand % 2 == 0)
                LoadLuigis();
            else
                LoadGoombaRun();
        }
    
    }

    //Evento de la interfaz speech recognition. Lanza el evento cuando detecta una frase.
    public bool SpeechPhraseRecognized(string phraseTag, float condidence)
    {
        switch (phraseTag)
        {
            case "CALIBRATION":
                this.LoadCalibration();
                break;

            case "MENU":
                break;

            case "NEXTGAME":
                break;

            case "STARTGAME": 
                
                break;

            case "FORWARD":
                break;

            case "BACK":
                break;

            case "LEFT":
                break;

            case "RIGHT":
                break;

            case "RUN":
                break;

            case "STOP":
                stopTimer = true;
                break;

            case "JUMP":
                break;

            case "HELLO":
                break;

            case "LUIGI":
                this.LoadLuigis();
                break;

            case "GOOMBA":
                this.LoadGoomba();
                break;
        }

        return true;
    }
}
