using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayWave : MonoBehaviour
{
    //Public Variables
    #region
    //public string wave = null;
    #endregion

    private Text waveText = null;

    // Use this for initialization
    void Start ()
    {
        waveText = GetComponent<Text>();
        waveText.text = (WaveRecorder.wave - 1).ToString();
        WaveRecorder.ResetWave();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
