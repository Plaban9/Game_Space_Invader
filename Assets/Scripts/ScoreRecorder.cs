using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreRecorder : MonoBehaviour
{
    // Public Variables
    #region
    public static int score = 0;
    public GameObject scoreParticle;
    #endregion

    private Text scoreText;

    // Use this for initialization
    void Start()
    {
        scoreText = GetComponent<Text>();
        ResetScore();
        Score(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void ResetScore()
    {
        score = 0;
    }

    public void Score(int points)
    {
        score += points;
        scoreText.text = "SCORE: " + score.ToString();

        if (scoreParticle)
        {
            //Instantiate(scoreParticle, transform.position, Quaternion.identity);

            GameObject particleScore = Instantiate(scoreParticle, gameObject.transform.position, Quaternion.identity) as GameObject;
            particleScore.GetComponent<ParticleSystem>().startColor = Color.yellow;
            Destroy(particleScore, 2f);
        }
    }
}