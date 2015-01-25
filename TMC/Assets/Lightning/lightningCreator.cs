using UnityEngine;
using System.Collections;

public class lightningCreator : MonoBehaviour {

	public Lightning lightningPrefab;

	// Use this for initialization
	IEnumerator Start () {
	
		while(true)
		{
			Instantiate(lightningPrefab,this.transform.position, transform.parent.rotation);
            Instantiate(lightningPrefab, this.transform.position, transform.parent.rotation);
            Instantiate(lightningPrefab, this.transform.position, transform.parent.rotation);

			yield return null;
		}
	
	}

}
