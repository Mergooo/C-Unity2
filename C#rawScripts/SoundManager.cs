using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{



    public static SoundManager instance;


    //array with with the sound effects which are added in editor 
    public AudioSource[] se;
    
    
    
    
    
    //instantiate Sound object
    private void Awake() 
    {
        if (instance == null)
        {
            instance = this; 
        }
        else if (instance!= this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        
        
    }
    
    
    /// <summary>
    ///  when called, sound effect with the respective number of "se[]" will be played
    /// </summary>
    /// <param name="x"></param>

  public void PlaySE(int x)
  {
    se[x].Stop();

    se[x].Play();
  }
  
 
}
