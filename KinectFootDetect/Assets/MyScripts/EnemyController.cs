using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private float nextActionTime = 3.0f;
    public float respawnTime = 3.0f;
    public GameObject enemyType;
    public GameObject spawnPoint;
    public int maxNumberEnemies = 20;
    public AudioClip spawnAudio;
    public AudioClip killAudio;
    public float enemySpeed = 2.0f;
    private AudioSource[] audioPlayer;
    private NavMeshAgent navMeshAgent;

    private List<GameObject> enemiesList = new List<GameObject>();

    private Transform[] respawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        //Invoca la funcion mencionada en el tiempo indicado
        //InvokeRepeating("SpawnEnemy", respawnTime, respawnTime);

        if (spawnPoint != null)
            respawnPoints = spawnPoint.GetComponentsInChildren<Transform>();

        audioPlayer = this.GetComponents<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Spawn of enemies
        if (Time.timeSinceLevelLoad > nextActionTime && enemiesList.Count < maxNumberEnemies)
        {
            nextActionTime += respawnTime;
            // execute block of code here           
            enemiesList.Add(SpawnEnemy());
            audioPlayer[0].clip = spawnAudio;
            audioPlayer[0].Play();
        }

        //Check for killed enemies
        for(int i=0; i < enemiesList.Count; i++)
        {
            if (enemiesList[i] == null)
            {
                enemiesList.RemoveAt(i);
                audioPlayer[1].clip = killAudio;
                audioPlayer[1].Play();
            }
        }

    }

    public GameObject SpawnEnemy()
    {
        GameObject temp = Instantiate(enemyType, GetRandomRespawnPosition(), GetRandomRespawnRotation());

        temp.transform.position = GetRandomRespawnPosition();

        //For changing speed
        navMeshAgent = temp.GetComponent<NavMeshAgent>();

        if (navMeshAgent != null)
            navMeshAgent.speed = enemySpeed;

        return temp;
    }

    public Vector3 GetRandomRespawnPosition()
    {
        int number = Random.Range(1, respawnPoints.Length - 1);

        return respawnPoints[number].position;
    }

    public Quaternion GetRandomRespawnRotation()
    {
        int number = Random.Range(1, respawnPoints.Length - 1);

        return respawnPoints[number].rotation;
    }

    public void DestroyAllEnemies()
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            Destroy(enemiesList[i].gameObject);
            if (enemiesList[i] == null)
                enemiesList.RemoveAt(i);
        }
    }
}
