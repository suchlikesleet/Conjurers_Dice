using UnityEngine;
namespace ConjurerDice {
    [System.Serializable]
    public struct TileRef {
        public int lane;
        public int index;
        public TileRef(int l, int i) { lane = l; index = i; }
        public override string ToString() { return "Lane " + lane + ", Index " + index; }
    }
}
