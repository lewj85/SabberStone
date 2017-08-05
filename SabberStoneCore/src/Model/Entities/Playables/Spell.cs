﻿using System.Collections.Generic;
using SabberStoneCore.Enums;
using SabberStoneCore.Model.Zones;

namespace SabberStoneCore.Model.Entities.Playables
{
	/// <summary>
	/// The entity built from <see cref="ECardType.SPELL"/> <see cref="Card"/>s.
	/// A spell can be moved into <see cref="BoardZone"/> or <see cref="SecretZone"/>.
	/// After activation it will be moved into <see cref="GraveyardZone"/>.
	/// </summary>
	/// <seealso cref="Playable{Spell}" />
	public class Spell : Playable<Spell>
	{
		#region TAG_SHORTCUTS
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

		public bool IsAffectedBySpellpower => this[EGameTag.AFFECTED_BY_SPELL_POWER] == 1;

		public bool IsSecret => this[EGameTag.SECRET] == 1;

		public bool IsQuest => this[EGameTag.QUEST] == 1;

		public bool IsCountered
		{
			get { return this[EGameTag.COUNTER] == 1; }
			set { this[EGameTag.COUNTER] = value ? 1 : 0; }
		}

		public bool ReceveivesDoubleSpellDamage
		{
			get { return this[EGameTag.RECEIVES_DOUBLE_SPELLDAMAGE_BONUS] == 1; }
			set { this[EGameTag.RECEIVES_DOUBLE_SPELLDAMAGE_BONUS] = value ? 1 : 0; }
		}

		public int QuestProgress
		{
			get { return this[EGameTag.QUEST_PROGRESS]; }
			set { this[EGameTag.QUEST_PROGRESS] = value; }
		}

		public int QuestTotalProgress => this[EGameTag.QUEST_PROGRESS_TOTAL];

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
		#endregion

		/// <summary>Initializes a new instance of the <see cref="Spell"/> class.</summary>
		/// <param name="controller">The controller.</param>
		/// <param name="card">The card.</param>
		/// <param name="tags">The tags.</param>
		/// <autogeneratedoc />
		public Spell(Controller controller, Card card, Dictionary<EGameTag, int> tags)
			: base(controller, card, tags)
		{
			Game.Log(ELogLevel.VERBOSE, EBlockType.PLAY, "Spell", $"{card.Name} ({card.Class}) was created.");
		}

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
		public override bool TargetingRequirements(ICharacter target)
		{
			return !target.CantBeTargetedBySpells && base.TargetingRequirements(target);
		}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

	}
}