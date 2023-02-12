using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// Controller script for the heads-up interface
public class HUDController : MonoBehaviour
{
	/// --- Singleton definitions ---
	private static HUDController _instance;
	public static HUDController Instance { get { return _instance; } }
	
	/// --- Element References ---
	private GameManager _gm;
	
	private TextMeshProUGUI _stageText;
	private TextMeshProUGUI _scoreText;
	private TextMeshProUGUI _livesText;
	public TextMeshProUGUI _bossName;
	public Image _bossHpBar;
	
	public GameObject _bossBlock;
	public GameObject _gameOverBlock;
	public GameObject _victoryBlock;
	
	void Awake()
	{
		// singleton logic, avoid duplicate elements
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}
	
	void Start()
	{
		_gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
		
		_stageText = GameObject.Find("Stage Tag").GetComponent<TextMeshProUGUI>();
		_scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
		_livesText = GameObject.Find("Lives Text").GetComponent<TextMeshProUGUI>();
	}
	
	/* ********************************* */
	public void SetStageText (int level)
	{
		switch(level)
		{
			case 1:
				_stageText.text = "Stage I";
				break;
			default:
				Debug.Log("Operational error: passed a stage name to HUDController.SetStageText that doesn't exist.");
				break;
		}
	}
	
	public void TurnOnBossBar (string bossName)
	{
		_bossBlock.SetActive(true);
		_bossName.text = bossName;
		_bossHpBar.fillAmount = 0f;
	}
	
	public void TurnOffBossBar ()
	{
		_bossBlock.SetActive(false);
	}
	
	public void SetHealthBar(float fill)
	{
		_bossHpBar.fillAmount = Mathf.Clamp(fill, 0, 1f);
	}
	
	public void SetScore (int score)
	{
		_scoreText.text = score.ToString().PadLeft(7, '0');
	}
	
	public void SetLives (int lives)
	{
		_livesText.text = lives.ToString();
	}
	
	public void DisplayGameOver (bool gover)
	{
		_gameOverBlock.SetActive(gover);
	}
	
	public void DisplayVictoryScreen (bool vic)
	{
		_victoryBlock.SetActive(vic);
	}
	
	public void RestartStage ()
	{
		_gm.RestartStage();
	}
}
