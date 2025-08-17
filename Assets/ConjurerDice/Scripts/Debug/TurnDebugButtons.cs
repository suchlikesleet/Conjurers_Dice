using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace ConjurerDice
{
    public class TurnDebugButtons : MonoBehaviour
    {
        [SerializeField] private TurnController controller;
        [SerializeField] private EncounterRunner runner;
        
        [SerializeField] private RecoveryService recovery; // optional
        [SerializeField] private DiceEngine dice;
        public List<EncounterSO> testEncounters = new List<EncounterSO>();
        
          

        private void Reset()
        {
            if (controller == null) controller = FindFirstObjectByType<TurnController>();
            if (!runner)   runner   = FindFirstObjectByType<EncounterRunner>();
            if (!recovery) recovery = FindFirstObjectByType<RecoveryService>();
            if (!dice)     dice     = FindFirstObjectByType<DiceEngine>();
        }

        [Button]
        public void ClickEndPlayerPhase()
        {
            controller?.EndPlayerPhase();
        }

        [Button]
        public void StartEncounter()
        {
            runner.StartEncounter(testEncounters[0]);
        }
        [Button]
        public void AddRunBuff_Plus2_3Turns()
        {
            recovery?.GrantManaSpring(+2, 3);
        }
        [Button]
        public void AddRunCurse_Minus1_2Turns()
        {
            recovery?.ApplyManaDrought(1, 2); // minus 1 for 2 turns
        }

        // convenience to spend mana quickly during Player phase
        public void SpendMana2()
        {
            // emulate a cost without rolling (or click your real Roll button)
            var mana = FindFirstObjectByType<ManaSystem>();
            if (mana && mana.SpendMana(2))
                Debug.Log("[Debug] Spent 2 mana");
        }
        
        
    }
}