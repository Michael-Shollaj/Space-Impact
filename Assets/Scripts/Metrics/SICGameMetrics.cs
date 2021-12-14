using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SpaceImpact.Utility;
using SpaceImpact.GameCore;

namespace SpaceImpact {

	public class SICGameMetrics : MonoBehaviour {
		// Public Variables	
		[SerializeField] private int score;
		[SerializeField] private int lives;
		[SerializeField] private int specialCount;
		[SerializeField] private ProjectileType special;

		// Private Variables
		private bool showMetrics;
		private Dictionary<string, string> metricsData;

		// Static Variables

		public const string UI_LIVES_FORMAT = "LIVES: {0}";
		public const string UI_SPECIAL_FORMAT = "{0} {1}";
		public const string UI_SCORE_FORMAT = "SCORE: {0}";

		public const string UI_SPECIAL_VALUE_FORMAT = "00";
		public const string UI_SCORE_VALUE_FORMAT = "00000";

		public void Awake() {
			metricsData = new Dictionary<string, string>();

			score = 0;
			lives = 0;
			special = ProjectileType.NONE;

			SaveMetricsData();
		}

# if UNITY_EDITOR
		public void Update() {
			if (Input.GetKeyDown(KeyCode.M)) {
				showMetrics = !showMetrics;
			}
		}

		public void OnGUI() {
			if (!showMetrics)
				return;

			GUI.skin.box.alignment = TextAnchor.MiddleLeft;
			string metricString = string.Empty;
			foreach (KeyValuePair<string, string> metrics in metricsData) {
				metricString += metrics.Key + ": " + metrics.Value + "\n";
			}
			GUILayout.Box("GAME METRICS\n\n" + metricString);
		}
# endif

		public void SaveMetricsData() {
			score = Mathf.Clamp(score, 0, int.MaxValue);
			lives = Mathf.Clamp(lives, 0, int.MaxValue);

			metricsData.Add(SICGameMetricsKey.GAME_SCORE, score.ToString());
			metricsData.Add(SICGameMetricsKey.GAME_LIVES, lives.ToString());
			metricsData.Add(SICGameMetricsKey.GAME_SPECIAL_COUNT, specialCount.ToString());
			metricsData.Add(SICGameMetricsKey.GAME_SPECIAL, special.ToString());
		}

		public void AddScore(int score) {
			this.score += score;
			SetScore(this.score);
		}

		public void SubtractScore(int score) {
			this.score -= score;
			SetScore(this.score);
		}

		public void SetScore(int score) {
			this.score = score;
			SICGameManager.SharedInstance.SetScoreUIText(string.Format(UI_SCORE_FORMAT, score.ToString(UI_SCORE_VALUE_FORMAT)));

			if (metricsData.ContainsKey(SICGameMetricsKey.GAME_SCORE)) {
				metricsData[SICGameMetricsKey.GAME_SCORE] = this.score.ToString();
			}
		}

		public int GetScore() {
			return this.score;
		}

		public void AddLife(int life) {
			this.lives += life;
			SetLife(this.lives);
		}

		public void SubtractLife(int life) {
			this.lives -= life;
			SetLife(this.lives);
		}

		public void SetLife(int life) {
			this.lives = life;
			SICGameManager.SharedInstance.SetLivesUIText(string.Format(UI_LIVES_FORMAT, SICGameUtility.GetLivesIcon(lives)));

			if (metricsData.ContainsKey(SICGameMetricsKey.GAME_LIVES)) {
				metricsData[SICGameMetricsKey.GAME_LIVES] = this.lives.ToString();
			}
		}

		public int GetLives() {
			return this.lives;
		}

		public void AddSpecialCount(int count) {
			this.specialCount += count;
			SetSpecialCount(this.specialCount);
		}

		public void SubtractSpecialCount(int count) {
			this.specialCount -= count;
			SetSpecialCount(this.specialCount);
		}

		public void SetSpecialCount(int count) {
			specialCount = count;
			SICGameManager.SharedInstance.SetSpecialUIText(string.Format(UI_SPECIAL_FORMAT, SICGameUtility.GetSpecialIcon(special), specialCount.ToString(UI_SPECIAL_VALUE_FORMAT)));

			if (metricsData.ContainsKey(SICGameMetricsKey.GAME_SPECIAL_COUNT)) {
				metricsData[SICGameMetricsKey.GAME_SPECIAL_COUNT] = this.specialCount.ToString();
			}
		}

		public int GetSpecialCount() {
			return this.specialCount;
		}

		public void SetSpecial(ProjectileType specialType) {
			this.special = specialType;
			SICGameManager.SharedInstance.SetSpecialUIText(string.Format(UI_SPECIAL_FORMAT, SICGameUtility.GetSpecialIcon(special), specialCount.ToString(UI_SPECIAL_VALUE_FORMAT)));

			if (metricsData.ContainsKey(SICGameMetricsKey.GAME_SPECIAL)) {
				metricsData[SICGameMetricsKey.GAME_SPECIAL] = this.special.ToString();
			}
		}

		public ProjectileType GetSpecial() {
			return this.special;
		}
	}
}