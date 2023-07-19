using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [SerializeField, Tooltip("move speed")]
    private  int moveSpeed;
   [SerializeField]
   private Animator playerAnim;

   public Rigidbody2D rb;

   [SerializeField]
   private Animator weaponAnim;


   [System.NonSerialized]

   public int currentHealth;
   public int maxHealth;

  private bool isknockingback;
  private Vector2 knockDir; //direction where player wil be pushed when he is attacked 

[SerializeField]
  private float knockbackTime , knockbackForce;
private float knockbackCounter;

[SerializeField]
  private float invicibilityTime;
private float invicibilityCounter;



    public float totalStamina, recoverySpeed;

   [System.NonSerialized]
    public float currentStamina;


[SerializeField]
    private float dashSpeed, dashLength, dashCost;

    private float dashCounter, activeMoveSpeed;

    private Flash flash;


    /// <summary>
    /// sets health of player on Screen
    /// </summary>
    void Start()
    {
        currentHealth = maxHealth;
        GameManager.instance.UpdateHealthUI();
        activeMoveSpeed = moveSpeed;
        currentStamina = totalStamina;
        GameManager.instance.UpdateStaminaUI();

        flash = GetComponent<Flash>(); 
        
        
        
    }

    // Update is called once per frame
    void Update()

    {

      if (GameManager.instance.statusPanel.activeInHierarchy)
      {
        return;
      }

      // when player is attacked, according to preset "invicibilityTime", "invicibilityCounter" will be set;
      // "invicibilityCounter" decreases each frame
      //
        if(invicibilityCounter > 0)
        {
            invicibilityCounter -= Time.deltaTime;
        }

        // when player is attacked "isknockingback" will be set true 
        //according to preset "knockbackTime", "knockbackCounter" will be set;
        //knockbackCounter decreases each frame
        if(isknockingback)
        {
          knockbackCounter -= Time.deltaTime;
          rb.velocity = knockDir * knockbackForce;

          if (knockbackCounter <= 0)
          {
              isknockingback = false;
          }
          else 
          {
            return;
          }
        }
        // when "knockbackCounter" is 0, script will continue



        // gets input of keyboard and mouse so player can move horizontally and vertically
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized*activeMoveSpeed; //Horizontal Vertical 


        //go in "if", when player doesnt move 
        if (rb.velocity!=Vector2.zero)
        {
          //lets the player turn his animation horizontally when "horizontal"- input is given
          if (Input.GetAxisRaw("Horizontal")!=0)
          {
            if (Input.GetAxisRaw("Horizontal") >0)
            {
              //turn right 
              playerAnim.SetFloat("X",1f);
              playerAnim.SetFloat("Y",0);

              weaponAnim.SetFloat("X",1f);
              weaponAnim.SetFloat("Y",0);
            }
            else
            {
              //turn left
               playerAnim.SetFloat("X",-1f);
              playerAnim.SetFloat("Y",0);

              weaponAnim.SetFloat("X",-1f);
              weaponAnim.SetFloat("Y",0);
            }
          }
          //lets the player turn his animation vertically when "vertical"- input is given
          else if (Input.GetAxisRaw("Vertical")>0)
          {
            //turn up
             playerAnim.SetFloat("X",0);
              playerAnim.SetFloat("Y",1);

              weaponAnim.SetFloat("X",0);
              weaponAnim.SetFloat("Y",1);
          }
          else
          {
            //turn down
             playerAnim.SetFloat("X",0);
              playerAnim.SetFloat("Y",-1);

              weaponAnim.SetFloat("X",0);
              weaponAnim.SetFloat("Y",-1);
          }
        }

        //when left mouse is clicked trigger Attack animation
        if (Input.GetMouseButtonDown(0))
        {
          weaponAnim.SetTrigger("Attack");
        }
        if (dashCounter <= 0)
        {
          if (Input.GetKeyDown(KeyCode.Space) &&currentStamina >dashCost )
          {
            activeMoveSpeed = dashSpeed;
            dashCounter = dashLength;

            currentStamina -= dashCost;

            GameManager.instance.UpdateStaminaUI();
          }
        }
        else
        {
          dashCounter -= Time.deltaTime;
          if (dashCounter <=0)
          {
            activeMoveSpeed = moveSpeed;  
          }
        }

        currentStamina = Mathf.Clamp(currentStamina + recoverySpeed * Time.deltaTime, 0, totalStamina);
        
        GameManager.instance.UpdateStaminaUI();
        
        
    }

    

/// <summary>
/// when player is attacked by enemy this function
/// lets him be pushed back according to his knockback time and -force
/// </summary>
/// <param name="position"></param>

  public void KnockBack(Vector3 position)
  {
    knockbackCounter = knockbackTime;
    isknockingback = true;
    knockDir = transform.position - position;
    

    knockDir.Normalize();


  }
    


    /// <summary>
    /// when player is attacked his health decreases according to parameter
    /// also gammManagers.UpdateHealthUI() will update HPSlider
    /// </summary>
    /// <param name="damage"></param>
public void DamagePlayer(int damage)
{
  if (invicibilityCounter<=0)
  {
    flash.PlayFeedback();

     currentHealth = Mathf.Clamp(currentHealth - damage, 0 , maxHealth );
     invicibilityCounter = invicibilityTime;
        SoundManager.instance.PlaySE(2);

     if (currentHealth==0)
     {
        gameObject.SetActive(false);
        SoundManager.instance.PlaySE(0);
        GameManager.instance.Load();
     }
  }
  GameManager.instance.UpdateHealthUI();
}

/// <summary>
/// when player touches portion, it will recover
/// some of his health
/// UI will be updated accordingly
/// </summary>
/// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision) 
    {
      if (collision.tag == "portion"  && maxHealth !=currentHealth && collision.GetComponent<items>().waitTime <= 0)
      {
       items items = collision.GetComponent<items>();
        SoundManager.instance.PlaySE(1);
       currentHealth = Mathf.Clamp(currentHealth + items.healthItemRecoveryValue, 0, maxHealth );
       GameManager.instance.UpdateHealthUI();

       Destroy(collision.gameObject);
      }  
      if (collision.tag == "staminaPortion"  && collision.GetComponent<items>().waitTime <= 0)
      {
       items items = collision.GetComponent<items>();
        SoundManager.instance.PlaySE(1);
        currentStamina = totalStamina;
       GameManager.instance.UpdateHealthUI();

       Destroy(collision.gameObject);
      }  
    }
    
}



