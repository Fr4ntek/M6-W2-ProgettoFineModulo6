using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeOfDeath : MonoBehaviour
{
    [SerializeField] private float totalTime = 10f;
    [SerializeField] private float requiredCrouchTime = 5f;

    private float _crouchTimer = 0f;
    private float _countdown = 0f;
    private bool _challengeActive = false;

    private PlayerController _playerController;
    private UIController _uiController;

    private void OnTriggerEnter(Collider other)
    {
        if (!_challengeActive && other.CompareTag("Player"))
        {
            _playerController = other.GetComponent<PlayerController>();
            _uiController = other.GetComponent<UIController>();
            StartCoroutine(CrouchChallenge());
        }
    }

    private IEnumerator CrouchChallenge()
    {
        _challengeActive = true;
        _countdown = 0f;
        _crouchTimer = 0f;

        // Ha 10 secondi per premere Crouch e tenerlo premuto per 5 secondi per vincere
        // altrimenti muore

        while (_countdown < totalTime)
        {
            _countdown += Time.deltaTime;

            if (_playerController.IsCrouching)
            {
                _crouchTimer += Time.deltaTime;
            }

            if (_crouchTimer >= requiredCrouchTime)
            {
                _uiController.ShowVictoryUI();
                _challengeActive = false;
                yield break;
            }

            yield return null;
        }
        _uiController.ShowDeathUI();
        _challengeActive = false;
    }
}

