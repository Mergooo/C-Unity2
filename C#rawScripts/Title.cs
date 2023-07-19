using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("Main");
        SoundManager.instance.PlaySE(4);
    }
    
    public void toAssets()
    {
        SceneManager.LoadScene("Assets");
        SoundManager.instance.PlaySE(4);
    }
    
    public void toTitle()
    {
        SceneManager.LoadScene("Title");
        SoundManager.instance.PlaySE(4);
    }
    
    
    
    
    
    
    
    
    
    
}
