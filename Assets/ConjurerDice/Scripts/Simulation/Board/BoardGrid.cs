using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConjurerDice {
    public interface IBoardGrid {
        bool TryGetFrontSpawn(int lane, out TileRef t);
        bool TryGetBackSpawn(int lane, out TileRef t);
        bool IsInBounds(TileRef t);
        TileController GetTile(TileRef t);
        int Lanes { get; }
        int LaneLength { get; }
    }

    public partial class BoardGrid : MonoBehaviour, IBoardGrid {
        [SerializeField] BoardConfigSO config;
        [SerializeField] TileController[,] tiles;
        public int Lanes => config != null ? config.lanes : 3;
        public int LaneLength => config != null ? config.laneLength : 8;

        void Awake() { tiles = new TileController[Lanes, LaneLength]; }

        public bool TryGetFrontSpawn(int lane, out TileRef t) { t = new TileRef(lane, LaneLength-1); return lane>=0 && lane<Lanes; }
        public bool TryGetBackSpawn(int lane, out TileRef t) { t = new TileRef(lane, 0); return lane>=0 && lane<Lanes; }
        public bool IsInBounds(TileRef tr) => tr.lane>=0 && tr.lane<Lanes && tr.index>=0 && tr.index<LaneLength;
        public TileController GetTile(TileRef tr) => tiles[tr.lane, tr.index];
        
        
    }
}
