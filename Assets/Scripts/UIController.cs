using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //反映対象
    public TextMeshProUGUI odometerText;
    public TextMeshProUGUI bulletText;
    public TextMeshProUGUI maxScoretext;
    public Slider lifeSlider;

    //データ元
    PlayerController player;
    Shooter shooter;

    //一次記録
    int currentShotPower;
    int currentPlayerLife;

    //ゲームステータスによる表示/非表示の指定のため取得
    public GameObject odometerPanel;
    public GameObject bulletPanel;
    public GameObject playerLifePanel;
    public GameObject gameOverPanel;
    public GameObject maxScorePanel; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        shooter = GameObject.FindGameObjectWithTag("shooter").GetComponent<Shooter>();
        maxScoretext.text = PlayerPrefs.GetFloat("Score").ToString("F1");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        //最高記録の保存
        odometerText.text = player.gameObject.transform.position.z.ToString("F1");

        //弾の残数表示
        if(currentShotPower != shooter.shotPower)
        {
            currentShotPower = shooter.shotPower;
            bulletText.text = currentShotPower.ToString();
        }

        //HPの表示
        if(currentPlayerLife != player.life)
        {
            currentPlayerLife = player.life;
            lifeSlider.value = currentPlayerLife;

        }

        if(GameManager.gameState == GameState.gameover)
        {
            odometerPanel.SetActive(false);
            bulletPanel.SetActive(false);
            playerLifePanel.SetActive(false);
            maxScorePanel.SetActive(false);

            gameOverPanel.SetActive(true);

            //gameover時のカーソル表示: CursorクラスのlockState, visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //何重にも描画処理しないためにステータスを変更
            GameManager.gameState = GameState.end;
        }
    }
}
