using System;

using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace ConjurerDice
{
    public class TurnDebugUI : MonoBehaviour
    {
        [SerializeField] private IntEventChannelSO startTurnEvent;
        [SerializeField] private IntEventChannelSO endTurnEvent;
        [SerializeField] private TurnPhaseEventChannelSO onPhaseChange;
        public TextMeshProUGUI turnDebugUI;
        public Image turnDebugImage;
        
        private void OnEnable()
        {
            startTurnEvent.OnRaised += HandleTurnStart;
            endTurnEvent.OnRaised += HandleTurnEnd;
            onPhaseChange.OnRaised += HandlePhaseChange;
        }

        private void HandlePhaseChange(TurnPhase arg0)
        {
            Debug.Log($"[UI] Phase changed to: {arg0}");
            
        }

        

        private void HandleTurnEnd(int arg0)
        {
            turnDebugUI.text = $"{arg0}";
            turnDebugImage.color = Color.red;
        }
        

        private void HandleTurnStart(int arg0)
        {
            turnDebugUI.text = $"{arg0}";
            turnDebugImage.color = Color.green;
        }

        private void OnDisable()
        {
            startTurnEvent.OnRaised -= HandleTurnStart;
            endTurnEvent.OnRaised -= HandleTurnEnd;
            onPhaseChange.OnRaised -= HandlePhaseChange;
        }
    }
}
