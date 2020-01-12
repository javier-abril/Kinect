using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerObjetivos : MonoBehaviour
{
    private Transform[] listaCubos;
    protected static ControllerObjetivos instance = null;

    // Start is called before the first frame update
    void Start()
    {
        listaCubos = this.gameObject.GetComponentsInChildren<Transform>();
    }


    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static ControllerObjetivos Instance
    {
        get
        {
            return instance;
        }
    }

    public Vector3 GetRandomDestinationPosition()
    {
        int number = Random.Range(1, listaCubos.Length - 1);

        return listaCubos[number].position;
    }
}
