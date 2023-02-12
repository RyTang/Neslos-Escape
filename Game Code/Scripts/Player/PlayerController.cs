using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameData gameData;

    
    [Header("Upgrade Values")]
    float rollUpgradeFactor = 0.02f;
    float knockbackDecreaseFactor = 2;
    float recoveryUpgradeFactor = 0.05f;        

    [Header("Components Required")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ParticleSystem dustEfx;
    [SerializeField] private ParticleSystem jumpEfx;
    [SerializeField] private SimpleFlash simpleFlash;
    [SerializeField] private AudioSource hurtSound;
    [SerializeField] private AudioSource collideSound;
    [SerializeField] private float catchUpSpeed = 0.01f;
    [SerializeField] private float slowDownDuration = 0.01f;
    [SerializeField] private float hurtDuration = 1;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float jumpHangTime = 0.3f;
    [SerializeField] private float rollTimer = 0.3f;
    [SerializeField] [Range(1, 2)] private float fallGravityScale = 1;

    private Coroutine slowDownCoroutine, hurtCoroutine, catchUpCoroutine, jumpHangTimeCoroutine;
    private Vector3 originalPosition;
    private float slowDownFactor, originalGravity, hurtTimer;
    private bool isAlive = true;
    private bool isGrounded = false;
    private bool isRolling = false, canRoll = true;
    private bool pressingJump = false, pressedJump = false, pressingRoll = false, jumpButtonReleased = false;
    private bool firstJumpReleaseAvailable;
    private bool slowingDown = false;
    private bool invulnerable = false;


    private void Start() {
        originalGravity = rb.gravityScale;
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAlive) {
            if (!isGrounded && rb.velocity.y < 0) {
                rb.gravityScale = originalGravity * fallGravityScale;
                animator.SetTrigger("Falling");
            }
            else {
                rb.gravityScale = originalGravity;
            }
        }
    }

    private void Update() {
        if (!isAlive) {
            return;
        }
        JumpInput();
        RollInput();
        if (isGrounded) {
            dustEfx.Play();
        }
        else {
            dustEfx.Stop();
        }
    }

    private void JumpInput() {
        if ((pressingJump || Input.GetAxisRaw("Vertical") > 0) && isGrounded) {
            // TODO: If Rolling can't jump
            if (!isRolling) {
                animator.SetTrigger("Jump");
            }
            isGrounded = false;
            pressedJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            jumpEfx.Play();
        }
        if (!pressingJump && rb.velocity.y > 0 && jumpButtonReleased && firstJumpReleaseAvailable) { // Trigger once only when button is let go
            firstJumpReleaseAvailable = false;
            jumpButtonReleased = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.5f);
        }
    }

    private void RollInput() {
        if ((pressingRoll || Input.GetAxisRaw("Vertical") < 0) && canRoll && isGrounded) {
            animator.SetBool("Rolling", true);
            canRoll = false;
            isRolling = true;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (!invulnerable && collision.gameObject.CompareTag("Obstacles")) {
            isAlive = false;
            collideSound.Play();
            StopAllCoroutines(); // Stop any of the catching up -> Prevent hammy moving forward when knocked out
            GameController.Controller.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Ground")) {
            if (!isGrounded) {
                animator.SetTrigger("Landed");
                jumpEfx.Play();
            }
            isGrounded = true;
            pressedJump = false;
            firstJumpReleaseAvailable = true;
            animator.SetBool("Grounded", true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        // TODO: Why is this needed
        if (collision.CompareTag("Ground")) {
            isGrounded = true;
            if (!isRolling && rb.velocity.y <= 0) {
                canRoll = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!pressedJump && isGrounded && collision.CompareTag("Ground")) {
            if (jumpHangTimeCoroutine != null) {
                StopCoroutine(jumpHangTimeCoroutine);
            }
            jumpHangTimeCoroutine = StartCoroutine(HangTime());
        }
    }

    public void Hurt(float slowDown) {
        if (invulnerable) {
            return;
        }

        hurtSound.Play();
        SlowDown(slowDown);
    }

    private void SlowDown(float slowDown) {
        slowDownFactor += slowDown * 1 - (knockbackDecreaseFactor * gameData.upgradeData.knockbackLevel);
        // Stopping all Coroutines, begin from new slate
        if (slowDownCoroutine != null) {
            StopCoroutine(slowDownCoroutine);
            slowDownCoroutine = null;
            slowingDown = false;
        }
        if (catchUpCoroutine != null) {
            StopCoroutine(catchUpCoroutine);
            catchUpCoroutine = null;
        }
        if (hurtCoroutine != null) {
            StopCoroutine(hurtCoroutine);
            hurtCoroutine = null;
        }

        slowDownCoroutine = StartCoroutine(SlowDown());
    }

    public void FinishedRolling() {
        StartCoroutine(RollingTimer());
    }

    // PowerUps
    public void TriggerInvulnerability(float duration) {
        StartCoroutine(Invulnerability(duration));
    }


    // Mobile Controls

    public void OnButtonJumpDown() {
        pressingJump = true;
        jumpButtonReleased = false;
    }

    public void OnButtonJumpUp() {
        pressingJump = false;
        if (!jumpButtonReleased) {
            jumpButtonReleased = true;
        }
        // TODO: Make Sure to figure out how to prevent double tapping jump, causing character to fly
    }

    public void OnButtonRollDown() {
        pressingRoll = true;
    }

    public void OnButtonRollUp() {
        pressingRoll = false;
    }

    // Coroutines

    private IEnumerator SlowDown() {
        if (slowingDown) {
            yield break;
        }
        slowingDown = true;
        simpleFlash.Flash(0.2f);
        float timer = 0;
        while (transform.position.x > originalPosition.x - slowDownFactor) {
            float xPos = Mathf.Lerp(transform.position.x, originalPosition.x - slowDownFactor, timer / slowDownDuration);
            transform.position = new Vector2(xPos, transform.position.y);
            timer += Time.fixedDeltaTime;
            yield return null;
        };
        slowingDown = false;
        hurtCoroutine = StartCoroutine(HurtTimer());
    }

    private IEnumerator HurtTimer() {
        hurtTimer = hurtDuration - gameData.upgradeData.recoveryLevel * recoveryUpgradeFactor;
        while (hurtTimer > 0) {
            hurtTimer -= Time.deltaTime;
            yield return null;
        }
        catchUpCoroutine = StartCoroutine(CatchUp());
        hurtCoroutine = null;
    }

    private IEnumerator CatchUp() {
        Vector3 startingPos = transform.position;
        while (transform.position.x <= originalPosition.x) {
            transform.Translate(new Vector3(catchUpSpeed, 0, 0));
            if (slowDownFactor > 0) slowDownFactor -= catchUpSpeed;
            yield return null;
        }
        catchUpCoroutine = null;
    }

    private IEnumerator RollingTimer() {
        isRolling = false;
        animator.SetBool("Rolling", false);
        yield return new WaitForSeconds(rollTimer - rollUpgradeFactor * gameData.upgradeData.rollLevel);
        canRoll = true;
    }
    
    private IEnumerator HangTime() {
        yield return new WaitForSeconds(jumpHangTime);
        isGrounded = false;
        animator.SetBool("Grounded", false);
        jumpHangTimeCoroutine = null;
    }

    private IEnumerator Invulnerability(float duration) {
        invulnerable = true;
        simpleFlash.Flash(duration);
        yield return new WaitForSeconds(duration);
        invulnerable = false;
    }
}
