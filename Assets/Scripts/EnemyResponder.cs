using UnityEngine;
using System.Collections;

public class EnemyResponder : MonoBehaviour
{
    // Public variables
    #region
    public float health = 15f;
    public float firingPositionAdjustment = 0f;
    public static float projectileSpeed = 5f;
    public float firingRatePerSecond = 0.5f;
    public static int scoreWeight = 150;
    public AudioClip shootSound;
    public AudioClip destroySound;
    public Color explosionColor;
    public GameObject laser;
    public GameObject particleEmission;
    public GameObject addScoreParticleEmitter;
    #endregion

    private float sfxVolume;
    private ScoreRecorder scoreRecorder;
    private Vector3 sfxPosition;

    // Use this for initialization
    void Start()
    {
        sfxVolume = MusicPlayer.GetSFXVolume();
        sfxPosition = new Vector3(this.transform.position.x, this.transform.position.y, -50); //for 3-D Sound
        scoreRecorder = GameObject.Find("Score").GetComponent<ScoreRecorder>();
    }

    // Update is called once per frame
    void Update()
    {
        float probability = Time.deltaTime * firingRatePerSecond;

        if (Random.value < probability)
        {
            ShootLaser();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile missile = collider.gameObject.GetComponent<Projectile>();
        Bomb bomb = collider.gameObject.GetComponent<Bomb>();

        if (missile)
        {
            missile.Hit();
            Hit(missile.GetDamage());
            print("Damage Taken from missile laser, Instance ID: " + GetInstanceID());
        }

        if (bomb)
        {
            bomb.Hit();
            Hit(bomb.GetDamage());
            print("Damage Taken from Bomb, Instance ID: " + GetInstanceID());
        }
    }

    void Hit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void ShootLaser()
    {
        Vector3 startPosition = transform.position + new Vector3(0, -1, 0);

        if (laser)
        {
            GameObject missile = Instantiate(laser, startPosition, Quaternion.Euler(0, 0, 180)) as GameObject;
            missile.GetComponent<Rigidbody2D>().velocity = Vector3.down * projectileSpeed;
        }

        if (shootSound)
        {
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, sfxVolume);
        }
    }

    void Die()
    {
        GameObject particleColor = Instantiate(particleEmission, gameObject.transform.position, Quaternion.identity) as GameObject;
        particleColor.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;

        if (destroySound)
        {
            AudioSource.PlayClipAtPoint(destroySound, Camera.main.transform.position, Mathf.Clamp(sfxVolume + 0.45f, 0.0f, 1.0f));
        }

        if (addScoreParticleEmitter)
        {
            Instantiate(addScoreParticleEmitter);
        }

        scoreRecorder.Score(scoreWeight);
        Destroy(this.gameObject);
    }
}