using System;
using TMPro;
using UnityEngine;

namespace ConjurerDice
{
    public class StateDebugUI : MonoBehaviour
    {
        [SerializeField] private GameStateEventChannelSO gameStateChanged;

        public TextMeshProUGUI debugText;

        private void Awake()
        {
            if (debugText==null)
            {
                debugText = GetComponent<TextMeshProUGUI>();
            }
            
        }

        private void OnEnable()
        {
            gameStateChanged.OnEventRaised += HandleStateChange;
        }

        private void OnDisable()
        {
            gameStateChanged.OnEventRaised -= HandleStateChange;
        }

        private void HandleStateChange(GameState state)
        {
            //Debug.Log($"[UI] State changed to: {state}");
            debugText.text = $"State changed to: {state}";
        }
    }
}