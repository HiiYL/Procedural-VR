using UnityEngine;
using System.Collections;

public class RetryButton : MonoBehaviour {

    public int secondsToActivate = 3;
    private float mouseoverSecondsLeft;

    private ButtonOverlay buttonOverlay;

    private bool isBeingGazed = false;

    // Use this for initialization
    void Start()
    {
        mouseoverSecondsLeft = secondsToActivate;
        buttonOverlay = GetComponentInChildren<ButtonOverlay>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingGazed)
        {
            Vector3 scale = buttonOverlay.transform.localScale;
            scale.x = (1 - (mouseoverSecondsLeft / secondsToActivate));
            buttonOverlay.transform.localScale = scale;
            mouseoverSecondsLeft -= Time.deltaTime;
            if (mouseoverSecondsLeft <= 0)
            {
                mouseoverSecondsLeft = secondsToActivate;
                GameManager.Instance.onContinue();
            }
        }

    }

    public void onMouseOver()
    {
        isBeingGazed = true;

    }
    public void onMouseOverCancel()
    {
        isBeingGazed = false;
        mouseoverSecondsLeft = secondsToActivate;
        Vector3 scale = buttonOverlay.transform.localScale;
        scale.x = (1 - (mouseoverSecondsLeft / secondsToActivate));
        buttonOverlay.transform.localScale = scale;
    }
}
