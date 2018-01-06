using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    #region
    public float rotationAngle = 180f;
    public Color explosionColor;
    public GameObject particleEmission;
    #endregion

    private float damage = 20f;

    void Update()
    {
        rotateBomb();
    }

    void rotateBomb()
    {
        this.transform.Rotate(0, 0, rotationAngle * Time.deltaTime);
    }

    public float GetDamage()
    {
        return damage;
    }

    public void Hit()
    {
        if (particleEmission)
        {
            GameObject particleColor = Instantiate(particleEmission, gameObject.transform.position, Quaternion.identity) as GameObject;
            particleColor.GetComponent<ParticleSystem>().startColor = explosionColor;
        }

        Destroy(gameObject);
    }
}
