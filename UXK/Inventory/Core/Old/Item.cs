// namespace DefaultNamespace
// {
//     [UnityEngine.CreateAssetMenu(fileName = "Item", menuName = "Game/Item", order = 0)]
//     public class Item : UnityEngine.ScriptableObject, IItem
//     {
//         // [SerializeField] [FoldoutGroup("Basic", false)]
//         // private string itemName;
//         // [SerializeField] [FoldoutGroup("Basic", false)]
//         // private string itemDescription;
//         // [SerializeField] [FoldoutGroup("Basic", false)]
//         // private CategoryObject category;
//         //
//         // [SerializeField] [FoldoutGroup("Game Specific", false)]
//         // private float _weight;
//         // [SerializeField] [FoldoutGroup("Game Specific", false)]
//         // private uint _cost;
//         //
//         // [SerializeField] [FoldoutGroup("Visual", false)]
//         // private GameObject visualPrefab;
//         // [SerializeField] [FoldoutGroup("Visual", false)]
//         // private Sprite icon;
//         //
//         // public string Name => itemName;
//         // public string Description => itemDescription;
//         // public ICategory GetCategory => category;
//         // public float Weight => _weight;
//         // public uint Cost => _cost;
//         //
//         // private const string DEFAULT_INWORLD_ICON_OBJ_PATH = "Prefabs/InWorldIcon";
//         //
//         // public GameObject GetVisualPrefab()
//         // {
//         //     if (visualPrefab != null)
//         //         return Instantiate(visualPrefab);
//         //
//         //     var inWorldIconInstance = Instantiate(Resources.Load<GameObject>(DEFAULT_INWORLD_ICON_OBJ_PATH));
//         //     inWorldIconInstance.GetComponentInChildren<SpriteRenderer>().sprite = icon;
//         //     return inWorldIconInstance;
//         // }
//     }
// }