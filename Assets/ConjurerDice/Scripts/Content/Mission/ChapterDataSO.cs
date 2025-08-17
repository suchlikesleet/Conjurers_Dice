using UnityEngine;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/ChapterDataSO")]
    public class ChapterDataSO : ScriptableObject {

public string chapterName;
public EncounterSO[] encounters;
public EncounterSO bossEncounter;

    }
}
