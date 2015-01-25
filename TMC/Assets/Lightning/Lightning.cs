using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour {

	private LineRenderer lineRenderer;

	private float maxZ = 8f;

	private int noSegments = 12;

	private Color colour = Color.white;

	private float posRange = 0.2f;

	private float radius = 1f;

	private Vector2 midPoint;

//	void Start () 
//	{
//		lineRenderer = GetComponent<LineRenderer>();
//
//		lineRenderer.SetVertexCount(noSegments);
//
//		for (int i=1; i < noSegments-1; ++i)
//		{
//			float z = ((float)i)*(maxZ)/(float)(noSegments-1);
//
//			lineRenderer.SetPosition(i, new Vector3(Random.Range(-posRange,posRange),Random.Range(-posRange,posRange),z));
//		}
//
//		lineRenderer.SetPosition (0, new Vector3 (0f, 0f, 0f));
//		lineRenderer.SetPosition (noSegments - 1, new Vector3 (0f, 0f, 8f));
//	}

	void Start () 
	{
		lineRenderer = GetComponent<LineRenderer>();
		
		lineRenderer.SetVertexCount(noSegments);

		midPoint = new Vector2 (Random.Range (-radius, radius), Random.Range (-radius, radius));
		
		for (int i=1; i < noSegments-1; ++i)
		{
			float z = ((float)i)*(maxZ)/(float)(noSegments-1);

			float x = -midPoint.x*z*z/16f + z*midPoint.x/2f;

			float y = -midPoint.y*z*z/16f + z*midPoint.y/2f;
			
			lineRenderer.SetPosition(i, new Vector3(x + Random.Range(-posRange,posRange),y + Random.Range(-posRange,posRange),z));
		}
		
		lineRenderer.SetPosition (0, new Vector3 (0f, 0f, 0f));
		lineRenderer.SetPosition (noSegments - 1, new Vector3 (0f, 0f, 8f));
	}
	
	void Update () 
	{
		colour.a -= 10f * Time.deltaTime;

		lineRenderer.SetColors (colour, colour);

		if (colour.a <= 0f)
			Destroy (this.gameObject);
	}
}
