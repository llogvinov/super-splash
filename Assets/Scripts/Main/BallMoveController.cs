using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine;
using UnityEngine.Events;

namespace Main
{
    public class BallMoveController : MonoBehaviour
    {
        [SerializeField] private SwipeListener swipeListener;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private LevelManager levelManager;

        [SerializeField] private float stepDuration;
        [SerializeField] private LayerMask wallsAndRoadsLayer;

        private const float MAX_RAY_DISTANCE = 100f;

        public UnityAction<List<RoadTile>, float> onMoveStart;

        private Vector3 moveDirection;
        private bool canMove = true;

        private void Start()
        {
            LevelManager.Initialized += OnInitialized;
        }

        private void OnDestroy()
        {
            LevelManager.Initialized -= OnInitialized;
        }

        private void OnInitialized()
        {
            transform.position = levelManager.DefaultBallRoadTile.position;
            transform.position = new Vector3(transform.position.x, transform.lossyScale.y / 2f +
               levelManager.DefaultBallRoadTile.transform.lossyScale.y / 2f, transform.position.z);
            canMove = true;

            playerInput.InputDone += OnInputDone;
            swipeListener.OnSwipe.AddListener(OnInputDone);
        }

        private void OnInputDone(string direction)
        {
            switch (direction)
            {
                case "Right":
                    moveDirection = Vector3.right;
                    break;
                case "Left":
                    moveDirection = Vector3.left;
                    break;
                case "Up":
                    moveDirection = Vector3.forward;
                    break;
                case "Down":
                    moveDirection = Vector3.back;
                    break;
                default:
                    return;
            }
            MoveBall();
        }

        private void MoveBall()
        {
            if (canMove)
            {
                canMove = false;
                RaycastHit[] hits = Physics.RaycastAll(transform.position, moveDirection, MAX_RAY_DISTANCE, wallsAndRoadsLayer.value)
                   .OrderBy(hit => hit.distance).ToArray();

                Vector3 targetPosition = transform.position;

                int steps = 0;

                List<RoadTile> pathRoadTiles = new List<RoadTile>();

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.isTrigger)
                    {
                        pathRoadTiles.Add(hits[i].transform.GetComponent<RoadTile>());
                    }
                    else
                    {
                        if (i == 0)
                        {
                            canMove = true;
                            return;
                        }
                        steps = i;
                        var finalPosition = hits[i - 1].transform.position;
                        targetPosition = new Vector3(finalPosition.x, targetPosition.y, finalPosition.z);
                        break;
                    }
                }

                float moveDuration = stepDuration * steps;
                transform
                    .DOMove(targetPosition, moveDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => canMove = true);

                if (onMoveStart != null)
                    onMoveStart.Invoke(pathRoadTiles, moveDuration);
            }
        }
    }
}