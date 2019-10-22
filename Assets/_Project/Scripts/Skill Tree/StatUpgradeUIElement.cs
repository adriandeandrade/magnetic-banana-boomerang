using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatUpgradeUIElement : MonoBehaviour
{
	// Inspector Fields
	public Button upgradeButton;
	[SerializeField] private TextMeshProUGUI upgradeAmountText;
    [SerializeField] private TextMeshProUGUI upgradeTitle;
	[SerializeField] private Image upgradePanelBackground;
	[SerializeField] private Color disabledColor;
	public Stat associatedStat;

	// Private Variables
	private Color originalPanelColor;

	private void Awake()
	{
		originalPanelColor = upgradePanelBackground.color;
        DisableUpgrade();
	}
    public void InitUpgrade(Stat _associatedStat)
    {
        associatedStat = _associatedStat;
        upgradeAmountText.SetText($"+{associatedStat.upgradeValue}");
        upgradeTitle.SetText($"{associatedStat.statTitle}[{associatedStat.currentCost}]");
    }

	public void EnableUpgrade()
	{
		upgradeButton.interactable = true;
		upgradePanelBackground.color = originalPanelColor;
	}

	public void DisableUpgrade()
	{
		upgradeButton.interactable = false;
		upgradePanelBackground.color = disabledColor;
	}
    public void UpdateUpgradeElement()
    {
        upgradeAmountText.SetText($"+{associatedStat.upgradeValue}");
        upgradeTitle.SetText($"{associatedStat.statTitle}[{associatedStat.currentCost}]");
    }
}
