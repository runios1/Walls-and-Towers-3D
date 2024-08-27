using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using static AldenGenerator;

public class PlayerMainScript : MonoBehaviour
{
    public float health;
    public int coins;
    public HealthBar healthBar;
    public CoinCounter coinCounter;
    public Shop shop;

    public Animator animator;
    public Transform attackPoint;
    public Transform player;
    public float attackRange = 20f;
    public string enemyTag = "Enemy";
    public Renderer playerRenderer;
    private Color originalColor;
    public Color damageColor = Color.red;
    private Coroutine damageColorChange = null;

    private int maxHealth = 100;

    //respawn vars
    public float respawnTime = 5f;
    //private bool isDead = false;
    public TMP_Text respawnText;
    public GameObject respawnMenu;
    private Vector3 respawnPosition;
    private float nextAttackTime = 0f;

    public Transform jumpTarget = null;

    [Header("Audio")]
    private AudioSource audioSource;

    public AudioClip attackSound;
    public AudioClip buildSound;
    public AudioClip hurtSound;


    private bool isBuying = false;
    private string itemBuying;
    public GameObject buyingUi;
    private ScoringSystem scoringSystem;

    public GameObject GameOverMenu;

    void Start()
    {
        coins = 15;
        health = maxHealth;
        healthBar.SetMaxHealth(health);
        coinCounter.IncreaseCounter(coins);

        originalColor = playerRenderer.material.color;
        respawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        audioSource = GetComponent<AudioSource>();
        scoringSystem = FindObjectOfType<ScoringSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime && Input.GetMouseButtonDown(0) && !IsAttackingAnimation())
        {
            if (animator)
            {
                animator.MatchTarget(jumpTarget.position, jumpTarget.rotation, AvatarTarget.LeftFoot,
                                                       new MatchTargetWeightMask(Vector3.one, 1f), 0.141f, 0.78f);
            }
            Attack();
            nextAttackTime = Time.time + 0.2f;

            audioSource.clip = attackSound;
            audioSource.loop = false;
            audioSource.Play();
        }

        // Placeables
        if (!isBuying)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                itemBuying = "Tower";
                shop.ShowPreview(itemBuying);
                isBuying = true;
                buyingUi.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                itemBuying = "Wall";
                shop.ShowPreview(itemBuying);
                isBuying = true;
                buyingUi.SetActive(true);
            }
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.Q))
            {
                itemBuying = "Tower";
                shop.ShowPreview(itemBuying);

            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                itemBuying = "Wall";
                shop.ShowPreview(itemBuying);

            }
            if (Input.GetMouseButtonDown(0)) // Left Click
            {
                if (shop.Buy(itemBuying))
                {
                    audioSource.clip = buildSound;
                    audioSource.loop = false;
                    audioSource.Play();
                }

                isBuying = false;
                buyingUi.SetActive(false);
            }
            else if (Input.GetMouseButtonDown(1)) // Right Click
            {
                shop.RemovePreview();
                isBuying = false;
                buyingUi.SetActive(false);
            }

        }

    }

    public void GetCoins(int amount)
    {
        coins += amount;
        coinCounter.IncreaseCounter(amount);

        scoringSystem.CoinsGained(amount);
    }

    public bool LoseCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            coinCounter.DecreaseCounter(amount);

            scoringSystem.CoinsUsed(amount);
            return true;
        }
        else
        {
            Debug.Log("Not enough coins");
            return false;
        }
    }

    public void TakeDamage(float damage)
    {
        if (health <= 0) return; // FIXME: Placeholder to not keep executing this function even though the castle is destroyed until gameover is implemented

        audioSource.clip = hurtSound;
        audioSource.loop = false;
        audioSource.Play();

        if (damageColorChange == null)
        {
            damageColorChange = StartCoroutine(ChangeColorOnDamage());
        }
        else
        {
            StopCoroutine(damageColorChange);
            damageColorChange = StartCoroutine(ChangeColorOnDamage());

        }
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    void Die()
    {

        // await LogAldenChat("Serpina died trying to save castle and now the monsters are coming for you");
        //StartCoroutine(Respawn());

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameOverMenu.SetActive(true);

        AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }

        GameOverMenu.GetComponent<AudioSource>().Play();


    }
    void Attack()
    {
        animator.SetTrigger("Attack");

        // Transform newAttackPoint = transform.position + attackDirection * attackRange;

        // Detect enemies in range of the attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange);
        Debug.Log(hitEnemies.ToString());
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag(enemyTag))
            {
                Debug.Log("We hit " + enemy.name);
                enemy.GetComponent<Enemy>().TakeDamage(10, this.transform); // Assuming 10 is the damage amount
            }
        }
    }
    bool IsAttackingAnimation()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }

    public void PlaceItem(Placeable item)
    {
        Vector3 adjustedPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Instantiate(item.prefab, adjustedPosition, transform.rotation);
    }
    private IEnumerator ChangeColorOnDamage()
    {

        playerRenderer.material.color = damageColor; // Change the color to red
        yield return new WaitForSeconds(0.5f); // Wait for 1 second
        playerRenderer.material.color = originalColor; // Revert to the original color
    }
    private IEnumerator Respawn()
    {
        //isDead = true;

        float countdown = respawnTime;

        respawnMenu.SetActive(true);

        while (countdown > 0)
        {
            respawnText.text = "Respawn in: " + countdown.ToString("F1") + " seconds";
            yield return new WaitForSeconds(0.1f);
            countdown--;
        }

        respawnText.text = "Respawning...";
        yield return new WaitForSeconds(1f);

        RespawnPlayer();

        respawnMenu.SetActive(false);
        //isDead = false;
    }
    private void RespawnPlayer()
    {

        health = maxHealth;
        healthBar.SetMaxHealth(health);
        transform.position = respawnPosition;
    }
}
