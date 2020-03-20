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
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentState != PlayerState.attack) {
            changeAttack.y = 1;
            StartCoroutine(AttackCo());
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentState != PlayerState.attack)
        {
            changeAttack.x = 1;
            StartCoroutine(AttackCo());
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && currentState != PlayerState.attack)
        {
            changeAttack.y = -1;
            StartCoroutine(AttackCo());
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentState != PlayerState.attack)
        {
            changeAttack.x = -1;
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.walk)
        {
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
        currentState = PlayerState.attack;
        animator.SetBool("attacking", true);
        animator.SetFloat("moveX", changeAttack.x);
        animator.SetFloat("moveY", changeAttack.y);
        animator.SetFloat("attackX", changeAttack.x);
        animator.SetFloat("attackY", changeAttack.y);
        yield return null;
        animator.SetBool("attacking", false);
        //yield return new WaitForSeconds(.33f);
        yield return null;
        currentState = PlayerState.walk;
    }

    public void Move() {
        change.Normalize(); // Убрать ускорение когда используешь перемещение (сразу 2 кнопки)
        myRigidbody.MovePosition(transform.position + change * player.characteristics.speed * Time.deltaTime);
    }
}
