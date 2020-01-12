using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dweiss;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, InteractionListenerInterface, SpeechRecognitionInterface
{
    private long playerUserID;
    private Vector3 rightFootPos;
    private Vector3 leftFootPos;
    private Vector3 rightFootScreenPos;
    private InteractionManager interactionManager;
    private KinectManager kinectManager;
    //private double factorTamanioPies = 8;
    


    [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
    public int playerIndex = 0;
    public Vector3 puntoCalibracion1;
    public Vector3 puntoCalibracion2;
    public Vector3 puntoCalibracion3;
    public Vector3 puntoCalibracion4;
    public bool calibracionConfirmada = false;
    public bool invertirEjeX = false;
    public bool invertirEjeY = false;
    public Text textCalib1;
    public Text textCalib2;
    public Text textCalib3;
    public Text textCalib4;
    public Text infoText;
    public Image playerFootLeft;
    public Image playerFootRight;
    public float footSize = 128.0f;
    public Camera camera;



    // Start is called before the first frame update
    void Start()
    {
        //get the interaction manager instance
        if (interactionManager == null)
        {
            interactionManager = InteractionManager.Instance;
        }

        //get the kinect manager instance
        if (kinectManager == null)
        {
            kinectManager = KinectManager.Instance;
        }

        //Si existe una calibración guardada la cargamos
        Settings.Instance.LoadToScript();
        if (Settings.Instance.punto1 != new Vector3(0, 0, 0))
        {
            puntoCalibracion1 = Settings.Instance.punto1;
            puntoCalibracion2 = Settings.Instance.punto2;
            puntoCalibracion3 = Settings.Instance.punto3;
            puntoCalibracion4 = Settings.Instance.punto4;

            if (textCalib1 != null && textCalib2 != null && textCalib3 != null && textCalib4 != null)
            {
                textCalib1.text = "Calibration point 1:" + puntoCalibracion1.ToString();
                textCalib2.text = "Calibration point 2:" + puntoCalibracion2.ToString();
                textCalib3.text = "Calibration point 3:" + puntoCalibracion3.ToString();
                textCalib4.text = "Calibration point 4:" + puntoCalibracion4.ToString();
            }

            calibracionConfirmada = true;

            //factorTamanioPies = 20 * DistanciaEntre2Puntos(puntoCalibracion1, puntoCalibracion2) / 1;

            Settings.Instance.lastLevel = SceneManager.GetActiveScene().name;
            Settings.Instance.SaveToFile();

            if (infoText != null)
                infoText.text = "Calibración cargada correctamente";
        }
        else
        {
            if (infoText != null)
                infoText.text = "Debe calibrar los 4 puntos en la pantalla";
        }

        if(playerFootLeft != null && playerFootRight != null)
        {
            playerFootRight.rectTransform.sizeDelta = new Vector2(footSize,footSize);
            playerFootLeft.rectTransform.sizeDelta = new Vector2(footSize, footSize);
        }
    }

    // Update is called once per frame
    void Update()
    {

        // update Kinect interaction
        if (kinectManager!=null && kinectManager.IsInitialized() &&
            interactionManager != null && interactionManager.IsInteractionInited())
        {
            
            playerUserID = kinectManager.GetUserIdByIndex(playerIndex);

            if (playerUserID != 0)
            {

                rightFootPos = kinectManager.GetJointPosition(playerUserID, (int)KinectInterop.JointType.FootRight);
                leftFootPos = kinectManager.GetJointPosition(playerUserID, (int)KinectInterop.JointType.FootLeft);

                //Debug.Log(rightFootPos);
                //Debug.Log(leftFootPos);

                if (calibracionConfirmada)
                {
                    //Primero miramos si está dentro del cuadro el pie derecho
                    bool pieDentro = EstaDentroDelCuadro(rightFootPos);

                    if (infoText != null)
                        infoText.text = "Pie derecho dentro del cuadro: " + pieDentro;

                    if (pieDentro)
                    {
                        //x=>ancho y=>alto  z=0 (no se usa)
                        Vector3 coordenadasPantalla = GetPosicionEnPantalla(rightFootPos);
                        //Asignamos la posición a la imagen del pie
                        playerFootRight.rectTransform.anchoredPosition3D = coordenadasPantalla;
                    }

                    //Despues miramos el izquierdo
                    pieDentro = EstaDentroDelCuadro(leftFootPos);

                    if (infoText != null)
                        infoText.text = infoText.text + "\nPie izquierdo dentro del cuadro: " + pieDentro;

                    if (pieDentro)
                    {
                        //x=>ancho y=>alto  z=0 (no se usa)
                        Vector3 coordenadasPantalla = GetPosicionEnPantalla(leftFootPos);
                        //Asignamos la posición a la imagen del pie
                        playerFootLeft.rectTransform.anchoredPosition3D = coordenadasPantalla;
    
                        
                    }

                    //if (camera != null)
                       // CheckCollision(Vector3.zero);
                }

            }
        }
    }

    public void CheckCollision(Vector3 posicionPie)
    {
        RaycastHit hit;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        //Ray ray = camera.ScreenPointToRay(posicionPie);

        Debug.Log(Input.mousePosition);

        Debug.DrawLine(Vector3.zero, Input.mousePosition, Color.red);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            Debug.Log("Colision detectada");
            // Do something with the object that was hit by the raycast.
        }
    }

    public void HandGripDetected(long userId, int userIndex, bool isRightHand, bool isHandInteracting, Vector3 handScreenPos)
    {
        if (!isHandInteracting || !interactionManager)
            return;
        if (userId != interactionManager.GetUserID())
            return;

        if (isRightHand)
        {
            if (puntoCalibracion1 == new Vector3(0, 0, 0))
                puntoCalibracion1 = rightFootPos;
            else if (puntoCalibracion2 == new Vector3(0, 0, 0))
                puntoCalibracion2 = rightFootPos;
            else if (puntoCalibracion3 == new Vector3(0, 0, 0))
                puntoCalibracion3 = rightFootPos;
            else if (puntoCalibracion4 == new Vector3(0, 0, 0))
                puntoCalibracion4 = rightFootPos;
            else//Si tenemos todos rellenos y cerramos el puño de nuevo limpiamos
            {
                if (!calibracionConfirmada)
                {
                    puntoCalibracion1 = new Vector3(0, 0, 0);
                    puntoCalibracion2 = new Vector3(0, 0, 0);
                    puntoCalibracion3 = new Vector3(0, 0, 0);
                    puntoCalibracion4 = new Vector3(0, 0, 0);
                }
               
            }

            if (textCalib1 != null && textCalib2 != null && textCalib3 != null && textCalib4 != null)
            {
                textCalib1.text = "Calibration point 1:" + puntoCalibracion1.ToString();
                textCalib2.text = "Calibration point 2:" + puntoCalibracion2.ToString();
                textCalib3.text = "Calibration point 3:" + puntoCalibracion3.ToString();
                textCalib4.text = "Calibration point 4:" + puntoCalibracion4.ToString();
            }
        }
    }

    public void HandReleaseDetected(long userId, int userIndex, bool isRightHand, bool isHandInteracting, Vector3 handScreenPos)
    {
        return;
    }

    public bool HandClickDetected(long userId, int userIndex, bool isRightHand, Vector3 handScreenPos)
    {
        if (puntoCalibracion1 != new Vector3(0, 0, 0) &&
                puntoCalibracion2 != new Vector3(0, 0, 0) &&
                puntoCalibracion3 != new Vector3(0, 0, 0) &&
                puntoCalibracion4 != new Vector3(0, 0, 0))
        {
            if (infoText != null)
                infoText.text = "Calibración confirmada";

            calibracionConfirmada = true;
            GuardarCalibracion();
            return true;
        }
        else
        {
            if (infoText != null)
                infoText.text = "Debe calibrar los 4 puntos en la pantalla";

            return false;
        }
        
    }

    private void GuardarCalibracion()
    {
        Settings.Instance.punto1 = puntoCalibracion1;
        Settings.Instance.punto2 = puntoCalibracion2;
        Settings.Instance.punto3 = puntoCalibracion3;
        Settings.Instance.punto4 = puntoCalibracion4;
        Settings.Instance.SaveToFile();
    }

    public bool HandPressDetected(long userId, int userIndex, bool isRightHand, Vector3 handScreenPos)
    {
        return true;
    }

    private double DistanciaEntre2Puntos(Vector3 p1, Vector3 p2)
    {
        return Mathf.Sqrt(
            (p1.x-p2.x)* (p1.x - p2.x)+
            (p1.z - p2.z) * (p1.z - p2.z)
            );
    }

    private Vector3 GetPosicionEnPantalla(Vector3 pie)
    {
        double distanciaAncho = DistanciaEntre2Puntos(puntoCalibracion1, puntoCalibracion2);
        double distanciaAlto = DistanciaEntre2Puntos(puntoCalibracion1, puntoCalibracion4);

        double posiPixelAlto;
        double posiPixelAncho;

        //Usamos una regla de tres para convertir la proporción del cuadro calibrado con la pantalla

        //Se refiere al eje X de la pantalla. Si invertimos cambiamos el punto desde el que 
        //tomamos la referencia por el opuesto, para el caso del 1 es el 2 para el ancho 
        if (invertirEjeX)
            posiPixelAncho = Screen.width * (pie.x - puntoCalibracion2.x) / distanciaAncho;
        else
            posiPixelAncho = Screen.width * (pie.x - puntoCalibracion1.x) / distanciaAncho;


        //Se refiere al eje Y de la pantalla. Si invertimos cambiamos el punto desde el que 
        //tomamos la referencia por el opuesto, para el caso del 1 es el 4 para el alto
        if (invertirEjeY)
        {
            //Al invertir tenemos que poner los valores negativos
            posiPixelAlto = -Mathf.Abs(Screen.height * (pie.z - puntoCalibracion4.z) / (float)distanciaAlto);
        }
        else
            posiPixelAlto = Screen.height * (pie.z - puntoCalibracion1.z) / distanciaAlto;


        //Debug.Log(posiPixelAlto);


        Vector3 vRetorno = new Vector3();

        //Para la x usamos el valor absoluto
        vRetorno.x = Mathf.Abs((float)posiPixelAncho);
        vRetorno.y = (float)posiPixelAlto;
        vRetorno.z = 0;

        return vRetorno;
    }

    private bool EstaDentroDelCuadro(Vector3 pie)
    {

        //Para saber si el punto está dentro del cuadrado hacemos lo siguiente:
        
        //Deje P(x,y), and rectangle A(x1,y1),B(x2,y2),C(x3,y3),D(x4,y4)
        //Calcular la suma de las áreas de △APD,△DPC,△CPB,△PBA.
        //Si la suma de las areas es mayor el punto está fuera.
        //Fuente: https://www.i-ciencias.com/pregunta/4657/como-comprobar-si-un-punto-esta-dentro-de-un-rectangulo

        double areaTriangulo1 = AreaDeTriangulo(puntoCalibracion1, pie, puntoCalibracion4);
        double areaTriangulo2 = AreaDeTriangulo(puntoCalibracion3, pie, puntoCalibracion4);
        double areaTriangulo3 = AreaDeTriangulo(puntoCalibracion2, pie, puntoCalibracion3);
        double areaTriangulo4 = AreaDeTriangulo(pie, puntoCalibracion1, puntoCalibracion2);

        double sumaAreas = areaTriangulo1 + areaTriangulo2 + areaTriangulo3 + areaTriangulo4;

        //El area del rectangulo es base * altura. Medimos la distancia entre los puntos de base y de la altura
        double areaRectanguloCalibrado = DistanciaEntre2Puntos(puntoCalibracion1, puntoCalibracion2) * DistanciaEntre2Puntos(puntoCalibracion1, puntoCalibracion4);

        if (sumaAreas > areaRectanguloCalibrado)
            return false;
        else
            return true;

    }

    private double AreaDeTriangulo(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //Para saber el area del triangulo usamos la formula de heron
        //Fuente: http://james-ramsden.com/area-of-a-triangle-in-3d-c-code/

        float a = (float)DistanciaEntre2Puntos(p1,p2);
        float b = (float)DistanciaEntre2Puntos(p2, p3);
        float c = (float)DistanciaEntre2Puntos(p3, p1);
        float s = (a + b + c) / 2;
        return Mathf.Sqrt(s * (s - a) * (s - b) * (s - c));
    }

    public void CalibrarBTN()
    {
        
            Calibrar();
        
    }

    public void Calibrar()
    {
        if (calibracionConfirmada)
        {
            calibracionConfirmada = false;
            puntoCalibracion1 = new Vector3(0, 0, 0);
            puntoCalibracion2 = new Vector3(0, 0, 0);
            puntoCalibracion3 = new Vector3(0, 0, 0);
            puntoCalibracion4 = new Vector3(0, 0, 0);
        }
    }

    //Evento de la interfaz speech recognition. Lanza el evento cuando detecta una frase.
    public bool SpeechPhraseRecognized(string phraseTag, float condidence)
    {
        switch (phraseTag)
        {
            case "CALIBRATION":
                Calibrar();
                break;

            case "MENU":
                break;

            case "NEXTGAME":
                break;

            case "STARTGAME": //Inicia el primer juego
                SceneManager.LoadScene("Goombas", LoadSceneMode.Single);
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
                break;

            case "JUMP":
                break;

            case "HELLO":
                break;

            case "LUIGI":
                break;

            case "GOOMBA":
                break;
        }

        return true;
    }
}
