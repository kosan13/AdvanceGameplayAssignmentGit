using System.Collections.Generic;
using System.Linq;
using Game.UnitClasses;
using UI;
using UnityEngine;

namespace Game
{
    public class Level : MonoBehaviour
    {
        public static Queue<Unit> TurnOrder = new ();
        private static GameObject PauseMenuGameObject => PauseMenu.Instance.gameObject;
        private void Update() { if (Input.GetKeyDown(KeyCode.Escape)) PauseMenuGameObject.SetActive(!PauseMenuGameObject.activeSelf); }

        public static void CreateLevel()
        {
            PlayerCharacter.CreatAndInstantiatePlayerCharacter();
            PauseMenu.CreatAndInstantiatePauseMenu();

            List<Unit> units = CreatEnemy(10).ToList();
            Debug.Log(units);
            Unit.Sort(units, true);
            Debug.Log(units);
            foreach (Unit unit in units) TurnOrder.Enqueue(unit);
        }
        public static Level AddLevel(GameObject gameObject) => gameObject.AddComponent<Level>();
        public static Level CreateLevelAndAddLevel(GameObject gameObject)
        {
            CreateLevel();
            return AddLevel(gameObject);
        }

        private static Unit[] CreatEnemy(int amount)
        {
            Unit[] enemy = new Unit[amount];
            for (int i = 0; i < amount; i++) enemy[i] = CreatEnemy();
            return enemy;
        }
        private static Unit CreatEnemy()
        {
            return new GameObject().AddComponent<Unit>();
        }
    }
}