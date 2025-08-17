using UnityEngine;
using System;
using System.Collections.Generic;

namespace ConjurerDice {
    public class AbilityCastContext {
        public int lane;
        public GameObject caster;
        public List<GameObject> targets = new List<GameObject>();
        public TileRef tile;
        public int magnitude;
    }
}
