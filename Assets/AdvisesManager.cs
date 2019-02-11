using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class AdvisesManager : MonoBehaviour {

    public GameObject panel;
    public Text field;

	void Start () {
      //  if (Data.Instance.isArcade) return;
      //  panel.SetActive(false);
      //  Data.Instance.events.AdvisesOn += AdvisesOn;
	}
    void OnDestroy()
    {
       // Data.Instance.events.AdvisesOn -= AdvisesOn;
    }
	
    void AdvisesOn(string name)
    {
        panel.SetActive(true);
        field.text = name.ToUpper();
       // StartCoroutine(Play(panel.GetComponent<Animation>(), "advisesShow", false));//, OnComplete));
      //  Time.timeScale = 0;
	}
    void OnComplete()
    {
       // Time.timeScale = 1;
    }
//    IEnumerator Play(Animation animation, string clipName, bool useTimeScale)//, Action onComplete)
//    {
//
//        //We Don't want to use timeScale, so we have to animate by frame..
//        if (!useTimeScale)
//        {
//            AnimationState _currState = animation[clipName];
//            bool isPlaying = true;
//            float _progressTime = 0F;
//            float _timeAtLastFrame = 0F;
//            float _timeAtCurrentFrame = 0F;
//            float deltaTime = 0F;
//
//
//            animation.Play(clipName);
//
//            _timeAtLastFrame = Time.realtimeSinceStartup;
//            while (isPlaying)
//            {
//                _timeAtCurrentFrame = Time.realtimeSinceStartup;
//                deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
//                _timeAtLastFrame = _timeAtCurrentFrame;
//
//                _progressTime += deltaTime;
//                _currState.normalizedTime = _progressTime / _currState.length;
//                animation.Sample();
//
//                if (_progressTime >= _currState.length)
//                {
//                    if (_currState.wrapMode != WrapMode.Loop)
//                    {
//                        isPlaying = false;
//                    }
//                    else
//                    {
//                        _progressTime = 0.0f;
//                    }
//
//                }
//
//                yield return new WaitForEndOfFrame();
//            }
//            yield return null;
//           // if (onComplete != null)
//         //   {
//           //     onComplete();
//         //   }
//        }
//        else
//            animation.Play(clipName);
//    }  
}
