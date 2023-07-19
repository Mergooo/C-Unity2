using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public GameObject backToTitle;
    private Rigidbody2D rb;
    private Animator enemyAnim;

    [SerializeField]
    private float moveSpeed, waitTime, walkTime;

    private float waitCounter, moveCounter;

    private Vector2 moveDir;

    [SerializeField]
    private BoxCollider2D area;


   [SerializeField, Tooltip("Attack Player?")]
    private bool chase;

    private bool isChasing;

   [SerializeField]
    private float chaseSpeed, rangeToChase;

    private Transform target;

   [SerializeField]
    private float waitAfterHitting;

   [SerializeField]
    private int attackDamage;

   [SerializeField]
    private float maxHealth;
    private float currentHealth;
    private bool isKnockingBack;

    [SerializeField]
    private float knockBackTime, knockbackForce;

    private float knockBackCounter;

    private Vector2 knockDir;


   [SerializeField]
    private GameObject portion;
   [SerializeField]
    private GameObject staminaPortion;

   [SerializeField]
    private float healthDropChance;

   [SerializeField]
    private GameObject blood;



   [SerializeField]
    private int exp; 

   [SerializeField]
   private Image hpImage;

   private Flash flash;







    void Start()
    {
        //get component
        rb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();

        //set up waiting time
        waitCounter = waitTime;

        target =GameObject.FindGameObjectWithTag("Player").transform;

        currentHealth = maxHealth;
        UpdateHealthImage();

        flash = GetComponent<Flash>(); 


    }


    void Update()
    {

        if (isKnockingBack)
        {   
            if (knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime;
                rb.velocity = knockDir * knockbackForce;
            }
            else
            {
                rb.velocity = Vector2.zero;
                isKnockingBack = false;
            }
            return;
        }

        if (!isChasing)
        {
            



        //when enemy waits
        if (waitCounter > 0)
        {
            //enemy waiting time decreases
            waitCounter -= Time.deltaTime;

            //moving speed is 0
            rb.velocity = Vector2.zero;

            //when waiting time is over
            if (waitCounter <= 0)
            {
                //move time and animation set up
                moveCounter = walkTime;
                enemyAnim.SetBool("moving", true);

                //get direction of movement
                moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                moveDir.Normalize();
            }
        }
        else// when enemy moves
        
        {
            //enemy move time decreases
            moveCounter -= Time.deltaTime;

            //movement
            rb.velocity = moveDir * moveSpeed;

            //when movement is over
            if (moveCounter <= 0)
            {
                //waiting time and animation setup
                enemyAnim.SetBool("moving", false);
                waitCounter = waitTime;
            }
        }


        if (chase)
        {
            //when player is in close range, enemy will follow 
            if (Vector3.Distance(transform.position, target.transform.position)< rangeToChase)
            {
                isChasing = true;
            }
        }


    }
    else
    {
        // "waitcounter" will be set after enemy attacked 
        //as long as it is >0 enemy is Idle
       
        if (waitCounter >0)
        {
            waitCounter -= Time.deltaTime;  //"waitcounter" decreases every frame
            rb.velocity = Vector2.zero;

            if (waitCounter <= 0)
            {
                enemyAnim.SetBool("moving", true); //when "waitcounter" reached 0, enemy will start moving agaon
            }
        }

        else
        {
            moveDir =  target.transform.position - transform.position ;
            moveDir.Normalize();

            rb.velocity = moveDir * chaseSpeed;
        }

        
            //when player leaves close range, enemy will stop follow
            if (Vector3.Distance(transform.position, target.transform.position) > rangeToChase)
            {
                isChasing = false;
                waitCounter = waitTime;
                enemyAnim.SetBool("moving" , false);
            }
        
        
        
        
    }    // enemy will not leave the border of area
          transform.position = new Vector3(Mathf.Clamp(transform.position.x,area.bounds.min.x+1, area.bounds.max.x -1),
          Mathf.Clamp(transform.position.y,area.bounds.min.y+1, area.bounds.max.y -1), transform.position. z );  
    }
    
    

    /// <summary>
    /// When enemy attacks player
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag =="Player")
        {
            if (isChasing)
            {
              PlayerController player = collision.gameObject.GetComponent<PlayerController>();   
                // get playerController and call its functions for getting attacked
              player.KnockBack(transform.position);
              player.DamagePlayer(attackDamage);
              waitCounter = waitAfterHitting;
              
              enemyAnim.SetBool("moving", false);
              enemyAnim.SetBool("moving", false);

            }
        }   
    }
    

/// <summary>
/// when player attacks enemy it will be pushed back
/// </summary>
/// <param name="position"></param>
   public void KnockBack(Vector3 position)
   {
        isKnockingBack = true;
        knockBackCounter = knockBackTime;

        knockDir = transform.position - position;
        knockDir.Normalize();

        enemyAnim.SetBool("moving", false);

        // drops stamina portion when enemy is hit
        Instantiate(staminaPortion, transform.position, transform.rotation);
        
   }
    

    /// <summary>
    /// when player attacks enemy, it will get damage accord to player attack power
    /// blood animation will be triggered on postion of enemy
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="position"></param>
   public void TakeDamage(int damage, Vector3 position)
   {
        currentHealth -= damage;


        UpdateHealthImage();

        flash.PlayFeedback();
        
        if (currentHealth <= 0)
        {
            Instantiate(blood, transform.position, transform.rotation );

            GameManager.instance.AddExp(exp);

            if(Random.Range(0,100)< healthDropChance &&portion !=null)
            {
                Instantiate(portion, transform.position , transform.rotation);
            }
            

            
            Destroy(gameObject);
            
            // actives button to return to "title"
            backToTitle.SetActive(true);
        }

        KnockBack(position);    
   }
    

    /// <summary>
    /// hp animation changes according to current health of enemy
    /// </summary>
    private void UpdateHealthImage()
    {
        hpImage.fillAmount = currentHealth / maxHealth; 
    }
    
}//HHHHH