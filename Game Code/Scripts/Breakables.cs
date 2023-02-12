using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Collider2D boxCollider;
    public ParticleSystem destroyEfx;
    public AudioSource destroySound;
    public BoxDrop[] powerUps;
    [Range(0,1)] public float dropChance;
    public float slowDownImpact;
    [Range(0,1)] public float parallaxEffect = 0.5f;
    public float destructionTime = 3;

    private void FixedUpdate() {
        float dist = GameController.Controller.ScrollSpeed * parallaxEffect;
        transform.position = new Vector2(transform.position.x - dist, transform.position.y);
    }

    public void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponentInParent<PlayerController>().Hurt(slowDownImpact);
            Destroy();
            if (Random.value <= dropChance) {
                Debug.Log("Applying effect");
                ApplyEffect(other.gameObject);
            }
        }
    }


    public void Destroy() {
        // TODO: Effects and everything here
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;
        if (destroyEfx == null) {
            Debug.Log(gameObject.name + " doesn't have destroyed Efx");
        }
        else {
            destroyEfx.Play();

        }
        if (destroySound == null) {
            Debug.Log(gameObject.name + " doesn't have destroyed sound Efx");
        }
        else {
            destroySound.Play();
        }
        StartCoroutine(SelfDestruct());
    }

    private void ApplyEffect(GameObject player) {

        PowerUpEffect effect = ChooseUpgrade();

        if (effect != null) {
            effect.Apply(player);
        }
        else {
            Debug.LogError(string.Format("{0}: Has no drop effect with {1}", gameObject.name, effect.name));
        }
    }

    private PowerUpEffect ChooseUpgrade() {
        float totalChance = 0;
        foreach(BoxDrop drop in powerUps) {
            totalChance += drop.dropChance;
        }

        float randomPoint = Random.value * totalChance;

        for (int i = 0; i < powerUps.Length; i++) {
            if (randomPoint < powerUps[i].dropChance) {
                return powerUps[i].effect;
            }
            else {
                randomPoint -= powerUps[i].dropChance;
            }
        }
        return powerUps[powerUps.Length - 1].effect;
    }

    private IEnumerator SelfDestruct() {
        yield return new WaitForSeconds(destructionTime);
        if (transform.root.childCount <= 1) Destroy(transform.root.gameObject);
        Destroy(gameObject);
    }

}
