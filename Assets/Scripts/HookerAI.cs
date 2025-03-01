using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HookerAI : EnemyAI
{
        public enum HookerAIState
        {
                Idle,
                Wandering,
                Alert,
                Seeking,
                Attacking,
                Hooking
        }

        public HookerAIState state;

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
        [Header("Seeking state")]
        [SerializeField] float memoryTime;
        [SerializeField] float memoryTimeCurrent;
        [Header("Attacking state")]
        [SerializeField] float attackDistance;
        [SerializeField] float attackInterval;
        [SerializeField] float attackIntervalCounter;
        [SerializeField] Melee melee;
        [Header("Hooking state")]
        [SerializeField] float hookingDistance;
        [SerializeField] float hookingLargeInterval;
        [SerializeField] float hookingLargeIntervalCounter;
        [SerializeField] float hookingInterval;
        [SerializeField] float hookingIntervalCounter;
        [SerializeField] Hook hook;


        void Start()
        {
                nmAgent.destination = transform.position;
                SwitchAIState(HookerAIState.Idle);
                player = GameManager.Instance.player;
                sightDistanceCurrent = sightDistance;
        }

        void FixedUpdate()
        {
                hookingLargeIntervalCounter -= Time.fixedDeltaTime;

                bool seesPlayer = Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out RaycastHit hit, sightDistanceCurrent, sightLayers);
                if (hit.collider != null)
                {
                        Player hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                        bool toSeek = hitPlayer != null && seesPlayer;
                        if (toSeek && state != HookerAIState.Attacking)
                        {
                                SwitchAIState(HookerAIState.Seeking);
                        }
                }

                if (GameManager.Instance.hadAllRelics && state != HookerAIState.Attacking)
                {
                        SwitchAIState(HookerAIState.Seeking);
                }

                UpdateState();
        }

        void UpdateState()
        {
                switch (state)
                {
                        case HookerAIState.Idle:

                                idleIntervalCounter -= Time.fixedDeltaTime;
                                if (idleIntervalCounter <= 0f)
                                {
                                        SwitchAIState(HookerAIState.Wandering);
                                }
                                break;

                        case HookerAIState.Wandering:

                                wanderDelayCounter -= Time.fixedDeltaTime;
                                if (wanderDelayCounter <= 0f)
                                {
                                        bool changeState = Random.Range(0f, 1f) > 0.5f;
                                        if (changeState)
                                        {
                                                SwitchAIState(HookerAIState.Idle);
                                        }
                                        else
                                        {
                                                SwitchAIState(HookerAIState.Wandering);
                                        }
                                }
                                break;

                        case HookerAIState.Alert:

                                alertWanderDelayCounter -= Time.fixedDeltaTime;
                                if (alertWanderDelayCounter <= 0f)
                                {
                                        sightDistanceCurrent = sightDistance;

                                        float changeState = Random.Range(0f, 1f);
                                        if (changeState < 0.3f)
                                        {
                                                SwitchAIState(HookerAIState.Idle);
                                        }
                                        else if (changeState < 0.6f)
                                        {
                                                SwitchAIState(HookerAIState.Wandering);
                                        }
                                        else
                                        {
                                                SwitchAIState(HookerAIState.Alert);
                                        }
                                }
                                break;

                        case HookerAIState.Seeking:
                                memoryTimeCurrent -= Time.fixedDeltaTime;
                                if (memoryTimeCurrent <= 0f)
                                {
                                        SwitchAIState(HookerAIState.Alert);
                                }

                                var distanceToPlayer = (player.transform.position - transform.position).magnitude;
                                if (distanceToPlayer <= hookingDistance && distanceToPlayer > attackDistance && hookingLargeIntervalCounter <= 0f)
                                {
                                        SwitchAIState(HookerAIState.Hooking);
                                }
                                else if (distanceToPlayer <= attackDistance)
                                {
                                        SwitchAIState(HookerAIState.Attacking);
                                }
                                break;

                        case HookerAIState.Attacking:

                                attackIntervalCounter -= Time.fixedDeltaTime;
                                {
                                        if (attackIntervalCounter <= 0f)
                                        {
                                                SwitchAIState(HookerAIState.Seeking);
                                        }
                                }

                                break;

                        case HookerAIState.Hooking:

                                hookingIntervalCounter -= Time.fixedDeltaTime;
                                {
                                        if (hookingIntervalCounter <= 0f)
                                        {
                                                SwitchAIState(HookerAIState.Seeking);
                                        }
                                }
                                break;

                        default:
                                break;
                }
        }

        public void SwitchAIState(HookerAIState newState)
        {
                state = newState;
                float randomAngle;
                Vector2 randomMovement;
                switch (newState)
                {
                        case HookerAIState.Idle:

                                idleIntervalCounter = idleInterval + Random.Range(-idleIntervalDev, idleIntervalDev);
                                break;

                        case HookerAIState.Wandering:

                                wanderDelayCounter = wanderDelay + Random.Range(-wanderDelayDev, wanderDelayDev);

                                randomAngle = Random.Range(0f, 360f);
                                randomMovement = new Vector2(Mathf.Sin(randomAngle * Mathf.Deg2Rad), Mathf.Cos(randomAngle * Mathf.Deg2Rad));
                                randomMovement *= Random.Range(wanderDistance / 2f, wanderDistance);
                                SetDestination(transform.position + new Vector3(randomMovement.x, 0f, randomMovement.y));
                                break;

                        case HookerAIState.Alert:

                                alertWanderDelayCounter = alertWanderDelay + Random.Range(-alertWanderDelayDev, alertWanderDelayDev);

                                sightDistanceCurrent = sightDistanceAlert;

                                randomAngle = Random.Range(0f, 360f);
                                randomMovement = new Vector2(Mathf.Sin(randomAngle * Mathf.Deg2Rad), Mathf.Cos(randomAngle * Mathf.Deg2Rad));
                                randomMovement *= Random.Range(alertWanderDistance / 2f, alertWanderDistance);
                                SetDestination(transform.position + new Vector3(randomMovement.x, 0f, randomMovement.y));
                                break;

                        case HookerAIState.Seeking:
                                memoryTimeCurrent = memoryTime;

                                SetDestination(player.transform.position);
                                break;

                        case HookerAIState.Hooking:

                                hookingIntervalCounter = hookingInterval;
                                hookingLargeIntervalCounter = hookingLargeInterval;

                                SetDestination(transform.position);

                                hook.CastHook((player.transform.position - transform.position).normalized);

                                break;

                        case HookerAIState.Attacking:

                                attackIntervalCounter = attackInterval;

                                SetDestination(transform.position);

                                melee.OnMelee((player.transform.position - transform.position).normalized * attackDistance);
                                break;
                        default:
                                break;
                }
        }


        void SetDestination(Vector3 destination)
        {
                if (nmAgent != null)
                {
                        nmAgent.destination = destination;
                }
        }
}
