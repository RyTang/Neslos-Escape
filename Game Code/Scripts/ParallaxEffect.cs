using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [Range(0, 1)] public float parallaxEffect;
    public float spawnX = 20;
    public float deSpawnX = -20;


    private void FixedUpdate() {
        float dist = GameController.Controller.ScrollSpeed * parallaxEffect;
        transform.position = new Vector2(transform.position.x - dist, transform.position.y);

        if (transform.position.x <= deSpawnX) {
            transform.position = new Vector2(transform.position.x + Mathf.Abs(spawnX - deSpawnX), transform.position.y);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(deSpawnX, transform.position.y), GetComponent<SpriteRenderer>().bounds.size);
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector3(spawnX, transform.position.y), GetComponent<SpriteRenderer>().bounds.size);
    }
}
