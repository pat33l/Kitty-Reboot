using UnityEngine;

public class GroundHitbox : MonoBehaviour
{
    private PlayerMovement player;

	private void Awake()
	{
		player = GetComponentInParent<PlayerMovement>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("Ground"))
        {
			player.GroundTouch();
        }
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Ground"))
		{
			player.GroundLeave();
		}
	}
}
