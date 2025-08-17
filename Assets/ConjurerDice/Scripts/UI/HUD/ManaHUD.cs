using UnityEngine;
using TMPro;

namespace ConjurerDice
{
    public class ManaHUD : MonoBehaviour
    {
        [SerializeField] private ManaSystem mana;
        [SerializeField] private IntEventChannelSO onManaChanged;
        [SerializeField] private TMP_Text label;

        private void Awake()
        {
            if (!label) label = GetComponent<TMP_Text>();
            if (!mana) mana = FindFirstObjectByType<ManaSystem>();
        }

        private void OnEnable()
        {
            if (onManaChanged) onManaChanged.OnRaised += UpdateView;
            UpdateView(mana ? mana.CurrentMana : 0);
        }
        private void OnDisable()
        {
            if (onManaChanged) onManaChanged.OnRaised -= UpdateView;
        }
        private void UpdateView(int v)
        {
            if (label) label.text = $"Mana: {v}";
        }
    }
}