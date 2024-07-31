using UnityEngine;
using UnityEngine.Tilemaps;

namespace Edgar.Unity
{
    public class DungeonTilemapLayersHandlerGrid2D : ITilemapLayersHandlerGrid2D
    {
        /// <summary>
        ///  Initializes individual tilemap layers.
        /// </summary>
        /// <param name="gameObject"></param>
        public void InitializeTilemaps(GameObject gameObject)
        {
            gameObject.AddComponent<Grid>();
            PrefabHolder prefabHolder;
            if(!gameObject.transform.parent.TryGetComponent(out prefabHolder))
            {
                Debug.LogWarning("PrefabHolder not found!!!");
                return;
            }

            prefabHolder.FloorTilemapInstance = CreateTilemapGameObject(prefabHolder.FloorTilemapPrefab, "Floor", gameObject, 0);

            var wallsTilemapObject = CreateTilemapGameObject(prefabHolder.WallsTilemapPrefab, "Walls", gameObject, 1);
            AddCompositeCollider(wallsTilemapObject);
            prefabHolder.WallsTilemapInstance = wallsTilemapObject;

            var collideableTilemapObject = CreateTilemapGameObject(prefabHolder.CollideableTilemapPrefab, "Collideable", gameObject, 2);
            AddCompositeCollider(collideableTilemapObject);
            prefabHolder.CollideableTilemapInstance = collideableTilemapObject;

            prefabHolder.Other1TilemapInstance = CreateTilemapGameObject(prefabHolder.Other1TilemapPrefab, "Other 1", gameObject, 3);

            prefabHolder.Other2TilemapInstance = CreateTilemapGameObject(prefabHolder.Other2TilemapPrefab, "Other 2", gameObject, 4);

            prefabHolder.Other3TilemapInstance = CreateTilemapGameObject(prefabHolder.Other3TilemapPrefab, "Other 3", gameObject, 5);
        }

        protected GameObject CreateTilemapGameObject(GameObject tilemapPrefab, string name, GameObject parentObject, int sortingOrder)
        {
            var tilemapObject = Object.Instantiate(tilemapPrefab);
            //var tilemapObject = new GameObject(name);
            tilemapObject.transform.SetParent(parentObject.transform);
            //var tilemap = tilemapObject.AddComponent<Tilemap>();
            //var tilemapRenderer = tilemapObject.AddComponent<TilemapRenderer>();
            tilemapObject.name = name;

            tilemapObject.GetComponent<TilemapRenderer>().sortingOrder = sortingOrder;

            return tilemapObject;
        }

        protected void AddCompositeCollider(GameObject gameObject)
        {
            var tilemapCollider2D = gameObject.AddComponent<TilemapCollider2D>();
            #if UNITY_2023_2_OR_NEWER
            tilemapCollider2D.compositeOperation = Collider2D.CompositeOperation.Merge;
            #else
            tilemapCollider2D.usedByComposite = true;
            #endif

            gameObject.AddComponent<CompositeCollider2D>();
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}