├── Scripts
  ├── App
    ├── Dialogue
    │   ├── DialogueController.cs (DialogueController (StartDialogue(string)))
    ├── Mission
    │   ├── ForkEvaluator.cs (ForkEvaluator (IsVisible(ForkDataSO)))
    │   ├── MissionDirector.cs (MissionDirector (StartChapter(ChapterDataSO), ChooseFork(int), EndChapter()))
    │   ├── RecoveryService.cs (RecoveryService (Awake(), GrantManaSpring(int, int), ApplyManaDrought(int, int), HealAll(), BoostMana(int), CleanseCurse()))
    ├── Progression
    │   ├── ProgressionManager.cs (ProgressionManager (UnlockMinion(MinionSO), SaveProgress()))
    ├── Run
    │   ├── RunEffectsService.cs (RunEffectsService (AddManaPerTurn(int, int), GetActiveManaPerTurnDelta(), DecayOneTurn(), Save(), Load(), Awake()))
  ├── Asmdef
    ├── App
    │   ├── ConjurerDice.App.asmdef
    ├── Content
    │   ├── ConjurerDice.Content.asmdef
    ├── Core
    │   ├── ConjurerDice.Core.asmdef
    ├── Dice
    │   ├── ConjurerDice.Dice.asmdef
    ├── Simulation
    │   ├── ConjurerDice.Simulation.asmdef
    ├── UI
    │   ├── ConjurerDice.UI.asmdef
  ├── Content
    ├── Abilities
    │   ├── AbilityEffectSO.cs (AbilityEffectSO)
    │   ├── AbilitySO.cs (AbilitySO)
    │   ├── DamageEffectSO.cs (DamageEffectSO)
    ├── Board
    │   ├── BoardConfigSO.cs (BoardConfigSO)
    │   ├── HazardDefinitionSO.cs (HazardDefinitionSO)
    │   ├── TileEffectSO.cs (TileEffectSO)
    ├── Dice
    │   ├── DiceDefinitionSO.cs (DiceDefinitionSO)
    │   ├── DiceFaceSO.cs (DiceFaceSO)
    │   ├── DiceLibrarySO.cs (DiceLibrarySO)
    │   ├── DiceSO.cs (DiceSO)
    │   ├── DiceUpgradeSO.cs (DiceUpgradeSO)
    │   ├── PlayerDiceLoadoutSO.cs (PlayerDiceLoadoutSO (Clear(), TryAdd(DiceDefinitionSO), RemoveAt(int)))
    ├── Events
    │   ├── AbilityCastEventChannelSO.cs (AbilityCastEventChannelSO (Raise(AbilityCastContext)))
    │   ├── DiceFaceEventChannelSO.cs (DiceFaceEventChannelSO (Raise(DiceFaceSO)))
    │   ├── EncounterResultEventChannelSO.cs (EncounterResultEventChannelSO (Raise(EncounterResult)))
    │   ├── GameStateEventChannelSO.cs (GameStateEventChannelSO (Raise(GameState)))
    │   ├── HazardEventChannelSO.cs (HazardEventChannelSO (Raise(HazardInfo)))
    │   ├── InputBusTargetingSO.cs (InputBusTargetingSO (RaisePointerMoved(Vector2), RaiseConfirm(), RaiseCancel(), RaiseReset(), RaiseNavigate(int, int)))
    │   ├── IntEventChannelSO.cs (IntEventChannelSO (Raise(int)))
    │   ├── LaneEventChannelSO.cs (LaneEventChannelSO (Raise(int)))
    │   ├── OpenDicePickerEventSO.cs (OpenDicePickerEventSO (Raise(int)), DiceLoadoutConfirmedEventSO (Raise()))
    │   ├── TargetingFeedbackEventChannelSO.cs (TargetingFeedbackEventChannelSO (Raise(string)))
    │   ├── TargetingRequestChannelSO.cs (TargetingRequestChannelSO (Raise(AbilitySO, int?)))
    │   ├── TargetingResultEventChannelSO.cs (TargetingResultEventChannelSO (Raise(List<GameObject>, TileRef?)))
    │   ├── UnitRefEventChannelSO.cs (UnitRefEventChannelSO (Raise(UnitRef)))
    │   ├── VoidEventChannelSO.cs (VoidEventChannelSO (Raise()))
    ├── Items
    ├── Mission
    │   ├── ChapterDataSO.cs (ChapterDataSO)
    │   ├── EncounterSO.cs (EncounterSO)
    │   ├── ForkConditionSO.cs (ForkConditionSO)
    │   ├── ForkDataSO.cs (ForkDataSO)
    │   ├── RewardTableSO.cs (RewardTableSO)
    ├── Progression
    │   ├── BalanceConfigSO.cs (BalanceConfigSO)
    │   ├── ConjurerSkillSO.cs (ConjurerSkillSO)
    │   ├── MetaProfileSO.cs (MetaProfileSO)
    │   ├── RelicSO.cs (RelicSO)
    │   ├── UnlockTableSO.cs (UnlockTableSO)
    ├── Units
    │   ├── EnemySO.cs (EnemySO)
    │   ├── MinionSO.cs (MinionSO)
    │   ├── StatusEffectSO.cs (StatusEffectSO)
    │   ├── TagSetSO.cs (TagSetSO)
  ├── Core
  │   ├── Bootstrap.cs (Bootstrap (Awake()))
  │   ├── EncounterRunner.cs (EncounterRunner (OnEnable(), OnDisable(), StartEncounter(EncounterSO), BuildBoard(EncounterSO), SpawnEnemies(EncounterSO), RegisterAlliesInScene(), HandlePhase(TurnPhase), OnStartPhase(), UpdateMana(), OnPlayerPhase(), OnEnemyPhase(), OnEndPhase(), if(IsDefeated(), SetPhasePlayer(), IsVictorious(), IsDefeated(), AddTempEncounterManaBuff(int, int), SumEncounterBuffs(), DecayEncounterBuffsOneTurn(), HandleLoadoutConfirmed(), CompleteEncounter(bool)))
  │   ├── GameState.cs
  │   ├── GameStateMachine.cs (GameStateMachine (Start(), ChangeState(GameState)))
  ├── Debug
  │   ├── StateDebugEvent.cs (StateDebugEvent (CallStateDebug()))
  │   ├── StateDebugUI.cs (StateDebugUI (Awake(), OnEnable(), OnDisable(), HandleStateChange(GameState)))
  │   ├── TurnDebugButtons.cs (TurnDebugButtons (Reset(), ClickEndPlayerPhase(), StartEncounter(), AddRunBuff_Plus2_3Turns(), AddRunCurse_Minus1_2Turns(), SpendMana2()))
  │   ├── TurnDebugUI.cs (TurnDebugUI (OnEnable(), HandlePhaseChange(TurnPhase), HandleTurnEnd(int), HandleTurnStart(int), OnDisable()))
  ├── Dice
    ├── Customize
      ├── Constraints
      │   ├── DiceConstraintSO.cs (DiceConstraintSO)
      │   ├── LoadoutValidator.cs (LoadoutValidator (CanAdd(PlayerDiceLoadoutSO, DiceDefinitionSO), CanConfirm(PlayerDiceLoadoutSO)))
      │   ├── MaxPerTagConstraint.cs (MaxPerTagConstraint, Rule)
      │   ├── RequireNumberFaceConstraint.cs (RequireNumberFaceConstraint (IsNumberFace(DiceFaceSO)))
    ├── Modifiers
    │   ├── BlindController.cs (BlindController)
    │   ├── PremonitionController.cs (PremonitionController)
  │   ├── AbilityResolver.cs (AbilityResolver (Awake(), OnEnable(), OnDisable(), Resolve(int, DiceFaceSO), NeedsTarget(AbilitySO), OnTargetingResult(List<GameObject>, TileRef?)))
  │   ├── DiceEngine.cs (DiceEngine (RollFromPool(), OnEnable(), OnDisable(), HandlePhase(TurnPhase), SetLane(int), RollSelected(DiceSO)))
  │   ├── DicePoolManager.cs (DicePoolManager (BuildEncounterPool(), GetActiveDice()))
  │   ├── DiceResolver.cs (DiceResolver (Resolve(int, DiceFaceSO)))
  │   ├── DiceRoller.cs (DiceRoller (Roll(DiceSO)))
  │   ├── MinionResolver.cs (MinionResolver (Resolve(int, DiceFaceSO), IsTileOccupied(TileRef)))
  │   ├── NumberResolver.cs (NumberResolver (Resolve(int, DiceFaceSO)))
  ├── Save
  │   ├── SaveService.cs (SaveService)
  ├── Simulation
    ├── Abilities
    │   ├── AbilityCastContext.cs (AbilityCastContext)
    │   ├── AbilityQueue.cs (AbilityQueue (Enqueue(AbilityCastContext), ResolveAll()))
    │   ├── AbilityService.cs (AbilityService (TryCast(AbilitySO, AbilityCastContext)))
    │   ├── TargetFinder.cs (TargetFinder (FindNearestEnemyInLane(int)))
    │   ├── TargetingReasonText.cs (TargetingReasonText (ToText(TargetInvalidReason)))
    │   ├── TargetingRules.cs (TargetingRules (Awake(), ValidateUnit(AbilitySO, int, GameObject), ValidateTile(AbilitySO, int, TileRef), CollectAffected(AbilitySO, int, int, GameObject), TryAdd(int, int), UnitOn(int, int)))
    ├── Board
    │   ├── BoardGrid.cs (BoardGrid (Awake(), TryGetFrontSpawn(int, out), TryGetBackSpawn(int, out), IsInBounds(TileRef), GetTile(TileRef)))
    │   ├── BoardGrid.Gizmos.cs (BoardGrid (TileCenter(int, int), OnDrawGizmos(), OnDrawGizmosSelected()))
    │   ├── BoardGrid.Helpers.cs (BoardGrid (GetUnitAt(int, int), IsTileOccupied(TileRef), UnitsInLane(int, bool, bool), FrontIndexForLane(int), BackIndexForLane(int)))
    │   ├── HazardSystem.cs (HazardSystem (Place(TileRef, HazardDefinitionSO), TickStartPhase(), TickEndPhase()))
    │   ├── LaneController.cs (LaneController)
    │   ├── TileController.cs (TileController)
    │   ├── TileRef.cs
    ├── Combat
    │   ├── DisplacementSystem.cs (DisplacementSystem (ChainPushAlliesFrom(TileRef), PushUnit(GameObject, int, int)))
    ├── Turns
    │   ├── ManaSystem.cs (ManaSystem (SetMana(int), GainMana(int), SpendMana(int)))
    │   ├── TurnController.cs (TurnController (BeginEncounter(), EndPlayerPhase(), SetPhase(TurnPhase), RunEnemyPhaseThenEnd()))
    │   ├── TurnPhase.cs
    │   ├── TurnPhaseEventChannelSO.cs (TurnPhaseEventChannelSO (Raise(TurnPhase)))
    ├── Units
    │   ├── EnemyActorAdapter.cs (EnemyActorAdapter (EnemyActorAdapter(EnemyController), TickEnemyTurn(IBoardGrid)))
    │   ├── EnemyController.cs (EnemyController (Teleport(TileRef), TakeDamage(int, object), ApplyStatus(StatusEffectSO), LaneIsClogged(), FindBestTargetInLane()))
    │   ├── Interfaces.cs
    │   ├── MinionController.cs (MinionController (Teleport(TileRef), TakeDamage(int, object), ApplyStatus(StatusEffectSO)))
    │   ├── MovementSystem.cs (MovementSystem (CalculateMovement(int, UnitStats), MoveByPoints(MonoBehaviour, int, int)))
    │   ├── StatusSystem.cs (StatusSystem (TickStartPhase(), TickEndPhase()))
    │   ├── UnitStats.cs
  ├── UI
    ├── Dev
    │   ├── LogConsole.cs (LogConsole (Log(string)))
    ├── Dice
    │   ├── BlindMask.cs (BlindMask)
    │   ├── DiceCardWidget.cs (DiceCardWidget (Bind(DiceDefinitionSO, Action, bool)))
    │   ├── DicePanel.cs (DicePanel (OnEnable(), OnDisable(), ShowFace(DiceFaceSO)))
    │   ├── DicePickerUI.cs (DicePickerUI (OnEnable(), OnDisable(), Open(int), Rebuild(), Clear(Transform), OnAddDie(DiceDefinitionSO), Confirm()))
    │   ├── PremonitionHint.cs (PremonitionHint)
    ├── HUD
    │   ├── ManaHUD.cs (ManaHUD (Awake(), OnEnable(), OnDisable(), UpdateView(int)))
    │   ├── TurnBanner.cs (TurnBanner (OnEnable(), OnDisable(), UpdateBanner(int)))
    ├── Lane
    │   ├── LaneSelector.cs (LaneSelector (SelectLane(int)))
    ├── Map
    │   ├── ChapterMapUI.cs (ChapterMapUI (ShowChapter(ChapterDataSO)))
    ├── Popups
    │   ├── RecoveryPopup.cs (RecoveryPopup (Open()))
    │   ├── RewardPicker.cs (RewardPicker (Open(RewardTableSO)))
    ├── Settings
    │   ├── AccessibilityMenu.cs (AccessibilityMenu)
    ├── Targeting
    │   ├── IInputTargeting.cs
    │   ├── NewInputProvider_Targeting.cs (NewInputProvider_Targeting (OnPoint(InputAction.CallbackContext), OnClick(InputAction.CallbackContext), OnCancel(InputAction.CallbackContext), OnReset(InputAction.CallbackContext), OnNav(InputAction.CallbackContext)))
    │   ├── PreviewHighlighter.cs (PreviewHighlighter (ShowPreview(List<GameObject>, bool), Clear()))
    │   ├── TargetCandidate.cs (TargetCandidate (LateUpdate(), SetHighlight(bool), SetReticle(bool)))
    │   ├── TargetCandidate.Gizmos.cs (TargetCandidate (OnDrawGizmosSelected()))
    │   ├── TargetingController.cs (TargetingController (Awake(), OnEnable(), OnDisable(), BeginTargeting(AbilitySO, int?), BuildEligible(), ClearEligible(), OnPointerMoved(Vector2), OnConfirm(), ConfirmNow(), OnCancel(), OnReset(), ValidateCandidate(TargetCandidate, TileRef?), ShowPrompt(bool)))
    │   ├── TargetingReticle.cs (TargetingReticle (AttachTo(Transform), Hide()))
  ├── Util
    ├── Debug
    │   ├── BalanceProbe.cs (BalanceProbe (RunAutoSim(int)))
    │   ├── DiceDebugButtons.cs (DiceDebugButtons (Reset(), ClickRoll()))
    │   ├── EncounterSandbox.cs (EncounterSandbox (SpawnTest()))
    │   ├── GodModeToggles.cs (GodModeToggles)
