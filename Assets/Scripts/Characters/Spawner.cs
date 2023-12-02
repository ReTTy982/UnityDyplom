using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject trentPrefab;
    public float cooldown = 1f;
    private float timePassed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void Update()
	{
        timePassed += Time.deltaTime;
		if(Input.GetKey(KeyCode.Space)) {
            if(timePassed >= cooldown)
            {
				GameObject newTrent = Instantiate(trentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                timePassed = 0f;
			}
            
        }
	}
}
