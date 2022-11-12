using RoR2;
using Unity;
using UnityEngine;
using System;
using RoR2.Skills;
using R2API;
using RoR2.UI;
using MonoMod.Cil;
using Mono.Cecil.Cil;
namespace GOTCE.Utils {
    public class PassiveReplacement {
        public static void RunHooks() {
            // passive slot hook
            On.RoR2.UI.LoadoutPanelController.Row.FromSkillSlot += (orig, owner, bodyIndex , slotIndex ,slot) => {
                LoadoutPanelController.Row row = (LoadoutPanelController.Row) orig(owner, bodyIndex, slotIndex, slot);
                if((slot.skillFamily as ScriptableObject).name.Contains("Passive")){
                    Transform label = row.rowPanelTransform.Find("SlotLabel") ?? row.rowPanelTransform.Find("LabelContainer").Find("SlotLabel");
                    if(label) {
                        label.GetComponent<LanguageTextMeshController>().token = "Passive";
                    }
                }
                return row;
            };

            // passive rendering il hook
            IL.RoR2.UI.CharacterSelectController.BuildSkillStripDisplayData += (il) => {
               ILCursor c = new ILCursor(il);
               int skillIndex = -1;
               int defIndex = -1;
               var label = c.DefineLabel();
               if(c.TryGotoNext(x => x.MatchLdloc(out skillIndex),x => x.MatchLdfld(typeof(GenericSkill).GetField("hideInCharacterSelect")),x => x.MatchBrtrue(out label)) && skillIndex != (-1) && c.TryGotoNext(MoveType.After,x => x.MatchLdfld(typeof(SkillFamily.Variant).GetField("skillDef")),x => x.MatchStloc(out defIndex))) {
                    c.Emit(OpCodes.Ldloc,defIndex); 
                    // c.EmitDelegate<System.Func<SkillDef,bool>>((def) => def == NoneDef);
                    c.Emit(OpCodes.Brtrue,label);
                    if(c.TryGotoNext(x => x.MatchCallOrCallvirt(typeof(List<CharacterSelectController.StripDisplayData>).GetMethod("Add")))){
                        c.Remove();
                        c.Emit(OpCodes.Ldloc,skillIndex);
                        c.EmitDelegate<System.Action<List<CharacterSelectController.StripDisplayData>, CharacterSelectController.StripDisplayData,GenericSkill>>((list,disp,ski) => {
                        if((ski.skillFamily as ScriptableObject).name.Contains("Passive")){
                            list.Insert(0,disp);
                        } else {
                            list.Add(disp);
                        }
                        });
                   }
               }
           };

           // i dont know what this does
           IL.RoR2.UI.LoadoutPanelController.Rebuild += (il) => {
               ILCursor c = new ILCursor(il);
               if(c.TryGotoNext(MoveType.After, x => x.MatchLdloc(0), x => x.MatchCallOrCallvirt(out _), x => x.MatchCallOrCallvirt(out _), x => x.MatchStloc(1))){
                   c.Emit(OpCodes.Ldloc_1);
                   c.Emit(OpCodes.Ldarg_0);
                   c.EmitDelegate<System.Action<List<GenericSkill>, LoadoutPanelController>>((list,self) => {
                    foreach(var slot in list.Where((slot) => {return slot != list.First() && (slot.skillFamily as ScriptableObject).name.Contains("Passive") ;})){
                      self.rows.Add(LoadoutPanelController.Row.FromSkillSlot(self, self.currentDisplayData.bodyIndex,list.FindIndex((skill) => skill == slot),slot));
                    }
                    list.RemoveAll((slot) => {return slot != list.First() && (slot.skillFamily as ScriptableObject).name.Contains("Passive");});
                   });
               }
           };
        }

        // THIS DOESNT WORK

        public static void ReplacePassiveSlot(GameObject prefab, SkillDef newSkillDef, SkillDef origDef = null) {
            GenericSkill passiveSlot = prefab.AddComponent<GenericSkill>();
            SkillFamily family = ScriptableObject.CreateInstance<SkillFamily>();
            SkillLocator locator = prefab.GetComponent<SkillLocator>();
            locator.passiveSkill.enabled = false;

            (family as ScriptableObject).name = prefab.name + "Passive";
            family.variants = new SkillFamily.Variant[2];
            SkillDef passiveDef = null;
            if (origDef == null) {
                passiveDef = ScriptableObject.CreateInstance<SkillDef>();
                passiveDef.skillNameToken = locator.passiveSkill.skillNameToken;
                passiveDef.skillDescriptionToken = locator.passiveSkill.skillDescriptionToken;
                passiveDef.icon = locator.passiveSkill.icon;
                passiveDef.keywordTokens = locator.passiveSkill.keywordToken.Length > 0 ? new string[] { locator.passiveSkill.keywordToken } : null;
                passiveDef.baseRechargeInterval = 0f;
                ContentAddition.AddSkillDef(passiveDef);
            }
            else {
                passiveDef = origDef;
            }

            family.variants[0] = new SkillFamily.Variant {
                skillDef = passiveDef,
                viewableNode = new ViewablesCatalog.Node(passiveDef.skillNameToken, false, null)
            };

            family.variants[1] = new SkillFamily.Variant {
                skillDef = newSkillDef,
                viewableNode = new ViewablesCatalog.Node(newSkillDef.skillNameToken, false, null)
            };

            passiveSlot._skillFamily = family;

            // ContentAddition.AddSkillFamily(passiveSlot.skillFamily);
        }
    }
}