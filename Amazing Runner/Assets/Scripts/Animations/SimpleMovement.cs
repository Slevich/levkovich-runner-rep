using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleMovement : MonoBehaviour
{
    #region Fields
    [Header("The Vector3 point to which the object moves.")]
    [SerializeField] private Vector3 movingTarget;
    [Header("The time it takes for an object to reach a point.")]
    [SerializeField] private float movingDuration;
    [Header("The time an object waits before moving again.")]
    [SerializeField] private float waitingTimer;

    //Vector3 point with the start position of the object.
    private Vector3 startPosition;
    #endregion

    #region Methods
    /// <summary>
    /// In Awake we get the start position of the object.
    /// </summary>
    private void Awake()
    {
        startPosition = transform.localPosition;
    }

    /// <summary>
    /// At the start, run the method with a motion animation.
    /// </summary>
    private void Start()
    {
        MoveObjectBetwenTwoPoints();
    }

    /// <summary>
    /// The sequence moves the object to a point, 
    /// then to the starting position.
    /// </summary>
    private void MoveObjectBetwenTwoPoints()
    {
        DOTween.Sequence()
               .Append(transform.DOLocalMove(movingTarget, movingDuration))
                   .SetEase(Ease.InOutSine)
                   .SetLoops(-1)
               .AppendInterval(waitingTimer)
               .Append(transform.DOLocalMove(startPosition, movingDuration))
                   .SetEase(Ease.InOutSine)
                   .SetLoops(-1);            
    }
    #endregion
}
