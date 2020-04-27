using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Health and Energy bars")]
    public Image healthBar;
    public Image energyBar;
    public Text currentPlayerName;
    public GameObject currentPlayer;
    public float currentPlayerHealth;
    public float currentPlayerEnergy;
    public float currentPlayerMaxHealth;
    public float currentPlayerMaxEnergy;

    [Header("Stats Display")]
    public Text nameText;
    public Text moveRangeText;
    public Text runRangeText;
    public Text resourcePointsText;
    public Text healthPointsText;
    public Text meleeSkillText;
    public Text rangeSkillText;
    public Text sanityText;
    public Text incomeText;
    public Text exertionPointsText;

    [Header("Current Player")]
    public GameObject highlightedPlayer;
    public int moveRange;
    public int runRange;
    public int resourcePoints;
    public int maxResource;
    public int health;
    public int maxHealth;
    public int melee;
    public int range;
    public int sanity;
    public int income;
    public int exertion;



    // Start is called before the first frame update
    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerName.text = currentPlayer.name;

        currentPlayerMaxHealth = maxHealth;
        currentPlayerMaxEnergy = maxResource;
        currentPlayerHealth = health;
        currentPlayerEnergy = resourcePoints;

        healthBar.fillAmount = currentPlayerHealth/currentPlayerMaxHealth;
        energyBar.fillAmount = currentPlayerEnergy/currentPlayerMaxEnergy;

        Stats();        
    }

    void Stats()
    {
        nameText.text = highlightedPlayer.name;
        moveRangeText.text = "Walk: " + moveRange.ToString();
        runRangeText.text = "Run: " + runRange.ToString();
        resourcePointsText.text = "Energy: " + resourcePoints.ToString() + "/" + maxResource.ToString();
        healthPointsText.text = "Health: " + health.ToString() + "/" + maxHealth.ToString();
        meleeSkillText.text = "Melee Skill: " + melee.ToString();
        rangeSkillText.text = "Range Skill: " + range.ToString();
        sanityText.text = "Sanity: " + sanity.ToString();
        incomeText.text = "Income: " + income.ToString();
        exertionPointsText.text = "Exertion: " + exertion.ToString();

    }
}
