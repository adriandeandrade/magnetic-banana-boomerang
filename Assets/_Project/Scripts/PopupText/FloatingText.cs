using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
	// Inspector Fields
	[SerializeField] private Animator animator;

	// Private Variables
	private TextMeshProUGUI damageText;

	private void Awake()
	{
		damageText = animator.GetComponent<TextMeshProUGUI>();
	}

	private void Start()
	{
		AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
		Destroy(gameObject, clipInfo[0].clip.length);

		damageText = animator.GetComponent<TextMeshProUGUI>();
	}

	public void SetText(string text)
	{
		damageText.SetText(text);
	}
}
