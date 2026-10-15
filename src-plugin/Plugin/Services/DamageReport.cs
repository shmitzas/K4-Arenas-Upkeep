using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Players;

namespace K4Arenas;

public sealed partial class Plugin
{
	/// <summary>Damage dealt this round between arena opponents, by (attacker slot, victim slot).</summary>
	private readonly Dictionary<(int Attacker, int Victim), (int Damage, int Hits)> _roundDamage = [];

	/// <summary>Health each victim had left after their last hit this round.</summary>
	private readonly Dictionary<int, int> _healthLeft = [];

	/// <summary>Name of everyone in <see cref="_roundDamage"/>, kept for opponents who leave.</summary>
	private readonly Dictionary<int, string> _damageNames = [];

	/// <summary>Players who already got this round's report.</summary>
	private readonly HashSet<int> _damageReported = [];

	private void ResetDamageReport()
	{
		_roundDamage.Clear();
		_healthLeft.Clear();
		_damageNames.Clear();
		_damageReported.Clear();
	}

	/// <summary>
	/// Counts a hit between two players in the same arena. The killing blow counts only the
	/// health the victim had left (the round type's health before the first hit), so overkill
	/// does not inflate it.
	/// </summary>
	private HookResult OnPlayerHurtTally(EventPlayerHurt ev)
	{
		var attacker = ev.AttackerPlayer;
		var victim = ev.UserIdPlayer;
		if (attacker?.IsValid != true || victim?.IsValid != true || attacker.Slot == victim.Slot)
			return HookResult.Continue;

		var arena = _playerManager.GetPlayer(victim)?.CurrentArena;
		if (arena == null || arena != _playerManager.GetPlayer(attacker)?.CurrentArena)
			return HookResult.Continue;

		var damage = Math.Clamp(ev.ActualDmgHealth, 0, _healthLeft.GetValueOrDefault(victim.Slot, arena.CurrentRoundType?.Health ?? 100));
		_healthLeft[victim.Slot] = ev.ActualHealth;

		var (total, hits) = _roundDamage.GetValueOrDefault((attacker.Slot, victim.Slot));
		_roundDamage[(attacker.Slot, victim.Slot)] = (total + damage, hits + 1);
		_damageNames[attacker.Slot] = attacker.Name;
		_damageNames[victim.Slot] = victim.Name;
		return HookResult.Continue;
	}

	/// <summary>
	/// Sends a player one chat line per opponent they traded damage with this round, once per
	/// round: on their death, on their kill (<paramref name="only"/> = the victim), or at round end.
	/// </summary>
	private void SendDamageReport(IPlayer player, IPlayer? only = null)
	{
		if (!player.IsValid || player.IsFakeClient || !_damageReported.Add(player.Slot))
			return;

		var opponents = new SortedSet<int>();
		foreach (var (attacker, victim) in _roundDamage.Keys)
		{
			if (attacker == player.Slot)
				opponents.Add(victim);
			else if (victim == player.Slot)
				opponents.Add(attacker);
		}

		if (only != null)
			opponents.RemoveWhere(slot => slot != only.Slot);

		var localizer = Core.Translation.GetPlayerLocalizer(player);
		foreach (var opponent in opponents)
		{
			var (dealt, dealtHits) = _roundDamage.GetValueOrDefault((player.Slot, opponent));
			var (taken, takenHits) = _roundDamage.GetValueOrDefault((opponent, player.Slot));
			var line = localizer["k4.chat.damage_report", _damageNames.GetValueOrDefault(opponent, "?"),
				dealt, localizer[dealtHits == 1 ? "k4.general.hit" : "k4.general.hits", dealtHits],
				taken, localizer[takenHits == 1 ? "k4.general.hit" : "k4.general.hits", takenHits]];
			player.SendChat($"{localizer["k4.general.prefix"]} {line}");
		}
	}

	/// <summary>Round end: the report for everyone whose duel ended without a kill.</summary>
	private void SendRemainingDamageReports()
	{
		foreach (var player in Core.PlayerManager.GetAllPlayers())
			SendDamageReport(player);
	}
}
