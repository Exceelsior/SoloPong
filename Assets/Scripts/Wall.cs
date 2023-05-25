using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private BoxCollider _ballTargetCollider;
    [SerializeField]
    private Vector2 _ballXTargetOnBoundsRandomness;
    [SerializeField]
    private Vector2 _ballYTargetOnBoundsRandomness;
    [SerializeField]
    private AnimationCurve _bouncinessForHitsCurve;
    [SerializeField]
    private AudioManager _audioManager;


    private GameManager _gameManager;



    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
        _ballTargetCollider.isTrigger = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.gameObject.GetComponent<Ball>();

        if (ball != null && ball.hasCollidedWithWall == false)
        {
            ball.hasCollidedWithWall = true;
            ball.hasCollidedWithRacket = false;
            _audioManager.PlayBallBounceSound(false);
            Vector3 newVelocity = Vector3.Reflect(ball.ballRigidbody.velocity, -1 * transform.forward) * _bouncinessForHitsCurve.Evaluate(_gameManager.TotalHits);
            ball.ballRigidbody.velocity = newVelocity;
        }
    }

    public Vector3 GetRandomPointOnBoundsFromBallPos(Vector3 ballPos)
    {
        Vector3 result = Vector3.zero;

        Bounds bounds = _ballTargetCollider.bounds;

        Vector3 closestPosition = bounds.ClosestPoint(ballPos);

        float closestXPositionAdjusted = Mathf.Clamp(closestPosition.x + Random.Range(_ballXTargetOnBoundsRandomness.x, _ballXTargetOnBoundsRandomness.y),
            bounds.center.x - bounds.extents.x, bounds.center.x + bounds.extents.x);
        float closestYPositionAdjusted = Mathf.Clamp(closestPosition.y + Random.Range(_ballYTargetOnBoundsRandomness.x, _ballXTargetOnBoundsRandomness.y),
            bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y);

        result = new Vector3(closestYPositionAdjusted, closestYPositionAdjusted, closestPosition.z);

        return result;
    }


}
