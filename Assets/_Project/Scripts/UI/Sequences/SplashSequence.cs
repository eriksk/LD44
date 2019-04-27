using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD44.UI.Sequences
{
    public class SplashSequence : MonoBehaviour
    {
        public AnimationClip Clip;
        public string TargetSceneName;

        void Start()
        {
            StartCoroutine(WaitForAnimationAndTransitionToScene());
        }

        private IEnumerator WaitForAnimationAndTransitionToScene()
        {
            yield return new WaitForSeconds(Clip.length);
            SceneManager.LoadScene(TargetSceneName, LoadSceneMode.Single);
        }
    }
}