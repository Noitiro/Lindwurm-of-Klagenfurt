using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialContoller : MonoBehaviour {
    [SerializeField] GameObject wsadInfo;
    [SerializeField] GameObject sprintInfo;
    [SerializeField] GameObject attackInfo;
    [SerializeField] GameObject changeAttackInfo;
    [SerializeField] GameObject typeEnemyInfo;
    [SerializeField] GameObject destroyHouseInfo;

    [SerializeField] GameObject barrierTutorial;
    [SerializeField] FadingScript fadingScript;
    [SerializeField] GameObject enemyWalk;
    [SerializeField] GameObject introCanvas;
    [SerializeField] GameObject audioAmbience;

    [SerializeField] WaveSpawn waveSpawn;

    private int state;
    private Animator anim;
    private bool areaAttack = false;

    private void Start () {
        anim = GetComponent<Animator>();
        state = 0;
    }


    private void Update () {
        if(anim.GetBool("isWalk") == true && state == 0) {
            showInfo(sprintInfo);
            state++;
        }

        if (state == 1 && areaAttack) {
            showInfo(attackInfo);
            enemyWalk.SetActive(true);
            state++;
        }
        if (state == 2 && waveSpawn.CurrentWaveNumber == 2) {
            hideInfo(attackInfo);
            showInfo(changeAttackInfo);
            state++;
        }
        if (state == 3 && waveSpawn.CurrentWaveNumber != 2) {
            barrierTutorial.SetActive(false);
            destroyHouseInfo.SetActive(true) ;
        }
    }
    public void showInfo(GameObject whatInfo) {
        whatInfo.SetActive(true);
    }

    private void hideInfo(GameObject whatInfo) {
        whatInfo.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name.Equals("TutorialCollider")) {
            areaAttack = true;
        }

        if (collision.gameObject.name.Equals("EndCollider")) {
            //  fadingScript.FadeOut();
            //StartCoroutine(WaitLoad());
            audioAmbience.SetActive(false) ;
            CursorManager.ShowCursor();
            introCanvas.SetActive(true);
        }

        Debug.Log(collision.gameObject.name);
    }

    //private IEnumerator WaitLoad() {
        //yield return new WaitForSeconds(2f);
       // SceneManager.LoadScene(2);
   // }
}