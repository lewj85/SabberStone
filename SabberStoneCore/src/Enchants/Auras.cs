﻿using System;
using System.Collections.Generic;
using System.Linq;
using SabberStoneCore.Enums;
using SabberStoneCore.Conditions;
using SabberStoneCore.Model;
using SabberStoneCore.Tasks;
using SabberStoneCore.Tasks.SimpleTasks;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCore.Enchants
{
    internal class Auras
    {

        public static Enchant SimpleInclSelf(EGameTag tag, int amount, params RelaCondition[] list)
        {
            // health is only allowed with the HealthRetentionTask
            if (tag == EGameTag.HEALTH)
            {
                throw new NotSupportedException();
            }

            var result = new Enchant
            {
                EnableConditions = new List<SelfCondition>
                {
                    SelfCondition.IsInZone(EZone.PLAY),
                    SelfCondition.IsNotSilenced
                },
                Effects = new Dictionary<EGameTag, int>()
                {
                    [tag] = amount
                }
            };
            list.ToList().ForEach(p => result.ApplyConditions.Add(p));
            return result;
        }

        public static Enchant SimpleInclSelf(Dictionary<EGameTag, int> effects, params RelaCondition[] list)
        {
            // health is only allowed with the HealthRetentionTask
            if (effects.Keys.Contains(EGameTag.HEALTH))
            {
                throw new NotSupportedException();
            }

            var result = new Enchant
            {
                EnableConditions = new List<SelfCondition>
                {
                    SelfCondition.IsInZone(EZone.PLAY),
                    SelfCondition.IsNotSilenced
                },
                Effects = effects
            };
            list.ToList().ForEach(p => result.ApplyConditions.Add(p));
            return result;
        }

        public static Enchant Attack(int amount, params RelaCondition[] list)
        {
            var result = new Enchant
            {
                EnableConditions = new List<SelfCondition>
                {
                    SelfCondition.IsInZone(EZone.PLAY),
                    SelfCondition.IsNotSilenced
                },
                ApplyConditions = new List<RelaCondition>
                {
                    RelaCondition.IsNotSelf
                },
                Effects = new Dictionary<EGameTag, int>()
                {
                    [EGameTag.ATK] = amount
                }
            };
            list.ToList().ForEach(p => result.ApplyConditions.Add(p));
            return result;
        }

        public static Enchant Health(int amount, params RelaCondition[] list)
        {
            var relaConditions = new List<RelaCondition>
            {
                RelaCondition.IsNotSelf
            };
            relaConditions.AddRange(list);
            var result = new Enchant
            {
                EnableConditions = new List<SelfCondition>
                {
                    SelfCondition.IsInZone(EZone.PLAY),
                    SelfCondition.IsNotSilenced
                },
                ApplyConditions = relaConditions,
                Effects = new Dictionary<EGameTag, int>()
                {
                    [EGameTag.HEALTH] = amount
                },
                // Health Retention task ... 
                RemovalTask = ComplexTask.Create(
                    new IncludeTask(EEntityType.MINIONS, new [] { EEntityType.SOURCE}),
                    new FilterStackTask(EEntityType.SOURCE, relaConditions.ToArray()),
                    new HealthRetentionTask(amount, EEntityType.STACK))
            };
            list.ToList().ForEach(p => result.ApplyConditions.Add(p));
            return result;
        }

        public static Enchant AttackHealth(int atk, int health, params RelaCondition[] list)
        {
            var relaConditions = new List<RelaCondition>
            {
                RelaCondition.IsNotSelf
            };
            relaConditions.AddRange(list);

            var result =  new Enchant
            {
                EnableConditions = new List<SelfCondition>
                {
                    SelfCondition.IsInZone(EZone.PLAY),
                    SelfCondition.IsNotSilenced
                },
                ApplyConditions = relaConditions,
                Effects = new Dictionary<EGameTag, int>()
                {
                    [EGameTag.ATK] = atk,
                    [EGameTag.HEALTH] = health,
                },
                // Health Retention task ... 
                RemovalTask = ComplexTask.Create(
                    new IncludeTask(EEntityType.MINIONS, new[] { EEntityType.SOURCE }),
                    new FilterStackTask(EEntityType.SOURCE, relaConditions.ToArray()),
                    new HealthRetentionTask(health, EEntityType.STACK))
            };
            list.ToList().ForEach(p => result.ApplyConditions.Add(p));
            return result;
        }

        public static Enchant SpellPowerDamage(int amount)
        {
            return new Enchant
            {
                EnableConditions = new List<SelfCondition>
                {
                    SelfCondition.IsInZone(EZone.PLAY),
                    SelfCondition.IsNotSilenced
                },
                Effects = new Dictionary<EGameTag, int>
                {
                    [EGameTag.SPELLPOWER] = amount
                }
            };
        }

        public static Enchant HeroPowerDamage(int amount)
        {
            return new Enchant
            {
                EnableConditions = new List<SelfCondition>
                {
                    SelfCondition.IsInZone(EZone.PLAY),
                    SelfCondition.IsNotSilenced
                },
                Effects = new Dictionary<EGameTag, int>
                {
                    [EGameTag.HEROPOWER_DAMAGE] = amount
                }
            };
        }

        public static Enchant Cost(int amount, params RelaCondition[] list)
        {
            var relaConditions = new List<RelaCondition>();
            relaConditions.AddRange(list);
            return new Enchant
            {
                EnableConditions = new List<SelfCondition>
                {
                    SelfCondition.IsInZone(EZone.PLAY),
                    SelfCondition.IsNotSilenced
                },
                ApplyConditions = relaConditions,
                Effects = new Dictionary<EGameTag, int>
                {
                    [EGameTag.COST] = amount
                }
            };
        }



        public static Enchant CostTurn(int amount, params RelaCondition[] list)
        {
            var relaConditions = new List<RelaCondition>();
            relaConditions.AddRange(list);
            return new Enchant
            {
                TurnsActive = 1,
                EnableConditions = new List<SelfCondition>
                {
                    SelfCondition.IsInZone(EZone.PLAY),
                    SelfCondition.IsNotSilenced
                },
                ApplyConditions = relaConditions,
                Effects = new Dictionary<EGameTag, int>
                {
                    [EGameTag.COST] = amount
                }
            };
        }

        public static Enchant CostFunc(Func<IPlayable, int> function, params RelaCondition[] list)
        {
            var relaConditions = new List<RelaCondition>();
            relaConditions.AddRange(list);
            return new Enchant
            {
                EnableConditions = new List<SelfCondition>
                {
                    SelfCondition.IsInZone(EZone.HAND)
                },
                Effects = new Dictionary<EGameTag, int>
                {
                    [EGameTag.COST] = 0
                },
                ApplyConditions = relaConditions,
                ValueFunc = function
            };
        }

        public static Enchant WeaponAttack(int amount, params RelaCondition[] list)
        {
            var result = new Enchant
            {
                EnableConditions = new List<SelfCondition>
                {
                    SelfCondition.IsThisWeaponEquiped
                },
                Effects = new Dictionary<EGameTag, int>()
                {
                    [EGameTag.ATK] = amount
                }
            };
            list.ToList().ForEach(p => result.ApplyConditions.Add(p));
            return result;
        }

        public static Enchant Immune(params RelaCondition[] list)
        {
            var result = new Enchant
            {
                EnableConditions = new List<SelfCondition>
                {
                    SelfCondition.IsThisWeaponEquiped
                },
                Effects = new Dictionary<EGameTag, int>()
                {
                    [EGameTag.IMMUNE] = 1
                }
            };
            list.ToList().ForEach(p => result.ApplyConditions.Add(p));
            return result;
        }
    }
}
