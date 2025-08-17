├── Scripts
  ├── App
    ├── Dialogue
    │   ├── DialogueController.cs (DialogueController)
    ├── Mission
    │   ├── ForkEvaluator.cs (ForkEvaluator)
    │   ├── MissionDirector.cs (MissionDirector)
    │   ├── RecoveryService.cs (RecoveryService)
    ├── Progression
    │   ├── ProgressionManager.cs (ProgressionManager)
    ├── Run
    │   ├── RunEffectsService.cs (RunEffectsService)
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
    │   ├── PlayerDiceLoadoutSO.cs (PlayerDiceLoadoutSO)
    ├── Events
    │   ├── AbilityCastEventChannelSO.cs (AbilityCastEventChannelSO)
    │   ├── DiceFaceEventChannelSO.cs (DiceFaceEventChannelSO)
    │   ├── EncounterResultEventChannelSO.cs (EncounterResultEventChannelSO)
    │   ├── GameStateEventChannelSO.cs (GameStateEventChannelSO)
    │   ├── HazardEventChannelSO.cs (HazardEventChannelSO)
    │   ├── InputBusTargetingSO.cs (InputBusTargetingSO)
    │   ├── IntEventChannelSO.cs (IntEventChannelSO)
    │   ├── LaneEventChannelSO.cs (LaneEventChannelSO)
    │   ├── OpenDicePickerEventSO.cs (OpenDicePickerEventSO, DiceLoadoutConfirmedEventSO)
    │   ├── TargetingFeedbackEventChannelSO.cs (TargetingFeedbackEventChannelSO)
    │   ├── TargetingRequestChannelSO.cs (TargetingRequestChannelSO)
    │   ├── TargetingResultEventChannelSO.cs (TargetingResultEventChannelSO)
    │   ├── UnitRefEventChannelSO.cs (UnitRefEventChannelSO)
    │   ├── VoidEventChannelSO.cs (VoidEventChannelSO)
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
  │   ├── Bootstrap.cs (Bootstrap)
  │   ├── EncounterRunner.cs (EncounterRunner)
  │   ├── GameState.cs
  │   ├── GameStateMachine.cs (GameStateMachine)
  ├── Debug
  │   ├── StateDebugEvent.cs (StateDebugEvent)
  │   ├── StateDebugUI.cs (StateDebugUI)
  │   ├── TurnDebugButtons.cs (TurnDebugButtons)
  │   ├── TurnDebugUI.cs (TurnDebugUI)
  ├── Dice
    ├── Customize
      ├── Constraints
      │   ├── DiceConstraintSO.cs (DiceConstraintSO)
      │   ├── LoadoutValidator.cs (LoadoutValidator)
      │   ├── MaxPerTagConstraint.cs (MaxPerTagConstraint, Rule)
      │   ├── RequireNumberFaceConstraint.cs (RequireNumberFaceConstraint)
    ├── Modifiers
    │   ├── BlindController.cs (BlindController)
    │   ├── PremonitionController.cs (PremonitionController)
  │   ├── AbilityResolver.cs (AbilityResolver)
  │   ├── DiceEngine.cs (DiceEngine)
  │   ├── DicePoolManager.cs (DicePoolManager)
  │   ├── DiceResolver.cs (DiceResolver)
  │   ├── DiceRoller.cs (DiceRoller)
  │   ├── MinionResolver.cs (MinionResolver)
  │   ├── NumberResolver.cs (NumberResolver)
  ├── Save
  │   ├── SaveService.cs (SaveService)
  ├── Simulation
    ├── Abilities
    │   ├── AbilityCastContext.cs (AbilityCastContext)
    │   ├── AbilityQueue.cs (AbilityQueue)
    │   ├── AbilityService.cs (AbilityService)
    │   ├── TargetFinder.cs (TargetFinder)
    │   ├── TargetingReasonText.cs (TargetingReasonText)
    │   ├── TargetingRules.cs (TargetingRules)
    ├── Board
    │   ├── BoardGrid.cs (BoardGrid)
    │   ├── BoardGrid.Gizmos.cs (BoardGrid)
    │   ├── BoardGrid.Helpers.cs (BoardGrid)
    │   ├── HazardSystem.cs (HazardSystem)
    │   ├── LaneController.cs (LaneController)
    │   ├── TileController.cs (TileController)
    │   ├── TileRef.cs
    ├── Combat
    │   ├── DisplacementSystem.cs (DisplacementSystem)
    ├── Turns
    │   ├── ManaSystem.cs (ManaSystem)
    │   ├── TurnController.cs (TurnController)
    │   ├── TurnPhase.cs
    │   ├── TurnPhaseEventChannelSO.cs (TurnPhaseEventChannelSO)
    ├── Units
    │   ├── EnemyActorAdapter.cs (EnemyActorAdapter)
    │   ├── EnemyController.cs (EnemyController)
    │   ├── Interfaces.cs
    │   ├── MinionController.cs (MinionController)
    │   ├── MovementSystem.cs (MovementSystem)
    │   ├── StatusSystem.cs (StatusSystem)
    │   ├── UnitStats.cs
  ├── UI
    ├── Dev
    │   ├── LogConsole.cs (LogConsole)
    ├── Dice
    │   ├── BlindMask.cs (BlindMask)
    │   ├── DiceCardWidget.cs (DiceCardWidget)
    │   ├── DicePanel.cs (DicePanel)
    │   ├── DicePickerUI.cs (DicePickerUI)
    │   ├── PremonitionHint.cs (PremonitionHint)
    ├── HUD
    │   ├── ManaHUD.cs (ManaHUD)
    │   ├── TurnBanner.cs (TurnBanner)
    ├── Lane
    │   ├── LaneSelector.cs (LaneSelector)
    ├── Map
    │   ├── ChapterMapUI.cs (ChapterMapUI)
    ├── Popups
    │   ├── RecoveryPopup.cs (RecoveryPopup)
    │   ├── RewardPicker.cs (RewardPicker)
    ├── Settings
    │   ├── AccessibilityMenu.cs (AccessibilityMenu)
    ├── Targeting
    │   ├── IInputTargeting.cs
    │   ├── NewInputProvider_Targeting.cs (NewInputProvider_Targeting)
    │   ├── PreviewHighlighter.cs (PreviewHighlighter)
    │   ├── TargetCandidate.cs (TargetCandidate)
    │   ├── TargetCandidate.Gizmos.cs (TargetCandidate)
    │   ├── TargetingController.cs (TargetingController)
    │   ├── TargetingReticle.cs (TargetingReticle)
  ├── Util
    ├── Debug
    │   ├── BalanceProbe.cs (BalanceProbe)
    │   ├── DiceDebugButtons.cs (DiceDebugButtons)
    │   ├── EncounterSandbox.cs (EncounterSandbox)
    │   ├── GodModeToggles.cs (GodModeToggles)
