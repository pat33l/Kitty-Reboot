using UnityEngine;

public class Spikes : MonoBehaviour
{
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			collision.GetComponent<PlayerMovement>().Damage();
		}
	}
}
