using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float bouncedamp;
	public float jumpspeed;
	private int count;
	public GUIText countText;
	public GUIText heightText;
	private float yvelocity;
	private float gravity;
	private bool inplayarea;
	private bool underground;
	public GUIText instructions;

	void Start()
	{
		count = 0;
		yvelocity = 0;
		gravity = 11f;
		SetCountText ();
		SetPositionText (Vector3.zero);
		SetInstructionText ();
	}
	
	void Update()
	{
		//Reset game when "R" key is pressed
		if (Input.GetKeyDown (KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		//Check if ball is within play area
		Vector3 temppos = gameObject.transform.position;
		inplayarea = (temppos.x > -10) && (temppos.x < 10) && (temppos.z > -10) && (temppos.z < 10);
		underground = temppos.y < 0;

		//Get movement in X and Z
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		int modifier;
		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			modifier = 2;
		} else {
			modifier = 1;
		}

		//Calculating movement in Y
		if (Input.GetKeyDown (KeyCode.Space) && gameObject.transform.position.y == 0.5f) {
			yvelocity = modifier * jumpspeed;
		}

		yvelocity -= gravity * Time.deltaTime;
		float moveHeight = yvelocity;

		//Apply translations
		Vector3 movement = new Vector3 (speed * moveHorizontal, moveHeight, speed * moveVertical);
		Vector3 current = gameObject.transform.position;
		current += movement * Time.deltaTime;
		gameObject.transform.position = current;

		if(gameObject.transform.position.y <= 0.5 && inplayarea && !underground)
		{
			Vector3 temp = gameObject.transform.position;
			temp.y = 0.5f;
			gameObject.transform.position = temp;
			yvelocity *= -1*bouncedamp;
			if(Mathf.Abs (yvelocity) > 0.5){
				count++;
				gameObject.GetComponent<AudioSource>().Play ();
			}
		}

		SetCountText ();
		SetPositionText (gameObject.transform.position);
		
	}

	void SetCountText()
	{
		countText.text = "Bounces: " + count.ToString();
	}

	void SetPositionText(Vector3 position)
	{
		string xmsg = "Position(x): " + position.x.ToString ("#0.00");
		string ymsg = "\nHeight: " + position.y.ToString ("#0.00");
		string zmsg = "\nPosition(z): " + position.z.ToString ("#0.00");
		heightText.text = xmsg + ymsg + zmsg;
	}

	void SetInstructionText(){
		string msg0 = "Controls";
		string dash = "\n------------------------";
		string msg1 = "\nSpace: Jump";
		string msg2 = "\nShift+Space: Super Jump";
		string msg3 = "\n1: Decrease orbit speed";
		string msg4 = "\n2: Increase orbit speed";
		string msg5 = "\n9: Decrease orbit radius";
		string msg6 = "\n0: Increase orbit radius";
		string msg7 = "\nR: Restart game";

		instructions.text = msg0 + dash + msg1 + msg2 + msg3 + msg4 + msg5 + msg6 + msg7;
	}

}