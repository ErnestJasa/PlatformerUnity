using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{

	public UnityEvent noCollidersRemain;

	public List<Collider2D> detectedColliders = new();
	Collider2D collider2d;
	// Start is called before the first frame update
	private void Awake()
	{
		collider2d = GetComponent<Collider2D>();
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		detectedColliders.Add(collision);
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		detectedColliders.Remove(collision);

		if (detectedColliders.Count <=0)
		{
			noCollidersRemain.Invoke();
		}
	}
}
