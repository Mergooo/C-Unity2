using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private Slider hpSlider;  // variable for healthslider
    [SerializeField]
    private PlayerController player; // player variable where playercontroller- script will be added

    [SerializeField]
    private Slider staminaSlider;







    //variables for showing of dialog
    public GameObject dialogBox;
    public Text dialogText;
    private string[] dialogLines;
    private int currentLine;
    private bool justStarted;





    public GameObject statusPanel;  // object to show stats of player

    [SerializeField]
    private Text hptext, stText, atText; //stats for statusPanel

    [SerializeField]
    private Weapon weapon;  // weapon of the player-Object



    //variable for level up
    private int totalEXP, //total of accumulated experience points
    currentLV; // current level of player-object

    [SerializeField, Tooltip("required Exp for level up")]
    private int[] requiredExp; // required experience points for next level up



    [SerializeField]
    private GameObject levelUpText; // text, which pops up on level up


    [SerializeField]
    private Canvas canvas; // canvas for hp- and stamina slider, dialogs and statusPanel








    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // at beginning if saveData is available, it will be loaded
    void Start()
    {

        if (PlayerPrefs.HasKey("MaxHP"))
        {
            LoadStatuse();
        }
    }



    void Update()
    {


        // quit game, when escape key is pressed
       if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        //when dialogBox is active and  and right mouseclick is pushed
        //player will start dialog
        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetMouseButtonUp(1))
            {
                SoundManager.instance.PlaySE(4); // dialog sound effect is played
                if (!justStarted)
                {
                    //if the first line is shown, the next line from the array "dialogLines" will be shown
                    currentLine++;

                    if (currentLine >= dialogLines.Length) //is all lines are shown dialogBox wont be shown anymor
                    {
                        dialogBox.SetActive(false);
                    }
                    else
                    {
                        dialogText.text = dialogLines[currentLine];
                    }
                }
                else
                {
                    justStarted = false;
                }
            }
        }

        // when player presses E statusPanel with player stats will be shown
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowStatusPanel();
        }



    }

    //regulates the health slider on screen
    public void UpdateHealthUI()
    {
        hpSlider.maxValue = player.maxHealth;
        hpSlider.value = player.currentHealth;
    }


    //regulates the stamina slider on screen
    public void UpdateStaminaUI()
    {
        staminaSlider.maxValue = player.totalStamina;
        staminaSlider.value = player.currentStamina;
    }









    /// <summary>
    /// string array is given as argument
    /// shows sentences in dialogBox
    /// </summary>
    /// <param name="lines"></param>
    public void ShowDialog(string[] lines)
    {
        dialogLines = lines;

        currentLine = 0;

        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);

        justStarted = true;
    }

    /// <summary>
    /// bool will be given as argument 
    /// activates or deactivates dialogBox
    /// </summary>
    /// <param name="x"></param>
    public void ShowDialogChange(bool x)
    {
        dialogBox.SetActive(x);
    }


    /// <summary>
    /// if saveData are available, they will be loaded
    ///
    /// </summary>
    public void Load()
    {
        SceneManager.LoadScene("Main");
    }


    /// <summary>
    /// stops game time and shows statusPanel
    /// with stats of player
    /// </summary>
    public void ShowStatusPanel()
    {
        statusPanel.SetActive(true);
        Time.timeScale = 0f;
        StatusUpdate();

    }


    /// <summary>
    /// closes statusPanel
    /// </summary>
    public void CloseStatusPanel()
    {
        statusPanel.SetActive(false);
        Time.timeScale = 1f;

    }

    /// <summary>
    /// updates informations of statusPanel
    /// </summary>
    public void StatusUpdate()
    {
        hptext.text = "Health : " + player.maxHealth;
        stText.text = "Stamina : " + player.totalStamina;
        atText.text = "Attack Power : " + weapon.attackDamage;
    }


    /// <summary>
    /// when player defeats enemy 
    /// exp value from anemy will be added
    /// to totalEXP and triggers levelUp
    /// with status update
    /// </summary>
    /// <param name="exp"></param>
    public void AddExp(int exp)
    {
        if (requiredExp.Length <= currentLV)
        {
            return;
        }


        totalEXP += exp;

        if (totalEXP >= requiredExp[currentLV])
        {
            currentLV++;

            player.maxHealth += 5;
            player.totalStamina += 5;
            weapon.attackDamage += 2;

            GameObject levelUp = Instantiate(levelUpText, player.transform.position, Quaternion.identity);
            levelUp.transform.SetParent(player.transform);
            // levelUp.transform.localPosition = player.transform.position + new Vector3(0,100,0);


        }
    }


    /// <summary>
    /// player stats will be saved
    /// </summary>
    public void SaveStatuse()
    {
        PlayerPrefs.SetInt("MaxHP", player.maxHealth);
        PlayerPrefs.SetFloat("MaxSt", player.totalStamina);
        PlayerPrefs.SetInt("At", weapon.attackDamage);

        PlayerPrefs.SetInt("Level", currentLV);
        PlayerPrefs.SetInt("Exp", totalEXP);

    }

    /// <summary>
    /// player stats will be loaded
    /// </summary>
    public void LoadStatuse()
    {
        player.maxHealth = PlayerPrefs.GetInt("MaxHP");
        player.totalStamina = PlayerPrefs.GetFloat("MaxSt");
        weapon.attackDamage = PlayerPrefs.GetInt("At");
        currentLV = PlayerPrefs.GetInt("Level");
        totalEXP = PlayerPrefs.GetInt("Exp");

    }


}




//HHHHHHHHHHHH