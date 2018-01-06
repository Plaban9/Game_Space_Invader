using UnityEngine;
using System.Collections;

public class Destroy_Laser : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		Destroy(collider.gameObject);
	}
}