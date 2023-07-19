using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

   [SerializeField]
    public int attackDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// is called when weapon touches enemy
    /// the function TakeDamage from "EnemyController" is called
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
     {
         if (collision.gameObject.tag == "Enemy")
         {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage, transform.position); 
            SoundManager.instance.PlaySE(3);
         }
    }
}
