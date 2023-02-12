using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUpEffect/InvincibilityBuff")]
public class InvincibilityBuff : PowerUpEffect {
    public float duration;

    public override void Apply(GameObject target) {
        target.GetComponent<PlayerController>().TriggerInvulnerability(duration);
    }
}
