using UnityEngine;

namespace ConjurerDice {
    public class ManaSystem : MonoBehaviour {

        [SerializeField] private int currentMana = 3;
        [SerializeField] private IntEventChannelSO onManaChanged;
        public int CurrentMana => currentMana;
        
        public void SetMana(int value) { currentMana = Mathf.Max(0, value); onManaChanged?.Raise(currentMana); }
        public void GainMana(int amount) { currentMana += Mathf.Max(0, amount); onManaChanged?.Raise(currentMana); }
        public bool SpendMana(int amount) {
            if (currentMana < amount) return false;
            currentMana -= amount; onManaChanged?.Raise(currentMana); return true;
        }

    }
}
