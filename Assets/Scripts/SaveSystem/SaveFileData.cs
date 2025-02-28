using System;
using System.Collections.Generic;
using Game.UnitClasses;
using TileSystem.Tile_Class;
using TileSystem.TileMap_Class;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public struct SaveFileData
    {
        public SaveFileData(TileMapClass tilemap, HashSet<Tile> level, Mesh worldMap, Queue<Unit> turnOrder, PlayerCharacter playerCharacter)
        {
            Tilemap = tilemap;
            Level = level;
            WorldMap = worldMap;
            TurnOrder = turnOrder;
            PlayerCharacter = playerCharacter;
        }

        public TileMapClass Tilemap { get; private set; }
        public HashSet<Tile> Level { get; private set; }
        public Mesh WorldMap { get; private set; }
        public Queue<Unit> TurnOrder { get; private set; }
        public PlayerCharacter PlayerCharacter { get; private set; }
    }
}