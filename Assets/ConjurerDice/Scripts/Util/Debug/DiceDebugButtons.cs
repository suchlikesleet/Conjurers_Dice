using UnityEngine;

namespace ConjurerDice
{
    public class DiceDebugButtons : MonoBehaviour
    {
        [SerializeField] private DiceEngine engine;
        [SerializeField] private DiceSO testDice;

        private void Reset()
        {
            if (engine == null) engine = FindFirstObjectByType<DiceEngine>();
        }

        public void ClickRoll()
        {
            engine?.RollSelected(testDice);
        }
    }
}