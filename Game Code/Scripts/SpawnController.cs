using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private ObjectToSpawn[] objects;
    [SerializeField] private Vector3 gizmosSize = new Vector3(1, 1, 0);
    public float spawnBetDuration = 2;
    public GameData gameData;
    public float decreaseFactor = 0.1f;
    public int scoreFactorChange = 2500;
    public int maxDecreaseCount = 10;
    private float spawnTimer;
    private int decreaseCount = 0;

    [System.Serializable]
    public class ObjectToSpawn {
        [SerializeField] private GameObject spawnObject;
        [SerializeField] private float yLocation;

        public GameObject SpawnObject {
            get { return spawnObject; }
        }

        public float YLocation {
            get { return yLocation; }
        }
    }
    private void Start() {
        spawnTimer = spawnBetDuration;
    }

    private void FixedUpdate() {
        spawnTimer -= Time.fixedDeltaTime;
        if (spawnTimer <= 0) {
            spawnTimer = spawnBetDuration - decreaseFactor * decreaseCount;
            int objChosen = Random.Range(0, objects.Length);
            ObjectToSpawn obj = objects[objChosen];
            Vector3 spawnPos = new Vector3(transform.position.x, obj.YLocation);
            Instantiate(obj.SpawnObject, spawnPos, Quaternion.identity);
        }
        if (gameData.Score > scoreFactorChange * (decreaseCount + 1) && decreaseCount < maxDecreaseCount) {
            decreaseCount += 1;
        }
    }


    private void OnDrawGizmosSelected() {
        foreach(ObjectToSpawn obj in objects){
            Gizmos.color = Color.red;
            Vector3 objSize = gizmosSize;

            SpriteRenderer renderer = obj.SpawnObject.GetComponent<SpriteRenderer>();
            if (renderer != null) {
                objSize = renderer.bounds.size;
            }
            Gizmos.DrawWireCube(new Vector3(transform.position.x, obj.YLocation, 0 ), objSize);
        }
    }
}
