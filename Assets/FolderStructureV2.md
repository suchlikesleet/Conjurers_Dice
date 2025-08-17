├── Scripts
  ├── App
    ├── Dialogue
    │   ├── DialogueController.cs (DialogueController (StartDialogue(string)))
    ├── Mission
    │   ├── ForkEvaluator.cs (ForkEvaluator (IsVisible(ForkDataSO)))
    │   ├── MissionDirector.cs (MissionDirector (StartChapter(ChapterDataSO)))
    │   ├── RecoveryService.cs (RecoveryService (Awake()))
    ├── Progression
    │   ├── ProgressionManager.cs (ProgressionManager (UnlockMinion(MinionSO)))
    ├── Run
    │   ├── RunEffectsService.cs (RunEffectsService (phase(any)))
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
    │   ├── AbilityEffectSO.cs (AbilityEffectSO (Apply(AbilityCastContext)))
    │   ├── AbilitySO.cs (AbilitySO)
    │   ├── DamageEffectSO.cs (DamageEffectSO (Apply(AbilityCastContext), if(ctx)))
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
    │   ├── PlayerDiceLoadoutSO.cs (PlayerDiceLoadoutSO (dice(definitions), Clear(), TryAdd(DiceDefinitionSO)))
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
    │   ├── ForkConditionSO.cs (ForkConditionSO (IsMet()))
    │   ├── ForkDataSO.cs (ForkDataSO)
    │   ├── RewardTableSO.cs (RewardTableSO)
    ├── Progression
    │   ├── BalanceConfigSO.cs (BalanceConfigSO (value(resets)))
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
  │   ├── EncounterRunner.cs (EncounterRunner (buffs(cleared)))
  │   ├── GameState.cs
  │   ├── GameStateMachine.cs (GameStateMachine)
  ├── Debug
  │   ├── StateDebugEvent.cs (StateDebugEvent (CallStateDebug()))
  │   ├── StateDebugUI.cs (StateDebugUI (Awake()))
  │   ├── TurnDebugButtons.cs (TurnDebugButtons (Reset()))
  │   ├── TurnDebugUI.cs (TurnDebugUI (OnEnable()))
  ├── Dice
    ├── Customize
      ├── Constraints
      │   ├── DiceConstraintSO.cs (DiceConstraintSO (die(always), reason(shown), Validate(PlayerDiceLoadoutSO, DiceDefinitionSO), ValidateFinal(PlayerDiceLoadoutSO)))
      │   ├── LoadoutValidator.cs (LoadoutValidator (CanAdd(PlayerDiceLoadoutSO, DiceDefinitionSO)))
      │   ├── MaxPerTagConstraint.cs (MaxPerTagConstraint, Rule)
      │   ├── RequireNumberFaceConstraint.cs (RequireNumberFaceConstraint (Face(Min), Validate(PlayerDiceLoadoutSO, DiceDefinitionSO), ValidateFinal(PlayerDiceLoadoutSO)))
    ├── Modifiers
    │   ├── BlindController.cs (BlindController)
    │   ├── PremonitionController.cs (PremonitionController)
  │   ├── AbilityResolver.cs (AbilityResolver (Awake()))
  │   ├── DiceEngine.cs (DiceEngine (RollFromPool(), if(!_playerPhase)))
  │   ├── DicePoolManager.cs (DicePoolManager (BuildEncounterPool()))
  │   ├── DiceResolver.cs (DiceResolver (Resolve(int, DiceFaceSO)))
  │   ├── DiceRoller.cs (DiceRoller (Roll(DiceSO)))
  │   ├── MinionResolver.cs (MinionResolver (Resolve(int, DiceFaceSO), TileRef(lane, index), if(IsTileOccupied(spawn)))
  │   ├── NumberResolver.cs (NumberResolver (Resolve(int, DiceFaceSO), lane(closest)))
  ├── Save
  │   ├── SaveService.cs (SaveService)
  ├── Simulation
    ├── Abilities
    │   ├── AbilityCastContext.cs (AbilityCastContext)
    │   ├── AbilityQueue.cs (AbilityQueue (Enqueue(AbilityCastContext)))
    │   ├── AbilityService.cs (AbilityService (TryCast(AbilitySO, AbilityCastContext), check(lane)))
    │   ├── TargetFinder.cs (TargetFinder (FindNearestEnemyInLane(int)))
    │   ├── TargetingReasonText.cs (TargetingReasonText (ToText(TargetInvalidReason)))
    │   ├── TargetingRules.cs (TargetingRules)
    ├── Board
    │   ├── BoardGrid.cs (BoardGrid (TryGetFrontSpawn(int, out), TryGetBackSpawn(int, out), IsInBounds(TileRef), GetTile(TileRef)))
    │   ├── BoardGrid.Gizmos.cs (BoardGrid (Color(0.25f, 0.8f, 1f, 0.5f), Color(0.2f, 1f, 0.4f, 0.35f), Color(1f, 0.5f, 0.1f, 0.9f), Color(0.8f, 0.8f, 0.8f, 0.9f), TileCenter(int, int), Vector3(lane, 0f, index)))
    │   ├── BoardGrid.Helpers.cs (BoardGrid (unit(ally), GetUnitAt(int, int)))
    │   ├── HazardSystem.cs (HazardSystem (Place(TileRef, HazardDefinitionSO)))
    │   ├── LaneController.cs (LaneController (helpers(FirstFreeFromBack, IsBlocked, etc.)))
    │   ├── TileController.cs (TileController)
    │   ├── TileRef.cs
    ├── Combat
    │   ├── DisplacementSystem.cs (DisplacementSystem (ChainPushAlliesFrom(TileRef)))
    ├── Turns
    │   ├── ManaSystem.cs (ManaSystem (SetMana(int)))
    │   ├── TurnController.cs (TurnController (transitions(enemy)))
    │   ├── TurnPhase.cs
    │   ├── TurnPhaseEventChannelSO.cs (TurnPhaseEventChannelSO (Raise(TurnPhase)))
    ├── Units
    │   ├── EnemyActorAdapter.cs (EnemyActorAdapter (EnemyActorAdapter(EnemyController)))
    │   ├── EnemyController.cs (EnemyController (Teleport(TileRef)))
    │   ├── Interfaces.cs
    │   ├── MinionController.cs (MinionController (Teleport(TileRef)))
    │   ├── MovementSystem.cs (MovementSystem (CalculateMovement(int, UnitStats)))
    │   ├── StatusSystem.cs (StatusSystem (TickStartPhase()))
    │   ├── UnitStats.cs
  ├── UI
    ├── Dev
    │   ├── LogConsole.cs (LogConsole (Log(string)))
    ├── Dice
    │   ├── BlindMask.cs (BlindMask)
    │   ├── DiceCardWidget.cs (DiceCardWidget (Bind(DiceDefinitionSO, Action, bool), onClick()))
    │   ├── DicePanel.cs (DicePanel (OnEnable()))
    │   ├── DicePickerUI.cs (DicePickerUI (OnEnable()))
    │   ├── PremonitionHint.cs (PremonitionHint)
    ├── HUD
    │   ├── ManaHUD.cs (ManaHUD (Awake()))
    │   ├── TurnBanner.cs (TurnBanner (OnEnable()))
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
    │   ├── NewInputProvider_Targeting.cs (NewInputProvider_Targeting (OnPoint(InputAction.CallbackContext)))
    │   ├── PreviewHighlighter.cs (PreviewHighlighter (ShowPreview(List<GameObject>, bool)))
    │   ├── TargetCandidate.cs (TargetCandidate (coords(runtime), LateUpdate()))
    │   ├── TargetCandidate.Gizmos.cs (TargetCandidate (Color(1f, 1f, 0f, 0.75f), OnDrawGizmosSelected()))
    │   ├── TargetingController.cs (TargetingController (Awake()))
    │   ├── TargetingReticle.cs (TargetingReticle (AttachTo(Transform)))
  ├── Util
    ├── Debug
    │   ├── BalanceProbe.cs (BalanceProbe (RunAutoSim(int)))
    │   ├── DiceDebugButtons.cs (DiceDebugButtons (Reset()))
    │   ├── EncounterSandbox.cs (EncounterSandbox (SpawnTest()))
    │   ├── GodModeToggles.cs (GodModeToggles)
