

using UnityEngine;


namespace Edgar.Unity
{
    public class PrefabHolder : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject RoomsPrefab;
        public GameObject TilemapPrefab;

        [Header("Instances")]
        [ReadOnly] public GameObject RoomsInstance;
        [ReadOnly] public GameObject TilemapInstance;
    }
}