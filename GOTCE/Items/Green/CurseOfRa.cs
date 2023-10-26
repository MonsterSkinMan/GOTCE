using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using RoR2.UI;
using TMPro;

namespace GOTCE.Items.Green
{
    public class CurseOfRa : ItemBase<CurseOfRa>
    {
        public override string ConfigName => "CURSE OF RA";

        public override string ItemName => "CURSE OF RA";

        public override string ItemLangTokenName => "GOTCE_CurseOfRa";

        public override string ItemPickupDesc => "CURSE OF RA 𓀀 𓀁 𓀂 𓀃 𓀄 𓀅 𓀆 𓀇 𓀈 𓀉 𓀊 𓀋 𓀌 𓀍 𓀎 𓀏 𓀐 𓀑 𓀒 𓀓 𓀔 𓀕 𓀖 𓀗 𓀘 𓀙 𓀚 𓀛 𓀜 𓀝 𓀞 𓀟 𓀠 𓀡 𓀢 𓀣 𓀤 𓀥 𓀦 𓀧 𓀨 𓀩 𓀪 𓀫 𓀬 𓀭 𓀮 𓀯 𓀰 𓀱 𓀲 𓀳 𓀴 𓀵 𓀶 𓀷 𓀸 𓀹 𓀺 𓀻 𓀼 𓀽 𓀾 𓀿 𓁀 𓁁 𓁂 𓁃 𓁄 𓁅 𓁆 𓁇 𓁈 𓁉 𓁊 𓁋 𓁌 𓁍 𓁎 𓁏 𓁐 𓁑";

        public override string ItemFullDescription => "Every <style=cIsUtility>10</style> <style=cStack>(-2 per stack)</style> seconds, a random Ra symbol with a random size randomly appears on your screen.";

        public override string ItemLore => "𓀀 𓀁 𓀂 𓀃 𓀄 𓀅 𓀆 𓀇 𓀈 𓀉 𓀊 𓀋 𓀌 𓀍 𓀎 𓀏 𓀐 𓀑 𓀒 𓀓 𓀔 𓀕 𓀖 𓀗 𓀘 𓀙 𓀚 𓀛 𓀜 𓀝 𓀞 𓀟 𓀠 𓀡 𓀢 𓀣 𓀤 𓀥 𓀦 𓀧 𓀨 𓀩 𓀪 𓀫 𓀬 𓀭 𓀮 𓀯 𓀰 𓀱 𓀲 𓀳 𓀴 𓀵 𓀶 𓀷 𓀸 𓀹 𓀺 𓀻 𓀼 𓀽 𓀾 𓀿 𓁀 𓁁 𓁂 𓁃 𓁄 𓁅 𓁆 𓁇 𓁈 𓁉 𓁊 𓁋 𓁌 𓁍 𓁎 𓁏 𓁐 𓁑\nI have no idea what to type here so here are my EXTREMELY UNDERRATED INFINITELY REPLAYABLE album recommendations:\nPeriphery's first album\n𓀀 𓀁 𓀂 𓀃 𓀄 𓀅 𓀆 𓀇 𓀈 𓀉 𓀊 𓀋 𓀌 𓀍 𓀎 𓀏 𓀐 𓀑 𓀒 𓀓 𓀔 𓀕 𓀖 𓀗 𓀘 𓀙 𓀚 𓀛 𓀜 𓀝 𓀞 𓀟 𓀠 𓀡 𓀢 𓀣 𓀤 𓀥 𓀦 𓀧 𓀨 𓀩 𓀪 𓀫 𓀬 𓀭 𓀮 𓀯 𓀰 𓀱 𓀲 𓀳 𓀴 𓀵 𓀶 𓀷 𓀸 𓀹 𓀺 𓀻 𓀼 𓀽 𓀾 𓀿 𓁀 𓁁 𓁂 𓁃 𓁄 𓁅 𓁆 𓁇 𓁈 𓁉 𓁊 𓁋 𓁌 𓁍 𓁎 𓁏 𓁐 𓁑\nUnprocessed's first and second album\n𓀀 𓀁 𓀂 𓀃 𓀄 𓀅 𓀆 𓀇 𓀈 𓀉 𓀊 𓀋 𓀌 𓀍 𓀎 𓀏 𓀐 𓀑 𓀒 𓀓 𓀔 𓀕 𓀖 𓀗 𓀘 𓀙 𓀚 𓀛 𓀜 𓀝 𓀞 𓀟 𓀠 𓀡 𓀢 𓀣 𓀤 𓀥 𓀦 𓀧 𓀨 𓀩 𓀪 𓀫 𓀬 𓀭 𓀮 𓀯 𓀰 𓀱 𓀲 𓀳 𓀴 𓀵 𓀶 𓀷 𓀸 𓀹 𓀺 𓀻 𓀼 𓀽 𓀾 𓀿 𓁀 𓁁 𓁂 𓁃 𓁄 𓁅 𓁆 𓁇 𓁈 𓁉 𓁊 𓁋 𓁌 𓁍 𓁎 𓁏 𓁐 𓁑";

        public override ItemTier Tier => ItemTier.Tier2;

        public override Enum[] ItemTags => new Enum[] { ItemTag.AIBlacklist, GOTCETags.Bullshit, GOTCETags.NonLunarLunar };

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => Main.MainAssets.LoadAsset<Sprite>("Assets/Textures/Icons/Item/drill.png");

        public override void Init(ConfigFile config)
        {
            base.Init(config);
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }

        public override void Hooks()
        {
            HUD.shouldHudDisplay += HUD_shouldHudDisplay;
            On.RoR2.Inventory.GiveItem_ItemIndex_int += Inventory_GiveItem_ItemIndex_int;
        }

        private void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (itemIndex == Instance.ItemDef.itemIndex)
            {
                for (int i = 0; i < HUD.instancesList.Count; i++)
                {
                    var hudInstance = HUD.instancesList[i];
                    var hud = hudInstance.GetComponent<HUD>();
                    var master = self.gameObject.GetComponent<CharacterMaster>();
                    if (master)
                    {
                        var body = master.GetBody();
                        if (body && hud.targetBodyObject == body.gameObject)
                        {
                            var curseOfRaController = hud.GetComponent<CurseOfRaController>();
                            if (curseOfRaController)
                            {
                                var stack = self.GetItemCount(Instance.ItemDef);
                                curseOfRaController.hasCurseOfRa = true;
                                curseOfRaController.characterAppearInterval = 10f - 2f * (stack - 1);
                            }
                        }
                    }
                }
            }
        }

        private void HUD_shouldHudDisplay(HUD hud, ref bool shouldDisplay)
        {
            if (hud && hud.GetComponent<CurseOfRaController>() == null)
            {
                hud.gameObject.AddComponent<CurseOfRaController>();
            }
        }
    }

    public class CurseOfRaController : MonoBehaviour
    {
        public HUD hudInstance;
        public string[] possibleCharacters;
        public float characterAppearTimer;
        public float characterAppearInterval = 10f;
        public float characterRefreshTimer;
        public float characterRefreshInterval = 0.25f;
        public float duration = 3f;
        public Dictionary<GameObject, float> gameObjectTimerPair = new();
        public bool hasCurseOfRa = false;
        public static TMP_FontAsset font;

        public void Start()
        {
            hudInstance = GetComponent<HUD>();
            possibleCharacters = new string[] { "𓀀", "𓀁", "𓀂", "𓀃", "𓀄", "𓀅", "𓀆", "𓀇", "𓀈", "𓀉", "𓀊", "𓀋", "𓀌", "𓀍", "𓀎", "𓀏", "𓀐", "𓀑", "𓀒", "𓀓", "𓀔", "𓀕", "𓀖", "𓀗", "𓀘", "𓀙", "𓀚", "𓀛", "𓀜", "𓀝", "𓀞", "𓀟", "𓀠", "𓀡", "𓀢", "𓀣", "𓀤", "𓀥", "𓀦", "𓀧", "𓀨", "𓀩", "𓀪", "𓀫", "𓀬", "𓀭", "𓀮", "𓀯", "𓀰", "𓀱", "𓀲", "𓀳", "𓀴", "𓀵", "𓀶", "𓀷", "𓀸", "𓀹", "𓀺", "𓀻", "𓀼", "𓀽", "𓀾", "𓀿", "𓁀", "𓁁", "𓁂", "𓁃", "𓁄", "𓁅", "𓁆", "𓁇", "𓁈", "𓁉", "𓁊", "𓁋", "𓁌", "𓁍", "𓁎", "𓁏", "𓁐", "𓁑" };
            font = Addressables.LoadAssetAsync<TMP_FontAsset>("RoR2/Base/Common/Fonts/Noto/NotoSans-Regular SDF(ROOT Extended ASCII + Turkish).asset").WaitForCompletion();
        }

        public void FixedUpdate()
        {
            if (!hasCurseOfRa)
            {
                return;
            }

            characterAppearTimer += Time.fixedDeltaTime;
            characterRefreshTimer += Time.fixedDeltaTime;
            if (characterAppearTimer >= characterAppearInterval)
            {
                var randomCharacter = possibleCharacters[Random.RandomRangeInt(0, possibleCharacters.Length)];

                var gameObject = new GameObject(randomCharacter);
                gameObject.transform.parent = hudInstance.transform;
                gameObject.transform.position = new Vector3(Random.Range(-25f, 25f), Random.Range(-25f, 25f), 500f);

                var textMeshPro = gameObject.AddComponent<TextMeshPro>();
                textMeshPro.text = randomCharacter;
                textMeshPro.font = font;
                textMeshPro.fontSize = Random.Range(28f, 96f);
                textMeshPro.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

                gameObjectTimerPair.Add(gameObject, duration);

                characterAppearInterval = 0f;
            }
            if (characterRefreshTimer >= characterRefreshInterval)
            {
                List<GameObject> gameObjectsToDestroy = new();

                foreach (var key in gameObjectTimerPair.Keys)
                {
                    gameObjectTimerPair[key] -= 1f * Time.fixedDeltaTime * 4f; // since it refreshes every 0.25s, we need to remove 4 times as much time ig

                    if (gameObjectTimerPair[key] <= 0f)
                    {
                        gameObjectsToDestroy.Add(key);
                    }
                }

                foreach (var gameObject in gameObjectsToDestroy)
                {
                    Destroy(gameObject);
                    gameObjectTimerPair.Remove(gameObject);
                }
                characterRefreshTimer = 0f;
            }
        }
    }
}