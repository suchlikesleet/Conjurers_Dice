using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace ConjurerDice
{
    public class DiceCardWidget : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text title;
        [SerializeField] private Button button;
        [SerializeField] private GameObject selectedMark;

        public void Bind(DiceDefinitionSO def, Action onClick, bool isSelected = false)
        {
            if (icon)  icon.sprite = def.icon;
            if (title) title.text  = def.displayName;
            if (selectedMark) selectedMark.SetActive(isSelected);

            button.onClick.RemoveAllListeners();
            if (onClick != null) button.onClick.AddListener(() => onClick());
        }
    }
}