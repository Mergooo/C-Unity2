using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{


    //this script will be added to a statue which can be talked to 
    //and where the player can save


   [SerializeField, Header("会話文章"),Multiline(3)]
   private string[] lines;


   private bool canActivater;
   
   
  [SerializeField]
   private bool savePoint;
   
   
   
   
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        
         if(Input.GetMouseButtonDown(1) && canActivater && !GameManager.instance.dialogBox.activeInHierarchy)
        {                                                                                  
                GameManager.instance.ShowDialog(lines);

                if (savePoint)
                {
                    GameManager.instance.SaveStatuse();
                }
        }   
    }

    
    //when player touches statue canActivor will be true 
    private void OnCollisionEnter2D(Collision2D collision) 
    {

        if(collision.gameObject.tag== "Player")
        {
            canActivater = true;
        }
    }
    

    //when player goes away the canActivator will be false and dialog wont be shown anymor
    private void OnCollisionExit2D(Collision2D collision) 
    {
        if(collision.gameObject.tag== "Player")
        {
            canActivater = false;

            GameManager.instance.ShowDialogChange(canActivater);
            
        }
    }
    
}


