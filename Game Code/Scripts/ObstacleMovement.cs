using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [Range(0, 1)]  public float parallaxEffect = 0.5f;

    private void FixedUpdate() {
        float dist = GameController.Controller.ScrollSpeed * parallaxEffect;
        transform.position = new Vector2(transform.position.x - dist, transform.position.y);
    }

}
