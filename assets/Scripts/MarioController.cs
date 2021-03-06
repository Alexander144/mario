﻿using UnityEngine;
using System.Collections;

public class MarioController : MonoBehaviour {
	public int speed;
	public int jumpForce;
	public static bool shoot = false;
	public Animator animate;
	public static GameObject startscript;
	private Rigidbody2D rb;
	public int check = 0;

	private Animator anim;
	bool facingRight = true;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	
	void Awake(){
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D>();

	}
	void FixedUpdate () {

		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool ("isGrounded", grounded);

		Move ();


	}

	void Update(){
		if(grounded == true && Input.GetButtonDown("Jump")){
			if(this.gameObject.transform.localScale.y == 6){
				jumpForce = 30;
			}
			else{jumpForce = 25;}
			anim.SetBool("isGrounded", false);
			rb.velocity = (new Vector2(rb.velocity.x,jumpForce));
		}
		if(grounded == true){
			check=0;
		}
		if(shoot == true){

		}
	}
	
	void Move (){
		float aSpeed = Input.GetAxis ("Horizontal");
		if(aSpeed != 0 && Input.GetButtonDown ("Fire1")){
			transform.Translate (new Vector2 ( aSpeed * speed * Time.deltaTime, 0));
		} else if(aSpeed != 0){
			transform.Translate (new Vector2 ( aSpeed * (speed * 1.4f)* Time.deltaTime, 0));
		}

		anim.SetFloat ("speed", Mathf.Abs (aSpeed));
		if (aSpeed > 0 && !facingRight) {
			Flip(-1);

		} else if (aSpeed < 0 && facingRight){
			Flip(1); 

		}
	}

	void Flip(float move){
		facingRight = !facingRight;
		Vector2 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		transform.Translate (new Vector2(move,0));
	}
	void OnCollisionEnter2D (Collision2D other) {
		if (other.collider.tag == "Shroom" && Super.go == true) {
			//Får Soppen
			_GM.Super+=1;
			if(this.gameObject.transform.localScale.x > 0){
			this.gameObject.transform.localScale = new Vector2(6,6);
			}
			else{this.gameObject.transform.localScale = new Vector2(-6,6);}
			_GM.Score+=500;
			Super.go = false;
			Destroy (other.gameObject);
		}
		if (other.collider.tag == "Flower" && Super.go == true) {
			//Får blomsten
			shoot=true;
			Super.go = false;
			Destroy (other.gameObject);
			if(this.gameObject.transform.localScale.x > 0){
				this.gameObject.transform.localScale = new Vector2(6,6);
			}
			else{this.gameObject.transform.localScale = new Vector2(-6,6);}
			_GM.Score+=500;
			Destroy (other.gameObject);
		}
		if (other.collider.tag == "Gomba") {
			//Lyd, Mario blir mindre
			_GM.Super = 0;
			if(this.gameObject.transform.localScale.y == 6){
				if(this.gameObject.transform.localScale.x > 0){
					this.gameObject.transform.localScale = new Vector2(4,4);
				}
				else{this.gameObject.transform.localScale = new Vector2(-4,4);}
			}
			else{
				//Mario dør, her legger til avlsunting scenen eller mellom scenen
			Destroy(this.gameObject);
			}
		}
		if (other.collider.tag == "Koopa") {
			//Lyd, Mario blir mindre
			_GM.Super = 0;
				if(this.gameObject.transform.localScale.y == 6){
				if(this.gameObject.transform.localScale.x > 0){
					this.gameObject.transform.localScale = new Vector2(4,4);
				}
				else{this.gameObject.transform.localScale = new Vector2(-4,4);}
			}
			else{
				//Mario dør, her legger til avlsunting scenen eller mellom scenen
			Destroy(this.gameObject);
			}
		}
		if (other.collider.tag == "Border") {
			//Mario dør, her legger til avlsunting scenen eller mellom scenen
			Destroy(this.gameObject);
			}
	}
	void OnTriggerEnter2D (Collider2D other) {
		if(other.gameObject.tag == "Gomba"){
			//Dreper gompaen
			this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 2000));
			Destroy(other.gameObject);
			if(grounded == false && check>0){
				_GM.Score+=200;
			}
			else{_GM.Score+=100;}
			check+=1;
		}
		if(other.gameObject.tag == "Koopa"){
			//Dreper Koopa
			animate.SetBool("StompedK", true);
			this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 2000));
			Destroy(other.gameObject);
		}
	}
}