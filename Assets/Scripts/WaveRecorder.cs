using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveRecorder : MonoBehaviour
{
    // Public Variables
    #region
    public static byte wave;
    public GameObject waveParticle;
    #endregion

    private Text waveText;

    // Use this for initialization
    void Start ()
    {
        waveText = GetComponent<Text>();
        ResetWave();
        Wave();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public static void ResetWave()
    {
        wave = 0;
    }

    public void Wave()
    {
        ++wave;
 
        waveText.text = "Wave: " + wave.ToString();

        if (waveParticle)
        {
            //Instantiate(scoreParticle, transform.position, Quaternion.identity);

            GameObject particleWave = Instantiate(waveParticle) as GameObject;
            particleWave.GetComponent<ParticleSystem>().startColor = Color.red;
            Destroy(particleWave, 2f);
        }
    }

    public static byte GetWave()
    {
        return wave;
    }
}