using UnityEngine;
using System.Collections;

public class PlayGif : MonoBehaviour 
{
	// Public Variables
	#region
	public Sprite[] gifSprites;
	public int delay = 2;
	#endregion
	
	private byte delayCounter = 0;
	private byte imgCounter = 0;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (delay == 0)
		{
			Debug.LogError("Delay is set to 0, which may result in divide by 0 error.");
		}
		
		else
		{	
			if ( delayCounter % delay == 0)
			{	
				LoadSprites();
			}
		}
		
		++delayCounter;		
	}
	
	void LoadSprites()
	{
		this.GetComponent<SpriteRenderer>().sprite = gifSprites[imgCounter % gifSprites.Length];
		++imgCounter;
	}	
}