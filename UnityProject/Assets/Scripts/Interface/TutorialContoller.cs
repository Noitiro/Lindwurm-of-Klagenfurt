using UnityEngine;
using UnityEngine.UI;

public class TutorialContoller : MonoBehaviour {
    [SerializeField] GameObject wsadInfo;
    [SerializeField] GameObject sprintInfo;
    [SerializeField] GameObject attackInfo;
    [SerializeField] GameObject changeAttackInfo;
    [SerializeField] GameObject typeEnemyInfo;
    [SerializeField] GameObject destroyHouseInfo;

    private int state;
    private Animator anim;

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
            showInfo(attackInfo);
            state++;
        }
        if (state == 3) {
            hideInfo(attackInfo);
        }
    }
    public void showInfo(GameObject whatInfo) {
        whatInfo.SetActive(true);
    }

    private void hideInfo(GameObject whatInfo) {
        whatInfo.SetActive(false);
    }
}