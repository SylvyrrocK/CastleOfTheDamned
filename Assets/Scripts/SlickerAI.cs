using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlickerAI : EnemyAI
{
        public enum SlickerAIState
        {
                Idle,
                Wandering,
                Alert,
                Seeking,
                Attacking,
                Fleeing
        }

        public SlickerAIState state;
        [SerializeField] GameObject deadSlicker;

        [Header("Vision")]
        [SerializeField] float sightDistance;
        [SerializeField] float sightDistanceAlert;
        [SerializeField] float sightDistanceCurrent;
        [Header("Idle state")]
        [SerializeField] float idleInterval;
        [SerializeField] float idleIntervalCounter;
        [SerializeField] float idleIntervalDev;
        [Header("Wander state")]
        [SerializeField] float wanderDistance;
        [SerializeField] float wanderDelay;
        [SerializeField] float wanderDelayCounter;
        [SerializeField] float wanderDelayDev;
        [Header("Alert state")]
        [SerializeField] float alertWanderDistance;
        [SerializeField] float alertWanderDelay;
        [SerializeField] float alertWanderDelayCounter;
        [SerializeField] float alertWanderDelayDev;
        [Header("Fleeing state")]
        [SerializeField] float fleeingHealthThreshold;
        [SerializeField] float fleeingDistance;
        [SerializeField] float fleeingTime;
        [SerializeField] float fleeingTimeCounter;
        [Header("Seeking state")]
        [SerializeField] float memoryTime;
        [SerializeField] float memoryTimeCurrent;
        [Header("Attacking state")]
        [SerializeField] float attackDistance;
        [SerializeField] float attackInterval;
        [SerializeField] float attackIntervalCounter;
        [SerializeField] Melee melee;

        void Start()
        {
                nmAgent.destination = transform.position;
                SwitchAIState(SlickerAIState.Idle);
                player = GameManager.Instance.player;
                sightDistanceCurrent = sightDistance;
        }

        void FixedUpdate()
        {
                bool seesPlayer = Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out RaycastHit hit, sightDistanceCurrent, sightLayers);
                if (hit.collider != null)
                {
                        Player hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                        bool toSeek = hitPlayer != null && seesPlayer;
                        if (toSeek && state != SlickerAIState.Attacking)
                        {
                                SwitchAIState(SlickerAIState.Seeking);
                        }
                }

                if (GameManager.Instance.hadAllRelics && state != SlickerAIState.Attacking)
                {
                        SwitchAIState(SlickerAIState.Seeking);
                }

                UpdateState();
        }

        void UpdateState()
        {
                switch (state)
                {
                        case SlickerAIState.Idle:

                                idleIntervalCounter -= Time.fixedDeltaTime;
                                if (idleIntervalCounter <= 0f)
                                {
                                        SwitchAIState(SlickerAIState.Wandering);
                                }
                                break;

                        case SlickerAIState.Wandering:

                                wanderDelayCounter -= Time.fixedDeltaTime;
                                if (wanderDelayCounter <= 0f)
                                {
                                        bool changeState = Random.Range(0f, 1f) > 0.5f;
                                        if (changeState)
                                        {
                                                SwitchAIState(SlickerAIState.Idle);
                                        }
                                        else
                                        {
                                                SwitchAIState(SlickerAIState.Wandering);
                                        }
                                }
                                break;

                        case SlickerAIState.Alert:

                                alertWanderDelayCounter -= Time.fixedDeltaTime;
                                if (alertWanderDelayCounter <= 0f)
                                {
                                        sightDistanceCurrent = sightDistance;

                                        float changeState = Random.Range(0f, 1f);
                                        if (changeState < 0.3f)
                                        {
                                                SwitchAIState(SlickerAIState.Idle);
                                        }
                                        else if (changeState < 0.6f)
                                        {
                                                SwitchAIState(SlickerAIState.Wandering);
                                        }
                                        else
                                        {
                                                SwitchAIState(SlickerAIState.Alert);
                                        }
                                }
                                break;

                        case SlickerAIState.Fleeing:

                                fleeingTimeCounter -= Time.fixedDeltaTime;
                                if (fleeingTimeCounter <= 0f)
                                {
                                        if (health.health < health.maxHealth * fleeingHealthThreshold)
                                        {
                                                SwitchAIState(SlickerAIState.Fleeing);
                                        }
                                        else
                                        {
                                                SwitchAIState(SlickerAIState.Alert);
                                        }
                                }
                                break;

                        case SlickerAIState.Seeking:
                                memoryTimeCurrent -= Time.fixedDeltaTime;
                                if (memoryTimeCurrent <= 0f)
                                {
                                        SwitchAIState(SlickerAIState.Alert);
                                }

                                if ((player.transform.position - transform.position).magnitude <= attackDistance)
                                {
                                        SwitchAIState(SlickerAIState.Attacking);
                                }
                                break;

                        case SlickerAIState.Attacking:

                                attackIntervalCounter -= Time.fixedDeltaTime;
                                {
                                        if (attackIntervalCounter <= 0f)
                                        {
                                                SwitchAIState(SlickerAIState.Seeking);
                                        }
                                }

                                break;

                        default:
                                break;
                }
        }

        public void SwitchAIState(SlickerAIState newState)
        {
                state = newState;
                float randomAngle;
                Vector2 randomMovement;
                switch (newState)
                {
                        case SlickerAIState.Idle:

                                idleIntervalCounter = idleInterval + Random.Range(-idleIntervalDev, idleIntervalDev);
                                break;

                        case SlickerAIState.Wandering:

                                wanderDelayCounter = wanderDelay + Random.Range(-wanderDelayDev, wanderDelayDev);

                                randomAngle = Random.Range(0f, 360f);
                                randomMovement = new Vector2(Mathf.Sin(randomAngle * Mathf.Deg2Rad), Mathf.Cos(randomAngle * Mathf.Deg2Rad));
                                randomMovement *= Random.Range(wanderDistance / 2f, wanderDistance);
                                SetDestination(transform.position + new Vector3(randomMovement.x, 0f, randomMovement.y));
                                break;

                        case SlickerAIState.Alert:

                                alertWanderDelayCounter = alertWanderDelay + Random.Range(-alertWanderDelayDev, alertWanderDelayDev);

                                sightDistanceCurrent = sightDistanceAlert;

                                randomAngle = Random.Range(0f, 360f);
                                randomMovement = new Vector2(Mathf.Sin(randomAngle * Mathf.Deg2Rad), Mathf.Cos(randomAngle * Mathf.Deg2Rad));
                                randomMovement *= Random.Range(alertWanderDistance / 2f, alertWanderDistance);
                                SetDestination(transform.position + new Vector3(randomMovement.x, 0f, randomMovement.y));
                                break;

                        case SlickerAIState.Fleeing:

                                fleeingTimeCounter = fleeingTime;

                                sightDistanceCurrent = 0f;

                                Vector3 fleeDirection = (transform.position - player.transform.position).normalized * fleeingDistance;
                                SetDestination(transform.position + new Vector3(fleeDirection.x, 0f, fleeDirection.z));
                                break;

                        case SlickerAIState.Seeking:
                                memoryTimeCurrent = memoryTime;

                                SetDestination(player.transform.position);
                                break;

                        case SlickerAIState.Attacking:

                                attackIntervalCounter = attackInterval;

                                SetDestination(transform.position);

                                melee.OnMelee((player.transform.position - transform.position).normalized * attackDistance);
                                break;
                        default:
                                break;
                }
        }

        public void OnDamage(float damage)
        {
                if (health.health < health.maxHealth * fleeingHealthThreshold)
                {
                        SwitchAIState(SlickerAIState.Fleeing);
                }
        }

        void SetDestination(Vector3 destination)
        {
                if (nmAgent != null && nmAgent.isActiveAndEnabled && nmAgent.isOnNavMesh)
                {
                        nmAgent.destination = destination;
                }
        }

        public void SpawnDeadSlicker()
        {
                Instantiate(deadSlicker, transform.position, Quaternion.identity);
        }
}
