using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Variables related to player character movement
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float speed = 3.0f;

    // Variables related to the health system
    public int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }

    // Variables related to temporary invincibility
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    // Variables related to animation
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

    // Variables related to projectiles
    public GameObject projectilePrefab;
    public InputAction Projectile;

    // Variables related to npc
    public InputAction talkAction;

    // Variables related to audio
    AudioSource audioSource;
    public AudioSource healthSource;
    public AudioSource damageSource;
    public AudioSource ammoSource;
    public AudioSource loseSource;
    public AudioSource winSource;

    //Variables related to game over
    bool canRestart;
    GameObject brokenBots;

    // Variables Related to Particle Systems
    [SerializeField] private ParticleSystem healthEffect = null;
    [SerializeField] private ParticleSystem damageEffect = null;
    [SerializeField] private ParticleSystem speedEffect = null;

    //Variables Related to Ammo
    int ammo;

    // Start is called before the first frame update
    void Start()
    {
        ammo = 2;
        canRestart = false;
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        talkAction.Enable();
        talkAction.performed += FindFriend;


        currentHealth = maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        brokenBots = GameObject.FindWithTag("brokenBot");

        if (brokenBots == null)
        {
            winSource.Play();
            GetComponent<Collider2D>().enabled = false;
            MoveAction.Disable();
            UIHandler.instance.DisplayGameOver();
            canRestart = true;
        }

        if (canRestart == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene("MainScene");
        }

        move = MoveAction.ReadValue<Vector2>();

        if( (move.x != 0 || move.y != 0) && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        if(move.x == 0 && move.y == 0)
        {
            audioSource.Stop();
        }

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }


        animator.SetFloat("Move X", moveDirection.x);
        animator.SetFloat("Move Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);


        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }


        // Detect input for projectile launch
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (ammo <= 0)
            {
                ammo = 0;
            }
            if (ammo == 0)
            {
                ammoSource.Play();
            }
            if (ammo > 0)
            {
                Launch();
                ammo -= 1;
            }
        }
        // Detect input for NPC interaction
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            }
        }
    }
    void FindFriend(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));


        if (hit.collider != null)
        {
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (character != null)
            {
                UIHandler.instance.DisplayDialogue();
            }
        }
    }
            // FixedUpdate has the same call rate as the physics system
            void FixedUpdate()

    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }


    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
        }


        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);

        if(amount > 0)
        {
            // health particles
            healthEffect.Play();
            healthSource.Play();
        }
        else
        {
            // damage particles
            damageEffect.Play();
            damageSource.Play();
        }

        if (currentHealth <= 0)
        {
            loseSource.Play();
            GetComponent<Collider2D>().enabled = false;
            MoveAction.Disable();
            UIHandler.instance.DisplayGameOver();
            canRestart = true;
        }

    }

    public void ChangeSpeed(int amount)
    {
        speed = speed + amount;
        speedEffect.Play();
        healthSource.Play();
    }

    public void ChangeAmmo(int amount)
    {
        ammo += amount;
        speedEffect.Play();
        healthSource.Play();
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);


        animator.SetTrigger("Launch");
    }


    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}