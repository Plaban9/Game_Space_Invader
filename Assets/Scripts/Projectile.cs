using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	// Public Variables
	#region
	public Color explosionColor;
	public GameObject particleEmission;
	#endregion	
	
	private float damage = 5f;
	
	public float GetDamage()
	{
		return damage;
	}
	
	public void Hit ()
	{
        if (particleEmission)
        {
            GameObject particleColor = Instantiate(particleEmission, gameObject.transform.position, Quaternion.identity) as GameObject;
            particleColor.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
        }

		Destroy(gameObject);
	}
}