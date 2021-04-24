using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        [Header("Audio")]
        public AudioClip jumpAudio;
        public AudioClip interactAudio;
        public AudioClip lifeAudio;
        public AudioClip respawnAudio;
        public AudioClip dieAudio;
        public AudioClip ouchAudio;
        public AudioClip winAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        [Header("Speed")]
        public float maxSpeed = 3.0f;
        public float sprintAdditionalSpeed = 2.0f;
        private float baseSpeed = 0.0f;
        public GameObject trailPrefab = null;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        [Header("Jump")]
        public float jumpTakeOffSpeed = 7;
        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        public GameObject bouncyPrefab = null;

        [Header("Player Object")]
        /*internal new*/
        public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        private int score = 0;
        private int tokencount = 0;

        bool isHaste = false;
        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            baseSpeed = maxSpeed;
            if (trailPrefab != null) trailPrefab.SetActive(false);
            if (bouncyPrefab != null) bouncyPrefab.SetActive(true);
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");

                if (Input.GetButtonDown("Sprint"))
                {
                    maxSpeed += sprintAdditionalSpeed;
                    baseSpeed += sprintAdditionalSpeed;
                }
                else if (Input.GetButtonUp("Sprint"))
                {
                    maxSpeed -= sprintAdditionalSpeed;
                    baseSpeed -= sprintAdditionalSpeed;
                }

                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else 
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();
        }

        public float GetOriginalSpeed()
        {
            return this.baseSpeed;
        }

        public int GetScore()
        {
            return this.score;
        }

        public int GetTokenCount()
        {
            return this.tokencount;
        }

        public void Hasten(bool flag)
        {
            isHaste = flag;
        }

        public bool Hasten()
        {
            return isHaste;
        }

        public void AddScore(int additonalScore)
        {
            score += additonalScore;
            if (score % 500 == 0 && score > 0)
            {
                health.Increment();
                if (audioSource && lifeAudio)
                    audioSource.PlayOneShot(model.player.lifeAudio);
            }

        }

        public void AddToken(int additonalToken)
        {
            tokencount += additonalToken;
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        public void recover()
        {
            Bounce(new Vector2(-9, 3));
            StartCoroutine("Recover");
        }
        IEnumerator Recover()
        {
            yield return new WaitForSeconds(0.5f);
            collider2d.enabled = true;
            controlEnabled = true;
        }

        public void victory()
        {
            animator.SetTrigger("victory");
            controlEnabled = false;
            if (audioSource && winAudio)
                audioSource.PlayOneShot(model.player.winAudio);
            StartCoroutine("LoadNextLevel");
        }

        IEnumerator LoadNextLevel()
        {
            yield return new WaitForSeconds(6.5f);
            SceneManager.LoadScene("StartScene");
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}