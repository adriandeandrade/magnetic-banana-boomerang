using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeScreen : MonoBehaviour
{
    // Inspector Fields
    [SerializeField] private GameObject upgradeScreenPanel;
    [SerializeField] private GameObject upgradeUIElementPrefab;
    [SerializeField] private Transform gorillaUIElementPanel;
    [SerializeField] private Transform turtleUIElementPanel;
    [SerializeField] private List<StatUpgradeUIElement> upgradesUIElements;
    [SerializeField] private TextMeshProUGUI availableSkillPointsText;
    [SerializeField] private StatManager statManager;
    [SerializeField] private WaveSpawner waveSpawner;

    // Private Variables
    private bool menuOpen = false;
    private Inventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        InitializeUpgrades();
    }

    private void Update()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();

        if(waveSpawner != null)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && !menuOpen && waveSpawner.Intermission)
            {
                OpenUpgradeScreen();
            }
            else if (Input.GetKeyDown(KeyCode.Tab) && menuOpen)
            {
                CloseUpgradeScreen();
            }

            if(menuOpen && !waveSpawner.Intermission)
            {
                CloseUpgradeScreen();
            }
        }
    }
    private void InitializeUpgrades()
    {
        foreach (Stat upgrade in statManager.startingStats)
        {
            GameObject newUpgradeElement = Instantiate(upgradeUIElementPrefab);

            switch(upgrade.statTarget)
            {
                case StatTarget.GORILLA:
                    newUpgradeElement.transform.parent = gorillaUIElementPanel.transform;
                    newUpgradeElement.transform.localScale = Vector3.one;
                    break;

                case StatTarget.TURTLE:
                    newUpgradeElement.transform.parent = turtleUIElementPanel.transform;
                    newUpgradeElement.transform.localScale = Vector3.one;
                    break;
            }

            StatUpgradeUIElement newElement = newUpgradeElement.GetComponent<StatUpgradeUIElement>();
            newElement.InitUpgrade(upgrade);
            newElement.upgradeButton.onClick.AddListener(delegate { UpgradeStat(newElement); });

            if(!upgradesUIElements.Contains(newElement))
            {
                upgradesUIElements.Add(newElement);
            }
        }
    }

    public void OpenUpgradeScreen()
    {
        menuOpen = true;
        upgradeScreenPanel.SetActive(true);
        UpdateUpgradeScreen();
    }

    public void CloseUpgradeScreen()
    {
        menuOpen = false;
        upgradeScreenPanel.SetActive(false);
    }

    public void UpdateUpgradeScreen()
    {
        foreach(StatUpgradeUIElement stat in upgradesUIElements)
        {
            if (inventory.SkillPoints >= stat.associatedStat.currentCost)
            {
                stat.EnableUpgrade();
            }
            else
            {
                stat.DisableUpgrade();
            }
        }

        availableSkillPointsText.SetText(inventory.SkillPoints.ToString());
    }
    public void UpgradeStat(StatUpgradeUIElement statToUpgrade)
    {
        inventory.RemoveSkillPoint(statToUpgrade.associatedStat.currentCost);
        statManager.UpgradeStat(statToUpgrade.associatedStat.lookupName);
        statToUpgrade.UpdateUpgradeElement();
        UpdateUpgradeScreen();
        Debug.Log("Upgraded");
    }
}
