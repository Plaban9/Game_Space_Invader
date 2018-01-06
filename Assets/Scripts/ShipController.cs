using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipController : MonoBehaviour
{
    // Public Variables
    #region
    public float bombCooldown = 2f;
    public float xSensitivity = 5.0f;
    public float ySensitivity = 5.0f;
    public float xPadding = 0.7f;
    public float yPadding = 0.8f;
    public float firingPositionAdjustmentX = 0f;
    public float firingPositionAdjustmentY = 0f;
    public float projectileSpeed = 1f;
    public float firingRate = 1f;
    public float maxHealth = 100f;
    public AudioClip shootSound;
    public AudioClip bombShootSound;
    public AudioClip destroySound;
    public GameObject laser;
    public GameObject bomb;
    public GameObject dieParticleEmission;
    public GameObject hitParticleEmission;
    public GameObject addBombParticleEmission;
    public GameObject shootBombParticleEmission;
    public Image fill;
    public Color explosionColor;
    public Color maxHealthColor = Color.green;
    public Color minHealthColor = Color.red;
    public Text bombText;
    #endregion

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;
    private float sfxVolume;
    private static float health;
    private static byte bombCounter;
    private bool canShootBomb;
    private Vector3 bottomLeftBoundary;
    private Vector3 topRightBoundary;
    private Vector3 shipPosition;
    private Slider healthSlider;
    private Vector3 sfxPosition;

    // Use this for initialization
    void Start()
    {
        bombCounter = 5;
        canShootBomb = true;

        if (bombText)
        {
            bombText.text = "Bomb: x" + bombCounter.ToString();
            bombText.color = Color.yellow;
        }

        float depth = transform.position.z - Camera.main.transform.position.z;
        bottomLeftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
        topRightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.35f, depth));
        xMin = bottomLeftBoundary.x + xPadding;
        xMax = topRightBoundary.x - xPadding;
        yMin = bottomLeftBoundary.y + yPadding;
        yMax = topRightBoundary.y - yPadding;

        health = maxHealth;

        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        healthSlider.wholeNumbers = true;
        healthSlider.maxValue = health;
        healthSlider.minValue = 0;

        sfxVolume = MusicPlayer.GetSFXVolume();
        sfxPosition = new Vector3(this.transform.position.x, this.transform.position.y, -50); //For 3-D sound
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("ShootLaser", 0.000001f, firingRate);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("ShootLaser");
        }

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && canShootBomb && (bombCounter > 0))
        {
            canShootBomb = false;
            bombCooldown = Time.time + 3.0f;
            ShootBomb();
        }

        if (!canShootBomb && Time.time > bombCooldown)
        {
            canShootBomb = true;
        }

        //DisplayHealth();
    }

    void MovePlayer()
    {
        MovePlayerYAxis();
        MovePlayerXAxis();

        // Restrict_Position
        this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, xMin, xMax), Mathf.Clamp(this.transform.position.y, yMin, yMax), this.transform.position.z);
    }

    void MovePlayerYAxis()
    {
        Quaternion rotXAxis = new Quaternion();
        rotXAxis.eulerAngles = new Vector3(0, 0, 0);

        // Y - axis
        if (Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.UpArrow)))
        {
            this.transform.position += Vector3.up * ySensitivity * Time.deltaTime;
            rotXAxis.eulerAngles = new Vector3(30, 0, 0);
            this.transform.rotation = rotXAxis;
        }

        else if (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.DownArrow)))
        {
            this.transform.position += Vector3.down * ySensitivity * Time.deltaTime;
            rotXAxis.eulerAngles = new Vector3(-30, 0, 0);
            this.transform.rotation = rotXAxis;
        }

        this.transform.rotation = rotXAxis;
    }

    void MovePlayerXAxis()
    {
        Quaternion rotYAxis = new Quaternion();
        rotYAxis.eulerAngles = new Vector3(0, 0, 0);

        // X - axis
        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.LeftArrow)))
        {
            this.transform.position += Vector3.left * xSensitivity * Time.deltaTime;
            rotYAxis.eulerAngles = new Vector3(0, 30, 0);
            this.transform.rotation = rotYAxis;
        }

        else if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.RightArrow)))
        {
            this.transform.position += Vector3.right * xSensitivity * Time.deltaTime;
            rotYAxis.eulerAngles = new Vector3(0, -30, 0);
            this.transform.rotation = rotYAxis;
        }

        //this.transform.rotation = rotYAxis;
    }

    void ShootLaser()
    {
        if (laser)
        {
            GameObject laserShot = Instantiate(laser, new Vector3(this.transform.position.x + firingPositionAdjustmentX, this.transform.position.y + firingPositionAdjustmentY, this.transform.position.z + 0.1f), Quaternion.identity) as GameObject;
            laserShot.GetComponent<Rigidbody2D>().velocity = Vector3.up * projectileSpeed;
        }

        if (shootSound)
        {
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, sfxVolume);
        }
    }

    void ShootBomb()
    {
        --bombCounter;
        DisplayBombCounter();

        if (bomb)
        {
            GameObject bombShot = Instantiate(bomb, new Vector3(this.transform.position.x + firingPositionAdjustmentX, this.transform.position.y + firingPositionAdjustmentY, this.transform.position.z + 0.1f), Quaternion.identity) as GameObject;
            bombShot.GetComponent<Rigidbody2D>().velocity = Vector3.up * projectileSpeed * 0.5f;
        }

        if (bombShootSound)
        {
            AudioSource.PlayClipAtPoint(bombShootSound, Camera.main.transform.position, sfxVolume);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile missile = collider.gameObject.GetComponent<Projectile>();

        if (missile)
        {
            Hit(missile.GetDamage());
            missile.Hit();
            print("Damage Taken, Instance ID: " + GetInstanceID());
        }
    }

    void Hit(float damage)
    {
        health -= damage;

        DisplayHealth();

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (dieParticleEmission)
        {
            GameObject dieParticleColor = Instantiate(dieParticleEmission, gameObject.transform.position, Quaternion.identity) as GameObject;
            dieParticleColor.GetComponent<ParticleSystem>().startColor = explosionColor;
        }

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -100);

        if (destroySound)
        {
            AudioSource.PlayClipAtPoint(destroySound, Camera.main.transform.position, Mathf.Clamp(sfxVolume + 0.25f, 0.0f, 1.0f));
        }

        Invoke("NextLevel", 1f);

        Destroy(this.gameObject, 2f);
        ResetEnemyAttributes();
    }

    void DisplayHealth()
    {
        healthSlider.value = health;
        fill.color = Color.Lerp(minHealthColor, maxHealthColor, (float)healthSlider.value / maxHealth);

        if (hitParticleEmission)
        {
            GameObject hitParticleColor = Instantiate(hitParticleEmission) as GameObject;
            hitParticleColor.GetComponent<ParticleSystem>().startColor = fill.color;
        }
    }

    public void DisplayHealth(float healthPoints)
    {
        AddHealth(healthPoints);
        DisplayHealth();
    }

    void DisplayBombCounter()
    {
        if (shootBombParticleEmission)
        {
            Instantiate(shootBombParticleEmission);
        }

        if (bombText)
        {
            bombText.text = "Bomb: x" + bombCounter.ToString();
            bombText.color = Color.yellow;
        }
    }

    public void DisplayBombCounter(byte bombCount)
    {
        addBombCount(bombCount);

        if (addBombParticleEmission)
        {
            Instantiate(addBombParticleEmission);
        }

        DisplayBombCounter();
    }

    void NextLevel()
    {
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelManager.LoadNextLevel();
    }

    void ResetEnemyAttributes()
    {
        EnemyResponder.projectileSpeed = 5f;
        EnemyResponder.scoreWeight = 150;
    }

    public static void addBombCount(byte bombCount)
    {
        bombCounter += bombCount;
    }

    public static void AddHealth(float healthPoints)
    {
        health += healthPoints;
    }
}