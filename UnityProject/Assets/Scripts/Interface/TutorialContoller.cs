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
        if (anim.GetBool("isSprint") == true && state == 1) {
            hideInfo(wsadInfo);
            hideInfo(sprintInfo);
            state++;
        }
        if (state == 2 && areaAttack) {
            showInfo(attackInfo);
            state++;
        }
        if (state == 3 && waveSpawn.CurrentWaveNumber == 2) {
            hideInfo(attackInfo);
            showInfo(changeAttackInfo);
            state++;
        }
        if (state == 4 && waveSpawn.CurrentWaveNumber != 2) {
            barrierTutorial.SetActive(false);
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
            fadingScript.FadeOut();
            StartCoroutine(WaitLoad());
        }

        Debug.Log(collision.gameObject.name);
    }

    private IEnumerator WaitLoad() {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(2);
    }
}