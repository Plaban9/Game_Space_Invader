
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemySpawner : MonoBehaviour 
{
	// Public Variables
	#region
	public float width = 10f;
	public float height = 5f;
	public float xSensitivity = 0.25f;
	public float ySensitivity = 0.25f;
	public float spawnDelay = 1f;
    public float addHealthPer5Wave = 5f;
    public float addHealthPer10Wave = 10f;
    public byte addBombsPer5Wave = 1;
    public byte addBombsPer10Wave = 2;
    public GameObject enemyPrefab;
    //public GameObject waveParticle;
    #endregion

    private float xMin;
	private float xMax;
	private float yMin;
	private float yMax;
	private float xPadding = 6.1f;
	private float yPadding = 3.25f;
	private Vector3 bottomLeftBoundary;
	private Vector3 topRightBoundary;
	private Vector3 shipPosition;
	private bool moveDirectionLeft = false;
	private bool moveDirectionDown = false;
    private ShipController shipController;
    private WaveRecorder waveRecorder;

    // Use this for initialization
    void Start () 
	{
        waveRecorder = GameObject.Find("Enemy_Wave").GetComponent<WaveRecorder>();

		float depth = transform.position.z - Camera.main.transform.position.z;
		bottomLeftBoundary = Camera.main.ViewportToWorldPoint( new Vector3( 0f, 0.3f, depth));
		topRightBoundary = Camera.main.ViewportToWorldPoint( new Vector3( 1f, 1f, depth));
		xMin = bottomLeftBoundary.x + xPadding;
		xMax = topRightBoundary.x - xPadding;
		yMin = bottomLeftBoundary.y + yPadding;
		yMax = topRightBoundary.y - yPadding;

        shipController = GameObject.Find("Player_Ship").GetComponent<ShipController>();

        SpawnEnemies();
	}
	
	public void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3( width, height));
	}
	
	// Update is called once per frame
	void Update () 
	{
		MoveEnemies();
	}
	
	void MoveEnemies()
	{
		MoveEnemiesX();
		MoveEnemiesY();		
		
		if (AllMembersDead())
		{
			Debug.Log ("Empty Formation!!");
			EnemyResponder.projectileSpeed += 0.5f;
			EnemyResponder.scoreWeight += 25;
			SpawnUntilFull();
		}
	}
	
	void MoveEnemiesX()
	{
		if (moveDirectionLeft)
		{
			this.transform.position += Vector3.left * xSensitivity * Time.deltaTime;
			
			if ( this.transform.position.x <= xMin)
			{
				moveDirectionLeft = false;
			}
		}
		
		else
		{
			this.transform.position += Vector3.right * xSensitivity * Time.deltaTime;
			
			if ( this.transform.position.x >= xMax)
			{
				moveDirectionLeft = true;
			}
		}
	}
	
	void MoveEnemiesY()
	{
		if (moveDirectionDown)
		{
			this.transform.position += Vector3.down * ySensitivity * Time.deltaTime;
			
			if ( this.transform.position.y <= yMin)
			{
				moveDirectionDown = false;
			}
		}
		
		else
		{
			this.transform.position += Vector3.up * ySensitivity * Time.deltaTime;
			
			if ( this.transform.position.y >= yMax)
			{
				moveDirectionDown = true;
			}
		}
	}
	
	Transform NextFreePosition()
	{
		foreach( Transform childPositionGameObject in transform)
		{
			if (childPositionGameObject.childCount == 0) //Check if empty
			{
				return childPositionGameObject;
			}
		}
		 
		return null;
	}
	
	bool AllMembersDead()
	{		
		foreach( Transform childPositionGameObject in transform)
		{
			if (childPositionGameObject.childCount > 0)
			{
				return false;
			}
		}

        waveRecorder.Wave();
        
        if (WaveRecorder.GetWave() % 5 == 0)
        {
            shipController.DisplayBombCounter(addBombsPer5Wave);
            shipController.DisplayHealth(addHealthPer5Wave);
        }

        if (WaveRecorder.GetWave() % 10 == 0)
        {
            shipController.DisplayBombCounter(addBombsPer10Wave);
            shipController.DisplayHealth(addHealthPer10Wave);
        }

		return true;
	}
	
	void SpawnEnemies() // Spawn all enemies at once
	{
		foreach (Transform child in transform)
		{
			GameObject enemy = Instantiate( enemyPrefab, child.transform.position, Quaternion.Euler( 0, 0, 180)) as GameObject;
			enemy.transform.parent = child;
		}
	}
	
	void SpawnUntilFull() // Spawn One Enemy at a time
	{
		Transform freePosition = NextFreePosition();
		
		if (freePosition)
		{
			GameObject enemy = Instantiate( enemyPrefab, freePosition.position, Quaternion.Euler( 0, 0, 180)) as GameObject;
			enemy.transform.parent = freePosition;
		}
		
		if (NextFreePosition())
		{
			Invoke ("SpawnUntilFull", spawnDelay);
		}
	}
}