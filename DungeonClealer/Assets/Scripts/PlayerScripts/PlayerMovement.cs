using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState {
    walk,
    attack,
    interact
}
public class PlayerMovement : MonoBehaviour, IMovable
{
    private Player player;
    //public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Vector3 changeAttack;
    private Animator animator;
    public PlayerState currentState;
    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        changeAttack = Vector3.zero;
        changeAttack.x = Input.GetAxisRaw("attack X");
        changeAttack.y = Input.GetAxisRaw("attack Y");
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            changeAttack.y = 1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            changeAttack.x = 1;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            changeAttack.y = -1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            changeAttack.x = -1;
        }

        if (currentState != PlayerState.attack) {
            UpdateAnimationAndAttack();
        }
        if (currentState == PlayerState.walk) { 
            UpdateAnimationAndMove(); 
        }
    }


    void UpdateAnimationAndMove() {
        if (change != Vector3.zero) {
            Move();
            animator.SetFloat("moveX",change.x);
            animator.SetFloat("moveY",change.y);
            animator.SetBool("moving",true);
          }
          else{
            animator.SetBool("moving",false);
          }
    }

    private IEnumerator AttackCo()
    {
        //currentState = PlayerState.attack;
        yield return null;
        currentState = PlayerState.walk;
    }
    void UpdateAnimationAndAttack()
    {
        if (changeAttack != Vector3.zero)
        {
            //StartCoroutine(AttackCo());
            animator.SetFloat("attackX", changeAttack.x);
            animator.SetFloat("attackY", changeAttack.y);
            animator.SetBool("attacking", true);
        }
        else
        {
            animator.SetBool("attacking", false);
        }
    }

    public void Move() {
        myRigidbody.MovePosition(transform.position + change * player.characteristics.speed * Time.deltaTime);
    }
}
