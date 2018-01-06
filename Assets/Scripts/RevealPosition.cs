using UnityEngine;
using System.Collections;

public class RevealPosition : MonoBehaviour 
{
	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere( transform.position, 1f);
	}
}