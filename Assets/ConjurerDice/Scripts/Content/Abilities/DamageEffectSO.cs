namespace ConjurerDice
{
    [UnityEngine.CreateAssetMenu(menuName = "ConjurerDice/Abilities/Effects/Damage")]
    public class DamageEffectSO : AbilityEffectSO
    {
        public int damage = 1;

        public override void Apply(AbilityCastContext ctx)
        {
            // Minimal: apply to first target if present
            if (ctx == null || ctx.targets == null || ctx.targets.Count == 0) return;
            var target = ctx.targets[0];
            if (target == null) return;

            var enemy = target.GetComponent<EnemyController>();
            if (enemy != null)
            {
                var s = enemy.stats;
                int mitigated = UnityEngine.Mathf.Max(0, damage - s.DEF);
                s.HP -= mitigated;
                enemy.stats = s;
#if UNITY_EDITOR
                UnityEngine.Debug.Log($"[DamageEffect] {target.name} takes {mitigated} dmg → {s.HP} HP");
#endif
                return;
            }

            var ally = target.GetComponent<MinionController>();
            if (ally != null)
            {
                var s = ally.stats;
                int mitigated = UnityEngine.Mathf.Max(0, damage - s.DEF);
                s.HP -= mitigated;
                ally.stats = s;
#if UNITY_EDITOR
                UnityEngine.Debug.Log($"[DamageEffect] Ally {target.name} takes {mitigated} dmg → {s.HP} HP");
#endif
            }
        }
    }
}