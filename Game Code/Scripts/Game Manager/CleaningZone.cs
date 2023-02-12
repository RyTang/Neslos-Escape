using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningZone : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            // End game
            Debug.Log("Game Lost");
            GameController.Controller.GameOver();
        }
        else if (!other.gameObject.CompareTag("Ground")){
            Destroy(other.transform.root.gameObject, 3);
        }
    }
}
