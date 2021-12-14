using UnityEngine;
using System.Collections;
using SpaceImpact.Utility;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

namespace SpaceImpact.GameCore {

	public class SICGameManager : MonoBehaviour {
		// Public Variables	
		[SerializeField] private GameObject mainMenuUi;
		[SerializeField] private GameObject mainUi;
		[SerializeField] private GameObject endUi;

		[SerializeField] private Text livesUi;
		[SerializeField] private Text specialUi;
		[SerializeField] private Text scoreUI;
		[SerializeField] private Text endScoreUI;

		[SerializeField] private SICCameraMover cameraMover;
		[SerializeField] private SICGameMetrics gameMetrics;
		[SerializeField] private StageType stage;
		[SerializeField] private List<SICGameStage> refStages;
		[SerializeField] private float shipInvulerability = 3f;

		// Private Variables	
		private GameObject spaceShipObj;
		private SICSpaceShip spaceShip;

		private GameObject stageObj;
		private SICGameStage curStage;

		private Vector3 shipInitPos;
		private SICGameBoss stageBoss;

		// Static Variables
		private static SICGameManager instance;

		public static SICGameManager SharedInstance { get { return instance; } }

		public SICSpaceShip SpaceShip { get { return spaceShip; } }

		public SICCameraMover CameraMover { get { return cameraMover; } } 

		public SICGameMetrics GameMetrics { get { return gameMetrics; } }

		public List<SICGameStage> RefStages { get { return refStages; } }

		public SICGameStage CurrentStage { get { return curStage; } }

		public bool IsStageComplete { 
			get {
				if (stageBoss == null)
					return false;

				return !stageBoss.IsElementActive; 
			} 
		}

		public Text LivesUI { get { return livesUi; } }

		public Text SpecialUI { get { return specialUi; } }

		public Text ScoreUI { get { return scoreUI; } }

		public Text EndScoreUI { get { return endScoreUI; } }

		private void Awake() {
			instance = this;
		}

		private void Start() {
			InitStage();
			shipInitPos = curStage.PStartPosition.position;
			InitShip();
			InitCameraMover();

			mainMenuUi.SetActive(true);
			mainUi.SetActive(false);
			endUi.SetActive(false);
		}

		public void LoadStage(StageType stageType) {
			if (stage == stageType)
				return;

			stage = stageType;
			mainMenuUi.SetActive(false);
			mainUi.SetActive(true);
			endUi.SetActive(false);

			if (stage == StageType.NONE) {
				ClearStage();
				return;
			}

			if (curStage != null) {
				curStage.DisableElement();
			}

			stageObj = SICObjectPoolManager.SharedInstance.GetObject(SICObjectPoolName.OBJECT_GAME_STAGE, stageType);
			if (stageObj == null) {
				Restart();
				return;
			}
			else {
				curStage = stageObj.GetComponent<SICGameStage>();
				stageBoss = curStage.StageBoss;
			}

			InitCameraMover();
			SetElementsActive(true);
			RespawnShip();
		}

		public void LoadFirstLevel() {
			LoadStage(StageType.STAGE_1);
		}

		public void Restart() {
			LoadStage(StageType.NONE);
			InitShip();
			mainMenuUi.SetActive(true);
			endUi.SetActive(false);
			mainUi.SetActive(false);
		}

		public void ExitApplication() {
			Application.Quit();
		}

		public void InitShip() {
			// Initialize Space Ship
			spaceShipObj = SICObjectPoolManager.SharedInstance.GetObject(SICObjectPoolName.OBJECT_SPACE_SHIP);

			if (spaceShipObj == null)
				return;

			spaceShip = spaceShipObj.GetComponent<SICSpaceShip>();
			spaceShip.SetHP(SICGameSettings.DEFAULT_LIVES);
			spaceShip.SetScorePoint(SICGameSettings.DEFAULT_SCORE);
			spaceShip.SetSpecial(ProjectileType.NONE);
			spaceShip.SetSpecial(SICGameSettings.DEFAULT_SPECIAL);
			spaceShip.SetPosition(shipInitPos);
		}

		public void ResetShip() {
			if (spaceShip == null)
				return;

			spaceShip.SetPosition(shipInitPos);
		}

		public void InitStage() {
			// Initialize Game Stage
			stageObj = SICObjectPoolManager.SharedInstance.GetObject(SICObjectPoolName.OBJECT_GAME_STAGE);
			if (stageObj == null)
				return;

			curStage = stageObj.GetComponent<SICGameStage>();
			stageBoss = curStage.StageBoss;
		}

		public void ResetStage() {
			if (curStage == null)
				return;

			curStage.ResetStage();
		}

		public void InitCameraMover() {
			// Initialize Camera Mover
			if (curStage == null)
				return;

			cameraMover.Initialize(curStage.PointA, curStage.PointB);
		}

		public void ResetCameraMover() {
			if (cameraMover == null)
				return;

			cameraMover.ResetCameraMover();
		}

		public void ClearStage() {
			ResetCameraMover();
			ResetStage();
			ResetShip();

			SetElementsActive(false);
		}

		public void ReloadStage() {
			ResetCameraMover();
			ResetStage();

			InitCameraMover();
			SetElementsActive(true);
			spaceShip.DisableElement();
			RespawnShip();
		}

		public void RespawnShip() {
			if (spaceShip.IsInvulnerable)
				return;

			// Load Nothing if ship is not available
			if (gameMetrics.GetLives() <= 0) {
				LoadStage(StageType.NONE);
				GameEnd();
				return;
			}

			ResetShip();
			spaceShip.EnableElement();
			spaceShip.EnableInvulnerability(shipInvulerability);
		}

		public void SetElementsActive(bool active) {
			if (active) {
				curStage.EnableElement();
				spaceShip.EnableElement();
				cameraMover.SetCameraState(CameraMoverState.MOVING);
			}
			else {
				curStage.DisableElement();
				spaceShip.DisableElement(false);
				cameraMover.SetCameraState(CameraMoverState.IDLE);
			}
		}

		public void GameEnd() {
			mainMenuUi.SetActive(false);
			mainUi.SetActive(false);
			endUi.SetActive(true);
			SetEndScoreUIText(gameMetrics.GetScore().ToString(SICGameMetrics.UI_SCORE_VALUE_FORMAT));
		}

# if UNITY_EDITOR
		public void Update() {
			LevelsTest();

			if (Input.GetKeyDown(KeyCode.U)) {
				foreach (GameObject obj in SICGameUtility.GetAllVisibleEnemies()) {
					Debug.Log(obj.name);
				}
			}
		}
#endif

		public void SetScoreUIText(string text) {
			scoreUI.text = text;
		}

		public void SetLivesUIText(string text) {
			livesUi.text = text;
		}

		public void SetSpecialUIText(string text) {
			specialUi.text = text;
		}

		public void SetEndScoreUIText(string text) {
			endScoreUI.text = text;
		}

		public void LevelsTest() {
			if (Input.GetKeyDown(KeyCode.F1)) {
				LoadStage(StageType.STAGE_1);
			}

			if (Input.GetKeyDown(KeyCode.F2)) {
				LoadStage(StageType.STAGE_2);
			}

			if (Input.GetKeyDown(KeyCode.F3)) {
				LoadStage(StageType.STAGE_3);
			}

			if (Input.GetKeyDown(KeyCode.F4)) {
				LoadStage(StageType.STAGE_4);
			}

			if (Input.GetKeyDown(KeyCode.F5)) {
				LoadStage(StageType.STAGE_5);
			}

			if (Input.GetKeyDown(KeyCode.Home)) {
				LoadStage(StageType.NONE);
			}

			if (Input.GetKeyDown(KeyCode.R)) {
				ReloadStage();
			}

			if (Input.GetKeyDown(KeyCode.P)) {
				RespawnShip();
			}

			if (Input.GetKeyDown(KeyCode.T)) {
				InitShip();
			}
		}
	}
}