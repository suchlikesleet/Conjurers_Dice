using Obvious.Soap;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ConjurerDice
{
    public class DicePickerUI : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private DiceLibrarySO library;
        [SerializeField] private PlayerDiceLoadoutSO playerLoadout;

        [Header("Events")]
        [SerializeField] private OpenDicePickerEventSO openPickerEvent;
        [SerializeField] private DiceLoadoutConfirmedEventSO loadoutConfirmedEvent;

        [Header("UI")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform libraryGridRoot;
        [SerializeField] private Transform chosenGridRoot;
        [SerializeField] private DiceCardWidget cardPrefab;
        [SerializeField] private TMP_Text counterText;
        [SerializeField] private Button confirmButton;

        private int _chapter;

        private void OnEnable()
        {
            openPickerEvent.OnRaised += Open;
            panel.SetActive(false);
            confirmButton.onClick.AddListener(Confirm);
        }

        private void OnDisable()
        {
            openPickerEvent.OnRaised -= Open;
            confirmButton.onClick.RemoveListener(Confirm);
        }

        private void Open(int chapter)
        {
            _chapter = chapter;
            panel.SetActive(true);
            Rebuild();
        }

        private void Rebuild()
        {
            Clear(libraryGridRoot);
            Clear(chosenGridRoot);

            var unlocked = library.GetUnlockedForChapter(_chapter);

            foreach (var def in unlocked)
            {
                var card = Instantiate(cardPrefab, libraryGridRoot);
                card.Bind(def, onClick: () =>
                {
                    if (playerLoadout.TryAdd(def))
                        Rebuild();
                });
            }

            for (int i = 0; i < playerLoadout.chosenDice.Count; i++)
            {
                var def = playerLoadout.chosenDice[i];
                var card = Instantiate(cardPrefab, chosenGridRoot);
                int idx = i;
                card.Bind(def, onClick: () =>
                {
                    playerLoadout.RemoveAt(idx);
                    Rebuild();
                }, isSelected:true);
            }

            counterText.text = $"{playerLoadout.chosenDice.Count} / {playerLoadout.maxDice}";
            confirmButton.interactable = playerLoadout.chosenDice.Count == playerLoadout.maxDice;
        }

        

        private void Clear(Transform root)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
                Destroy(root.GetChild(i).gameObject);
        }
        
        [SerializeField] private LoadoutValidator validator;
        [SerializeField] private ScriptableEventString toast;

        void OnAddDie(DiceDefinitionSO def)
        {
            var why = validator.CanAdd(playerLoadout, def);
            if (!string.IsNullOrEmpty(why)) { toast?.Raise(why); return; }

            if (playerLoadout.TryAdd(def)) Rebuild();
            else toast?.Raise("Can't add that die.");
        }

        void Confirm()
        {
            var err = validator.CanConfirm(playerLoadout);
            if (!string.IsNullOrEmpty(err)) { toast?.Raise(err); return; }

            panel.SetActive(false);
            loadoutConfirmedEvent.Raise();
        }
    }
}
