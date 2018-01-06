using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour 
{
	// Public Variables
	#region
	public float firingPositionAdjustment = 0f;
	public float projectileSpeed = 0f;
	public float firingRate = 1f;
	public GameObject laser;
	#endregion

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Random.Range( 0f, 1f) < 0.1f)
		{
			InvokeRepeating( "ShootLaser", 0.000001f, firingRate);
		}	
		
		if (Random.Range( 0f, 1f) > 0.1f) 
		{
			CancelInvoke("ShootLaser");
		}
	}
	
	void ShootLaser()
	{
		
		GameObject laserShot = Instantiate( laser, new Vector3( this.transform.position.x + firingPositionAdjustment, this.transform.position.y, this.transform.position.z + 0.1f), Quaternion.identity) as GameObject;
		laserShot.GetComponent<Rigidbody2D>().velocity = Vector3.down * projectileSpeed;
	}
}