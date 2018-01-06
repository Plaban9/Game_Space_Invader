using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FontChanger : MonoBehaviour 
{
	//Public Variables
	#region 
	public Text text = null;
	public string greeting = null;
	
	//Initial Font Settings
	public byte minFontSize;
	public byte maxFontSize;
	
	//Cycle Font Size
	public int delay = 5;
	#endregion
	
	//Counter Variables
	bool isAscCounter = true;	
	byte cycleCounter;
	byte counter = 1;

	//Use this for initialization
	void Start () 
	{
		cycleCounter = minFontSize;
	}
	
	//Update is called once per frame
	void Update () 
	{
		if (delay == 0)
		{
			Debug.LogError("Delay is set to 0, which may result in divide by 0 error.");
		}
		
		else
		{
			if ((counter % delay) == 0)
			{	
				GrowShrinkCycler();	
			}
		}
		
		++counter;
	}
	
	void GrowShrinkCycler()
	{
		if (cycleCounter == minFontSize)
		{
			isAscCounter = true;
		}
		
		else if (cycleCounter == maxFontSize)
		{
			isAscCounter = false;
		}
		
		if (!isAscCounter)
		{
			--cycleCounter;
		}	
		
		else
		{
			++cycleCounter;
		}
		
		text.fontSize = cycleCounter;
	}
}