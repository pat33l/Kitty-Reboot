using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float groundSpeed;
	[SerializeField] float airSpeed;
	[SerializeField] float jumpForce;
	[SerializeField] float dodgeLength = 0.25f;
	[SerializeField] float dodgeCooldown;
	[SerializeField] float reviveInvulTime = 1f;

	public GameObject deadUI;
	public TextMeshProUGUI livesText;

    private Rigidbody2D rb;
	private Animator anim;
	private SpriteRenderer sprite;

    private Vector2 moveVector;
	private bool onGround;
	private bool dead;
	private bool invul;
	private int lives = 9;

	private bool canDodge = true;
	private bool dodging;

	public void OnMove(InputValue value)
	{
		moveVector = value.Get<Vector2>();
		if (dead) return;
		if (moveVector.x < 0)
		{
			transform.localScale = Vector3.one;
		} else if (moveVector.x > 0)
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}
	}

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
	}

	public void OnJump(InputValue _value)
	{
		if (dead)
		{
			StartCoroutine(Revive());
			return;
		}
		if (onGround)
		{
			rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			anim.SetTrigger("Jump");
			GroundLeave();
		}
	}

	private IEnumerator Revive()
	{
		dead = false;
		deadUI.SetActive(false);
		anim.speed = 1;
		sprite.color = new Color(1, 1, 1, 0.5f);
		invul = true;
		yield return new WaitForSeconds(reviveInvulTime);
		invul = false;
		sprite.color = Color.white;
	}

	public void OnDodge(InputValue _value)
	{
		if (canDodge && !dead)
		{
			StartCoroutine(Dodge());
		}
	}

	private IEnumerator Dodge()
	{
		anim.SetTrigger("Dodge");
		canDodge = false;
		dodging = true;
		yield return new WaitForSeconds(dodgeLength);
		dodging = false;
		yield return new WaitForSeconds(dodgeCooldown - dodgeLength);
		canDodge = true;
	}

	private void Update()
	{
		anim.SetFloat("Horizontal Speed", Mathf.Abs(rb.linearVelocityX));
		anim.SetFloat("Vertical Velocity", rb.linearVelocityY);
	}

	private void FixedUpdate()
	{
		if (!dodging && !dead)
			rb.AddForce(Vector2.right * moveVector.x * (onGround ? groundSpeed : airSpeed));
	}

	public void GroundTouch()
	{
		onGround = true;
		anim.SetBool("Grounded", true);
	}

	public void GroundLeave()
	{
		onGround = false;
		anim.SetBool("Grounded", false);
	}

	public void Damage()
	{
		if (!invul && !dodging && !dead)
		{
			dead = true;
			deadUI.SetActive(true);
			lives--;
			sprite.color = Color.black;
			livesText.text = $"Lives: {lives}";
			anim.speed = 0;
			if (lives <= 0)
			{
				SceneManager.LoadScene("Lose");
			}
		}
	}
}
