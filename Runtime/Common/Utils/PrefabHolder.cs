

using UnityEngine;


namespace Edgar.Unity
{
    public class PrefabHolder : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject RoomsPrefab;
        public GameObject TilemapsRootPrefab;

        [Header("Shared Tilemap Prefabs")]
        public GameObject FloorTilemapPrefab;
        public GameObject WallsTilemapPrefab;
        public GameObject CollideableTilemapPrefab;
        public GameObject Other1TilemapPrefab;
        public GameObject Other2TilemapPrefab;
        public GameObject Other3TilemapPrefab;

        [Header("Instances")]
        [ReadOnly] public GameObject RoomsInstance;
        [ReadOnly] public GameObject TilemapInstance;

        [ReadOnly] public GameObject FloorTilemapInstance;
        [ReadOnly] public GameObject WallsTilemapInstance;
        [ReadOnly] public GameObject CollideableTilemapInstance;
        [ReadOnly] public GameObject Other1TilemapInstance;
        [ReadOnly] public GameObject Other2TilemapInstance;
        [ReadOnly] public GameObject Other3TilemapInstance;

        public GameObject[] GetAllSharedTileMaps()
        {
            return  new[] { FloorTilemapInstance,
                            WallsTilemapInstance,
                            CollideableTilemapInstance,
                            Other1TilemapInstance,
                            Other2TilemapInstance,
                            Other3TilemapInstance };
        }
    }
}