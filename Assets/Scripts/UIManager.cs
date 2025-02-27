using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

namespace Assets.Scripts
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private GameObject gameOverUI;
		[SerializeField] private GameObject gameWinUI;
		[SerializeField] private GameObject chest;
		private bool isGameOver = false;
		private bool isGameWin = false;

		public GameObject damageTextPrefab;
		public GameObject healthTextPrefab;

		public Canvas gameCanvas;

		private void Awake()
		{
			gameCanvas = FindFirstObjectByType<Canvas>();
		}

		private void Start()
		{
			gameOverUI.SetActive(false);
			gameWinUI.SetActive(false);
			chest.SetActive(false);
		}

		private void OnEnable()
		{
			CharacterEvents.characterDamaged += CharacterTookDamage;
			CharacterEvents.characterHealed += CharacterHealed;
		}

		private void OnDisable()
		{
			CharacterEvents.characterDamaged -= CharacterTookDamage;
			CharacterEvents.characterHealed -= CharacterHealed;
		}

		public void CharacterTookDamage(GameObject character, int damageReceived)
		{
			Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

			TMP_Text tmp_Text = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
				.GetComponent<TMP_Text>();

			tmp_Text.text = damageReceived.ToString();
		}

		public void CharacterHealed(GameObject character, int healthRestored)
		{
			Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

			TMP_Text tmp_Text = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
				.GetComponent<TMP_Text>();

			tmp_Text.text = healthRestored.ToString();
		}

		public void OnExitGame(InputAction.CallbackContext context)
		{
			if (context.started)
			{
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
				Debug.Log(this.name + ": " + this.GetType() + ": " + System.Reflection.MethodBase.GetCurrentMethod().Name);
#endif

#if (UNITY_EDITOR)
				UnityEditor.EditorApplication.isPlaying = false;
#endif
			}
		}

		public void GameOver()
		{
			isGameOver = true;
			gameOverUI.SetActive(true);
		}

		public void GameWin()
		{
			isGameWin = true;
			gameWinUI.SetActive(true);
		}

		public void ActiveChest()
		{
			chest.SetActive(true);
		}

		public void RestartGame()
		{
			isGameOver = false;
			isGameWin = false;

			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		public void QuitGame()
		{
			Application.Quit();
		}

		public bool IsGameOver()
		{
			return isGameOver;
		}

		public bool IsGameWin()
		{
			return isGameWin;
		}
	}
}