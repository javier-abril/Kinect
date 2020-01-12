using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveEnemy : MonoBehaviour
{
    
    private float nextActionTime = 0.0f;
    public float period = 3.0f;
    private ControllerObjetivos controllerObjetivos;

    private GameObject canvasObject;
    private Canvas canvasUI;
    public List<RaycastResult> list;
    public Vector2 screenPoint;

    // Start is called before the first frame update
    void Start()
    {
        if(controllerObjetivos == null)
        {
            controllerObjetivos = ControllerObjetivos.Instance;
        }

        canvasObject = GameObject.Find("UI-Canvas");

        if (canvasObject != null)
            canvasUI = canvasObject.GetComponent<Canvas>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            // execute block of code here
            this.gameObject.GetComponent<NavMeshAgent>().destination = controllerObjetivos.GetRandomDestinationPosition();
            //Debug.Log("Objetivo Cambiado");
        }

        GetScreenPosition();
    }

    public Vector2 GetScreenPosition()
    {
        

        Vector3 target = this.gameObject.GetComponent<Transform>().position;
        GraphicRaycaster graphicRaycaster = canvasUI.GetComponent<GraphicRaycaster>();


        list = new List<RaycastResult>();
        screenPoint = Camera.main.WorldToScreenPoint(target);
        PointerEventData ed = new PointerEventData(EventSystem.current);
        ed.position = screenPoint;
        graphicRaycaster.Raycast(ed, list);

        if (list != null && list.Count > 0)
        {
            Debug.Log("Hit: " + list[0].gameObject.name);

            switch (list[0].gameObject.name)
            {
                case "P1":
                    ScoreManager.AddPointsP1(100);
                    Destroy(this.gameObject);
                    break;
                case "P2":
                    ScoreManager.AddPointsP2(100);
                    Destroy(this.gameObject);
                    break;
                case "P3":
                    ScoreManager.AddPointsP3(100);
                    Destroy(this.gameObject);
                    break;
                case "P4":
                    ScoreManager.AddPointsP4(100);
                    Destroy(this.gameObject);
                    break;
                default:
                    break;
            }
            //target.GetComponent<Renderer>().material.color = Color.red;
        }

        //Debug.Log("ScreenPoint: " +  screenPoint);

        return screenPoint;
    }

}
