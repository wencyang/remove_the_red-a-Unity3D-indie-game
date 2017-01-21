using UnityEngine;
using System.Collections;

public class GameSetUp : MonoBehaviour 
{
	public GameObject Stick;
	public GameObject SpecialStick;
	public Vector3 spawnValues;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public void SetUp(int index)
	{
		StartCoroutine (SpawnWaves (index));
		Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x/30, spawnValues.x/30), spawnValues.y, spawnValues.z);				
		Instantiate (SpecialStick, spawnPosition, Quaternion.identity);
	}
	
	IEnumerator SpawnWaves (int index)
	{
		yield return new WaitForSeconds (startWait);
		for (int i = 0; i < index; i++)
		{
			Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
			Quaternion spawnRotation = Quaternion.identity;
			GameObject clone=(GameObject)Instantiate (Stick, spawnPosition, spawnRotation);
			clone.name = "Stick"+i;
			yield return new WaitForSeconds (spawnWait);
		}
		yield return new WaitForSeconds (waveWait);
	}
}
